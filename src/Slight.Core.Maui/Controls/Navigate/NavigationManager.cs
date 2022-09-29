namespace Slight.Core
{
    public class NavigationManager : INavigateManager
    {
        public INavigationAware Aware(string navigateHostName)
        {
            if (navigateHostName is null)
            {
                throw new ArgumentNullException(nameof(navigateHostName));
            }

            NavigationAware aware = new(navigateHostName, this);

            return aware;
        }

        public bool NavigateBack(string navigateHostName)
        {
            if (navigateHostName is null)
            {
                throw new ArgumentNullException(nameof(navigateHostName));
            }

            if (NavigationHost.navigateAwareMapper.TryGetValue(navigateHostName, out NavigationHost host) == false)
            {
                throw new ArgumentException($"target NavigateHost with the name :{navigateHostName} not exists");
            }

            return host.NavigateBack();

        }

        // [DebuggerNonUserCode]
        public void NavigateTo(string navigateHostName, string targetPageName, NavigationParameter navigateParameter = null)
        {
            if (navigateHostName is null)
            {
                throw new ArgumentNullException(nameof(navigateHostName));
            }

            if (NavigationHost.navigateAwareMapper.ContainsKey(navigateHostName) == false)
            {
                throw new ArgumentException($"target NavigateHost with the name :{navigateHostName} not exists");
            }

            if (targetPageName is null)
            {
                throw new ArgumentNullException(nameof(targetPageName));
            }
            MauiContainerExtensions.ViewViewModelMapper mapper = MauiContainerExtensions.FindMapper(targetPageName);

            if (mapper is null)
            {
                throw new ArgumentException($"target NavigatePage with the name :{targetPageName} not exists");
            }

            if (mapper.ViewType is null)
            {
                return;
            }

            object dataContext = null;

            if (mapper.ViewModelType != null)
            {
                dataContext = SlightApplication.Provider.Resolve(mapper.ViewModelType);
            }

            NavigationHost.NavigateTo(navigateHostName, () =>
            {
                Page visual = SlightApplication.Provider.Resolve(mapper.ViewType) as Page;

                if (visual is BindableObject binding && dataContext != null)
                {
                    if (binding.BindingContext?.GetHashCode() != dataContext.GetHashCode())
                    {
                        binding.BindingContext = Navigated(() => dataContext, navigateParameter);
                    }
                    else
                    {
                        Navigated(() => dataContext, navigateParameter);
                    }
                }
                else
                {
                    Navigated(() => visual, navigateParameter);
                }

                return visual;
            });

            object Navigated(Func<object> func, NavigationParameter p)
            {
                object obj = func.Invoke();
                if (obj is INavigationParameterAware nav)
                {
                    nav.Navigated(p);
                }
                return obj;
            }
        }
    }

    public interface INavigateManager
    {
        INavigationAware Aware(string navigateHostName);

        /// <summary>
        /// must implement the interface:<see cref="INavigationParameterAware"/> to pass parameters on View or ViewModel
        /// 
        /// </summary>
        /// <param name="navigateHostName">host name</param>
        /// <param name="targetPageName">target name</param>
        /// <param name="navigateParameter">parameter collection</param>
        void NavigateTo(string navigateHostName, string targetPageName, NavigationParameter navigateParameter = null);

        /// <summary>
        ///  navigate back from navigateHostName
        /// </summary>
        /// <param name="navigateHostName"> host name</param>
        /// <returns></returns>
        bool NavigateBack(string navigateHostName);
    }
}
