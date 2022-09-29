using System.Collections.Concurrent;
using System.Diagnostics;
namespace Slight.Core
{
    public class NavigationHost : NavigationPage
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal static readonly ConcurrentDictionary<string, NavigationHost> navigateAwareMapper = new();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Page currentPage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly Stack<Page> stack = new();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly Thread UiThead = Thread.CurrentThread;

        public NavigationHost()
        {

        }

        public NavigationHost(Page page) : base(page)
        {
            currentPage = page;
        }

        ~NavigationHost()
        {
            navigateAwareMapper.TryRemove(HostName, out NavigationHost _);
        }

        public static readonly BindableProperty HostNameProperty = PropertyAssist.PropertyRegister<NavigationHost, string>(i => i.HostName, null, (s, e) =>
        {
            if (e.OldValue != null)
            {
                bool unused = navigateAwareMapper.TryRemove(e.OldValue, out NavigationHost _);
            }
            if (e.NewValue != null)
            {
                navigateAwareMapper[e.NewValue] = s;
            }
        });

        public string HostName
        {
            get => (string)GetValue(HostNameProperty);
            set => SetValue(HostNameProperty, value);
        }



        public void NavigateTo(Func<Page> uiCreator)
        {
            if (CheckAccess())
            {
                Page navigatePage = uiCreator?.Invoke();

                if (navigatePage is null)
                {
                    return;
                }

                if (currentPage?.GetHashCode() == navigatePage.GetHashCode())
                {
                    return;
                }

                if (CurrentPage != null)
                {
                    stack.Push(currentPage);
                }

                navigatePage.Opacity = 0;
                currentPage = navigatePage;
                Navigation.PushModalAsync(navigatePage, false);
                navigatePage.FadeTo(1, 300, Easing.CubicOut);

                return;
            }

            Dispatcher.Dispatch(() => NavigateTo(uiCreator));

        }

        public bool NavigateBack()
        {
            if (CheckAccess())
            {
                if (stack.Count > 0)
                {
                    currentPage = stack.Pop();
                    currentPage.Opacity = 0;
                    Navigation.PopModalAsync(false);
                    currentPage.FadeTo(1, 300, Easing.CubicOut);

                    return true;
                }
                return false;
            }
            return Dispatcher.Dispatch(() => NavigateBack());
        }

        public static void NavigateTo(string navigateHostName, Func<Page> uiContent)
        {
            if (navigateAwareMapper.TryGetValue(navigateHostName, out NavigationHost host))
            {
                host.NavigateTo(uiContent);
                return;
            }
            throw new ArgumentException($"target NavigateHost with the name :{navigateHostName} not exists");

        }

        public static bool NavigateBack(string navigateHostName)
        {
            return navigateAwareMapper.TryGetValue(navigateHostName, out NavigationHost host)
                ? host.NavigateBack()
                : throw new ArgumentException($"target NavigateHost with the name :{navigateHostName} not exists");
        }



        public bool CheckAccess()
        {
            return UiThead.ManagedThreadId == Thread.CurrentThread.ManagedThreadId;
        }
    }
}
