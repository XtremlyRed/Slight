using System;
using System.ComponentModel;
using System.Diagnostics; 
namespace Slight.Core
{
    internal class ContainerRegistry : IContainerRegistry
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal Container Container = new();


        public IRegisteredType Register<Target>(Type type)
        {
            return Container.Register<Target>(type);
        }

        public IRegisteredType Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            return Container.Register<TInterface, TImplementation>();
        }

        public IRegisteredType Register<Target>(Func<Target> factory)
        {
            return Container.Register<Target>(factory);
        }

        public IRegisteredType Register<Target>()
        {
            return Container.Register<Target>();
        }

        public IRegisteredType Register(Type interfaceType, Type ImplementationType)
        {
            return Container.Register(interfaceType, ImplementationType);
        }

        public void RegisterInstance<Target>(Target instace)
        {
            Container.RegisterInstance<Target>(instace);
        }
    }


    internal class ContainerProvider : IContainerProvider
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal Container Container;

        internal ContainerProvider(ContainerRegistry containerRegistry)
        {
            Container = containerRegistry.Container;
        }

        public Target Resolve<Target>()
        {
            if (Container.GetService(typeof(Target)) is Target target)
            {
                return target;
            }

            return default;
        }

        public object Resolve(Type type)
        {
            return Container.GetService(type);
        }
    }
}
