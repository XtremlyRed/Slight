using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Slight.Core
{
    public class EventRaiserCollection : FreezableCollection<EventRaiser>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private DependencyObject associatedObject;
        protected override Freezable CreateInstanceCore()
        {
            return new EventRaiserCollection();
        }

        public void Attach(DependencyObject dependencyObject)
        {
            if (Equals(dependencyObject, associatedObject))
            {
                return;
            }

            associatedObject = dependencyObject;

            if (dependencyObject is FrameworkElement framework && this is INotifyCollectionChanged notify)
            {
                notify.CollectionChanged += Notify_CollectionChanged;
                framework.Loaded += Framework_Loaded;

                void Framework_Loaded(object sender, RoutedEventArgs e)
                {
                    framework.Loaded -= Framework_Loaded;

                    if (FindVisualParent(framework) is FrameworkElement parent)
                    {
                        parent.Unloaded += Framework_Unloaded;
                    }
                }

                void Framework_Unloaded(object sender, RoutedEventArgs e)
                {
                    framework.Unloaded -= Framework_Unloaded;
                    notify.CollectionChanged -= Notify_CollectionChanged;
                    this?.ForEach(i => i?.Dispose());
                    this?.Clear();
                }

                void Notify_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
                {
                    this.ForEach(i => i.Attach(dependencyObject));
                    e.OldItems?.OfType<EventRaiser>()?.ForEach(i => i?.Dispose());
                }
            }
        }



        public void Detach()
        {
            associatedObject = null;
        }

        private DependencyObject FindVisualParent(FrameworkElement framework)
        {
            DependencyObject parent = VisualTreeAssist.FindParent<UserControl>(framework);
            parent ??= VisualTreeAssist.FindParent<Page>(framework);
            parent ??= VisualTreeAssist.FindParent<Window>(framework);
            parent ??= Window.GetWindow(framework);
            return parent;
        }
    }
}
