using System;
using NUnit.Framework;
using Smartstore.Core.Messaging;
using Smartstore.Core.Security;

namespace Smartstore.Core.Tests.Platform.Messaging;

[TestFixture]
public class QueuedEmailRateLimiterTests
{
    [Test]
    public void GetAllowedMailCount_returns_requested_count_when_rate_limit_is_disabled()
    {
        using var limiter = CreateLimiter(sendRateLimit: null, maxPerRun: null);

        Assert.That(limiter.MaxMailsPerRun, Is.Null);
        Assert.That(limiter.GetAllowedMailCount(7), Is.EqualTo(7));
    }

    [Test]
    public void GetAllowedMailCount_returns_partial_count_when_only_partial_quota_is_available()
    {
        using var limiter = CreateLimiter(sendRateLimit: 2, maxPerRun: 5);

        Assert.That(limiter.GetAllowedMailCount(1), Is.EqualTo(1));
        Assert.That(limiter.GetAllowedMailCount(2), Is.EqualTo(1));
        Assert.That(limiter.GetAllowedMailCount(1), Is.EqualTo(0));
    }

    private static QueuedEmailRateLimiter CreateLimiter(int? sendRateLimit, int? maxPerRun)
    {
        var settings = new ResiliencySettings
        {
            QueuedMailMaxPerRun = maxPerRun,
            QueuedMailSendRateLimit = sendRateLimit,
            QueuedMailSendRateWindow = TimeSpan.FromMinutes(1)
        };

        return new QueuedEmailRateLimiter(settings);
    }
}
