namespace Actioneer.Core;

/// <summary>
/// A primary action dispatcher for fire-and-forget actions.
/// </summary>
public interface IAsyncDispatcher
{
    /// <summary>
    /// Dispatches a sync fire-and-forget action.
    /// </summary>
    Task DispatchAsync(IAsyncDispatchable action, CancellationToken cancellationToken = default);

    /// <summary>
    /// Dispatches a sync action returned the expected result.
    /// </summary>
    Task<TReturn> DispatchAsync<TReturn>(
        IAsyncDispatchable<TReturn> action,
        CancellationToken cancellationToken = default
    );
}
