using System.Reflection;
using Actioneer.Core;

namespace Actioneer;

/// <inheritdoc cref="IDispatcher"/>
public class Dispatcher : ActioneerDispatcherBase, IDispatcher
{
    private readonly Dictionary<Type, Assembly> _typeAssemblyCache = new();
    private readonly Dictionary<Type, Type?> _sideEffectCache = new();

    /// <inheritdoc />
    public void Dispatch(IDispatchable? action)
    {
        ArgumentNullException.ThrowIfNull(action);

        // Check for any side effects associated to the action
        var dispatchedActionType = action.GetType();

        if (!_typeAssemblyCache.TryGetValue(dispatchedActionType, out var assembly))
        {
            assembly =
                Assembly.GetAssembly(action.GetType())
                ?? throw new NullReferenceException("No assembly found associated to action type.");
            _typeAssemblyCache[dispatchedActionType] = assembly;
        }

        var types = assembly.GetTypes();

        if (!_sideEffectCache.TryGetValue(dispatchedActionType, out var associatedSideEffect))
        {
            associatedSideEffect = types.FirstOrDefault(t =>
                t.GetInterfaces().Any(i => ActionHasCorrespondingSideEffect(i, dispatchedActionType))
            );
            _sideEffectCache[dispatchedActionType] = associatedSideEffect;
        }

        // Invoke the action
        action.Execute();

        if (associatedSideEffect is null)
        {
            return;
        }

        // Invoke the side effect, if detected
        var sideEffectInstance = Activator.CreateInstance(associatedSideEffect);
        var runMethod = sideEffectInstance?.GetType().GetMethod("Run");
        runMethod?.Invoke(sideEffectInstance, [action]);
    }

    /// <inheritdoc />
    public TReturn Dispatch<TReturn>(IDispatchable<TReturn>? action)
    {
        ArgumentNullException.ThrowIfNull(action);

        // Check for any side effects associated to the action
        var dispatchedActionType = action.GetType();

        if (!_typeAssemblyCache.TryGetValue(dispatchedActionType, out var assembly))
        {
            assembly =
                Assembly.GetAssembly(action.GetType())
                ?? throw new NullReferenceException("No assembly found associated to action type.");
            _typeAssemblyCache[dispatchedActionType] = assembly;
        }

        var types = assembly.GetTypes();

        if (!_sideEffectCache.TryGetValue(dispatchedActionType, out var associatedSideEffect))
        {
            associatedSideEffect = types.FirstOrDefault(t =>
                t.GetInterfaces().Any(i => ActionHasCorrespondingSideEffect(i, dispatchedActionType))
            );
            _sideEffectCache[dispatchedActionType] = associatedSideEffect;
        }

        // Invoke the action
        var result = action.Execute();
        if (associatedSideEffect is null)
        {
            return result;
        }

        // Invoke the side effect, if detected
        var sideEffectInstance = Activator.CreateInstance(associatedSideEffect);
        var runMethod = sideEffectInstance?.GetType().GetMethod("Run");
        runMethod?.Invoke(sideEffectInstance, [action]);

        return result;
    }
}
