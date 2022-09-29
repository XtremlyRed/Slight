namespace Slight.Core
{
    public static class VisualElementAssist
    {
        public static bool TryGetParentPage(this VisualElement visualElement, out Page page)
        {
            page = GetParentPage(visualElement);
            return page != null;
        }

        public static Element GetRoot(this Element element)
        {
            return element.Parent switch
            {
                null => element,
                _ => GetRoot(element.Parent),
            };
        }

        public static Page GetParentPage(this Element visualElement)
        {
            return visualElement.Parent switch
            {
                Page page => page,
                null => null,
                _ => GetParentPage(visualElement.Parent),
            };
        }


        public static Brush ToBrush(this Color color)
        {
            return new SolidColorBrush(color);
        }
    }
}
