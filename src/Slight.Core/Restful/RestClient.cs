using System;
using System.Diagnostics;
namespace Slight.Core
{
    /// <summary>
    /// IRestClient
    /// </summary>
    public interface IRestClient : IDisposable
    {
        /// <summary>
        /// Create rest request  
        /// </summary>
        /// <returns></returns>
        IRestRequest Create();
    }

    [DebuggerDisplay("BaseURL:{builder.BaseUrl}  Timeout:{builder.MillisecondsTimeout}ms  Encoding:{builder.Encoding}")]
    internal class RestClient : IRestClient, IDisposable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly RestBuilder builder;

        internal RestClient(RestBuilder builder)
        {
            this.builder = builder;
        }

        public void Dispose()
        {

        }

        IRestRequest IRestClient.Create()
        {
            RestRequest req = new()
            {
                MillisecondsTimeout = builder.MillisecondsTimeout,
                Encoding = builder.Encoding,
                HostUri = builder.BaseUrl,
                Serializer = builder.Serializer,
                Deserializer = builder.Deserializer,
                Decoder = builder.Decoder
            };

            return req;
        }
    }
}