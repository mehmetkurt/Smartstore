using Iyzipay.Request;
using Smartstore.Core;
using Smartstore.Engine;

namespace Smartstore.Iyzico.Models;

public class InstallmentQuery : RetrieveInstallmentInfoRequest
{
    public InstallmentQuery()
    {
        Locale = EngineContext.Current.ResolveService<IWorkContext>().WorkingLanguage.LanguageCulture == "tr-TR"
        ? Iyzipay.Model.Locale.TR.ToString() : Iyzipay.Model.Locale.EN.ToString();

        ConversationId = ConversationId.IsNullOrEmpty() ? Guid.NewGuid().ToString() : ConversationId;
    }
}
