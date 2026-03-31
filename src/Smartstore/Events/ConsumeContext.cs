#nullable enable

using Microsoft.AspNetCore.Http;

namespace Smartstore.Events;

/// <summary>
/// Read-only covariant view of a <see cref="ConsumeContext{TMessage}"/>.
/// Declare consumer parameters as <c>IConsumeContext&lt;TBase&gt;</c> instead of
/// <c>ConsumeContext&lt;TBase&gt;</c> to receive events of any derived message type.
/// </summary>
/// <typeparam name="TMessage">Type of message (covariant).</typeparam>
public interface IConsumeContext<out TMessage> where TMessage : IEventMessage
{
    TMessage Message { get; }
    Endpoint? Endpoint { get; }
    string? RawUrl { get; }
    string? Scheme { get; }
    HostString Host { get; }
    PathString PathBase { get; }
    PathString Path { get; }
    QueryString QueryString { get; }
}

/// <summary>
/// Wrapper/Envelope for event message objects. For now, only wraps
/// the message. There may be other context data in future.
/// </summary>
/// <typeparam name="TMessage">Type of message.</typeparam>
public class ConsumeContext<TMessage> : IConsumeContext<TMessage>, IEventMessage
    where TMessage : IEventMessage
{
    public ConsumeContext(TMessage message)
    {
        Message = Guard.NotNull(message);
    }

    public TMessage Message { get; }

    public Endpoint? Endpoint { get; internal set; }
    public string? RawUrl { get; internal set; }

    public string? Scheme { get; internal set; }
    public HostString Host { get; internal set; }
    public PathString PathBase { get; internal set; }
    public PathString Path { get; internal set; }
    public QueryString QueryString { get; internal set; }
}
