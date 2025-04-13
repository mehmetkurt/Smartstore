using System.Globalization;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Smartstore.Core;
using Smartstore.Core.Checkout.Cart;
using Smartstore.Core.Checkout.Orders;
using Smartstore.Core.Checkout.Payment;
using Smartstore.Core.Data;
using Smartstore.Core.Security;
using Smartstore.Engine.Modularity;
using Smartstore.Http;
using Smartstore.Iyzico.Components;
using Smartstore.Iyzico.Configuration;
using Smartstore.Iyzico.Models;

namespace Smartstore.Iyzico.Providers;

[SystemName(IyzicoDefaults.OnlinePayment.SystemName)]
[FriendlyName(IyzicoDefaults.OnlinePayment.FriendlyName)]
[Order(IyzicoDefaults.OnlinePayment.DisplayOrder)]
public class IyzicoOnlinePaymentProvider : IyzicoPaymentProviderBase, IConfigurable
{
    private readonly IEncryptor _encryptor;
    private readonly IyzicoSettings _iyzicoSettings;
    private readonly SmartDbContext _smartDbContext;
    private readonly ICommonServices _commonServices;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICheckoutStateAccessor _checkoutStateAccessor;
    private readonly IValidator<OnlinePaymentInfoModel> _onlinePaymentInfoModelValidator;

    public IyzicoOnlinePaymentProvider
    (
        IEncryptor encryptor,
        IyzicoSettings iyzicoSettings,
        SmartDbContext smartDbContext,
        ICommonServices commonServices,
        IHttpContextAccessor httpContextAccessor,
        ICheckoutStateAccessor checkoutStateAccessor,
        IValidator<OnlinePaymentInfoModel> onlinePaymentInfoModelValidator
    )
    {
        _encryptor = encryptor;
        this._iyzicoSettings = iyzicoSettings;
        this._smartDbContext = smartDbContext;
        _commonServices = commonServices;
        _httpContextAccessor = httpContextAccessor;
        _checkoutStateAccessor = checkoutStateAccessor;
        _onlinePaymentInfoModelValidator = onlinePaymentInfoModelValidator;
    }

    public RouteInfo GetConfigurationRoute() => new("Configure", "IyzicoAdmin", new { area = "Admin" });
    protected override Type GetViewComponentType() => typeof(OnlinePaymentViewComponent);
    public override RecurringPaymentType RecurringPaymentType => RecurringPaymentType.Manual;
    public override PaymentMethodType PaymentMethodType => PaymentMethodType.Standard;
    public override bool RequiresInteraction => true;

    public override async Task<string> GetPaymentSummaryAsync()
    {
        string result = string.Empty;
        IyzicoCheckoutState state = _checkoutStateAccessor.CheckoutState.GetCustomState<IyzicoCheckoutState>();
        result = $"{state.BrandName}, {state.CardType}, {state.CreditCardNumberMask}, {state.Installment} Installment";

        return await Task.FromResult(result);
    }
    public override Task<ProcessPaymentRequest> GetPaymentInfoAsync(IFormCollection form)
    {
        var paymentInfo = new ProcessPaymentRequest
        {
            OrderGuid = Guid.NewGuid(),
            CreditCardType = form["CreditCardType"],
            CreditCardName = form["CardholderName"],
            CreditCardNumber = form["CardNumber"],
            CreditCardExpireMonth = DateTime.ParseExact(form["ExpirationDate"].ToString(), "MM/yy", CultureInfo.InvariantCulture).Month,
            CreditCardExpireYear = DateTime.ParseExact(form["ExpirationDate"].ToString(), "MM/yy", CultureInfo.InvariantCulture).Year,
            CreditCardCvv2 = form["CardCode"].ToString().Trim()
        };


        IyzicoCheckoutState state = _checkoutStateAccessor.CheckoutState.GetCustomState<IyzicoCheckoutState>();
        state.Installment = form["Installment"].ToString().ToInt();
        state.CreditCardNumberMask = paymentInfo.CreditCardNumber.Mask(4);

        Core.Localization.Language language = _commonServices.WorkContext.WorkingLanguage;
        TextInfo textInfo = (new CultureInfo(language.LanguageCulture)).TextInfo;

        IEnumerable<string> cardInfo = paymentInfo.CreditCardType.EmptyNull().SplitSafe('|', StringSplitOptions.TrimEntries);
        if (cardInfo.Any())
        {
            string cardType = cardInfo.ElementAtOrDefault(0);
            string brandName = cardInfo.ElementAtOrDefault(1);

            if (cardType.HasValue())
            {
                paymentInfo.CreditCardType = textInfo.ToTitleCase(textInfo.ToLower(cardType).Replace("_", " "));
                state.CardType = paymentInfo.CreditCardType;
            }

            if (brandName.HasValue())
                state.BrandName = textInfo.ToTitleCase(textInfo.ToLower(brandName).Replace("_", " ")); ;
        }


        return Task.FromResult(paymentInfo);
    }

    /// <summary>
    /// İkinci method. 
    /// </summary>
    /// <param name="processPaymentRequest"></param>
    /// <returns></returns>
    public override Task<ProcessPaymentResult> ProcessPaymentAsync(ProcessPaymentRequest processPaymentRequest)
    {
        IyzicoCheckoutState state = _checkoutStateAccessor.CheckoutState.GetCustomState<IyzicoCheckoutState>();

        int installmentNumber = state.Installment;

        return base.ProcessPaymentAsync(processPaymentRequest);
    }

    /// <summary>
    /// İlk çalışan method.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public override Task<PreProcessPaymentResult> PreProcessPaymentAsync(ProcessPaymentRequest request)
    {
        return base.PreProcessPaymentAsync(request);
    }

    public override Task PostProcessPaymentAsync(PostProcessPaymentRequest postProcessPaymentRequest)
    {
        return base.PostProcessPaymentAsync(postProcessPaymentRequest);
    }

    public override async Task<PaymentValidationResult> ValidatePaymentDataAsync(IFormCollection form)
    {
        var model = new OnlinePaymentInfoModel
        {
            CardholderName = form["CardholderName"],
            CardNumber = form["CardNumber"],
            CardCode = form["CardCode"].ToString().Trim(),
            CreditCardType = form["CreditCardType"],
            ExpirationDate = form["ExpirationDate"],
            Installment = form["Installment"].ToString().ToInt(),
            TestMode = _iyzicoSettings.IsTestMode,
        };

        FluentValidation.Results.ValidationResult result = await _onlinePaymentInfoModelValidator.ValidateAsync(model);
        return new PaymentValidationResult(result);
    }

    public override async Task<ProcessPaymentRequest> CreateProcessPaymentRequestAsync(ShoppingCart cart)
    {
        Order lastOrder = await _smartDbContext.Orders
            .Where(o => o.PaymentMethodSystemName == IyzicoDefaults.OnlinePayment.SystemName && o.AllowStoringCreditCardNumber)
            .ApplyStandardFilter(cart.Customer.Id, cart.StoreId)
            .FirstOrDefaultAsync();

        if (lastOrder is null)
            return null;

        var model = new OnlinePaymentInfoModel
        {
            CreditCardType = _encryptor.DecryptText(lastOrder.CardType),
            CardholderName = _encryptor.DecryptText(lastOrder.CardName),
            CardNumber = _encryptor.DecryptText(lastOrder.CardNumber),
            ExpirationDate = $"{_encryptor.DecryptText(lastOrder.CardExpirationMonth)}/{_encryptor.DecryptText(lastOrder.CardExpirationYear)}",
            CardCode = _encryptor.DecryptText(lastOrder.CardCvv2),
            TestMode = _iyzicoSettings.IsTestMode
        };

        FluentValidation.Results.ValidationResult validation = await _onlinePaymentInfoModelValidator.ValidateAsync(model);
        if (!validation.IsValid)
            return null;

        var request = new ProcessPaymentRequest
        {
            CreditCardType = model.CreditCardType,
            CreditCardName = model.CardholderName,
            CreditCardNumber = model.CardNumber,
            CreditCardExpireMonth = DateTime.Parse(model.ExpirationDate).Month,
            CreditCardExpireYear = DateTime.Parse(model.ExpirationDate).Year,
            CreditCardCvv2 = model.CardCode
        };

        // Required when navigating back to payment selection.
        CheckoutState state = _checkoutStateAccessor.CheckoutState;
        state.PaymentData["CreditCardType"] = request.CreditCardType;
        state.PaymentData["CardholderName"] = request.CreditCardName;
        state.PaymentData["CardNumber"] = request.CreditCardNumber;
        state.PaymentData["ExpireMonth"] = request.CreditCardStartMonth;
        state.PaymentData["ExpireYear"] = request.CreditCardStartYear;
        state.PaymentData["CardCode"] = request.CreditCardCvv2;

        return await Task.FromResult(request);
    }
}
