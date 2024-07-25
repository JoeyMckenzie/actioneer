namespace Actioneer.Core;

/// <summary>
/// A side effect contract for running tasks when certain actions are dispatched.
/// </summary>
public interface ISideEffect<in TAction>
    where TAction : IDispatchable
{
    /// <summary>
    /// Runs the side effect associated to the action.
    /// </summary>
    void Run(TAction action);
}
