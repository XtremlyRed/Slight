using System.Diagnostics;

namespace Slight.Core
{
    public interface INavigationAware
    {
        void NavigateTo(string targetPageName, NavigationParameter navigateParameter = null);

        bool NavigateBack();
    }



    [DebuggerDisplay("{hostName}")]
    internal class NavigationAware : INavigationAware
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly string hostName;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly INavigateManager navigateManager;
        internal NavigationAware(string hostName, INavigateManager navigateManager)
        {
            this.hostName = hostName;
            this.navigateManager = navigateManager;
        }

        public void NavigateTo(string targetPageName, NavigationParameter navigateParameter = null)
        {
            navigateManager.NavigateTo(hostName, targetPageName, navigateParameter);
        }

        public bool NavigateBack()
        {
            return navigateManager.NavigateBack(hostName);
        }
    }
}
