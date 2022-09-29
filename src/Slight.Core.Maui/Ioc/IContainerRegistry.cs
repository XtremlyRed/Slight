


namespace Slight.Core
{
    public interface IContainerRegistry
    {
        IRegisteredType Register<Target>(Type type);

        IRegisteredType Register(Type interfaceType, Type ImplementationType);

        IRegisteredType Register<TInterface, TImplementation>()
            where TImplementation : TInterface;

        IRegisteredType Register<Target>(Func<Target> factory);

        void RegisterInstance<Target>(Target instace);

        IRegisteredType Register<Target>();
    }

    public interface IContainerProvider
    {
        Target Resolve<Target>();

        object Resolve(Type type);
    }
}
