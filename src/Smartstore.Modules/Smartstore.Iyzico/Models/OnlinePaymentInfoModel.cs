using FluentValidation;
using Smartstore.Core.Localization;
using Smartstore.Web.Modelling;

namespace Smartstore.Iyzico.Models;

public abstract class PaymentInfoModelBase : ModelBase
{
}

[LocalizedDisplay("Plugins.Payments.Iyzico.OnlinePayment.")]
public class OnlinePaymentInfoModel : PaymentInfoModelBase
{
    public bool TestMode { get; set; }
    public string CreditCardType { get; set; }

    [LocalizedDisplay("*CardholderName")]
    public string CardholderName { get; set; }

    [LocalizedDisplay("*CardNumber")]
    public string CardNumber { get; set; }

    [LocalizedDisplay("*ExpirationDate")]
    public string ExpirationDate { get; set; }

    [LocalizedDisplay("*CardCode")]
    public string CardCode { get; set; }

    public int Installment { get; set; } = 1;
}


public class OnlinePaymentInfoValidator : SmartValidator<OnlinePaymentInfoModel>
{
    public OnlinePaymentInfoValidator(Localizer T)
    {
        RuleFor(x => x.CardholderName)
            .NotEmpty()
            .WithMessage(T("Plugins.Payments.Iyzico.OnlinePayment.CardholderName.NotEmpty"));

        RuleFor(x => x.CardNumber)
            .NotEmpty()
            .WithMessage(T("Plugins.Payments.Iyzico.OnlinePayment.CardNumber.NotEmpty"))
            .CreditCard()
            .WithMessage(T("Plugins.Payments.Iyzico.OnlinePayment.CardNumber.Wrong"));

        RuleFor(card => card.CardCode)
            .NotEmpty()
            .WithMessage(T("Plugins.Payments.Iyzico.OnlinePayment.CardCode.NotEmpty"))
            .CreditCardCvvNumber()
            .WithMessage(T("Plugins.Payments.Iyzico.OnlinePayment.CardCode.Wrong"));

        RuleFor(card => card.Installment)
            .GreaterThanOrEqualTo(1)
            .WithMessage(T("Plugins.Payments.Iyzico.OnlinePayment.Installment.GreaterThanOrEqualToZero"));

        RuleFor(x => x.ExpirationDate)
            .NotEmpty()
            .WithMessage(T("Plugins.Payments.Iyzico.OnlinePayment.ExpirationDate.NotEmpty"));
    }
}