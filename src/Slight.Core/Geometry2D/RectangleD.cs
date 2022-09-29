
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Slight.Core.Geometry2D
{

    /// <summary>
    /// rectangle
    /// </summary>
    [TypeConverter(typeof(TargetTypeConverter<RectangleD, double>))]
    [Serializable]
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    [ComVisible(true)]
    public struct RectangleD
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private double x;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private double y;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private double width;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private double height;



        /// <summary>
        /// empty rectangle
        /// </summary>
        public static RectangleD Empty = new(0d, 0, 0, 0);

        /// <summary>
        /// create a new rectangle
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public RectangleD(double x, double y, double width, double height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }



        /// <summary>
        /// create a new rectangle
        /// </summary>
        /// <param name="location"></param>
        /// <param name="size"></param>
        public RectangleD(PointD location, SizeD size) : this(location.X, location.Y, size.Width, size.Height)
        {
        }

        /// <summary>
        /// Right
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public double Right => x + width;

        /// <summary>
        /// Top
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public double Top => y + height;

        /// <summary>
        /// Left
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public double Left => x;

        /// <summary>
        /// Bottom
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public double Bottom => y;

        /// <summary>
        /// Width
        /// </summary>
        [DataMember]
        public double Width { get => width; set => width = value; }

        /// <summary>
        /// Height
        /// </summary>
        [DataMember]
        public double Height { get => height; set => height = value; }

        /// <summary>
        /// Y
        /// </summary>
        [DataMember]
        public double Y { get => y; set => y = value; }

        /// <summary>
        /// X
        /// </summary>
        [DataMember]
        public double X { get => x; set => x = value; }

        /// <summary>
        /// invalid rectangle
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsEmpty => width <= 0d || height <= 0d;

        /// <summary>
        /// Size
        /// </summary>
        [Browsable(false)]
        public SizeD Size => new(width, height);

        /// <summary>
        /// Location
        /// </summary>
        [Browsable(false)]
        public PointD Location => new(x, y);

        /// <summary>
        /// Center
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public PointD Center => new(x + width / 2, y + height / 2);

        /// <summary>
        /// LeftBottom
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public PointD LeftBottom => new(x, y);

        /// <summary>
        /// LeftTop
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public PointD LeftTop => new(x, y + height);

        /// <summary>
        /// RightBottom
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public PointD RightBottom => new(x + width, y);

        /// <summary>
        /// RightTop
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public PointD RightTop => new(x + width, y + height);

        /// <summary>
        /// Copy
        /// </summary>
        public void CopyTo(RectangleD other)
        {
            other.X = x;
            other.Y = y;
            other.Width = width;
            other.Height = height;
        }

        /// <summary>
        /// Copy
        /// </summary>
        public RectangleD Copy()
        {
            return new RectangleD(x, y, width, height);
        }

        /// <summary>
        /// container point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public bool Contains(PointD pt)
        {
            return Contains(pt.X, pt.Y);
        }

        /// <summary>
        /// container point
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Contains(double x, double y)
        {
            bool verBool = Left <= x && x <= Right;
            bool hotBool = Bottom <= Y && Y <= Top;
            return verBool && hotBool;
        }

        /// <summary>
        /// container other rectangle
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public bool Contains(RectangleD rect)
        {
            bool f1 = Left <= rect.Left;
            bool f2 = Right >= rect.Right;
            bool f3 = Top >= rect.Top;
            bool f4 = Bottom <= rect.Bottom;
            return f1 && f2 && f3 && f4;
        }

        /// <summary>
        /// union rectangle
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public RectangleD Union(RectangleD other)
        {
            double minLeft = Math.Min(Left, other.Left);
            double maxRight = Math.Max(Right, other.Right);
            double maxTop = Math.Max(Top, other.Top);
            double minBottom = Math.Min(Bottom, other.Bottom);

            return new RectangleD(minLeft, minBottom, maxRight - minLeft, maxTop - minBottom);
        }

        /// <summary>
        /// Judge intersection does not include inclusion relation
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public bool IntersectsWith(RectangleD rect)
        {
            if (IsEmpty || rect.IsEmpty)
            {
                return false;
            }

            return rect.Left <= Right && Left <= rect.Right && Bottom <= rect.Top && rect.Bottom <= Top;
        }

        /// <summary>
        /// Intersect  Rectangle
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public RectangleD Intersect(RectangleD rect)
        {
            if (IntersectsWith(rect) == false)
            {
                return new RectangleD(0, 0, 0, 0);
            }

            double x = Math.Max(Left, rect.Left);
            double y = Math.Max(Bottom, rect.Bottom);
            double w = Math.Max(Math.Min(Right, rect.Right) - x, 0.0);
            double h = Math.Max(Math.Min(Top, rect.Top) - y, 0.0);
            return new RectangleD(x, y, w, h);
        }

        /// <summary>
        /// offset rectangle location by <paramref name="point"/>
        /// </summary>
        /// <param name="point"></param>
        public RectangleD Offset(PointD point)
        {
            return Offset(point.X, point.Y);
        }

        /// <summary>
        /// offset rectangle location by <paramref name="x"/> and <paramref name="y"/>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public RectangleD Offset(double x, double y)
        {
            return new RectangleD(this.x + x, this.y + y, width, height);
        }

        /// <summary>
        /// inflate rectangle size by <paramref name="width"/> and <paramref name="height"/>
        ///   </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public RectangleD Inflate(double width, double height)
        {
            return new RectangleD(x, y, this.width + width, this.height + height);
        }

        /// <summary>
        /// inflate rectangle size by <paramref name="size"/>
        /// </summary>
        /// <param name="size"></param>
        public RectangleD Inflate(SizeD size)
        {
            return Inflate(size.Width, size.Height);
        }

        /// <summary>
        /// equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is RectangleD rectangle && Location == rectangle.Location && Size == rectangle.Size;
        }

        /// <summary>
        /// string format
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({x},{y}) ({width},{height})";
        }
        /// <summary>
        /// get hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Location.GetHashCode() ^ Size.GetHashCode();
        }

        ///// <summary>
        ///// Union
        ///// </summary>
        ///// <param name="rect1"></param>
        ///// <param name="rect2"></param>
        ///// <returns></returns>
        //public static RectangleD operator +(RectangleD rect1, RectangleD rect2)
        //{
        //    return rect1.Union(rect2);
        //}

        ///// <summary>
        ///// Intersect
        ///// </summary>
        ///// <param name="rect1"></param>
        ///// <param name="rect2"></param>
        ///// <returns></returns>
        //public static RectangleD operator -(RectangleD rect1, RectangleD rect2)
        //{
        //    return rect1.Intersect(rect2);
        //}

        /// <summary>
        /// ==
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(RectangleD left, RectangleD right)
        {
            return left.Location == right.Location && left.Size == right.Size;
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RectangleD left, RectangleD right)
        {
            return left.Location != right.Location || left.Size != right.Size;
        }

        /// <summary>
        /// create rectangle by <see cref="PointD.Empty"/> and  <paramref name="size"/>
        /// </summary>
        /// <param name="size"></param>
        public static implicit operator RectangleD(SizeD size)
        {
            PointD l = PointD.Empty;

            return new RectangleD(l.X, l.Y, size.Width, size.Height);
        }

        /// <summary>
        /// string format
        /// </summary>
        /// <param name="retainDecimalPlaces"></param>
        /// <returns></returns>
        public string Format(int retainDecimalPlaces = 2)
        {
            string pointString = Location.Format(retainDecimalPlaces);
            string sizeString = Size.Format(retainDecimalPlaces);
            return $"({pointString}) {sizeString}";
        }

        /// <summary>
        /// retain Decimal Places
        /// </summary>
        /// <param name="retainDecimalPlaces"></param>
        /// <returns></returns>
        public RectangleD Round(int retainDecimalPlaces = 2)
        {
            PointD location = Location.Round(retainDecimalPlaces);
            SizeD size = Size.Round(retainDecimalPlaces);
            return new RectangleD(location, size);
        }
    }
}
