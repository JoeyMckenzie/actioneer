using System.Collections.Concurrent;
using System.Reflection;
using Actioneer.Core;

namespace Actioneer;

/// <inheritdoc cref="IDispatcher"/>
public class AsyncDispatcher : ActioneerDispatcherBase, IAsyncDispatcher
{
    private readonly ConcurrentDictionary<Type, Assembly> _typeAssemblyCache = new();
    private readonly ConcurrentDictionary<Type, IEnumerable<Type>?> _sideEffectCache = new();
    private readonly ConcurrentDictionary<Type, IEnumerable<Type>?> _asyncSideEffectCache = new();

    /// <inheritdoc />
    public async Task DispatchAsync(IAsyncDispatchable? action, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(action);
        cancellationToken.ThrowIfCancellationRequested();

        var dispatchedActionType = action.GetType();
        var assembly = _typeAssemblyCache.GetOrAdd(
            dispatchedActionType,
            Assembly.GetAssembly(action.GetType())
                ?? throw new NullReferenceException("No assembly found associated to action type.")
        );

        var types = assembly.GetTypes();
        var associatedSideEffects = _sideEffectCache
            .GetOrAdd(
                dispatchedActionType,
                types.Where(t => t.GetInterfaces().Any(i => ActionHasCorrespondingSideEffect(i, dispatchedActionType)))
            )
            ?.ToList();
        var associatedAsyncSideEffect = _asyncSideEffectCache
            .GetOrAdd(
                dispatchedActionType,
                types.Where(t =>
                    t.GetInterfaces().Any(i => ActionHasCorrespondingAsyncSideEffect(i, dispatchedActionType))
                )
            )
            ?.ToList();

        // Invoke the action
        await action.ExecuteAsync(cancellationToken);

        // Run any associated side effects for the action
        if (associatedSideEffects is not null && associatedSideEffects.Count > 0)
        {
            var sideEffects = associatedSideEffects.ToList();
            foreach (var sideEffect in sideEffects)
            {
                // Invoke the side effect, if detected
                var sideEffectInstance = Activator.CreateInstance(sideEffect);
                var runMethod = sideEffectInstance?.GetType().GetMethod(nameof(ISideEffect<IAsyncDispatchable>.Run));
                runMethod?.Invoke(sideEffectInstance, [action]);
            }
        }

        // Run any associated async side effects for the action
        if (associatedAsyncSideEffect is not null && associatedAsyncSideEffect.Count > 0)
        {
            foreach (var asyncSideEffect in associatedAsyncSideEffect)
            {
                // Invoke the side effect, if detected
                var sideEffectInstance = Activator.CreateInstance(asyncSideEffect);
                var runMethod = sideEffectInstance
                    ?.GetType()
                    .GetMethod(nameof(IAsyncSideEffect<IAsyncDispatchable>.RunAsync));
                await Task.Run(
                    () => runMethod?.Invoke(sideEffectInstance, [action, cancellationToken]),
                    cancellationToken
                );
            }
        }
    }

    /// <inheritdoc />
    public async Task<TReturn> DispatchAsync<TReturn>(
        IAsyncDispatchable<TReturn>? action,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        cancellationToken.ThrowIfCancellationRequested();

        var dispatchedActionType = action.GetType();
        var assembly = _typeAssemblyCache.GetOrAdd(
            dispatchedActionType,
            Assembly.GetAssembly(action.GetType())
                ?? throw new NullReferenceException("No assembly found associated to action type.")
        );

        var types = assembly.GetTypes();
        var associatedSideEffects = _sideEffectCache
            .GetOrAdd(
                dispatchedActionType,
                types.Where(t => t.GetInterfaces().Any(i => ActionHasCorrespondingSideEffect(i, dispatchedActionType)))
            )
            ?.ToList();
        var associatedAsyncSideEffect = _asyncSideEffectCache
            .GetOrAdd(
                dispatchedActionType,
                types.Where(t =>
                    t.GetInterfaces().Any(i => ActionHasCorrespondingAsyncSideEffect(i, dispatchedActionType))
                )
            )
            ?.ToList();

        // Invoke the action
        var result = await action.ExecuteAsync(cancellationToken);

        // Run any associated side effects for the action
        if (associatedSideEffects is not null)
        {
            var sideEffects = associatedSideEffects.ToList();
            foreach (var sideEffect in sideEffects)
            {
                // Invoke the side effect, if detected
                var sideEffectInstance = Activator.CreateInstance(sideEffect);
                var runMethod = sideEffectInstance
                    ?.GetType()
                    .GetMethod(nameof(ISideEffect<IAsyncDispatchable<TReturn>>.Run));
                runMethod?.Invoke(sideEffectInstance, [action]);
            }
        }

        // Run any associated async side effects for the action
        if (associatedAsyncSideEffect is not null)
        {
            foreach (var asyncSideEffect in associatedAsyncSideEffect)
            {
                // Invoke the side effect, if detected
                var sideEffectInstance = Activator.CreateInstance(asyncSideEffect);
                var runMethod = sideEffectInstance
                    ?.GetType()
                    .GetMethod(nameof(IAsyncSideEffect<IAsyncDispatchable<TReturn>>.RunAsync));
                await Task.Run(
                    () => runMethod?.Invoke(sideEffectInstance, [action, cancellationToken]),
                    cancellationToken
                );
            }
        }

        return result;
    }
}
