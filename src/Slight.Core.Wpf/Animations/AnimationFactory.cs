using System.Windows.Media.Animation;
namespace Slight.Core
{
    public static class AnimationFactory
    {
        public static ObjectKeyFrameAnimationBuilder ObjectKeyFrameAnimationBuilder(this Storyboard storyboard)
        {
            return new ObjectKeyFrameAnimationBuilder().SetOwner(storyboard);
        }
        public static BoolKeyFrameAnimationBuilder BoolKeyFrameAnimationBuilder(this Storyboard storyboard)
        {
            return new BoolKeyFrameAnimationBuilder().SetOwner(storyboard);
        }
        public static ByteKeyFrameAnimationBuilder ByteKeyFrameAnimationBuilder(this Storyboard storyboard)
        {
            return new ByteKeyFrameAnimationBuilder().SetOwner(storyboard);
        }
        public static ColorKeyFrameAnimationBuilder ColorKeyFrameAnimationBuilder(this Storyboard storyboard)
        {
            return new ColorKeyFrameAnimationBuilder().SetOwner(storyboard);
        }
        public static DoubleKeyFrameAnimationBuilder DoubleKeyFrameAnimationBuilder(this Storyboard storyboard)
        {
            return new DoubleKeyFrameAnimationBuilder().SetOwner(storyboard);
        }
        public static DecimalKeyFrameAnimationBuilder DecimalKeyFrameAnimationBuilder(this Storyboard storyboard)
        {
            return new DecimalKeyFrameAnimationBuilder().SetOwner(storyboard);
        }
        public static Int16KeyFrameAnimationBuilder Int16KeyFrameAnimationBuilder(this Storyboard storyboard)
        {
            return new Int16KeyFrameAnimationBuilder().SetOwner(storyboard);
        }
        public static Int32KeyFrameAnimationBuilder Int32KeyFrameAnimationBuilder(this Storyboard storyboard)
        {
            return new Int32KeyFrameAnimationBuilder().SetOwner(storyboard);
        }
        public static Int64KeyFrameAnimationBuilder Int64KeyFrameAnimationBuilder(this Storyboard storyboard)
        {
            return new Int64KeyFrameAnimationBuilder().SetOwner(storyboard);
        }
        public static MatrixKeyFrameAnimationBuilder MatrixKeyFrameAnimationBuilder(this Storyboard storyboard)
        {
            return new MatrixKeyFrameAnimationBuilder().SetOwner(storyboard);
        }
        public static PointKeyFrameAnimationBuilder PointKeyFrameAnimationBuilder(this Storyboard storyboard)
        {
            return new PointKeyFrameAnimationBuilder().SetOwner(storyboard);
        }
        public static QuaternionKeyFrameAnimationBuilder QuaternionKeyFrameAnimationBuilder(this Storyboard storyboard)
        {
            return new QuaternionKeyFrameAnimationBuilder().SetOwner(storyboard);
        }
        public static RectKeyFrameAnimationBuilder RectKeyFrameAnimationBuilder(this Storyboard storyboard)
        {
            return new RectKeyFrameAnimationBuilder().SetOwner(storyboard);
        }
        public static Rotation3DKeyFrameAnimationBuilder Rotation3DKeyFrameAnimationBuilder(this Storyboard storyboard)
        {
            return new Rotation3DKeyFrameAnimationBuilder().SetOwner(storyboard);
        }
        public static SingleKeyFrameAnimationBuilder SingleKeyFrameAnimationBuilder(this Storyboard storyboard)
        {
            return new SingleKeyFrameAnimationBuilder().SetOwner(storyboard);
        }
        public static StringKeyFrameAnimationBuilder StringKeyFrameAnimationBuilder(this Storyboard storyboard)
        {
            return new StringKeyFrameAnimationBuilder().SetOwner(storyboard);
        }
        public static SizeKeyFrameAnimationBuilder SizeKeyFrameAnimationBuilder(this Storyboard storyboard)
        {
            return new SizeKeyFrameAnimationBuilder().SetOwner(storyboard);
        }
        public static ThicknessKeyFrameAnimationBuilder ThicknessKeyFrameAnimationBuilder(this Storyboard storyboard)
        {
            return new ThicknessKeyFrameAnimationBuilder().SetOwner(storyboard);
        }
        public static Vector3DKeyFrameAnimationBuilder Vector3DKeyFrameAnimationBuilder(this Storyboard storyboard)
        {
            return new Vector3DKeyFrameAnimationBuilder().SetOwner(storyboard);
        }
        public static VectorKeyFrameAnimationBuilder VectorKeyFrameAnimationBuilder(this Storyboard storyboard)
        {
            return new VectorKeyFrameAnimationBuilder().SetOwner(storyboard);
        }

        public static ObjectKeyFrameAnimationBuilder ObjectKeyFrameAnimationBuilder()
        {
            return new ObjectKeyFrameAnimationBuilder();
        }
        public static BoolKeyFrameAnimationBuilder BoolKeyFrameAnimationBuilder()
        {
            return new BoolKeyFrameAnimationBuilder();
        }
        public static ByteKeyFrameAnimationBuilder ByteKeyFrameAnimationBuilder()
        {
            return new ByteKeyFrameAnimationBuilder();
        }
        public static ColorKeyFrameAnimationBuilder ColorKeyFrameAnimationBuilder()
        {
            return new ColorKeyFrameAnimationBuilder();
        }
        public static DoubleKeyFrameAnimationBuilder DoubleKeyFrameAnimationBuilder()
        {
            return new DoubleKeyFrameAnimationBuilder();
        }
        public static DecimalKeyFrameAnimationBuilder DecimalKeyFrameAnimationBuilder()
        {
            return new DecimalKeyFrameAnimationBuilder();
        }
        public static Int16KeyFrameAnimationBuilder Int16KeyFrameAnimationBuilder()
        {
            return new Int16KeyFrameAnimationBuilder();
        }
        public static Int32KeyFrameAnimationBuilder Int32KeyFrameAnimationBuilder()
        {
            return new Int32KeyFrameAnimationBuilder();
        }
        public static Int64KeyFrameAnimationBuilder Int64KeyFrameAnimationBuilder()
        {
            return new Int64KeyFrameAnimationBuilder();
        }
        public static MatrixKeyFrameAnimationBuilder MatrixKeyFrameAnimationBuilder()
        {
            return new MatrixKeyFrameAnimationBuilder();
        }
        public static PointKeyFrameAnimationBuilder PointKeyFrameAnimationBuilder()
        {
            return new PointKeyFrameAnimationBuilder();
        }
        public static QuaternionKeyFrameAnimationBuilder QuaternionKeyFrameAnimationBuilder()
        {
            return new QuaternionKeyFrameAnimationBuilder();
        }
        public static RectKeyFrameAnimationBuilder RectKeyFrameAnimationBuilder()
        {
            return new RectKeyFrameAnimationBuilder();
        }
        public static Rotation3DKeyFrameAnimationBuilder Rotation3DKeyFrameAnimationBuilder()
        {
            return new Rotation3DKeyFrameAnimationBuilder();
        }
        public static SingleKeyFrameAnimationBuilder SingleKeyFrameAnimationBuilder()
        {
            return new SingleKeyFrameAnimationBuilder();
        }
        public static StringKeyFrameAnimationBuilder StringKeyFrameAnimationBuilder()
        {
            return new StringKeyFrameAnimationBuilder();
        }
        public static SizeKeyFrameAnimationBuilder SizeKeyFrameAnimationBuilder()
        {
            return new SizeKeyFrameAnimationBuilder();
        }
        public static ThicknessKeyFrameAnimationBuilder ThicknessKeyFrameAnimationBuilder()
        {
            return new ThicknessKeyFrameAnimationBuilder();
        }
        public static Vector3DKeyFrameAnimationBuilder Vector3DKeyFrameAnimationBuilder()
        {
            return new Vector3DKeyFrameAnimationBuilder();
        }
        public static VectorKeyFrameAnimationBuilder VectorKeyFrameAnimationBuilder()
        {
            return new VectorKeyFrameAnimationBuilder();
        }













        //public static ObjectAnimationBuilder ObjectBuilder(this Storyboard storyboard)
        //{
        //    return new ObjectAnimationBuilder().SetOwner(storyboard);

        //}
        //public static BoolAnimationBuilder BoolBuilder(this Storyboard storyboard)
        //{
        //    return new BoolAnimationBuilder().SetOwner(storyboard);
        //}
        public static ByteAnimationBuilder ByteAnimationBuilder(this Storyboard storyboard)
        {
            return new ByteAnimationBuilder().SetOwner(storyboard);
        }
        public static ColorAnimationBuilder ColorAnimationBuilder(this Storyboard storyboard)
        {
            return new ColorAnimationBuilder().SetOwner(storyboard);
        }
        public static DoubleAnimationBuilder DoubleAnimationBuilder(this Storyboard storyboard)
        {
            return new DoubleAnimationBuilder().SetOwner(storyboard);
        }
        public static DecimalAnimationBuilder DecimalAnimationBuilder(this Storyboard storyboard)
        {
            return new DecimalAnimationBuilder().SetOwner(storyboard);
        }
        public static Int16AnimationBuilder Int16AnimationBuilder(this Storyboard storyboard)
        {
            return new Int16AnimationBuilder().SetOwner(storyboard);
        }
        public static Int32AnimationBuilder Int32AnimationBuilder(this Storyboard storyboard)
        {
            return new Int32AnimationBuilder().SetOwner(storyboard);
        }
        public static Int64AnimationBuilder Int64AnimationBuilder(this Storyboard storyboard)
        {
            return new Int64AnimationBuilder().SetOwner(storyboard);
        }

        public static PointAnimationBuilder PointAnimationBuilder(this Storyboard storyboard)
        {
            return new PointAnimationBuilder().SetOwner(storyboard);
        }
        public static QuaternionAnimationBuilder QuaternionAnimationBuilder(this Storyboard storyboard)
        {
            return new QuaternionAnimationBuilder().SetOwner(storyboard);
        }
        public static RectAnimationBuilder RectAnimationBuilder(this Storyboard storyboard)
        {
            return new RectAnimationBuilder().SetOwner(storyboard);
        }
        public static Rotation3DAnimationBuilder Rotation3DAnimationBuilder(this Storyboard storyboard)
        {
            return new Rotation3DAnimationBuilder().SetOwner(storyboard);
        }
        public static SingleAnimationBuilder SingleAnimationBuilder(this Storyboard storyboard)
        {
            return new SingleAnimationBuilder().SetOwner(storyboard);
        }
        //public static StringAnimationBuilder StringBuilder(this Storyboard storyboard)
        //{
        //    return new StringAnimationBuilder().SetOwner(storyboard);
        //}
        public static SizeAnimationBuilder SizeAnimationBuilder(this Storyboard storyboard)
        {
            return new SizeAnimationBuilder().SetOwner(storyboard);
        }
        public static ThicknessAnimationBuilder ThicknessAnimationBuilder(this Storyboard storyboard)
        {
            return new ThicknessAnimationBuilder().SetOwner(storyboard);
        }
        public static Vector3DAnimationBuilder Vector3DAnimationBuilder(this Storyboard storyboard)
        {
            return new Vector3DAnimationBuilder().SetOwner(storyboard);
        }
        public static VectorAnimationBuilder VectorAnimationBuilder(this Storyboard storyboard)
        {
            return new VectorAnimationBuilder().SetOwner(storyboard);
        }



        //public static ObjectAnimationBuilder ObjectBuilder()
        //{
        //    return new ObjectAnimationBuilder();

        //}
        //public static BoolAnimationBuilder BoolBuilder()
        //{
        //    return new BoolAnimationBuilder();
        //}
        public static ByteAnimationBuilder ByteAnimationBuilder()
        {
            return new ByteAnimationBuilder();
        }
        public static ColorAnimationBuilder ColorAnimationBuilder()
        {
            return new ColorAnimationBuilder();
        }
        public static DoubleAnimationBuilder DoubleAnimationBuilder()
        {
            return new DoubleAnimationBuilder();
        }
        public static DecimalAnimationBuilder DecimalAnimationBuilder()
        {
            return new DecimalAnimationBuilder();
        }
        public static Int16AnimationBuilder Int16AnimationBuilder()
        {
            return new Int16AnimationBuilder();
        }
        public static Int32AnimationBuilder Int32AnimationBuilder()
        {
            return new Int32AnimationBuilder();
        }
        public static Int64AnimationBuilder Int64AnimationBuilder()
        {
            return new Int64AnimationBuilder();
        }
        //public static MatrixAnimationBuilder MatrixBuilder()
        //{
        //    return new MatrixAnimationBuilder();
        //}
        public static PointAnimationBuilder PointAnimationBuilder()
        {
            return new PointAnimationBuilder();
        }
        public static QuaternionAnimationBuilder QuaternionAnimationBuilder()
        {
            return new QuaternionAnimationBuilder();
        }
        public static RectAnimationBuilder RectAnimationBuilder()
        {
            return new RectAnimationBuilder();
        }
        public static Rotation3DAnimationBuilder Rotation3DAnimationBuilder()
        {
            return new Rotation3DAnimationBuilder();
        }
        public static SingleAnimationBuilder SingleAnimationBuilder()
        {
            return new SingleAnimationBuilder();
        }
        //public static StringAnimationBuilder StringBuilder()
        //{
        //    return new StringAnimationBuilder();
        //}
        public static SizeAnimationBuilder SizeAnimationBuilder()
        {
            return new SizeAnimationBuilder();
        }
        public static ThicknessAnimationBuilder ThicknessAnimationBuilder()
        {
            return new ThicknessAnimationBuilder();
        }
        public static Vector3DAnimationBuilder Vector3DAnimationBuilder()
        {
            return new Vector3DAnimationBuilder();
        }
        public static VectorAnimationBuilder VectorAnimationBuilder()
        {
            return new VectorAnimationBuilder();
        }
    }
}
