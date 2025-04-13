using Microsoft.AspNetCore.Mvc.Filters;
using Smartstore.Core;
using Smartstore.Core.Checkout.Orders;
using Smartstore.Iyzico.Configuration;
using Smartstore.Iyzico.Helpers;
using Smartstore.Web.Controllers;

namespace Smartstore.Iyzico.Filters;

public class CheckoutFilter
    (
        IyzicoHelper iyzicoHelper,
        IyzicoSettings iyzicoSettings,
        ICommonServices commonServices,
        ICheckoutStateAccessor _checkoutStateAccessor
    ) : IAsyncResultFilter
{
    private readonly IyzicoHelper _iyzicoHelper = iyzicoHelper;
    private readonly IyzicoSettings _iyzicoSettings = iyzicoSettings;
    private readonly ICommonServices _commonServices = commonServices;
    private readonly ICheckoutStateAccessor checkoutStateAccessor = _checkoutStateAccessor;

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var action = context.RouteData.Values.GetActionName();
        if (!action.EqualsNoCase(nameof(CheckoutController.PaymentMethod)))
        {
            await next();
            return;
        }

        var customer = _commonServices.WorkContext.CurrentCustomer;
        if (!await _iyzicoHelper.IsIyzicoOnlinePaymentActiveAsync())
        {
            await next();
            return;
        }

        if (!_iyzicoSettings.PrivateKey.HasValue() || !_iyzicoSettings.SecretKey.HasValue() ||
            !(_iyzicoSettings.IsTestMode ? _iyzicoSettings.TestUrl : _iyzicoSettings.ProductionUrl).HasValue())
        {
            await next();
            return;
        }

        await next();
    }
}
