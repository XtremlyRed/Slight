using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;

namespace Slight.Core
{

    // RegisteredType is supposed to be a short lived object tying an item to its container
    // and allowing users to mark it as a singleton or per-scope item
    internal class RegisteredType : IRegisteredType
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private readonly Type _itemType;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private readonly Action<Func<ILifetime, object>> _registerFactory;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private readonly Func<ILifetime, object> _factory;

        public RegisteredType(Type itemType, Action<Func<ILifetime, object>> registerFactory, Func<ILifetime, object> factory)
        {
            _itemType = itemType;
            _registerFactory = registerFactory;
            _factory = factory;

            registerFactory(_factory);
        }

        public void AsSingleton()
        {
            _registerFactory(lifetime => lifetime.GetServiceAsSingleton(_itemType, _factory));
        }

        public void PerScope()
        {
            _registerFactory(lifetime => lifetime.GetServicePerScope(_itemType, _factory));
        }
    }



    #region Lifetime management
    // ILifetime management adds resolution strategies to an IScope
    internal interface ILifetime : IScopeContainer
    {
        object GetServiceAsSingleton(Type type, Func<ILifetime, object> factory);

        object GetServicePerScope(Type type, Func<ILifetime, object> factory);
    }

    // ObjectCache provides common caching logic for lifetimes
    internal abstract class ObjectCache
    {
        // Instance cache  
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private readonly ConcurrentDictionary<Type, object> _instanceCache = new();

        // Get from cache or create and cache object
        protected object GetCached(Type type, Func<ILifetime, object> factory, ILifetime lifetime)
        {
            return _instanceCache.GetOrAdd(type, _ => factory(lifetime));
        }

        public void Dispose()
        {
            foreach (object obj in _instanceCache.Values)
            {
                (obj as IDisposable)?.Dispose();
            }
        }
    }

    // Container lifetime management
    internal class ContainerLifetime : ObjectCache, ILifetime
    {
        // Retrieves the factory functino from the given type, provided by owning container
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Func<Type, Func<ILifetime, object>> GetFactory { get; private set; }

        public ContainerLifetime(Func<Type, Func<ILifetime, object>> getFactory)
        {
            GetFactory = getFactory;
        }

        public object Resolve(Type type)
        {
            return GetFactory(type)(this);
        }

        // Singletons get cached per container
        public object GetServiceAsSingleton(Type type, Func<ILifetime, object> factory)
        {
            return GetCached(type, factory, this);
        }

        // At container level, per-scope items are equivalent to singletons
        public object GetServicePerScope(Type type, Func<ILifetime, object> factory)
        {
            return GetServiceAsSingleton(type, factory);
        }
        public object GetService(Type serviceType)
        {
            return Resolve(serviceType);
        }
    }

    // Per-scope lifetime management
    internal class ScopeLifetime : ObjectCache, ILifetime
    {
        // Singletons come from parent container's lifetime
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private readonly ContainerLifetime _parentLifetime;

        public ScopeLifetime(ContainerLifetime parentContainer)
        {
            _parentLifetime = parentContainer;
        }

        public object Resolve(Type type)
        {
            return _parentLifetime.GetFactory(type)(this);
        }

        // Singleton resolution is delegated to parent lifetime
        public object GetServiceAsSingleton(Type type, Func<ILifetime, object> factory)
        {
            return _parentLifetime.GetServiceAsSingleton(type, factory);
        }

        // Per-scope objects get cached
        public object GetServicePerScope(Type type, Func<ILifetime, object> factory)
        {
            return GetCached(type, factory, this);
        }

        public object GetService(Type serviceType)
        {
            return Resolve(serviceType);
        }
    }
    #endregion
}
