using Actioneer.Core;

namespace Actioneer;

/// <inheritdoc />
public class AsyncDispatcher : IAsyncDispatcher
{
    /// <inheritdoc />
    public Task DispatchAsync(IAsyncDispatchable action, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<TReturn> DispatchAsync<TReturn>(
        IAsyncDispatchable<TReturn> action,
        CancellationToken cancellationToken = default
    )
    {
        throw new NotImplementedException();
    }
}
