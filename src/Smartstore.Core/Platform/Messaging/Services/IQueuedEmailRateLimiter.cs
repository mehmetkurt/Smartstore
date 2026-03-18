#nullable enable

namespace Smartstore.Core.Messaging;

public interface IQueuedEmailRateLimiter
{
    int GetAllowedMailCount(int requestedCount);
}
