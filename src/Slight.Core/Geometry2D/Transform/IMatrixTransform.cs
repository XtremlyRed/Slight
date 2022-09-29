namespace Slight.Core.Geometry2D
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMatrixTransform
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        IMatrixTransform Offset(double x, double y);

        /// <summary>
        /// 
        /// </summary>
        IMatrixTransform Rotate(double rad);

        /// <summary>
        /// 
        /// </summary>

        IMatrixTransform Scale(double scaleX, double scaleY);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scaleX"></param>
        /// <param name="scaleY"></param>
        /// <param name="scaleCenterX"></param>
        /// <param name="scaleCenterY"></param>
        /// <returns></returns>
        IMatrixTransform Scale(double scaleX, double scaleY, double scaleCenterX, double scaleCenterY);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IMatrixTransform InvertMatrix();


        /// <summary>
        /// 
        /// </summary>
        void Transform(ref double x, ref double y);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="transformX"></param>
        /// <param name="transformY"></param>
        void Transform(double x, double y, out double transformX, out double transformY);
    }
}
