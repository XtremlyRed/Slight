namespace Slight.Core
{
    public interface IPopupMessage : IPopupContent
    {
        public string Title { set; }
        public string Message { set; }

        void DisplayCancelVisual(bool isVisible);
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
