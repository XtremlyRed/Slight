using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization; 
using System.Text;

namespace Slight.Core.Geometry2D
{

    /// <summary>
    /// 
    /// </summary>
    [TypeConverter(typeof(TargetTypeArrayConverter<MatrixTransform, double>))]
    [Serializable]
    [DataContract]
    public struct MatrixTransform : IMatrixTransform
    {
        [DataMember]
        private readonly double[] transformData =
        {
            1, 0, 0,
            0, 1, 0,
            0, 0, 1
        };

        /// <summary>
        /// 
        /// </summary>
        public static MatrixTransform Default => new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transformData"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public MatrixTransform(double[] transformData)
        {
            if (transformData == null)
            {
                throw new ArgumentNullException(nameof(transformData));
            }

            if (transformData.Length is not 9 and not 6)
            {
                throw new ArgumentException("ProjectionMatrix parameter data.Length!=9 or 6");
            }
            transformData.CopyTo(this.transformData, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        public MatrixTransform()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IMatrixTransform InvertMatrix()
        {
            MatrixTransform transform = (MatrixTransform)Copy();
            double[][] matrix = transform.transformData.Chunk(3)
                .Select(i => i.ToArray()).ToArray();

            double[][] matrix1 = MatrixExtensions.InverseMatrix(matrix);

            matrix1.SelectMany(i => i).ToArray().CopyTo(transform.transformData, 0);

            return transform;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public IMatrixTransform Offset(double x, double y)
        {
            MatrixTransform transform = (MatrixTransform)Copy();
            transform.transformData[2] = x;
            transform.transformData[5] = y;
            return transform;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rad"></param>
        /// <returns></returns>
        public IMatrixTransform Rotate(double rad)
        {
            MatrixTransform transform = (MatrixTransform)Copy();

            double sin = Math.Sin(rad);
            double cos = Math.Cos(rad);

            double[] matrix =
            {
                cos, -sin, 0,
                sin, cos,  0,
                0,   0,    1
            };
            MatrixExtensions.Multipy(transform.transformData, matrix)
                .CopyTo(transform.transformData, 0);

            return transform;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Transform(ref double x, ref double y)
        {
            double @base = transformData[6] * x + transformData[7] * y + 1;

            double nx = (transformData[0] * x + transformData[1] * y + transformData[2]) / @base;
            double ny = (transformData[3] * x + transformData[4] * y + transformData[5]) / @base;
            x = nx;
            y = ny;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="transformX"></param>
        /// <param name="transformY"></param>
        public void Transform(double x, double y, out double transformX, out double transformY)
        {
            double @base = transformData[6] * x + transformData[7] * y + 1;

            double nx = (transformData[0] * x + transformData[1] * y + transformData[2]) / @base;
            double ny = (transformData[3] * x + transformData[4] * y + transformData[5]) / @base;
            transformX = nx;
            transformY = ny;
        }

        #region

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scaleX"></param>
        /// <param name="scaleY"></param>
        /// <returns></returns>
        public IMatrixTransform Scale(double scaleX, double scaleY)
        {
            MatrixTransform transform = (MatrixTransform)Copy();

            double[] matrix =
            {
                scaleX, 0,      0,
                0,      scaleY, 0,
                0,      0,      1
            };

            MatrixExtensions.Multipy(transform.transformData, matrix)
           .CopyTo(transform.transformData, 0);

            return transform;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="scaleX"></param>
        /// <param name="scaleY"></param>
        /// <param name="scaleCenterX"></param>
        /// <param name="scaleCenterY"></param>
        /// <returns></returns>
        public IMatrixTransform Scale(double scaleX, double scaleY, double scaleCenterX, double scaleCenterY)
        {
            IMatrixTransform translate2Origin = Offset(-scaleCenterX, -scaleCenterY);
            IMatrixTransform scaled = translate2Origin.Scale(scaleX, scaleY);
            return scaled.Offset(scaleCenterX, scaleCenterY);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IMatrixTransform Copy()
        {
            MatrixTransform copy = new(transformData);
            return copy;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string[] stringValues = transformData.Select(x => x.ToString()).ToArray();

            int length = stringValues.Max(x => x.Length);

            StringBuilder s = new();
            stringValues.Chunk(3)
                .ForEach(ii =>
                {
                    ii.ForEach(i => s.Append($"{i},".PadRight(length + 2)));
                    s.AppendLine();
                });

            return s.ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        public static IMatrixTransform MatchMatrix(IList<PointD> src, IList<PointD> dst)
        {
            return MatrixExtensions.FindHomography(src, dst);
        }
    }
}
