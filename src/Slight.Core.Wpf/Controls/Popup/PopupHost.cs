using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;


namespace Slight.Core
{
    internal enum PopupMode { Show, Confirm, Popup }

    [ContentProperty("Content")]
    [DefaultProperty("Content")]
    [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
    public sealed class PopupHost : ContentControl, IPopupAware
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal static readonly ConcurrentDictionary<string, PopupHostStatus> popupAwareMapper = new();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly ConcurrentQueue<PopupInfo> PopupInfos = new();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private FrameworkElement storyboardTarget;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private PopupHostStatus currentStatus;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private bool isPopuped;


        public static readonly DependencyProperty HostNameProperty = PropertyAssist.PropertyRegister<PopupHost, string>(i => i.HostName, null, (s, e) =>
        {
            if (e.OldValue != null)
            {
                popupAwareMapper.TryRemove(e.OldValue, out PopupHostStatus _);
            }

            if (e.NewValue != null)
            {
                popupAwareMapper[e.NewValue] = s.currentStatus = new PopupHostStatus()
                {
                    PopupHost = s,
                    PopupStatus = PopupStatus.Registed,
                };
            }
        });

        public string HostName
        {
            get => (string)GetValue(HostNameProperty);
            set => SetValue(HostNameProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty = PropertyAssist.PropertyRegister<PopupHost, CornerRadius>(i => i.CornerRadius, new CornerRadius(), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits);
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty PopupStoryboardProperty = PropertyAssist.PropertyRegister<PopupHost, Storyboard>(i => i.PopupStoryboard, null);
        public Storyboard PopupStoryboard
        {
            get => (Storyboard)GetValue(PopupStoryboardProperty);
            set => SetValue(PopupStoryboardProperty, value);
        }

        public static readonly DependencyProperty PopupMessageContainerProperty = PropertyAssist.PropertyRegister<PopupHost, IPopupMessageContainer>(i => i.PopupMessageContainer, null, FrameworkPropertyMetadataOptions.AffectsRender);
        public IPopupMessageContainer PopupMessageContainer
        {
            get => (IPopupMessageContainer)GetValue(PopupMessageContainerProperty);
            set => SetValue(PopupMessageContainerProperty, value);
        }

        public static readonly DependencyProperty PopupUIElementContainerProperty = PropertyAssist.PropertyRegister<PopupHost, IPopupContent>(i => i.PopupUIElementContainer, null, FrameworkPropertyMetadataOptions.AffectsRender);
        public IPopupContent PopupUIElementContainer
        {
            get => (IPopupContent)GetValue(PopupUIElementContainerProperty);
            set => SetValue(PopupUIElementContainerProperty, value);
        }

        public static readonly DependencyProperty MaskBrushProperty = PropertyAssist.PropertyRegister<PopupHost, Brush>(i => i.MaskBrush, Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender);
        public Brush MaskBrush
        {
            get => (Brush)GetValue(MaskBrushProperty);
            set => SetValue(MaskBrushProperty, value);
        }

        internal static readonly DependencyProperty PopupContentProperty = PropertyAssist.PropertyRegister<PopupHost, object>(i => i.PopupContent, null, FrameworkPropertyMetadataOptions.AffectsRender);
        internal object PopupContent
        {
            get => GetValue(PopupContentProperty);
            set => SetValue(PopupContentProperty, value);
        }

        internal static readonly DependencyProperty PopupVisibilityProperty = PropertyAssist.PropertyRegister<PopupHost, Visibility>(i => i.PopupVisibility, Visibility.Collapsed, FrameworkPropertyMetadataOptions.AffectsRender);
        internal Visibility PopupVisibility
        {
            get => (Visibility)GetValue(PopupVisibilityProperty);
            set => SetValue(PopupVisibilityProperty, value);
        }


        public static readonly DependencyProperty PopupContentVerticalAlignmentProperty = PropertyAssist.PropertyRegister<PopupHost, VerticalAlignment>(i => i.PopupContentVerticalAlignment, VerticalAlignment.Center, FrameworkPropertyMetadataOptions.AffectsRender);
        public VerticalAlignment PopupContentVerticalAlignment
        {
            get => (VerticalAlignment)GetValue(PopupContentVerticalAlignmentProperty);
            set => SetValue(PopupContentVerticalAlignmentProperty, value);
        }

        public static readonly DependencyProperty PopupContentHorizontalAlignmentProperty = PropertyAssist.PropertyRegister<PopupHost, HorizontalAlignment>(i => i.PopupContentHorizontalAlignment, HorizontalAlignment.Center, FrameworkPropertyMetadataOptions.AffectsRender);
        public HorizontalAlignment PopupContentHorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(PopupContentHorizontalAlignmentProperty);
            set => SetValue(PopupContentHorizontalAlignmentProperty, value);
        }

        static PopupHost()
        {
            PropertyAssist.DefaultStyle<PopupHost>(DefaultStyleKeyProperty);
        }

        public PopupHost()
        {
            Unloaded += (s, e) => currentStatus.PopupStatus = PopupStatus.Unshown;
            Loaded += (s, e) => currentStatus.PopupStatus = PopupStatus.Showing;
        }



        private void PopupWindow(PopupInfo popupInfo)
        {
            lock (PopupInfos)
            {
                PopupInfos.Enqueue(popupInfo);

                if (isPopuped)
                {
                    return;
                }

                isPopuped = true;
            }

            Dispatcher.InvokeAsync(async () =>
            {
                PopupVisibility = Visibility.Visible;

                PopupStoryboard.Begin(storyboardTarget);

                while (PopupInfos.TryDequeue(out PopupInfo info))
                {

                    switch (info.PopupMode)
                    {
                        case PopupMode.Popup:

                            IPopupContent currentVisual = info.UIElementFunc.Invoke();

                            Init(info, currentVisual);

                            PopupContent = currentVisual;

                            currentVisual.RequestClose += RequestClose;

                            break;

                            void RequestClose(object sender, PopupResultEventArgs popupResult)
                            {
                                currentVisual.RequestClose -= RequestClose;
                                info.Close(popupResult.PopupResult);
                            }

                        case PopupMode.Show:
                        case PopupMode.Confirm:
                        default:

                            Navigation(PopupMode.Show, info);

                            IPopupMessageContainer messageView = PopupMessageContainer;

                            messageView.RequestClose += RequestClose2;

                            messageView.Title = info.PopupConfig?.Title ?? PopupConfig.DefaultTitle;

                            messageView.Message = info.Message;

                            break;

                            void RequestClose2(object sender, PopupResultEventArgs popupResult)
                            {
                                messageView.RequestClose -= RequestClose2;
                                info.Close(popupResult.PopupResult);
                            }
                    }

                    await info.DisplayAsync();

                }

                PopupContent = null;
                //此处需要关闭弹窗 等待下次请求
                PopupVisibility = Visibility.Collapsed;

                isPopuped = false;

            });

        }

        private void Navigation(PopupMode popupMode, PopupInfo info)
        {
            switch (popupMode)
            {
                case PopupMode.Popup:

                    break;
                case PopupMode.Show:
                case PopupMode.Confirm:
                default:

                    PopupMessageContainer ??= new DefaultPopupMessageView();
                    Init(info, PopupMessageContainer);

                    PopupContent = PopupMessageContainer;
                    PopupMessageContainer.DisplayCancelVisual(info.PopupMode != PopupMode.Show);

                    break;
            }
        }

        private static void Init(PopupInfo infos, object msgView1)
        {
            if (msgView1 is not FrameworkElement msgView)
            {
                return;
            }

            if (infos.PopupConfig != null)
            {
                msgView.Width = infos.PopupConfig.PopupVisualSize.Width;
                msgView.Height = infos.PopupConfig.PopupVisualSize.Height;
            }
            else
            {
                msgView.Width = PopupConfig.DefaultPopupVisualSize.Width;
                msgView.Height = PopupConfig.DefaultPopupVisualSize.Height;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            storyboardTarget = (Border)GetTemplateChild("_PopupContainer");

            if (PopupStoryboard is null)
            {
                Storyboard target = PopupStoryboard = new Storyboard();

                int totalTime = 400;

                target.DoubleAnimationBuilder()
                    .FromTo(0, 1, totalTime)
                    .SetTargetProperty(new PropertyPath(Border.OpacityProperty));
            }
        }

        #region  

        string IPopupAware.HostName => CheckAccess() ? HostName : Dispatcher.Invoke(() => HostName);

        public Task ShowAsync(string message, PopupConfig config = null)
        {
            string hostName = CheckAccess() ? HostName : Dispatcher.Invoke(() => HostName);

            return ShowAsync(hostName, message, config);
        }

        public Task<bool> ConfirmAsync(string message, PopupConfig config = null)
        {
            string hostName = CheckAccess() ? HostName : Dispatcher.Invoke(() => HostName);
            return ConfirmAsync(hostName, message, config);
        }

        public Task<bool> PopupAsync<Target>(Func<Target> uIElementFunc, PopupConfig config = null)
            where Target : UIElement, IPopupContent
        {
            string hostName = CheckAccess() ? HostName : Dispatcher.Invoke(() => HostName);
            return PopupAsync<Target>(hostName, uIElementFunc, config);
        }

        public Task<bool> PopupAsync<Target>(PopupConfig config = null)
            where Target : UIElement, IPopupContent, new()
        {
            string hostName = CheckAccess() ? HostName : Dispatcher.Invoke(() => HostName);
            return PopupAsync<Target>(hostName, config);
        }

        public static Task ShowAsync(string popupHostName, string message, PopupConfig config = null)
        {
            if (string.IsNullOrWhiteSpace(popupHostName))
            {
                throw new ArgumentException(nameof(popupHostName));
            }

            popupAwareMapper.TryGetValue(popupHostName, out PopupHostStatus popup);

            if (popup != null)
            {
                if (popup.PopupStatus != PopupStatus.Showing)
                {
                    throw new ArgumentException($"PopupHost Currently not displayed");
                }

                PopupInfo PopupInfo = new(popupHostName, PopupMode.Show, message, null, config);

                popup.PopupHost.PopupWindow(PopupInfo);

                return PopupInfo.DisplayAsync();
            }
            throw new ArgumentException($"target PopupHost with the name :{popupHostName} not exists");

        }

        public static Task<bool> ConfirmAsync(string popupHostName, string message, PopupConfig config = null)
        {
            if (string.IsNullOrWhiteSpace(popupHostName))
            {
                throw new ArgumentException(nameof(popupHostName));
            }

            popupAwareMapper.TryGetValue(popupHostName, out PopupHostStatus popup);

            if (popup != null)
            {
                if (popup.PopupStatus != PopupStatus.Showing)
                {
                    throw new ArgumentException($"PopupHost Currently not displayed");
                }

                PopupInfo PopupInfo = new(popupHostName, PopupMode.Confirm, message, null, config);

                popup.PopupHost.PopupWindow(PopupInfo);

                return PopupInfo.DisplayAsync();
            }

            throw new ArgumentException($"target PopupHost with the name :{popupHostName} not exists");

        }

        public static Task<bool> PopupAsync<Target>(string popupHostName, Func<Target> uIElementFunc, PopupConfig config = null)
            where Target : UIElement, IPopupContent

        {
            if (string.IsNullOrWhiteSpace(popupHostName))
            {
                throw new ArgumentException(nameof(popupHostName));
            }

            if (uIElementFunc is null)
            {
                throw new ArgumentNullException(nameof(uIElementFunc));
            }

            popupAwareMapper.TryGetValue(popupHostName, out PopupHostStatus popup);

            if (popup != null)
            {
                if (popup.PopupStatus != PopupStatus.Showing)
                {
                    throw new ArgumentException($"PopupHost Currently not displayed");
                }

                PopupInfo PopupInfo = new(popupHostName, PopupMode.Popup, null, uIElementFunc, config);

                popup.PopupHost.PopupWindow(PopupInfo);

                return PopupInfo.DisplayAsync();
            }

            throw new ArgumentException($"target PopupHost with the name :{popupHostName} not exists");

        }

        public static Task<bool> PopupAsync<Target>(string popupHostName, PopupConfig config = null)
            where Target : UIElement, IPopupContent, new()
        {
            if (string.IsNullOrWhiteSpace(popupHostName))
            {
                throw new ArgumentException(nameof(popupHostName));
            }

            popupAwareMapper.TryGetValue(popupHostName, out PopupHostStatus popup);

            if (popup != null)
            {
                if (popup.PopupStatus != PopupStatus.Showing)
                {
                    throw new ArgumentException($"PopupHost Currently not displayed");
                }

                PopupInfo PopupInfo = new(popupHostName, PopupMode.Popup, null, () => new Target(), config);

                popup.PopupHost.PopupWindow(PopupInfo);

                return PopupInfo.DisplayAsync();
            }
            throw new ArgumentException($"target PopupHost with the name :{popupHostName} not exists");

        }

        #endregion

        internal class PopupHostStatus
        {
            public PopupHost PopupHost { get; set; }

            public PopupStatus PopupStatus { get; set; } = PopupStatus.Registed;
        }

        internal enum PopupStatus { Unregiste, Registed, Showing, Unshown, }
    }
}
