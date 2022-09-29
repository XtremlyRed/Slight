using System;
using System.Diagnostics;
using System.Windows;

namespace Slight.Core
{
    public class NavigateManager : INavigateManager
    {
        public INavigateAware Aware(string navigateHostName)
        {
            if (navigateHostName is null)
            {
                throw new ArgumentNullException(nameof(navigateHostName));
            }

            NavigateAware aware = new(navigateHostName, this);

            return aware;
        }

        public bool NavigateBack(string navigateHostName)
        {
            return navigateHostName is null
                ? throw new ArgumentNullException(nameof(navigateHostName))
                : NavigateHost.FindHost(navigateHostName) is null
                ? throw new ArgumentException($"target NavigateHost with the name :{navigateHostName} not exists")
                : NavigateHost.NavigateBack(navigateHostName);
        }

        [DebuggerNonUserCode]
        public void NavigateTo(string navigateHostName, string targetPageName, NavigateParameter navigateParameter = null)
        {
            if (navigateHostName is null)
            {
                throw new ArgumentNullException(nameof(navigateHostName));
            }

            if (NavigateHost.FindHost(navigateHostName) is null)
            {
                throw new ArgumentException($"target NavigateHost with the name :{navigateHostName} not exists");
            }

            if (targetPageName is null)
            {
                throw new ArgumentNullException(nameof(targetPageName));
            }
            WpfContainerExtensions.ViewViewModelMapper mapper = WpfContainerExtensions.FindMapper(targetPageName);

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

            NavigateHost.NavigateTo(navigateHostName, () =>
            {
                UIElement ui = SlightApplication.Provider.Resolve(mapper.ViewType) as UIElement;

                if (ui is FrameworkElement fe)
                {
                    if (fe.DataContext is null && dataContext != null)
                    {
                        fe.DataContext = Navigated(() => dataContext, navigateParameter);
                    }
                    else
                    {
                        Navigated(() => fe.DataContext, navigateParameter);
                    }
                }

                Navigated(() => ui, navigateParameter);
                return ui;
            });

            object Navigated(Func<object> func, NavigateParameter p)
            {
                object obj = func?.Invoke();
                if (obj is INavigateViewModel nav)
                {
                    nav.Navigated(p);
                }
                return obj;
            }
        }
    }

    public interface INavigateManager
    {
        INavigateAware Aware(string navigateHostName);

        /// <summary>
        /// must implement the interface:<see cref="INavigateViewModel"/> to pass parameters on View or ViewModel
        /// 
        /// </summary>
        /// <param name="navigateHostName">host name</param>
        /// <param name="targetPageName">target name</param>
        /// <param name="navigateParameter">parameter collection</param>
        void NavigateTo(string navigateHostName, string targetPageName, NavigateParameter navigateParameter = null);

        /// <summary>
        ///  navigate back from navigateHostName
        /// </summary>
        /// <param name="navigateHostName"> host name</param>
        /// <returns></returns>
        bool NavigateBack(string navigateHostName);
    }

    public interface INavigateAware
    {
        void NavigateTo(string targetPageName, NavigateParameter navigateParameter = null);

        bool NavigateBack();
    }

    [DebuggerDisplay("{hostName}")]
    internal class NavigateAware : INavigateAware
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly string hostName;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly INavigateManager navigateManager;
        internal NavigateAware(string hostName, INavigateManager navigateManager)
        {
            this.hostName = hostName;
            this.navigateManager = navigateManager;
        }

        public void NavigateTo(string targetPageName, NavigateParameter navigateParameter = null)
        {
            navigateManager.NavigateTo(hostName, targetPageName, navigateParameter);
        }

        public bool NavigateBack()
        {
            return navigateManager.NavigateBack(hostName);
        }
    }

    public interface INavigateViewModel
    {
        public void Navigated(NavigateParameter parameter);
    }
}
