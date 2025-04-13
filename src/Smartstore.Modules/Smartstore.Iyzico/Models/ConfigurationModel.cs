using Smartstore.Web.Modelling;

namespace Smartstore.Iyzico.Models;

[LocalizedDisplay("Plugins.Smartstore.Iyzico.")]
public class ConfigurationModel : ModelBase
{
    [LocalizedDisplay("*PrivateKey")]
    public string PrivateKey { get; set; }

    [LocalizedDisplay("*SecretKey")]
    public string SecretKey { get; set; }

    [LocalizedDisplay("*TestUrl")]
    public string TestUrl { get; set; }

    [LocalizedDisplay("*ProductionUrl")]
    public string ProductionUrl { get; set; }

    [LocalizedDisplay("*IsTestMode")]
    public bool IsTestMode { get; set; }
}
