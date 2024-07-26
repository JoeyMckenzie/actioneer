namespace Actioneer.Core;

/// <summary>
/// A side effect contract for running tasks when certain actions are dispatched.
/// </summary>
public interface IAsyncSideEffect<in TAction>
{
    /// <summary>
    /// Runs the side effect associated to the action.
    /// </summary>
    Task RunAsync(TAction action, CancellationToken cancellationToken = default);
}
