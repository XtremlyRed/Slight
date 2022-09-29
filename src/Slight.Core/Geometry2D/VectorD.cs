
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Slight.Core.Geometry2D
{
    /// <summary>
    /// 
    /// </summary>
    [TypeConverter(typeof(TargetTypeConverter<VectorD, double>))]
    [Serializable]
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    [ComVisible(true)]
    public struct VectorD
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private double x;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private double y;

        /// <summary>
        /// 
        /// </summary>
        public static readonly VectorD Empty = new(0, 0);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public VectorD(double x, double y)
        {
            this.x = x;
            this.y = y;
        }


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
        /// 
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsEmpty => x == 0f && y == 0f;

        /// <summary>
        ///  length
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public double Length => Math.Sqrt(x * x + y * y);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public VectorD Add(VectorD vector)
        {
            return new VectorD(x + vector.X, y + vector.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public VectorD Subtract(VectorD vector)
        {
            return new VectorD(x - vector.X, y - vector.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scalar"></param>
        /// <returns></returns>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public VectorD Multiply(double scalar)
        {
            return new VectorD(x * scalar, y * scalar);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scalar"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public VectorD Divide(double scalar)
        {
            return scalar == 0d
                ? throw new ArgumentOutOfRangeException(nameof(scalar))
                : new VectorD(x / scalar, y / scalar);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public VectorD Offset(double dx, double dy)
        {
            return new VectorD(x + dx, y + dy);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public VectorD Offset(VectorD vector)
        {
            return Offset(vector.x, vector.y);
        }


        /// <summary>
        /// Normalize
        /// </summary>
        /// <returns></returns>
        public static VectorD Normalize(VectorD vector)
        {
            double length = vector.Length;

            return Math.Abs(length) <= 0 ? new VectorD(vector.x, vector.y) : new VectorD(vector.x / length, vector.y / length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static double CrossProduct(VectorD vector1, VectorD vector2)
        {
            return vector1.x * vector2.y - vector1.y * vector2.x;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static double AngleBetween(VectorD vector1, VectorD vector2)
        {
            double y = vector1.x * vector2.y - vector2.x * vector1.y;
            double x = vector1.x * vector2.x + vector1.y * vector2.y;
            return Math.Atan2(y, x) * 57.295779513082323;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static double GetDistance(VectorD current, VectorD other)
        {
            double x2 = Math.Pow(current.x - other.x, 2);
            double y2 = Math.Pow(current.y - other.y, 2);

            return Math.Sqrt(x2 + y2);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({x},{y})";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static VectorD operator +(VectorD v1, VectorD v2)
        {
            return new VectorD(v1.x + v2.x, v1.y + v2.y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static VectorD operator -(VectorD v1, VectorD v2)
        {
            return new VectorD(v1.x - v2.x, v1.y - v2.y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vetcor"></param>
        /// <returns></returns>
        public static VectorD operator -(VectorD vetcor)
        {
            return new VectorD(-vetcor.x, -vetcor.y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static VectorD operator *(VectorD vector, double scalar)
        {
            return new VectorD(vector.x * scalar, vector.y * scalar);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static VectorD operator *(double scalar, VectorD vector)
        {
            return new VectorD(vector.x * scalar, vector.y * scalar);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static VectorD operator /(VectorD vector, double scalar)
        {
            return scalar == 0d
                ? throw new ArgumentOutOfRangeException(nameof(scalar))
                : new VectorD(vector.x / scalar, vector.y / scalar);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(VectorD left, VectorD right)
        {
            return left.x == right.x && left.y == right.y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(VectorD left, VectorD right)
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
            return obj is VectorD p1 && x == p1.x && y == p1.y;
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
        /// <returns></returns>
        public PointD ToPoint()
        {
            return new PointD(x, y);
        }
    }
}
