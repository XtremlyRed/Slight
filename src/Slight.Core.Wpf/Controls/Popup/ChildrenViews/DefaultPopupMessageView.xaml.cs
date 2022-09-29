using System;
using System.Windows;
using System.Windows.Controls;

namespace Slight.Core
{
    /// <summary>
    /// DefaultPopupMessageView.xaml 的交互逻辑
    /// </summary>
    public partial class DefaultPopupMessageView : UserControl, IPopupMessageContainer
    {
        public DefaultPopupMessageView()
        {
            InitializeComponent();
        }

        public string Title
        {
            set => TitleText.Text = value;
        }
        public string Message
        {
            set => MessageText.Text = value;
        }

        public event EventHandler<PopupResultEventArgs> RequestClose;

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Tag is bool result)
                {
                    RequestClose?.Invoke(this, result);
                }
            }
        }


        public void DisplayCancelVisual(bool isVisible)
        {
            CancelButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
