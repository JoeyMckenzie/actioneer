using System.Collections.Concurrent;
using System.Reflection;
using Actioneer.Core;

namespace Actioneer;

/// <inheritdoc />
public class Dispatcher : IDispatcher
{
    private readonly ConcurrentDictionary<Type, Assembly> _typeAssemblyCache = new();
    private readonly ConcurrentDictionary<Type, Type?> _sideEffectCache = new();

    /// <inheritdoc />
    public void Dispatch(IDispatchable? action)
    {
        ArgumentNullException.ThrowIfNull(action);

        // Check for any side effects associated to the action
        var dispatchedActionType = action.GetType();
        var assembly = _typeAssemblyCache.GetOrAdd(
            dispatchedActionType,
            Assembly.GetAssembly(action.GetType())
                ?? throw new NullReferenceException("No assembly found associated to action type.")
        );

        var types = assembly.GetTypes();
        var associatedSideEffect = _sideEffectCache.GetOrAdd(
            dispatchedActionType,
            types.FirstOrDefault(t =>
                t.GetInterfaces().Any(i => ActionHasCorrespondingSideEffect(i, dispatchedActionType))
            )
        );

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
        var assembly = Assembly.GetAssembly(action.GetType());

        if (assembly is null)
        {
            throw new NullReferenceException($"No assembly was found associated to action type {dispatchedActionType}");
        }

        var types = assembly.GetTypes();
        var associatedSideEffect = types.FirstOrDefault(t =>
            t.GetInterfaces().Any(i => ActionHasCorrespondingSideEffect(i, dispatchedActionType))
        );

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

    private static bool ActionHasCorrespondingSideEffect(Type i, Type dispatchedActionType)
    {
        return i.IsGenericType
            && i.GetGenericTypeDefinition() == typeof(ISideEffect<>)
            && i.GenericTypeArguments.Contains(dispatchedActionType);
    }
}
