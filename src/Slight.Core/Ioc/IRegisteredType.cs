using System;

namespace Slight.Core
{

    /// <summary>
    /// IRegisteredType is return by Container.Register and allows further configuration for the registration
    /// </summary>
    public interface IRegisteredType
    {
        /// <summary>
        /// Make registered type a singleton
        /// </summary>
        void AsSingleton();

        /// <summary>
        /// Make registered type a per-scope type (single instance within a Scope)
        /// </summary>
        void PerScope();
    }



    #region Public interfaces
    /// <summary>
    /// Represents a scope in which per-scope objects are instantiated a single time
    /// </summary>
    public interface IScopeContainer : IServiceProvider
    {
        object Resolve(Type serviceType);
    }
    #endregion
}
