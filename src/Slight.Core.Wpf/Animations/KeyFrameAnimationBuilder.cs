using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
namespace Slight.Core
{
    public sealed class ObjectKeyFrameAnimationBuilder : KeyFrameAnimationBuildBase<ObjectKeyFrameAnimationBuilder, ObjectAnimationUsingKeyFrames>
    {
        public ObjectKeyFrameAnimationBuilder AddKeyFrame(object value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteObjectKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }
    }
    public sealed class BoolKeyFrameAnimationBuilder : KeyFrameAnimationBuildBase<BoolKeyFrameAnimationBuilder, BooleanAnimationUsingKeyFrames>
    {
        public BoolKeyFrameAnimationBuilder AddKeyFrame(bool value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteBooleanKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }
    }
    public sealed class ByteKeyFrameAnimationBuilder : KeyFrameAnimationBuildBase<ByteKeyFrameAnimationBuilder, ByteAnimationUsingKeyFrames>
    {
        public ByteKeyFrameAnimationBuilder AddEasingKeyFrame(byte value, int milliseconds, IEasingFunction easingFunction)
        {
            return base.AddKeyFrame(new EasingByteKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds), easingFunction));
        }

        public ByteKeyFrameAnimationBuilder AddDiscreteKeyFrame(byte value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteByteKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public ByteKeyFrameAnimationBuilder AddLinearKeyFrame(byte value, int milliseconds)
        {
            return base.AddKeyFrame(new LinearByteKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public ByteKeyFrameAnimationBuilder AddSplineKeyFrame(byte value, int milliseconds)
        {
            return base.AddKeyFrame(new SplineByteKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }
    }
    public sealed class ColorKeyFrameAnimationBuilder : KeyFrameAnimationBuildBase<ColorKeyFrameAnimationBuilder, ColorAnimationUsingKeyFrames>
    {
        public ColorKeyFrameAnimationBuilder AddEasingKeyFrame(Color value, int milliseconds, IEasingFunction easingFunction)
        {
            return base.AddKeyFrame(new EasingColorKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds), easingFunction));
        }

        public ColorKeyFrameAnimationBuilder AddDiscreteKeyFrame(Color value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteColorKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public ColorKeyFrameAnimationBuilder AddLinearKeyFrame(Color value, int milliseconds)
        {
            return base.AddKeyFrame(new LinearColorKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public ColorKeyFrameAnimationBuilder AddSplineKeyFrame(Color value, int milliseconds)
        {
            return base.AddKeyFrame(new SplineColorKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }
    }
    public sealed class DoubleKeyFrameAnimationBuilder : KeyFrameAnimationBuildBase<DoubleKeyFrameAnimationBuilder, DoubleAnimationUsingKeyFrames>
    {
        public DoubleKeyFrameAnimationBuilder AddEasingKeyFrame(double value, int milliseconds, IEasingFunction easingFunction)
        {
            return base.AddKeyFrame(new EasingDoubleKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds), easingFunction));
        }

        public DoubleKeyFrameAnimationBuilder AddDiscreteKeyFrame(double value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteDoubleKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public DoubleKeyFrameAnimationBuilder AddLinearKeyFrame(double value, int milliseconds)
        {
            return base.AddKeyFrame(new LinearDoubleKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public DoubleKeyFrameAnimationBuilder AddSplineKeyFrame(double value, int milliseconds)
        {
            return base.AddKeyFrame(new SplineDoubleKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }
    }
    public sealed class DecimalKeyFrameAnimationBuilder : KeyFrameAnimationBuildBase<DecimalKeyFrameAnimationBuilder, DecimalAnimationUsingKeyFrames>
    {
        public DecimalKeyFrameAnimationBuilder AddEasingKeyFrame(decimal value, int milliseconds, IEasingFunction easingFunction)
        {
            return base.AddKeyFrame(new EasingDecimalKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds), easingFunction));
        }

        public DecimalKeyFrameAnimationBuilder AddDiscreteKeyFrame(decimal value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteDecimalKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public DecimalKeyFrameAnimationBuilder AddLinearKeyFrame(decimal value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteDecimalKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public DecimalKeyFrameAnimationBuilder AddSplineKeyFrame(decimal value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteDecimalKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }
    }
    public sealed class Int16KeyFrameAnimationBuilder : KeyFrameAnimationBuildBase<Int16KeyFrameAnimationBuilder, Int16AnimationUsingKeyFrames>
    {
        public Int16KeyFrameAnimationBuilder AddEasingKeyFrame(short value, int milliseconds, IEasingFunction easingFunction)
        {
            return base.AddKeyFrame(new EasingInt16KeyFrame(value, TimeSpan.FromMilliseconds(milliseconds), easingFunction));
        }

        public Int16KeyFrameAnimationBuilder AddDiscreteKeyFrame(short value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteInt16KeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public Int16KeyFrameAnimationBuilder AddLinearKeyFrame(short value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteInt16KeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public Int16KeyFrameAnimationBuilder AddSplineKeyFrame(short value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteInt16KeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }
    }
    public sealed class Int32KeyFrameAnimationBuilder : KeyFrameAnimationBuildBase<Int32KeyFrameAnimationBuilder, Int32AnimationUsingKeyFrames>
    {
        public Int32KeyFrameAnimationBuilder AddEasingKeyFrame(int value, int milliseconds, IEasingFunction easingFunction)
        {
            return base.AddKeyFrame(new EasingInt32KeyFrame(value, TimeSpan.FromMilliseconds(milliseconds), easingFunction));
        }

        public Int32KeyFrameAnimationBuilder AddDiscreteKeyFrame(int value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteInt32KeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public Int32KeyFrameAnimationBuilder AddLinearKeyFrame(int value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteInt32KeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public Int32KeyFrameAnimationBuilder AddSplineKeyFrame(int value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteInt32KeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }
    }
    public sealed class Int64KeyFrameAnimationBuilder : KeyFrameAnimationBuildBase<Int64KeyFrameAnimationBuilder, Int64AnimationUsingKeyFrames>
    {
        public Int64KeyFrameAnimationBuilder AddEasingKeyFrame(long value, int milliseconds, IEasingFunction easingFunction)
        {
            return base.AddKeyFrame(new EasingInt64KeyFrame(value, TimeSpan.FromMilliseconds(milliseconds), easingFunction));
        }

        public Int64KeyFrameAnimationBuilder AddDiscreteKeyFrame(long value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteInt64KeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public Int64KeyFrameAnimationBuilder AddLinearKeyFrame(long value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteInt64KeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public Int64KeyFrameAnimationBuilder AddSplineKeyFrame(long value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteInt64KeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }
    }
    public sealed class MatrixKeyFrameAnimationBuilder : KeyFrameAnimationBuildBase<MatrixKeyFrameAnimationBuilder, MatrixAnimationUsingKeyFrames>
    {


        public MatrixKeyFrameAnimationBuilder AddKeyFrame(Matrix value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteMatrixKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }
    }
    public sealed class PointKeyFrameAnimationBuilder : KeyFrameAnimationBuildBase<PointKeyFrameAnimationBuilder, PointAnimationUsingKeyFrames>
    {
        public PointKeyFrameAnimationBuilder AddEasingKeyFrame(Point value, int milliseconds, IEasingFunction easingFunction)
        {
            return base.AddKeyFrame(new EasingPointKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds), easingFunction));
        }

        public PointKeyFrameAnimationBuilder AddDiscreteKeyFrame(Point value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscretePointKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public PointKeyFrameAnimationBuilder AddLinearKeyFrame(Point value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscretePointKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public PointKeyFrameAnimationBuilder AddSplineKeyFrame(Point value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscretePointKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }
    }
    public sealed class QuaternionKeyFrameAnimationBuilder : KeyFrameAnimationBuildBase<QuaternionKeyFrameAnimationBuilder, QuaternionAnimationUsingKeyFrames>
    {
        public QuaternionKeyFrameAnimationBuilder AddEasingKeyFrame(Quaternion value, int milliseconds, IEasingFunction easingFunction)
        {
            return base.AddKeyFrame(new EasingQuaternionKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds), easingFunction));
        }

        public QuaternionKeyFrameAnimationBuilder AddDiscreteKeyFrame(Quaternion value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteQuaternionKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public QuaternionKeyFrameAnimationBuilder AddLinearKeyFrame(Quaternion value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteQuaternionKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public QuaternionKeyFrameAnimationBuilder AddSplineKeyFrame(Quaternion value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteQuaternionKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }
    }
    public sealed class RectKeyFrameAnimationBuilder : KeyFrameAnimationBuildBase<RectKeyFrameAnimationBuilder, RectAnimationUsingKeyFrames>
    {
        public RectKeyFrameAnimationBuilder AddEasingKeyFrame(Rect value, int milliseconds, IEasingFunction easingFunction)
        {
            return base.AddKeyFrame(new EasingRectKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds), easingFunction));
        }

        public RectKeyFrameAnimationBuilder AddDiscreteKeyFrame(Rect value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteRectKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public RectKeyFrameAnimationBuilder AddLinearKeyFrame(Rect value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteRectKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public RectKeyFrameAnimationBuilder AddSplineKeyFrame(Rect value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteRectKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }
    }
    public sealed class Rotation3DKeyFrameAnimationBuilder : KeyFrameAnimationBuildBase<Rotation3DKeyFrameAnimationBuilder, Rotation3DAnimationUsingKeyFrames>
    {
        public Rotation3DKeyFrameAnimationBuilder AddEasingKeyFrame(Rotation3D value, int milliseconds, IEasingFunction easingFunction)
        {
            return base.AddKeyFrame(new EasingRotation3DKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds), easingFunction));
        }

        public Rotation3DKeyFrameAnimationBuilder AddDiscreteKeyFrame(Rotation3D value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteRotation3DKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public Rotation3DKeyFrameAnimationBuilder AddLinearKeyFrame(Rotation3D value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteRotation3DKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public Rotation3DKeyFrameAnimationBuilder AddSplineKeyFrame(Rotation3D value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteRotation3DKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }
    }
    public sealed class SingleKeyFrameAnimationBuilder : KeyFrameAnimationBuildBase<SingleKeyFrameAnimationBuilder, SingleAnimationUsingKeyFrames>
    {
        public SingleKeyFrameAnimationBuilder AddEasingKeyFrame(float value, int milliseconds, IEasingFunction easingFunction)
        {
            return base.AddKeyFrame(new EasingSingleKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds), easingFunction));
        }

        public SingleKeyFrameAnimationBuilder AddDiscreteKeyFrame(float value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteSingleKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public SingleKeyFrameAnimationBuilder AddLinearKeyFrame(float value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteSingleKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public SingleKeyFrameAnimationBuilder AddSplineKeyFrame(float value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteSingleKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }
    }
    public sealed class StringKeyFrameAnimationBuilder : KeyFrameAnimationBuildBase<StringKeyFrameAnimationBuilder, StringAnimationUsingKeyFrames>
    {

        public StringKeyFrameAnimationBuilder AddKeyFrame(string value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteStringKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }
    }
    public sealed class SizeKeyFrameAnimationBuilder : KeyFrameAnimationBuildBase<SizeKeyFrameAnimationBuilder, SizeAnimationUsingKeyFrames>
    {
        public SizeKeyFrameAnimationBuilder AddEasingKeyFrame(Size value, int milliseconds, IEasingFunction easingFunction)
        {
            return base.AddKeyFrame(new EasingSizeKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds), easingFunction));
        }

        public SizeKeyFrameAnimationBuilder AddDiscreteKeyFrame(Size value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteSizeKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public SizeKeyFrameAnimationBuilder AddLinearKeyFrame(Size value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteSizeKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public SizeKeyFrameAnimationBuilder AddSplineKeyFrame(Size value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteSizeKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }
    }
    public sealed class ThicknessKeyFrameAnimationBuilder : KeyFrameAnimationBuildBase<ThicknessKeyFrameAnimationBuilder, ThicknessAnimationUsingKeyFrames>
    {
        public ThicknessKeyFrameAnimationBuilder AddEasingKeyFrame(Thickness value, int milliseconds, IEasingFunction easingFunction)
        {
            return base.AddKeyFrame(new EasingThicknessKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds), easingFunction));
        }

        public ThicknessKeyFrameAnimationBuilder AddDiscreteKeyFrame(Thickness value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteThicknessKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public ThicknessKeyFrameAnimationBuilder AddLinearKeyFrame(Thickness value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteThicknessKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public ThicknessKeyFrameAnimationBuilder AddSplineKeyFrame(Thickness value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteThicknessKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }
    }
    public sealed class Vector3DKeyFrameAnimationBuilder : KeyFrameAnimationBuildBase<Vector3DKeyFrameAnimationBuilder, Vector3DAnimationUsingKeyFrames>
    {
        public Vector3DKeyFrameAnimationBuilder AddEasingKeyFrame(Vector3D value, int milliseconds, IEasingFunction easingFunction)
        {
            return base.AddKeyFrame(new EasingVector3DKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds), easingFunction));
        }

        public Vector3DKeyFrameAnimationBuilder AddDiscreteKeyFrame(Vector3D value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteVector3DKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public Vector3DKeyFrameAnimationBuilder AddLinearKeyFrame(Vector3D value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteVector3DKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public Vector3DKeyFrameAnimationBuilder AddSplineKeyFrame(Vector3D value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteVector3DKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }
    }
    public sealed class VectorKeyFrameAnimationBuilder : KeyFrameAnimationBuildBase<VectorKeyFrameAnimationBuilder, VectorAnimationUsingKeyFrames>
    {
        public VectorKeyFrameAnimationBuilder AddEasingKeyFrame(Vector value, int milliseconds, IEasingFunction easingFunction)
        {
            return base.AddKeyFrame(new EasingVectorKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds), easingFunction));
        }

        public VectorKeyFrameAnimationBuilder AddDiscreteKeyFrame(Vector value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteVectorKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public VectorKeyFrameAnimationBuilder AddLinearKeyFrame(Vector value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteVectorKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }

        public VectorKeyFrameAnimationBuilder AddSplineKeyFrame(Vector value, int milliseconds)
        {
            return base.AddKeyFrame(new DiscreteVectorKeyFrame(value, TimeSpan.FromMilliseconds(milliseconds)));
        }
    }
}
