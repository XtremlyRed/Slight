using System.Windows.Media;

namespace Slight.Core
{
    /// <summary>
    /// Color Assist Class
    /// </summary>
    public static class ColorAssist
    {
        /// <summary>
        ///  Color To Brush
        /// </summary>
        /// <param name="color"></param>
        /// <param name="opacity"></param>
        /// <returns></returns>
        public static Brush ToBrush(this Color color, double opacity = 1d)
        {
            double op = opacity.FromRange(0d, 1d);

            return new SolidColorBrush(color)
            {
                Opacity = op,
            };
        }
    }
}
