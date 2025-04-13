using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Smartstore.ComponentModel;
using Smartstore.Core.Checkout.Cart;
using Smartstore.Iyzico.Configuration;
using Smartstore.Iyzico.Extensions;
using Smartstore.Iyzico.Models;

namespace Smartstore.Iyzico.Services;

public class IyzicoPaymentService : IIyzicoPaymentService
{
    private readonly IyzicoSettings _iyzicoSettings;
    private readonly IShoppingCartService _shoppingCartService;

    public IyzicoPaymentService
    (
        IyzicoSettings iyzicoSettings,
        IShoppingCartService shoppingCartService
    )
    {
        _iyzicoSettings = iyzicoSettings;
        _shoppingCartService = shoppingCartService;
    }

    #region Utilities
    private Options GetOptions()
    {
        return new Options()
        {
            ApiKey = _iyzicoSettings.PrivateKey,
            SecretKey = _iyzicoSettings.SecretKey,
            BaseUrl = _iyzicoSettings.IsTestMode ? _iyzicoSettings.TestUrl : _iyzicoSettings.ProductionUrl
        };
    }

    public ILogger Logger { get; set; } = NullLogger.Instance;
    #endregion

    public async Task<InstallmentResponse> GetInstallments(InstallmentQuery query)
    {
        Guard.NotEmpty(query.BinNumber);
        Guard.NotEmpty(query.ConversationId);
        Guard.NotEmpty(query.Price);   

        var request = MiniMapper.Map<InstallmentQuery, RetrieveInstallmentInfoRequest>(query);
        request.BinNumber = request.BinNumber[0..6];

        var options = GetOptions();
        var response = InstallmentInfo.Retrieve(request, options);

        if (response.IsValid(request))
        {
            var model = MiniMapper.Map<InstallmentDetail, InstallmentResponse>(response.InstallmentDetails.FirstOrDefault());
            model.Success = true;
            return await Task.FromResult(model);
        }
        else
        {
            Logger.Log(response);
        }

        return null;
    }
}
