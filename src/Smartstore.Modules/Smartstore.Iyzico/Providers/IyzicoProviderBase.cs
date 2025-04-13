using Smartstore.Core.Checkout.Payment;
using Smartstore.Core.Widgets;

namespace Smartstore.Iyzico.Providers;

public abstract class IyzicoPaymentProviderBase : PaymentMethodBase
{
    public override bool RequiresPaymentSelection => false;
    public override PaymentMethodType PaymentMethodType => PaymentMethodType.Standard;

    protected virtual Type GetViewComponentType()
          => null;

    public override Widget GetPaymentInfoWidget()
    {
        var type = GetViewComponentType();
        return type != null ? new ComponentWidget(type) : null;
    }

    public override Task<ProcessPaymentResult> ProcessPaymentAsync(ProcessPaymentRequest processPaymentRequest)
    {
        var result = new ProcessPaymentResult
        {
            NewPaymentStatus = PaymentStatus.Pending
        };

        return Task.FromResult(result);
    }
}
