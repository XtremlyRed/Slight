using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
namespace Slight.Core
{
    public interface IRestResponse : IDisposable
    {

        #region Property

        IReadOnlyDictionary<string, string> Headers { get; }
        IReadOnlyList<Slight.Core.Cookie> Cookies { get; }

        string ProtocolVersion { get; }

        string ContentEncoding { get; }

        string ContentType { get; }

        long ContentLength { get; }

        string ResponseUrl { get; }

        int StatusCode { get; }

        string StatusDescription { get; }


        #endregion

        ResponseStatus ResponseStatus { get; }

        TType GetResult<TType>();

        Task<TType> GetResultAsync<TType>(CancellationToken cancellationToken = default);
    }



    public interface IFileWriter
    {
        long Length { get; }

        long WriteTo(Stream stream);

        byte[] ToArray();

        Stream ToStream();
    }
}