using System;
using System.Collections.Generic;
using System.Windows;

namespace Slight.Core
{
    public static class WpfContainerExtensions
    {
        public static void RegisterForNavigation<TView, TViewModel>(this IContainerRegistry registry, string navigationName = null)
            where TView : UIElement
            where TViewModel : class
        {
            if (registry is not ContainerRegistry container)
            {
                return;
            }
            ContainerForNavigation dict = container.Container.Resolve<ContainerForNavigation>();

            Type viewType = typeof(TView);
            Type viewModelType = typeof(TViewModel);

            string name = string.IsNullOrWhiteSpace(navigationName) ? viewType.Name : navigationName;

            if (dict.TryGetValue(name, out ViewViewModelMapper mapper) == false)
            {
                dict[name] = mapper = new ViewViewModelMapper();
            }

            if (container.Container.IsRegistered(viewType) == false)
            {
                container.Register<TView>().AsSingleton();
            }
            if (container.Container.IsRegistered(viewModelType) == false)
            {
                container.Register<TViewModel>().AsSingleton();
            }
            mapper.ViewType = viewType;
            mapper.ViewModelType = viewModelType;
        }

        public static void RegisterForNavigation<TView>(this IContainerRegistry registry, string navigationName = null)
              where TView : UIElement
        {

            if (registry is not ContainerRegistry container)
            {
                return;
            }

            ContainerForNavigation dict = container.Container.Resolve<ContainerForNavigation>();

            Type viewType = typeof(TView);
            string name = string.IsNullOrWhiteSpace(navigationName) ? viewType.Name : navigationName;

            if (dict.TryGetValue(name, out ViewViewModelMapper mapper) == false)
            {
                dict[name] = mapper = new ViewViewModelMapper();
            }

            if (container.Container.IsRegistered(viewType) == false)
            {
                container.Register<TView>().AsSingleton();
            }

            mapper.ViewType = viewType;
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
