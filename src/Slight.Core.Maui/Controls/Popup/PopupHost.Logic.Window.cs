 
using System.Diagnostics; 

namespace Slight.Core
{
    public partial class PopupHost
    {

#if WINDOWS

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Grid popupLayer;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Grid contentLayer;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly TimeSpan animationTimeSpan = TimeSpan.FromMilliseconds(350);

        public PopupHost()
        {
            ControlTemplate = new ControlTemplate(() =>
            {
                contentLayer = new Grid();
                popupLayer = new Grid()
                {
                    Background = new SolidColorBrush(MaskColor),
                    IsVisible = false,
                };
                Grid grid = new()
                {
                    contentLayer,
                    popupLayer
                };
                return grid;
            });

            BindingContextChanged += (s, e) =>
            {
                if (contentLayer.BindingContext != BindingContext)
                {
                    contentLayer.BindingContext = BindingContext;
                }
            };
        }


        private async Task PopupLogic()
        {
            try
            {
                PopupVisible = true;

                while (PopupInfos.TryDequeue(out PopupInfo info))
                {
                    switch (info.PopupMode)
                    {
                        case PopupMode.Popup:
                            IPopupContent currentVisual = info.UIElementFunc.Invoke();
                            PopupContent = InitSize(info, currentVisual as View);
                            currentVisual.RequestClose += RequestClose;

                            break;
                            void RequestClose(object sender, PopupResultEventArgs popupResult)
                            {
                                currentVisual.RequestClose -= RequestClose;
                                info.Close(popupResult.PopupResult);
                            }

                        default:

                            IPopupMessage messageView = PopupMessageContainer ??= new PopupMessageBox();
                            messageView.DisplayCancelVisual(info.PopupMode != PopupMode.Show);
                            PopupContent = InitSize(info, messageView as View);
                            messageView.RequestClose += RequestClose2;
                            messageView.Title = info.PopupConfig?.Title ?? "";
                            messageView.Message = info.Message;
                            break;
                            void RequestClose2(object sender, PopupResultEventArgs popupResult)
                            {
                                messageView.RequestClose -= RequestClose2;
                                info.Close(popupResult.PopupResult);
                            }
                    }

                    await info.DisplayAsync();
                    PopupContent = null;

                }
            }
            finally
            {
                isPopuped = false;
                PopupVisible = false;
            }


            static View InitSize(PopupInfo infos, View visual)
            {
                if (infos.PopupConfig is null)
                {
                    return visual;
                }

                visual.WidthRequest = infos.PopupConfig.PopupSize.Width;
                visual.HeightRequest = infos.PopupConfig.PopupSize.Height;

                return visual;
            }
        }




        public static new readonly BindableProperty ContentProperty =
            PropertyAssist.PropertyRegister<PopupHost, View>(i => i.Content, null, BindingMode.OneWay, (s, e) =>
                {
                    Invoker.WhenNotNull(s.contentLayer, i =>
                    {
                        i.Children.Clear();
                        if (e.NewValue != null)
                        {
                            i.Children.Add(e.NewValue);
                        }
                    });
                });
        public new View Content
        {
            get => (View)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }


        private static readonly BindableProperty PopupContentProperty =
            PropertyAssist.PropertyRegister<PopupHost, View>(i => i.PopupContent, null, BindingMode.OneWay, (s, e) =>
            {
                Invoker.WhenNotNull(s.popupLayer, i =>
                 {
                     i.Children.Clear();
                     if (e.NewValue != null)
                     {
                         i.Children.Add(e.NewValue);
                     }
                 });
            });

        private View PopupContent
        {
            get => (View)GetValue(PopupContentProperty);
            set => SetValue(PopupContentProperty, value);
        }

        private static readonly BindableProperty PopupVisibleProperty =
             PropertyAssist.PropertyRegister<PopupHost, bool>(i => i.PopupVisible, false, BindingMode.OneWay, (s, e) =>
             {
                 if (s.popupLayer is null)
                 {
                     return;
                 }
                 if (e.NewValue)
                 {
                     s.popupLayer.Opacity = 0;
                     s.popupLayer.IsVisible = true;
                     s.popupLayer.FadeTo(1, (uint)s.animationTimeSpan.TotalMilliseconds).ContinueWith(t =>
                     {
                         s.Dispatcher.Dispatch(() =>
                         {
                             s.popupLayer.Opacity = 1;
                         });
                     });
                     return;
                 }
                 s.popupLayer.Opacity = 1;
                 s.popupLayer.FadeTo(0, (uint)s.animationTimeSpan.TotalMilliseconds).ContinueWith(t =>
                 {
                     s.Dispatcher.Dispatch(() =>
                     {
                         s.popupLayer.IsVisible = false;
                         s.popupLayer.Opacity = 0;
                     });
                 });
             });

        private bool PopupVisible
        {
            get => (bool)GetValue(PopupVisibleProperty);
            set => SetValue(PopupVisibleProperty, value);
        }
#endif
    }
}
