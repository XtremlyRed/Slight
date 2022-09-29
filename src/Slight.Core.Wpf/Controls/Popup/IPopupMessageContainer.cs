using System;

namespace Slight.Core
{
    public interface IPopupMessageContainer : IPopupContent
    {
        string Title { set; }
        string Message { set; }
        void DisplayCancelVisual(bool isVisible);
    }


    public interface IPopupContentContainer
    {
        IPopupContent PopupContent { set; }
    }


    public interface IPopupContent
    {
        event EventHandler<PopupResultEventArgs> RequestClose;
    }


    public class PopupResultEventArgs : EventArgs
    {
        public PopupResultEventArgs(bool popupResult)
        {
            PopupResult = popupResult;
        }

        public bool PopupResult { get; }


        public static implicit operator PopupResultEventArgs(bool popupResult)
        {
            return new PopupResultEventArgs(popupResult);
        }

        public static explicit operator bool(PopupResultEventArgs popupResultEventArgs)
        {
            return popupResultEventArgs.PopupResult;
        }
    };
}
