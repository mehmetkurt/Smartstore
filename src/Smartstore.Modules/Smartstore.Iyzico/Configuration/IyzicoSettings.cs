using Smartstore.Core.Configuration;

namespace Smartstore.Iyzico.Configuration;

public class IyzicoSettings : ISettings
{
    public string PrivateKey { get; set; }
    public string SecretKey { get; set; }
    public string TestUrl { get; set; } = "https://sandbox-api.iyzipay.com";
    public string ProductionUrl { get; set; } = "https://api.iyzipay.com";
    public bool IsTestMode { get; set; } = true;
}
