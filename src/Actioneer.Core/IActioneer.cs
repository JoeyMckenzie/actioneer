namespace Actioneer.Core;

/// <summary>
/// Primary actioneer orchestrator, responsible for handling and dispatching actions to the library for callers.
/// </summary>
public interface IActioneer
{
    /// <summary>
    /// Dispatches a synchronous fire-and-forget action.
    /// </summary>
    /// <param name="action">Action type.</param>
    void Dispatch(IDispatchable action);

    /// <summary>
    /// Dispatches a synchronous action with an expected result.
    /// </summary>
    /// <param name="action">Action type.</param>
    /// <typeparam name="T">Value type to return.</typeparam>
    /// <returns>Action outcome.</returns>
    T Dispatch<T>(IDispatchable<T> action)
        where T : notnull;

    /// <summary>
    /// Dispatches an asynchronous fire-and-forget action.
    /// </summary>
    /// <param name="action">Action type.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>Awaitable task.</returns>
    Task DispatchAsync(IAsyncDispatchable action, CancellationToken cancellationToken = default);

    /// <summary>
    /// Dispatches an asynchronous action with an expected result.
    /// </summary>
    /// <param name="action">Action type.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <typeparam name="T">Value type to return.</typeparam>
    /// <returns>Awaitable action outcome.</returns>
    Task<T> DispatchAsync<T>(IAsyncDispatchable<T> action, CancellationToken cancellationToken = default)
        where T : notnull;
}
