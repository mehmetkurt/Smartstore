#nullable enable

namespace Smartstore.Core.Messaging;

public interface IQueuedEmailRateLimiter
{
    int? MaxMailsPerRun { get; }

    int GetAllowedMailCount(int requestedCount);
}
