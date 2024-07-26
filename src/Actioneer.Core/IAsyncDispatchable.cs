namespace Actioneer.Core;

/// <summary>
/// Represents an action that can be dispatched via the action dispatcher.
/// </summary>
public interface IAsyncDispatchable
{
    /// <summary>
    /// Executes the action logic with payload information.
    /// </summary>
    /// <returns>Typed result information.</returns>
    public Task ExecuteAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Represents an action that can be dispatched via the action dispatcher with a typed result.
/// </summary>
public interface IAsyncDispatchable<TReturn>
{
    /// <summary>
    /// Executes the action logic with payload information.
    /// </summary>
    /// <returns>Typed result information.</returns>
    public Task<TReturn> ExecuteAsync(CancellationToken cancellationToken = default);
}
