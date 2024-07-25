namespace Actioneer.Core;

/// <summary>
/// A primary action dispatcher for fire-and-forget actions.
/// </summary>
public interface IDispatcher
{
    /// <summary>
    /// Dispatches a sync fire-and-forget action.
    /// </summary>
    void Dispatch(IDispatchable? action);

    /// <summary>
    /// Dispatches a sync action returned the expected result.
    /// </summary>
    TReturn Dispatch<TReturn>(IDispatchable<TReturn>? action);
}
