using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
namespace Slight.Core
{
    public class NavigateHost : ContentControl
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal static readonly ConcurrentDictionary<string, NavigateHost> navigateAwareMapper = new();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private object currentUi;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private FrameworkElement storyboardTarget;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly Stack<UIElement> stack = new();

        static NavigateHost()
        {
            PropertyAssist.DefaultStyle<NavigateHost>(DefaultStyleKeyProperty);
        }

        public NavigateHost()
        {
            Unloaded += NavigateHost_Unloaded;
        }

        private void NavigateHost_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= NavigateHost_Unloaded;
            navigateAwareMapper.TryRemove(HostName, out NavigateHost _);
        }

        public static readonly DependencyProperty HostNameProperty = PropertyAssist.PropertyRegister<NavigateHost, string>(i => i.HostName, null, (s, e) =>
        {
            if (e.OldValue != null)
            {
                navigateAwareMapper.TryRemove(e.OldValue, out NavigateHost _);
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

        public static readonly DependencyProperty CornerRadiusProperty = PropertyAssist.PropertyRegister<NavigateHost, CornerRadius>(i => i.CornerRadius, new CornerRadius(), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits);
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty NavigateStoryboardProperty = PropertyAssist.PropertyRegister<NavigateHost, Storyboard>(i => i.NavigateStoryboard, null);

        public Storyboard NavigateStoryboard
        {
            get => (Storyboard)GetValue(NavigateStoryboardProperty);
            set => SetValue(NavigateStoryboardProperty, value);
        }

        public static NavigateHost FindHost(string hostName)
        {
            return hostName is null
                ? throw new ArgumentNullException(nameof(hostName))
                : navigateAwareMapper.TryGetValue(hostName, out NavigateHost s) ? s : null;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PATH_NavigateContainer") is Border border)
            {
                storyboardTarget = border;
            }
        }

        public void NavigateTo(Func<UIElement> uiCreator)
        {
            if (CheckAccess())
            {
                UIElement ui = uiCreator?.Invoke();

                if (ui is null)
                {
                    return;
                }

                if (currentUi != null && (currentUi.GetHashCode() == ui.GetHashCode()))
                {
                    return;
                }

                if (Content is UIElement iElement)
                {
                    stack.Push(iElement);
                }
                if (storyboardTarget != null)
                {
                    NavigateStoryboard?.Begin(storyboardTarget);
                }
                currentUi = Content = ui;
                return;
            }

            Dispatcher.InvokeAsync(() => NavigateTo(uiCreator));

        }

        public bool NavigateBack()
        {
            if (CheckAccess())
            {
                if (stack.Count > 0)
                {
                    NavigateStoryboard?.Begin(storyboardTarget);
                    Content = stack.Pop();
                    return true;
                }
                return false;
            }
            return Dispatcher.Invoke(() => NavigateBack());
        }

        public static void NavigateTo(string navigateHostName, Func<UIElement> uiContent)
        {
            if (navigateAwareMapper.TryGetValue(navigateHostName, out NavigateHost host))
            {
                host.NavigateTo(uiContent);
                return;
            }
            throw new ArgumentException($"target NavigateHost with the name :{navigateHostName} not exists");

        }

        public static bool NavigateBack(string navigateHostName)
        {
            return navigateAwareMapper.TryGetValue(navigateHostName, out NavigateHost host)
                ? host.NavigateBack()
                : throw new ArgumentException($"target NavigateHost with the name :{navigateHostName} not exists");
        }
    }
}
