

using System.ComponentModel;
using System.Diagnostics;

[assembly: XmlnsDefinition("http://slight-library.org/", "Slight.Core")]

namespace Slight.Core
{
    public abstract class SlightApplication : Application
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never), EditorBrowsable(EditorBrowsableState.Never)]
        private readonly Thread uiThread = Thread.CurrentThread;

        [DebuggerBrowsable(DebuggerBrowsableState.Never), EditorBrowsable(EditorBrowsableState.Never)]
        private readonly Page mainPage;
        public static new SlightApplication Current { get; private set; }
        public static IContainerRegistry Registry { get; private set; }
        public static IContainerProvider Provider { get; private set; }
        public static INavigationAware Navigation { get; private set; }

        public new Page MainPage { get; set; }


        static SlightApplication()
        {
            ContainerRegistry registry = new();
            Provider = new ContainerProvider(registry);
            Registry = registry;

            registry.NavigationExtensionsRegister();
            registry.Register<INavigateManager, NavigationManager>().AsSingleton();
            registry.Register<IPopupManager, PopupManager>().AsSingleton();
            registry.Register<IMessenger, Messenger>().AsSingleton();
        }



        protected SlightApplication()
        {
            global::Microsoft.Maui.Controls.Xaml.Extensions.LoadFromXaml(this, GetType());

            Current = this;

            OnThemeInitialize();

            ContainerRegistry(Registry);

            mainPage = CreateMainPage(Provider);

            OnInitialized();
        }

        public bool CheckAccess()
        {
            return uiThread.ManagedThreadId == Thread.CurrentThread.ManagedThreadId;
        }


        /// <summary>
        /// ContainerRegistry
        /// </summary>
        /// <param name="registry"></param>
        protected abstract void ContainerRegistry(IContainerRegistry registry);

        /// <summary>
        /// CreateMainPage
        /// </summary>
        /// <param name="provider"></param>
        /// <returns><see cref="Page"/></returns>
        protected abstract Page CreateMainPage(IContainerProvider provider);

        /// <summary> 
        /// </summary>
        protected virtual void OnInitialized()
        {
            if (mainPage != null)
            {
                string defaultHostMame = "#Default_NavigationHost";
                base.MainPage = new NavigationHost(MainPage = mainPage)
                {
                    HostName = defaultHostMame
                };


                Navigation = Provider.Resolve<INavigateManager>().Aware(defaultHostMame);
            }
        }

        protected virtual void OnThemeInitialize()
        {

        }
    }
}
