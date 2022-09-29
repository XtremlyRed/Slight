namespace Slight.Core
{
    public static class Serializer
    {
        public static string ToJson<TObject>(TObject @object)
        {
            return new JosnWriter().ToJson(@object);
        }


        public static TObject FromJson<TObject>(string jsonString)
        {
            return new JosnParser().FromJson<TObject>(jsonString);
        }
    }
}
