namespace Actioneer.Core;

/// <summary>
/// Represents an action that can be dispatched via the action dispatcher.
/// </summary>
public interface IDispatchable
{
    /// <summary>
    /// Executes the action logic with payload information.
    /// </summary>
    /// <returns>Typed result information.</returns>
    public void Execute();
}

/// <summary>
/// Represents an action that can be dispatched via the action dispatcher with a typed result.
/// </summary>
public interface IDispatchable<out TReturn>
{
    /// <summary>
    /// Executes the action logic with payload information.
    /// </summary>
    /// <returns>Typed result information.</returns>
    public TReturn Execute();
}
