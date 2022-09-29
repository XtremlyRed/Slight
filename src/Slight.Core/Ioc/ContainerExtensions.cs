using System;

namespace Slight.Core
{
    /// <summary>
    /// Extension methods for Container
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Registers an implementation type for the specified interface
        /// </summary>
        /// <typeparam name="Target">Interface to register</typeparam>
        /// <param name="container">This container instance</param>
        /// <param name="type">Implementing type</param>
        /// <returns>IRegisteredType object</returns>
        public static IRegisteredType Register<Target>(this Container container, Type type)
        {
            return container.Register(typeof(Target), type);
        }

        /// <summary>
        /// Registers an implementation type for the specified interface
        /// </summary>
        /// <typeparam name="TInterface">Interface to register</typeparam>
        /// <typeparam name="TImplementation">Implementing type</typeparam>
        /// <param name="container">This container instance</param>
        /// <returns>IRegisteredType object</returns>
        public static IRegisteredType Register<TInterface, TImplementation>(this Container container)
            where TImplementation : TInterface
        {
            return container.Register(typeof(TInterface), typeof(TImplementation));
        }

        /// <summary>
        /// Registers a factory function which will be called to resolve the specified interface
        /// </summary>
        /// <typeparam name="Target">Interface to register</typeparam>
        /// <param name="container">This container instance</param>
        /// <param name="factory">Factory method</param>
        /// <returns>IRegisteredType object</returns>
        public static IRegisteredType Register<Target>(this Container container, Func<Target> factory)
        {
            return container.Register(typeof(Target), () => factory());
        }


        /// <summary>
        /// Registers a factory function which will be called to resolve the specified interface
        /// </summary>
        /// <typeparam name="Target">Interface to register</typeparam>
        /// <param name="container">This container instance</param>
        /// <param name="factory">Factory method</param>
        /// <returns>IRegisteredType object</returns>
        public static void RegisterInstance<Target>(this Container container, Target instace)
        {
            container.Register(typeof(Target), () => instace).AsSingleton();
        }

        /// <summary>
        /// Registers a type
        /// </summary>
        /// <param name="container">This container instance</param>
        /// <typeparam name="Target">Type to register</typeparam>
        /// <returns>IRegisteredType object</returns>
        public static IRegisteredType Register<Target>(this Container container)
        {
            return container.Register(typeof(Target), typeof(Target));
        }

        /// <summary>
        /// Returns an implementation of the specified interface
        /// </summary>
        /// <typeparam name="Target">Interface type</typeparam>
        /// <param name="scope">This scope instance</param>
        /// <returns>Object implementing the interface</returns>
        public static Target Resolve<Target>(this IScopeContainer scope)
        {
            return (Target)scope.Resolve(typeof(Target));
        }
    }
}
