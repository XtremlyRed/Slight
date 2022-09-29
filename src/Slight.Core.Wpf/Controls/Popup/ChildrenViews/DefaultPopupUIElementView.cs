using System;
using System.Windows.Controls;
namespace Slight.Core
{
    internal class DefaultPopupUIElementView : ContentControl, IPopupContentContainer
    {
        IPopupContent IPopupContentContainer.PopupContent { set => base.Content = value; }

        public new object Content
        {
            get => base.Content;
            set
            {
                if (value is not null and not IPopupContent)
                {
                    throw new ArgumentException($"{typeof(IPopupContent)} must be assignable from Content Type");
                }
                base.Content = value;
            }
        }
    }
}
