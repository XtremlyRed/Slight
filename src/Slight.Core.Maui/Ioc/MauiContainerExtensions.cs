

namespace Slight.Core
{
    public static class MauiContainerExtensions
    {
        public static void RegisterForNavigation<TView, TViewModel>(this IContainerRegistry registry, string navigationName = null)
            where TView : Page
            where TViewModel : class
        {
            if (registry is not ContainerRegistry container)
            {
                return;
            }
            ContainerForNavigation dict = container.Container.Resolve<ContainerForNavigation>();

            Type pageType = typeof(TView);
            Type viewModelType = typeof(TViewModel);

            string name = string.IsNullOrWhiteSpace(navigationName) ? pageType.Name : navigationName;

            if (dict.TryGetValue(name, out ViewViewModelMapper mapper) == false)
            {
                dict[name] = mapper = new ViewViewModelMapper();
            }

            if (container.Container.IsRegistered(pageType) == false)
            {
                container.Register<TView>().AsSingleton();
            }
            if (container.Container.IsRegistered(viewModelType) == false)
            {
                container.Register<TViewModel>().AsSingleton();
            }
            mapper.ViewType = pageType;
            mapper.ViewModelType = viewModelType;
        }

        public static void RegisterForNavigation<TView>(this IContainerRegistry registry, string navigationName = null)
              where TView : Page
        {

            if (registry is not ContainerRegistry container)
            {
                return;
            }

            ContainerForNavigation dict = container.Container.Resolve<ContainerForNavigation>();

            Type pageType = typeof(TView);
            string name = string.IsNullOrWhiteSpace(navigationName) ? pageType.Name : navigationName;

            if (dict.TryGetValue(name, out ViewViewModelMapper mapper) == false)
            {
                dict[name] = mapper = new ViewViewModelMapper();
            }

            if (container.Container.IsRegistered(pageType) == false)
            {
                container.Register<TView>().AsSingleton();
            }

            mapper.ViewType = pageType;
        }

        internal static void NavigationExtensionsRegister(this IContainerRegistry container)
        {
            ContainerForNavigation target = new();
            container.RegisterInstance(target);
        }

        internal static ViewViewModelMapper FindMapper(string targetHostName)
        {
            ContainerForNavigation mapper = SlightApplication.Provider.Resolve<ContainerForNavigation>();

            return mapper.TryGetValue(targetHostName, out ViewViewModelMapper mapperType) ? mapperType : null;
        }

        private class ContainerForNavigation
            : Dictionary<string, ViewViewModelMapper>
        {

        }

        internal class ViewViewModelMapper
        {
            public Type ViewType { get; set; }
            public Type ViewModelType { get; set; }
        }
    }
}
