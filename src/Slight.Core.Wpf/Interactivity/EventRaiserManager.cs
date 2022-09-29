using System.Windows;

namespace Slight.Core
{
    public class EventRaiserManager : DependencyObject
    {
        public static EventRaiserCollection GetEventRaisers(DependencyObject dependencyObject)
        {
            if (dependencyObject.GetValue(EventRaisersProperty) is not EventRaiserCollection raiser)
            {
                dependencyObject.SetValue(EventRaisersProperty, raiser = new EventRaiserCollection());
            }
            return raiser;

        }

        public static readonly DependencyProperty EventRaisersProperty =
            DependencyProperty.RegisterAttached("ShadowEventRaisers", typeof(EventRaiserCollection),
            typeof(EventRaiserManager),
            new FrameworkPropertyMetadata((s, e) =>
            {
                if (e.NewValue is EventRaiserCollection eventRaisers)
                {
                    eventRaisers.Attach(s);
                }
            }));
    }
}
