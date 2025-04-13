using Iyzipay;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Smartstore.Iyzico.Extensions;

public static class ILoggerExtensions
{
    public static void Log(this ILogger logger, IyzipayResource response, string message = null, LogLevel logLevel = LogLevel.Warning)
    {
        var responseError = new
        {
            response.Locale,
            response.ConversationId,
            response.ErrorGroup,
            response.ErrorMessage,
            response.ErrorCode
        };

        logger.Log(logLevel, new Exception(JsonConvert.SerializeObject(responseError)), message ?? response.ErrorMessage, null);
    }
}
