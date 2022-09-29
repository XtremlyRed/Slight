
using System;
using System.Diagnostics;
using System.Text;
namespace Slight.Core
{
    public enum Method
    {
        GET, POST, PUT, DELETE, HEAD, OPTIONS,
        PATCH, MERGE, COPY
    }





    [DebuggerDisplay("BaseURL:{BaseUrl}  Timeout:{MillisecondsTimeout}ms  Encoding:{Encoding}")]
    public sealed class RestBuilder : IDisposable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal int MillisecondsTimeout;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal Encoding Encoding;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal string BaseUrl;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal Func<object, string> Serializer;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal Func<string, Type, object> Deserializer;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal Func<byte[], byte[]> Decoder;
        public RestBuilder()
        {
            Encoding = Encoding.UTF8;
            MillisecondsTimeout = 100000;
        }

        public void Dispose()
        {
        }

        public RestBuilder UseTimeout(int useMillisecondsTimeout)
        {
            MillisecondsTimeout = useMillisecondsTimeout;
            return this;
        }

        public RestBuilder UseBaseUrl(string useBaseUrl)
        {
            BaseUrl = useBaseUrl;
            return this;
        }

        public RestBuilder UseEncoding(Encoding encoding)
        {
            Encoding = encoding;
            return this;
        }

        public RestBuilder UseSerializer(Func<object, string> serializer)
        {
            Serializer = serializer;
            return this;
        }

        public RestBuilder UseDeserializer(Func<string, Type, object> deserializer)
        {
            Deserializer = deserializer;
            return this;
        }

        public RestBuilder UseDecoder(Func<byte[], byte[]> decoder)
        {
            Decoder = decoder;
            return this;
        }

        public IRestClient Build()
        {
            if (string.IsNullOrWhiteSpace(BaseUrl))
            {
                throw new ArgumentNullException(nameof(BaseUrl),
                    $" {nameof(RestBuilder.UseBaseUrl)} must be registered");
            }

            if (Serializer is null)
            {
                throw new ArgumentNullException(nameof(Serializer),
                    $" {nameof(RestBuilder.UseSerializer)} must be registered");
            }

            if (Deserializer is null)
            {
                throw new ArgumentNullException(nameof(Deserializer),
                    $" {nameof(RestBuilder.UseDeserializer)} must be registered");
            }

            if (MillisecondsTimeout < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(MillisecondsTimeout));
            }


            if (Encoding is null)
            {
                throw new ArgumentNullException(nameof(Encoding),
                   $" {nameof(RestBuilder.Encoding)} must be registered");
            }



            RestClient client = new(this);

            return client;
        }



        /// <summary>
        /// Create Default Configuration Object
        /// </summary>
        /// <returns></returns>
        public static RestBuilder Default()
        {
            return new RestBuilder();
        }
    }
}