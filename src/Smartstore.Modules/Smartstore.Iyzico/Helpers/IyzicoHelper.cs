using Smartstore.Core;
using Smartstore.Core.Checkout.Payment;

namespace Smartstore.Iyzico.Helpers;

public class IyzicoHelper(ICommonServices commonServices, IPaymentService paymentService)
{
    private readonly ICommonServices _commonServices = commonServices;
    private readonly IPaymentService _paymentService = paymentService;

    public async Task<bool> IsIyzicoOnlinePaymentActiveAsync()
      => await _paymentService.IsPaymentProviderActiveAsync(IyzicoDefaults.OnlinePayment.SystemName, null, _commonServices.StoreContext.CurrentStore.Id);

    public async Task<bool> IsIyzicoOnlinePaymentEnabledAsync()
        => await _paymentService.IsPaymentProviderEnabledAsync(IyzicoDefaults.OnlinePayment.SystemName, _commonServices.StoreContext.CurrentStore.Id);
}
