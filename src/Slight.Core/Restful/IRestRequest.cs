
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
namespace Slight.Core
{
    public interface IRestRequest : IDisposable
    {
        IRestRequest UseFileWriter(Action<IFileWriter> fileWriterFunc);
        IRestRequest AddParameter<TParam>(string key, TParam parameter);
        IRestRequest AddParameters(IEnumerable<KeyValuePair<string, object>> parameters);
        IRestRequest AddParameter<TParam>(TParam requestBody, DataFormat dataFormat = DataFormat.Json);
        IRestRequest AddFile(string filePath);
        IRestRequest AddFiles(IEnumerable<string> filePaths);
        IRestRequest AddFile(string fileName, Stream stream);
        IRestRequest AddHeader(string name, string value);
        IRestRequest AddCookie(string name, string value);
        Task<IRestResponse> ExecuteAsync(CancellationToken cancellationToken = default);
        IRestResponse Execute();
        Task<TType> ExecuteAsync<TType>(CancellationToken cancellationToken = default);
        TType Execute<TType>();

        Method Method { get; }


        #region Config

        IRestRequest UseTimeout(int millisecondsTimeout);
        IRestRequest UseReadWriteTimeout(int millisecondsReadWriteTimeout);
        IRestRequest UseContinueTimeout(int millisecondsContinueTimeout);
        IRestRequest UseUserAgent(string userAgent);
        IRestRequest UseMethod(Method method);
        IRestRequest UseUrl(string url);
        IRestRequest UseContentType(string contentType);
        IRestRequest UseConnectionGroupName(string connectionGroupName);
        IRestRequest UseDefaultCredentials(bool useDefaultCredentials);
        IRestRequest UseExpect(bool expect);
        IRestRequest UseClientCertificates(X509CertificateCollection clientCertificates);

        IRestRequest UseRemoteCertificateValidationCallback(
              RemoteCertificateValidationCallback remoteCertificateValidationCallback);

        IRestRequest UseIfModifiedSince(DateTime ifModifiedSince);
        IRestRequest UseDate(DateTime date);
        IRestRequest UseSendChunked(string sendChunked);
        IRestRequest UseConnection(string connection);
        IRestRequest UseTransferEncoding(string transferEncoding);
        IRestRequest UseKeepAlive(bool keepAlive);
        IRestRequest UsePipelined(bool pipelined);
        IRestRequest UseAllowReadStreamBuffering(bool allowReadStreamBuffering);
        IRestRequest UseAllowWriteStreamBuffering(bool allowWriteStreamBuffering);
        IRestRequest UseAllowAutoRedirect(bool allowAutoRedirect);


        IRestRequest UseMaximumResponseHeadersLength(int maximumResponseHeadersLength);
        IRestRequest UseProxy(IWebProxy proxy);

        IRestRequest UseUnsafeAuthenticatedConnectionSharing(bool unsafeAuthenticatedConnectionSharing);
        IRestRequest UsePreAuthenticate(bool preAuthenticate);
        IRestRequest UseReferer(string referer);
        IRestRequest UseMediaType(string mediaType);
        IRestRequest UseAccept(string accept);
        IRestRequest UseDisableAutomaticCompression(bool disableAutomaticCompression);

        IRestRequest UseAlwaysMultipartFormData(bool alwaysMultipartFormData);

        #endregion
    }
}