using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Slight.Core
{
    /// <summary>
    /// Inversion of control container handles dependency injection for registered types
    /// </summary>
    public class Container : IScopeContainer, IDisposable
    {
        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //private static Container @default;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private bool isDispose;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private static readonly object defaultSyncRoot = new();

        // Map of registered types 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private IDictionary<Type, Func<ILifetime, object>> _registeredTypes = new ConcurrentDictionary<Type, Func<ILifetime, object>>();

        // Lifetime management 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private ContainerLifetime _lifetime;

        ///// <summary>
        ///// default container
        ///// </summary>
        //public static Container Default
        //{
        //    get
        //    {
        //        if (@default is null)
        //        {
        //            lock (defaultSyncRoot)
        //            {
        //                @default ??= new Container();
        //            }
        //        }

        //        return @default;
        //    }
        //}

        /// <summary>
        /// Creates a new instance of IoC Container
        /// </summary>
        public Container()
        {
            _lifetime = new ContainerLifetime(t => _registeredTypes[t]);
        }

        public bool IsRegistered(Type type)
        {
            return isDispose ? throw new ObjectDisposedException(nameof(Container)) : _registeredTypes.ContainsKey(type);
        }

        /// <summary>
        /// Registers a factory function which will be called to resolve the specified interface
        /// </summary>
        /// <param name="interface">Interface to register</param>
        /// <param name="factory">Factory function</param>
        /// <returns></returns>
        public IRegisteredType Register(Type @interface, Func<object> factory)
        {
            return isDispose ? throw new ObjectDisposedException(nameof(Container)) : RegisterType(@interface, _ => factory());
        }

        /// <summary>
        /// Registers an implementation type for the specified interface
        /// </summary>
        /// <param name="interface">Interface to register</param>
        /// <param name="implementation">Implementing type</param>
        /// <returns></returns>
        public IRegisteredType Register(Type @interface, Type implementation)
        {
            return isDispose
                ? throw new ObjectDisposedException(nameof(Container))
                : RegisterType(@interface, ObjectBuilder.FactoryFromType(implementation));
        }

        private IRegisteredType RegisterType(Type itemType, Func<ILifetime, object> factory)
        {
            return isDispose
                ? throw new ObjectDisposedException(nameof(Container))
                : (IRegisteredType)new RegisteredType(itemType, f => _registeredTypes[itemType] = f, factory);
        }

        /// <summary>
        /// Returns the object registered for the given type, if registered
        /// </summary>
        /// <param name="type">Type as registered with the container</param>
        /// <returns>Instance of the registered type, if registered; otherwise <see langword="null"/></returns>
        public object Resolve(Type serviceType)
        {
            return GetService(serviceType);
        }

        /// <summary>
        /// Creates a new scope
        /// </summary>
        /// <returns>Scope object</returns>
        public IScopeContainer CreateScope()
        {
            return isDispose ? throw new ObjectDisposedException(nameof(Container)) : (IScopeContainer)new ScopeLifetime(_lifetime);
        }

        /// <summary>
        /// Disposes any <see cref="IDisposable"/> objects owned by this container.
        /// </summary>
        public void Dispose()
        {
            isDispose = true;

            _registeredTypes?.Clear();
            _registeredTypes = null;

            _lifetime?.Dispose();
            _lifetime = null;
        }

        public object GetService(Type serviceType)
        {
            if (isDispose)
            {
                throw new ObjectDisposedException(nameof(Container));
            }

            if (serviceType is null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            bool result = _registeredTypes.TryGetValue(serviceType, out Func<ILifetime, object> registeredType);

            return result ? registeredType(_lifetime) : null;
        }
    }
}
