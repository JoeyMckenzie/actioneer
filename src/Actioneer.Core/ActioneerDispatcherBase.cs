namespace Actioneer.Core;

/// <summary>
/// A root dispatcher type for corresponding dispatchers to use as a base.
/// </summary>
public abstract class ActioneerDispatcherBase
{
    /// <summary>
    /// Determines if an action has a corresponding effect within the calling assembly.
    /// </summary>
    /// <param name="sideEffectType">Side effect generic type definition.</param>
    /// <param name="dispatchedActionType">Action type definition.</param>
    /// <returns>True, if a side effect is found corresponding to the action.</returns>
    protected static bool ActionHasCorrespondingSideEffect(Type sideEffectType, Type dispatchedActionType)
    {
        return sideEffectType.IsGenericType
            && sideEffectType.GetGenericTypeDefinition() == typeof(ISideEffect<>)
            && sideEffectType.GenericTypeArguments.Contains(dispatchedActionType);
    }

    /// <summary>
    /// Determines if an action has a corresponding effect within the calling assembly.
    /// </summary>
    /// <param name="sideEffectType">Side effect generic type definition.</param>
    /// <param name="dispatchedActionType">Action type definition.</param>
    /// <returns>True, if a side effect is found corresponding to the action.</returns>
    protected static bool ActionHasCorrespondingAsyncSideEffect(Type sideEffectType, Type dispatchedActionType)
    {
        return sideEffectType.IsGenericType
            && sideEffectType.GetGenericTypeDefinition() == typeof(IAsyncSideEffect<>)
            && sideEffectType.GenericTypeArguments.Contains(dispatchedActionType);
    }
}
