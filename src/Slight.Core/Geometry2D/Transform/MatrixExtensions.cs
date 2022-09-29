using System;
using System.Collections.Generic;
using System.Linq;

namespace Slight.Core.Geometry2D
{
    /// <summary>
    /// 
    /// </summary>
    public static class MatrixExtensions
    {

        /// <summary>
        ///     3 X 3，  left * right
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static double[] Multipy(double[] left, double[] right)
        {
            double[] m = new double[9];
            m[0] = left[0] * right[0] + left[1] * right[3] + left[2] * right[6];
            m[1] = left[0] * right[1] + left[1] * right[4] + left[2] * right[7];
            m[2] = left[0] * right[2] + left[1] * right[5] + left[2] * right[8];

            m[3] = left[3] * right[0] + left[4] * right[3] + left[5] * right[6];
            m[4] = left[3] * right[1] + left[4] * right[4] + left[5] * right[7];
            m[5] = left[3] * right[2] + left[4] * right[5] + left[5] * right[8];

            m[6] = left[6] * right[0] + left[7] * right[3] + left[8] * right[6];
            m[7] = left[6] * right[1] + left[7] * right[4] + left[8] * right[7];
            m[8] = left[6] * right[2] + left[7] * right[5] + left[8] * right[8];
            return m;
        }

        /// <summary>
        /// Inverse Matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static double[][] InverseMatrix(double[][] matrix)
        {
            if (matrix == null || matrix.Length == 0)
            {
                throw new Exception("invalid matrix array");
            }

            int len = matrix.Length;
            for (int counter = 0; counter < matrix.Length; counter++)
            {
                if (matrix[counter].Length != len)
                {
                    throw new Exception("matrix Must be a square array");
                }
            }

            double dDeterminant = Determinant(matrix);
            if (Math.Abs(dDeterminant) <= 1E-9)
            {
                throw new Exception("Matrix irreversibility");
            }

            double[][] result = AdjointMatrix(matrix);

            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix.Length; j++)
                {
                    result[i][j] = result[i][j] / dDeterminant;
                }
            }

            return result;
        }

        /// <summary>
        /// Recursively calculate the value of determinant
        /// </summary>
        /// <param name="matrix">matrix</param>
        /// <returns></returns>
        public static double Determinant(double[][] matrix)
        {
            if (matrix.Length == 0)
            {
                return 0;
            }
            else if (matrix.Length == 1)
            {
                return matrix[0][0];
            }
            else if (matrix.Length == 2)
            {
                return matrix[0][0] * matrix[1][1] - matrix[0][1] * matrix[1][0];
            }

            double dSum = 0, dSign = 1;
            for (int i = 0; i < matrix.Length; i++)
            {
                double[][] matrixTemp = new double[matrix.Length - 1][];
                for (int count = 0; count < matrix.Length - 1; count++)
                {
                    matrixTemp[count] = new double[matrix.Length - 1];
                }

                for (int j = 0; j < matrixTemp.Length; j++)
                {
                    for (int k = 0; k < matrixTemp.Length; k++)
                    {
                        matrixTemp[j][k] = matrix[j + 1][k >= i ? k + 1 : k];
                    }
                }

                dSum += matrix[0][i] * dSign * Determinant(matrixTemp);
                dSign *= -1;
            }

            return dSum;
        }

        /// <summary>
        /// Calculate the adjoint matrix of the matrix
        /// </summary>
        /// <param name="matrix">matrix</param>
        /// <returns></returns>
        public static double[][] AdjointMatrix(double[][] matrix)
        {
            double[][] result = new double[matrix.Length][];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new double[matrix[i].Length];
            }

            for (int i = 0; i < result.Length; i++)
            {
                for (int j = 0; j < result.Length; j++)
                {
                    double[][] temp = new double[result.Length - 1][];
                    for (int k = 0; k < result.Length - 1; k++)
                    {
                        temp[k] = new double[result[k].Length - 1];
                    }

                    for (int x = 0; x < temp.Length; x++)
                    {
                        for (int y = 0; y < temp.Length; y++)
                        {
                            temp[x][y] = matrix[x < i ? x : x + 1][y < j ? y : y + 1];
                        }
                    }

                    result[j][i] = ((i + j) % 2 == 0 ? 1 : -1) * Determinant(temp);
                }
            }

            return result;
        }

        internal static void gaussian_elimination(ref double[] input, int n)
        {
            int i = 0;
            int j = 0;
            //m = 8 rows, n = 9 cols
            int m = n - 1;
            while (i < m && j < n)
            {
                // Find pivot in column j, starting in row i:
                int maxi = i;
                for (int k = i + 1; k < m; k++)
                {
                    int ind = k * n + j;
                    int ind2 = maxi * n + j;
                    if (Math.Abs(input[ind]) > Math.Abs(input[ind2]))
                    {
                        maxi = k;
                    }
                }
                if (input[maxi * n + j] != 0)
                {
                    //swap rows i and maxi, but do not change the value of i
                    if (i != maxi)
                    {
                        for (int k = 0; k < n; k++)
                        {
                            double f1 = input[i * n + k];
                            double f2 = input[maxi * n + k];
                            input[i * n + k] = f2;
                            input[maxi * n + k] = f1;
                        }
                    }
                    //Now A[i,j] will contain the old value of A[maxi,j].
                    //divide each entry in row i by A[i,j]
                    //将主行归一化
                    double A_ij = input[i * n + j];
                    for (int k = 0; k < n; k++)
                    {
                        input[i * n + k] /= A_ij;
                    }
                    //Now A[i,j] will have the value 1.
                    //主行*A[u,j]，再用A[u,j]-该数即可消除
                    for (int u = i + 1; u < m; u++)
                    {
                        //subtract A[u,j] * row i from row u
                        double A_uj = input[u * n + j];
                        for (int k = 0; k < n; k++)
                        {
                            input[u * n + k] -= A_uj * input[i * n + k];
                        }
                        //Now A[u,j] will be 0, since A[u,j] - A[i,j] * A[u,j] = A[u,j] - 1 * A[u,j] = 0.
                    }
                    i++;
                }
                j++;
            }

            //back substitution
            //最后一位不用管，其他各行用最后一个数-前面各列数*已求的未知数
            for (i = m - 2; i >= 0; i--)
            {
                for (j = i + 1; j < n - 1; j++)
                {
                    input[i * n + m] -= input[i * n + j] * input[j * n + m];
                }
            }
        }

        internal static unsafe IMatrixTransform FindHomography(IList<PointD> src, IList<PointD> dst)
        {
            double[] arrat = new double[]
           {
             -src[0].X, -src[0].Y, -1,   0,   0,  0, src[0].X * dst[0].X, src[0].Y * dst[0].X, -dst[0].X  , // h11
			   0,   0,  0, -src[0].X, -src[0].Y, -1, src[0].X * dst[0].Y, src[0].Y * dst[0].Y, -dst[0].Y  , // h12

			 -src[1].X, -src[1].Y, -1,   0,   0,  0, src[1].X * dst[1].X, src[1].Y * dst[1].X, -dst[1].X  , // h13
			   0,   0,  0, -src[1].X, -src[1].Y, -1, src[1].X * dst[1].Y, src[1].Y * dst[1].Y, -dst[1].Y  , // h21

			 -src[2].X, -src[2].Y, -1,   0,   0,  0, src[2].X * dst[2].X, src[2].Y * dst[2].X, -dst[2].X  , // h22
			   0,   0,  0, -src[2].X, -src[2].Y, -1, src[2].X * dst[2].Y, src[2].Y * dst[2].Y, -dst[2].Y  , // h23

			 -src[3].X, -src[3].Y, -1,   0,   0,  0, src[3].X * dst[3].X, src[3].Y * dst[3].X, -dst[3].X  , // h31
			   0,   0,  0, -src[3].X, -src[3].Y, -1, src[3].X * dst[3].Y, src[3].Y * dst[3].Y, -dst[3].Y  , // h32
            };


            gaussian_elimination(ref arrat, 9);

            double[] homography = new double[9];
            homography[8] = 1;
            arrat.Select((item, index) => new { item, index })
                .Where(i => (i.index + 1) % 9 == 0)
                .ForEach((i, index) => homography[index] = i.item);

            return new MatrixTransform(homography);
        }
    }
}
