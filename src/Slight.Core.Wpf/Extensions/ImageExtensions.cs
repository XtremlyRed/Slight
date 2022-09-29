using System;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using PixelFormat = System.Windows.Media.PixelFormat;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace Slight.Core
{
    public static class ImageExtensions
    {
        public static WriteableBitmap ToWriteableBitmap(this Bitmap bitmap, Action<Graphics> action = null)
        {
            WriteableBitmap wBitmap = new(bitmap.Width, bitmap.Height, 96, 96, PixelFormats.Pbgra32, null);
            wBitmap.Lock();
            using Bitmap backBitmap = new(bitmap.Width, bitmap.Height, wBitmap.BackBufferStride, System.Drawing.Imaging.PixelFormat.Format32bppPArgb, wBitmap.BackBuffer);

            using Graphics graphics = Graphics.FromImage(backBitmap);
            graphics.Clear(System.Drawing.Color.Black);

            Rectangle rect = new(new Point(), new Size(bitmap.Width, bitmap.Height));

            graphics.DrawImage(bitmap, rect, rect, GraphicsUnit.Pixel);

            action?.Invoke(graphics);

            wBitmap.AddDirtyRect(new System.Windows.Int32Rect(0, 0, bitmap.Width, bitmap.Height));
            wBitmap.Unlock();

            return wBitmap;
        }



        public static WriteableBitmap ToWriteableBitmap(this Bitmap bitmap, double dpiX = 96, double dpiY = 96, PixelFormat? pixelFormat = null, BitmapPalette palette = null, Action<Graphics> action = null)
        {
            PixelFormat pixelFormat1 = pixelFormat.HasValue ? pixelFormat.Value : PixelFormats.Pbgra32;

            WriteableBitmap wBitmap = new(bitmap.Width, bitmap.Height, dpiX, dpiY, pixelFormat1, palette);
            wBitmap.Lock();
            using Bitmap backBitmap = new(bitmap.Width, bitmap.Height, wBitmap.BackBufferStride, System.Drawing.Imaging.PixelFormat.Format32bppPArgb, wBitmap.BackBuffer);

            using Graphics graphics = Graphics.FromImage(backBitmap);
            graphics.Clear(System.Drawing.Color.Black);

            Rectangle rect = new(new Point(), new Size(bitmap.Width, bitmap.Height));

            graphics.DrawImage(bitmap, rect, rect, GraphicsUnit.Pixel);

            action?.Invoke(graphics);

            wBitmap.AddDirtyRect(new System.Windows.Int32Rect(0, 0, bitmap.Width, bitmap.Height));
            wBitmap.Unlock();

            return wBitmap;
        }
    }
}
