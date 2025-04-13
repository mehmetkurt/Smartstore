using Microsoft.AspNetCore.Mvc;
using Smartstore.Core.Checkout.Orders;
using Smartstore.Iyzico.Configuration;
using Smartstore.Iyzico.Models;
using Smartstore.Web.Components;

namespace Smartstore.Iyzico.Components;

public class OnlinePaymentViewComponent : SmartViewComponent
{
    private readonly IyzicoSettings _iyzicoSettings;
    private readonly ICheckoutStateAccessor _checkoutStateAccessor;

    public OnlinePaymentViewComponent
    (
        IyzicoSettings iyzicoSettings,
        ICheckoutStateAccessor checkoutStateAccessor
    )
    {
        _iyzicoSettings = iyzicoSettings;
        _checkoutStateAccessor = checkoutStateAccessor;
    }

    public IViewComponentResult Invoke()
    {
        var model = new OnlinePaymentInfoModel();
        return View(model);
    }
}
