using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Smartstore.Engine;
using Smartstore.Engine.Builders;
using Smartstore.Iyzico.Filters;
using Smartstore.Iyzico.Helpers;
using Smartstore.Iyzico.Services;
using Smartstore.Web.Controllers;

namespace Smartstore.Iyzico;

internal class Startup : StarterBase
{
    public override void ConfigureServices(IServiceCollection services, IApplicationContext appContext)
    {

        if (!appContext.IsInstalled)
            return;

        services.AddScoped<IyzicoHelper>();
        services.AddScoped<IIyzicoPaymentService, IyzicoPaymentService>();

        services.Configure<MvcOptions>(o =>
        {
            o.Filters.AddConditional<CheckoutFilter>(context =>
                context.ControllerIs<CheckoutController>() && !context.HttpContext.Request.IsAjax());
        });
        //services.Configure<MvcOptions>(o =>
        //{
        //    o.Filters.AddConditional<OffCanvasShoppingCartFilter>(
        //        context => context.RouteData?.Values?.IsSameRoute("ShoppingCart", nameof(ShoppingCartController.OffCanvasShoppingCart)) ?? false);

        //    o.Filters.AddConditional<CheckoutFilter>(
        //        context => context.ControllerIs<CheckoutController>() && !context.HttpContext.Request.IsAjax());
        //});
    }
}
