using Microsoft.AspNetCore.Mvc;
using Smartstore.Core.Checkout.Cart;
using Smartstore.Core.Checkout.Orders;
using Smartstore.Core.Common;
using Smartstore.Iyzico.Models;
using Smartstore.Iyzico.Services;
using Smartstore.Web.Controllers;

namespace Smartstore.Iyzico.Controllers;

public class IyzicoController : PublicController
{
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IIyzicoPaymentService _iyzicoPaymentService;
    private readonly IOrderCalculationService _orderCalculationService;

    public IyzicoController
    (
        IShoppingCartService shoppingCartService,
        IIyzicoPaymentService iyzicoPaymentService,
        IOrderCalculationService orderCalculationService
    )
    {
        _shoppingCartService = shoppingCartService;
        _iyzicoPaymentService = iyzicoPaymentService;
        this._orderCalculationService = orderCalculationService;
    }

    [HttpPost]
    public async Task<IActionResult> GetInstallments(InstallmentQuery query)
    {
        var result = default(InstallmentResponse);
        var message = string.Empty;
        var messageType = string.Empty;

        try
        {
            var store = Services.StoreContext.CurrentStore;
            var customer = Services.WorkContext.CurrentCustomer;
            var currentScheme = Services.WebHelper.IsCurrentConnectionSecured() ? "https" : "http";
            var cart = await _shoppingCartService.GetCartAsync(customer, ShoppingCartType.ShoppingCart, store.Id);

            var warnings = new List<string>();
            var cartTotal = (Money?)await _orderCalculationService.GetShoppingCartTotalAsync(cart);
            if (cartTotal.Value.Amount > 0)
            {
                query.Price = cartTotal.Value.Amount.ToString();
                result = await _iyzicoPaymentService.GetInstallments(query);
            }
        }
        catch (Exception ex)
        {
            message = ex.Message;
            messageType = "error";
        }

        return Json(new
        {
            success = result?.Success,
            data = result,
            message,
            messageType
        });
    }
}
