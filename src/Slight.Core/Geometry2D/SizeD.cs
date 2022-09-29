
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Slight.Core.Geometry2D
{
    /// <summary>
    /// <see cref="SizeD"/>
    /// </summary>
    [TypeConverter(typeof(TargetTypeConverter<SizeD, double>))]
    [Serializable]
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    [ComVisible(true)]
    public struct SizeD
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private   double width;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private   double height;

        /// <summary>
        /// empty size
        /// </summary>
        public static readonly SizeD Empty = new(0, 0);

        /// <summary>
        /// create new size object
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public SizeD(double width, double height)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// create new size object
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public SizeD()
        {
            width = height = 0;
        }

        /// <summary>
        /// is empty
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsEmpty => width == 0d && height == 0d;


        /// <summary>
        /// width
        /// </summary>
        [DataMember]
        public double Width { get => width; set => width = value; }

        /// <summary>
        /// height
        /// </summary>
        [DataMember]
        public double Height  { get => height; set => height = value; }


        /// <summary>
        /// size get area
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static double GetArea(SizeD size)
        {
            return size.width * size.height;
        }

        /// <summary>
        /// string format
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({width},{height})";
        }

        /// <summary>
        /// +
        /// </summary>
        /// <param name="sz1"></param>
        /// <param name="sz2"></param>
        /// <returns></returns>
        public static SizeD operator +(SizeD sz1, SizeD sz2)
        {
            return new SizeD(sz1.width + sz2.width, sz1.height + sz2.height);
        }

        /// <summary>
        /// -
        /// </summary>
        /// <param name="sz1"></param>
        /// <param name="sz2"></param>
        /// <returns></returns>
        public static SizeD operator -(SizeD sz1, SizeD sz2)
        {
            return new SizeD(sz1.width - sz2.width, sz1.height - sz2.height);
        }

        /// <summary>
        /// ==
        /// </summary>
        /// <param name="sz1"></param>
        /// <param name="sz2"></param>
        /// <returns></returns>
        public static bool operator ==(SizeD sz1, SizeD sz2)
        {
            return sz1.width == sz2.width && sz1.height == sz2.height;
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="sz1"></param>
        /// <param name="sz2"></param>
        /// <returns></returns>
        public static bool operator !=(SizeD sz1, SizeD sz2)
        {
            return sz1.width != sz2.width || sz1.height != sz2.height;
        }

        /// <summary>
        /// equale
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is SizeD size && width == size.width && height == size.height;
        }

        /// <summary>
        /// hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return width.GetHashCode() ^ height.GetHashCode();
        }


        /// <summary>
        /// string format 
        /// </summary>
        /// <param name="retainDecimalPlaces"></param>
        /// <returns></returns>
        public string Format(int retainDecimalPlaces = 2)
        {
            return $"{Math.Round(width, retainDecimalPlaces)},{Math.Round(height, retainDecimalPlaces)}";
        }

        /// <summary>
        /// round
        /// </summary>
        /// <param name="retainDecimalPlaces"></param>
        /// <returns></returns>
        public SizeD Round(int retainDecimalPlaces = 2)
        {
            return new SizeD(Math.Round(width, retainDecimalPlaces), Math.Round(height, retainDecimalPlaces));
        }

        /// <summary>
        /// to pointd
        /// </summary>
        /// <returns></returns>
        public PointD ToPoint()
        {
            return new PointD(width, height);
        }
    }
}
