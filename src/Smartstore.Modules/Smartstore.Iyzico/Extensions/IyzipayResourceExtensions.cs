using System.Diagnostics;
using System.Runtime.CompilerServices;
using Iyzipay;
using Iyzipay.Model;

namespace Smartstore.Iyzico.Extensions;

public static class IyzipayResourceExtensions
{
    /// <summary>
    /// Checks if the Iyzipay resource represents a successful operation.
    /// </summary>
    /// <param name="resource">The Iyzipay resource to check.</param>
    /// <returns>True if the operation is successful; otherwise, false.</returns>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSuccess(this IyzipayResource resource)
    {
        return resource.Status == Status.SUCCESS.ToString();
    }

    /// <summary>
    /// Checks if the Iyzipay resource represents a valid operation associated with the provided base request.
    /// </summary>
    /// <param name="resource">The Iyzipay resource to check.</param>
    /// <param name="baseRequest">The base request associated with the operation.</param>
    /// <returns>True if the operation is valid; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when either <paramref name="resource"/> or <paramref name="baseRequest"/> is null.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid(this IyzipayResource resource, BaseRequest baseRequest)
    {
        Guard.NotNull(resource);
        Guard.NotNull(baseRequest);

        return resource.IsSuccess() && resource.ConversationId.Equals(baseRequest.ConversationId, StringComparison.OrdinalIgnoreCase);
    }
}
