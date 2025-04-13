using Smartstore.Engine.Modularity;
using Smartstore.Http;
using Smartstore.Iyzico.Configuration;

namespace Smartstore.Iyzico;
internal class Module : ModuleBase, IConfigurable
{
    public RouteInfo GetConfigurationRoute() => new("Configure", "IyzicoAdmin", new { area = "Admin" });

    public override async Task InstallAsync(ModuleInstallationContext context)
    {
        await ImportLanguageResourcesAsync();
        await TrySaveSettingsAsync<IyzicoSettings>();
        await base.InstallAsync(context);
    }

    public override async Task UninstallAsync()
    {
        await DeleteLanguageResourcesAsync();
        await DeleteSettingsAsync<IyzicoSettings>();
        await base.UninstallAsync();
    }
}
