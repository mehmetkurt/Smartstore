using Microsoft.AspNetCore.Mvc;
using Smartstore.ComponentModel;
using Smartstore.Core.Security;
using Smartstore.Iyzico.Configuration;
using Smartstore.Web.Controllers;
using Smartstore.Web.Modelling.Settings;
using Smartstore.Iyzico.Models;

namespace Smartstore.Iyzico.Controllers;

[AuthorizeAdmin]
public class IyzicoAdminController : AdminController
{

    [LoadSetting]
    public IActionResult Configure(IyzicoSettings settings)
    {
        var model = MiniMapper.Map<IyzicoSettings, ConfigurationModel>(settings);
        return View(model);
    }

    [HttpPost, SaveSetting]
    public IActionResult Configure(ConfigurationModel model,  IyzicoSettings settings)
    {
        if (!ModelState.IsValid)
            return Configure(settings);

        ModelState.Clear();
        MiniMapper.Map(model, settings);

        return RedirectToAction(nameof(Configure));
    }
}
