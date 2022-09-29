

using System.Diagnostics;

namespace Slight.Core
{
    public partial class PopupHost
    {

#if !WINDOWS

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private PopupContainerPage containerPage;

        private async Task PopupLogic()
        {
            Page parentPage = this.GetParentPage();
            containerPage ??= new PopupContainerPage(this);

            containerPage.Opacity = 0;
            parentPage.Navigation.PushModalAsync(containerPage, false).NoAwaiter();
            containerPage.FadeTo(1, 200, Easing.SinIn).NoAwaiter();
            try
            {
                while (PopupInfos.TryDequeue(out PopupInfo popupInfo))
                {
                    containerPage.InitializeSize(popupInfo);

                    switch (popupInfo.PopupMode)
                    {
                        case PopupMode.Popup:
                            IPopupContent currentVisual = popupInfo.UIElementFunc.Invoke();
                            currentVisual.RequestClose += RequestClose;
                            containerPage.InitializeContent(currentVisual);
                            break;

                            void RequestClose(object sender, PopupResultEventArgs popupResult)
                            {
                                currentVisual.RequestClose -= RequestClose;
                                popupInfo.Close(popupResult.PopupResult);
                            }
                        default:

                            IPopupMessage messageView = PopupMessageContainer ??= new PopupMessageBox();
                            messageView.DisplayCancelVisual(popupInfo.PopupMode != PopupMode.Show);
                            messageView.RequestClose += RequestClose2;
                            messageView.Title = popupInfo.PopupConfig?.Title ?? "";
                            messageView.Message = popupInfo.Message;

                            containerPage.InitializeContent(messageView);
                            break;

                            void RequestClose2(object sender, PopupResultEventArgs popupResult)
                            {
                                messageView.RequestClose -= RequestClose2;
                                popupInfo.Close(popupResult.PopupResult);

                            }
                    }

                    await popupInfo.DisplayAsync();
                }
            }
            finally
            {
                isPopuped = false;

                await containerPage.FadeTo(0, 150, Easing.SinIn).ContinueWith(t =>
                {
                    Dispatcher.Dispatch(async () => await containerPage.GoBack());
                });

            }
        }

        private class PopupContainerPage : ContentPage
        {
            public PopupContainerPage(object bindingContext)
            {
                NavigationPage.SetHasNavigationBar(this, false);

                BackgroundColor = Colors.Transparent;
                Background = new SolidColorBrush(Colors.Transparent);

                BoxView box = new()
                {
                    Margin = new Thickness(0),
                };
                box.SetBinding(BoxView.ColorProperty, new Binding($"{nameof(MaskColor)}", BindingMode.OneWay, null, null, null, bindingContext));
                box.SetBinding(BoxView.OpacityProperty, new Binding($"{nameof(MaskOpacity)}", BindingMode.OneWay, null, null, null, bindingContext));
                box.ZIndex = 1;
                Border border = popupContainer = new Border()
                {
                    Margin = new Thickness(0),
                    Background = Brush.Transparent,
                    BackgroundColor = Colors.Transparent,
                    StrokeThickness = 0,
                    Stroke = Brush.Transparent,
                };
                border.ZIndex = 10;

                Grid grid = new()
                {
                    Margin = new Thickness(0),
                    BackgroundColor = Colors.Transparent,
                    Background = new SolidColorBrush(Colors.Transparent),
                    IsClippedToBounds = true,
                };
                grid.Add(box);
                grid.Add(border);

                Content = grid;

            }

            protected override bool OnBackButtonPressed()
            {
                return false;
            }

            private readonly Border popupContainer;


            public Task GoBack(bool animated = false)
            {
                return Navigation.PopModalAsync(animated);
            }


            public void InitializeSize(PopupInfo info)
            {
                if (info.PopupConfig is null)
                {
                    return;
                }

                popupContainer.WidthRequest = info.PopupConfig.PopupSize.Width;
                popupContainer.HeightRequest = info.PopupConfig.PopupSize.Height;
            }


            public void InitializeContent(IPopupContent content)
            {
                popupContainer.Content = content as View;
            }
        }



#endif
    }
}
