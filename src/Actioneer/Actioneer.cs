using Actioneer.Core;

namespace Actioneer;

/// <inheritdoc />
public class Actioneer : IActioneer
{
    private readonly Lazy<IDispatcher> _lazyDispatcherInstance = new(() => new Dispatcher());
    private readonly Lazy<IAsyncDispatcher> _lazyAsyncDispatcherInstance = new(() => new AsyncDispatcher());

    /// <inheritdoc />
    public void Dispatch(IDispatchable action)
    {
        _lazyDispatcherInstance.Value.Dispatch(action);
    }

    /// <inheritdoc />
    public T Dispatch<T>(IDispatchable<T> action)
        where T : notnull
    {
        return _lazyDispatcherInstance.Value.Dispatch(action);
    }

    /// <inheritdoc />
    public Task DispatchAsync(IAsyncDispatchable action, CancellationToken cancellationToken = default)
    {
        return _lazyAsyncDispatcherInstance.Value.DispatchAsync(action, cancellationToken);
    }

    /// <inheritdoc />
    public Task<T> DispatchAsync<T>(IAsyncDispatchable<T> action, CancellationToken cancellationToken = default)
        where T : notnull
    {
        return _lazyAsyncDispatcherInstance.Value.DispatchAsync(action, cancellationToken);
    }
}
