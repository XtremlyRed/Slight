
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Slight.Core.Geometry2D
{
    /// <summary>
    /// point
    /// </summary>
    [TypeConverter(typeof(TargetTypeConverter<PointD, double>))]
    [Serializable]
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    [ComVisible(true)]
    public struct PointD
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private double x;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private double y;

        /// <summary>
        ///  Origin point
        /// </summary>
        public static readonly PointD Empty = new(0, 0);

        /// <summary>
        /// create  new point
        /// </summary>
        public PointD()
        {
            x = y = 0;
        }

        /// <summary>
        /// create  new point
        /// </summary>
        public PointD(double x, double y)
        {
            this.x = x;
            this.y = y;
        }



        /// <summary>
        /// empty point
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsEmpty => x == 0d && y == 0d;

        /// <summary>
        /// X
        /// </summary>
        [DataMember]
        public double X { get => x; set => x = value; }

        /// <summary>
        /// Y
        /// </summary>
        [DataMember]
        public double Y { get => y; set => y = value; }

        /// <summary>
        /// add
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public PointD Add(VectorD vector)
        {
            return new PointD(x + vector.X, y + vector.Y);
        }

        /// <summary>
        /// Subtract
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public PointD Subtract(VectorD vector)
        {
            return new PointD(x - vector.X, y - vector.Y);
        }

        /// <summary>
        /// offset
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Offset(double dx, double dy)
        {
            x += dx;
            y += dy;
        }

        /// <summary>
        /// offset
        /// </summary>
        /// <param name="p"></param>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Offset(PointD p)
        {
            x += p.X;
            y += p.Y;
        }

        /// <summary>
        /// string format
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({x},{y})";
        }

        /// <summary>
        /// subtract
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static PointD operator +(PointD pt, VectorD vector)
        {
            return new PointD(pt.x + vector.X, pt.y + vector.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static PointD operator -(PointD pt, VectorD point)
        {
            return new PointD(pt.x - point.X, pt.y - point.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static PointD operator -(PointD point)
        {
            return new PointD(-point.x, -point.y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static PointD operator +(PointD pt, PointD point)
        {
            return new PointD(pt.x + point.x, pt.y + point.y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static PointD operator -(PointD pt, PointD vector)
        {
            return new PointD(pt.x - vector.X, pt.y - vector.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(PointD left, PointD right)
        {
            return left.x == right.x && left.y == right.y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(PointD left, PointD right)
        {
            return left.x != right.x || left.y != right.y;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is not PointD p1)
            {
                return false;
            }

            return x == p1.x && y == p1.y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="retainDecimalPlaces"></param>
        /// <returns></returns>
        public string Format(int retainDecimalPlaces = 2)
        {
            return $"{Math.Round(x, retainDecimalPlaces)},{Math.Round(y, retainDecimalPlaces)}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="retainDecimalPlaces"></param>
        /// <returns></returns>
        public PointD Round(int retainDecimalPlaces = 2)
        {
            return new PointD(Math.Round(x, retainDecimalPlaces), Math.Round(y, retainDecimalPlaces));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public VectorD ToVector()
        {
            return new VectorD(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point2"></param>
        /// <returns></returns>
        public double GetDistance(PointD point2)
        {
            double x2 = Math.Pow(x - point2.x, 2);
            double y2 = Math.Pow(x - point2.x, 2);

            return Math.Sqrt(x2 + y2);
        }
    }
}
