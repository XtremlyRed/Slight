
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Slight.Core.Geometry2D
{
    /// <summary>
    /// <see cref="CircleD"/>
    /// </summary>
    [TypeConverter(typeof(TargetTypeConverter<CircleD, double>))]
    [Serializable]
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    [ComVisible(true)]
    public struct CircleD
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private double x;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private double y;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private double r;
        /// <summary>
        /// empty circle
        /// </summary>
        public static readonly CircleD Empty = new(new PointD(0d, 0d), 0d);

        /// <summary>
        /// create new circle
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        public CircleD(PointD center, double radius) : this(center.X, center.Y, radius)
        {

        }

        /// <summary>
        /// create new circle
        /// </summary>
        /// <param name="centerX"></param>
        /// <param name="centerY"></param>
        /// <param name="radius"></param>
        public CircleD(double centerX, double centerY, double radius)
        {
            x = centerX;
            y = centerY;
            r = radius;
        }

        /// <summary>
        /// is empty
        /// </summary>
        [Browsable(false)]
        public bool IsEmpty => x == 0 && y == 0 && r == 0;

        /// <summary>
        /// center
        /// </summary>
        [Browsable(false)]
        public PointD Center => new(x, y);

        /// <summary>
        /// center x
        /// </summary>
        [DataMember]
        public double X { get => x; set => x = value; }

        /// <summary>
        /// center y
        /// </summary>
        [DataMember]
        public double Y { get => y; set => y = value; }

        /// <summary>
        /// radius
        /// </summary>
        [DataMember]
        public double Radius { get => r; set => r = value; }

        /// <summary>
        /// circle bound
        /// </summary>
        [Browsable(false)]
        public RectangleD Bound => new(x - r, y - r, r * 2, r * 2);

        /// <summary>
        /// offset circle
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public CircleD Offset(double dx, double dy)
        {
            CircleD circle = new(x + dx, y + dy, r);

            return circle;
        }

        /// <summary>
        /// offset circle
        /// </summary>
        /// <param name="p"></param>
        public CircleD Offset(VectorD p)
        {
            return Offset(p.X, p.Y);
        }

        /// <summary>
        /// Get Perimeter 
        /// </summary>
        /// <param name="circel"></param>
        /// <returns></returns>
        [Browsable(false)]
        public static double GetPerimeter(CircleD circel)
        {
            return 2 * Math.PI * circel.r;
        }

        /// <summary>
        /// Get Area
        /// </summary>
        /// <param name="circel"></param>
        /// <returns></returns>
        [Browsable(false)]
        public static double GetArea(CircleD circel)
        {
            return Math.PI * circel.r * circel.r;
        }

        /// <summary>
        /// string format
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({x},{y}) {r}";
        }

        /// <summary>
        ///  Inflate radio
        /// </summary>
        /// <param name="radius"></param>
        public void Inflate(double radius)
        {
            r += radius;
        }

        /// <summary>
        /// offset circel
        /// </summary>
        /// <param name="circel"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static CircleD operator +(CircleD circel, VectorD vector)
        {
            return new CircleD(circel.x + vector.X, circel.y + vector.Y, circel.r);
        }

        /// <summary>
        /// offset circel
        /// </summary>
        /// <param name="circel"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static CircleD operator -(CircleD circel, VectorD vector)
        {
            return new CircleD(circel.x - vector.X, circel.y - vector.Y, circel.r);
        }

        /// <summary>
        ///  scaler circle radius
        /// </summary>
        /// <param name="circel"></param>
        /// <param name="scaler"></param>
        /// <returns></returns>
        public static CircleD operator *(CircleD circel, double scaler)
        {
            return new CircleD(circel.x, circel.y, circel.r * scaler);
        }

        /// <summary>
        /// scaler circle radius
        /// </summary>
        /// <param name="circel"></param>
        /// <param name="scaler"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">scaler is zero</exception>
        public static CircleD operator /(CircleD circel, double scaler)
        {
            return scaler == 0d
                ? throw new ArgumentOutOfRangeException(nameof(scaler))
                : new CircleD(circel.x, circel.y, circel.r / scaler);
        }

        /// <summary>
        /// ==
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(CircleD left, CircleD right)
        {
            return left.x == right.x && left.y == right.y && left.r == right.r;
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(CircleD left, CircleD right)
        {
            return left.x != right.x || left.y != right.y || left.r != right.r;
        }

        /// <summary>
        /// equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is CircleD p1 && x == p1.x && y == p1.y && r == p1.r;
        }

        /// <summary>
        /// hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ r.GetHashCode();
        }

        /// <summary>
        /// string format
        /// </summary>
        /// <param name="retainDecimalPlaces"></param>
        /// <returns></returns>
        public string Format(int retainDecimalPlaces = 2)
        {
            return $"({Math.Round(x, retainDecimalPlaces)},{Math.Round(y, retainDecimalPlaces)}) {Math.Round(r, retainDecimalPlaces)}";
        }

        /// <summary>
        /// round
        /// </summary>
        /// <param name="retainDecimalPlaces"></param>
        /// <returns></returns>
        public CircleD Round(int retainDecimalPlaces = 2)
        {
            return new CircleD(Math.Round(x, retainDecimalPlaces), Math.Round(y, retainDecimalPlaces), Math.Round(r, retainDecimalPlaces));
        }
    }
}
