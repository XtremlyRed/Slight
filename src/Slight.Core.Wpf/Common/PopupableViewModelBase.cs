
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Slight.Core
{
    public abstract class PopupableViewModelBase : ViewModelBase
    {
        protected IPopupManager PopupManager { get; }

        protected PopupableViewModelBase(IPopupManager popupManager)
        {
            PopupManager = popupManager;
        }

        public abstract string PopupHostName { get; }

        public Task ShowMessageAsync(string message, PopupConfig popupConfig = null)
        {
            if (string.IsNullOrWhiteSpace(PopupHostName))
            {
                throw new ArgumentNullException(nameof(PopupHostName));
            }

            return PopupManager.Aware(PopupHostName).ShowAsync(message, popupConfig);
        }

        public Task<bool> ConfirmMessageAsync(string message, PopupConfig popupConfig = null)
        {
            if (string.IsNullOrWhiteSpace(PopupHostName))
            {
                throw new ArgumentNullException(nameof(PopupHostName));
            }
            return PopupManager.Aware(PopupHostName).ConfirmAsync(message, popupConfig);
        }

        public Task<bool> PopupContentAsync<Target>(Func<Target> elementCreator, PopupConfig popupConfig = null) where Target : UIElement, IPopupContent
        {
            if (string.IsNullOrWhiteSpace(PopupHostName))
            {
                throw new ArgumentNullException(nameof(PopupHostName));
            }

            if (elementCreator is null)
            {
                throw new ArgumentNullException(nameof(elementCreator));
            }

            return PopupManager.Aware(PopupHostName).PopupAsync(elementCreator, popupConfig);
        }
    }
}
