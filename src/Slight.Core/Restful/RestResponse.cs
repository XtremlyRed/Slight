
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Slight.Core
{

    [DebuggerDisplay("{StatusCode} {ResponseStatus}")]
    internal sealed class RestResponse : IRestResponse
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal Stream ResponseStream;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal Exception FileWriterException;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal bool CanGetResult;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal Func<byte[], byte[]> Decoder;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal Encoding Encoding;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal Func<string, Type, object> Deserializer;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal HttpWebResponse HttpWebResponse;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal Action<IFileWriter> FileWriterFunc;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal Exception Exception;
        internal RestResponse()
        {
        }

        public void Dispose()
        {

        }


        public ResponseStatus ResponseStatus { get; private set; }
        public IReadOnlyDictionary<string, string> Headers { get; private set; } = new Dictionary<string, string>();
        public IReadOnlyList<Slight.Core.Cookie> Cookies { get; private set; } = new List<Slight.Core.Cookie>();
        public string ProtocolVersion { get; private set; }
        public string ContentEncoding { get; private set; }
        public string ContentType { get; private set; }
        public long ContentLength { get; private set; }
        public string ResponseUrl { get; private set; }
        public int StatusCode { get; private set; }
        public string StatusDescription { get; private set; }


        ~RestResponse()
        {
            Dispose();
        }



        #region IResultResponse

        public TType GetResult<TType>()
        {
            if (ResponseStatus != ResponseStatus.Success)
            {
                Exception ex = Exception.GetBaseException();
                throw new Exception($"ResponseStatus :{ResponseStatus} {Environment.NewLine}{ex.Message}",
                    ex.GetBaseException());
            }

            if (FileWriterException is Exception e)
            {
                throw e;
            }



            if (ResponseStream is null || ResponseStream.Length == 0 || ResponseStream.CanRead == false)
            {
                throw new InvalidOperationException("No valid response stream ");
            }

            if (CanGetResult == false)
            {
                throw new InvalidOperationException("fileWriter callback method may have been registered");
            }

            byte[] buffer = new byte[ResponseStream.Length];
            ResponseStream.Read(buffer, 0, buffer.Length);
            ResponseStream.Seek(0, SeekOrigin.Begin);

            buffer = Decoder?.Invoke(buffer) ?? buffer;

            string stringBuffer = Encoding.GetString(buffer);


            try
            {
                object o = Deserializer.Invoke(stringBuffer, typeof(TType));

                return o is TType t ? t : default;
            }
            catch (Exception excep)
            {
                throw new Exception("An exception occurred during deserialization", excep);
            }
        }

        public Task<TType> GetResultAsync<TType>(CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(((IRestResponse)this).GetResult<TType>, cancellationToken,
                TaskCreationOptions.DenyChildAttach,
                TaskScheduler.Default);
        }

        #endregion


        #region CreateResult 

        internal void SuccessResponse(HttpWebResponse response)
        {
            CanGetResult = true;

            HttpWebResponse = response;

            Stream responseStream = response.GetResponseStream();


            SemaphoreSlim ManualReset = null;
            if (HttpStatusCode.OK == response.StatusCode && responseStream != null)
            {
                Stream stream = new MemoryStream();
                responseStream.CopyTo(stream);
                ResponseStream = stream;
                stream.Seek(0, SeekOrigin.Begin);

                responseStream?.Dispose();

                if (FileWriterFunc != null)
                {
                    ManualReset = new SemaphoreSlim(0, 1);
                    CanGetResult = false;
                    Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            FileWriterFunc.Invoke(new FileWriter(stream));
                        }
                        catch (Exception e)
                        {
                            FileWriterException = new Exception("fileWriter Invoke Error", e);
                        }
                        finally
                        {
                            ManualReset.Release();
                            stream.Seek(0, SeekOrigin.Begin);
                        }
                    }, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
                }
            }

            Dictionary<string, string> dic = new();
            Headers = dic;
            foreach (string itemKey in response.Headers.AllKeys)
            {
                dic[itemKey] = response.Headers[itemKey] ?? "";
            }

            List<Slight.Core.Cookie> list = new();
            Cookies = list;
            foreach (System.Net.Cookie cookie in response.Cookies.OfType<System.Net.Cookie>().ToArray())
            {
                list.Add(new Slight.Core.Cookie(cookie));
            }

            ProtocolVersion = "HTTP/" + response.ProtocolVersion;
            ContentEncoding = response.ContentEncoding ?? response.CharacterSet;
            ContentType = response.ContentType;
            ContentLength = response.ContentLength;
            ResponseUrl = response.ResponseUri.ToString();
            StatusCode = (int)response.StatusCode;
            StatusDescription = response.StatusDescription;
            ResponseStatus = ResponseStatus.Success;

            ManualReset?.Wait();
        }

        internal void ErrorResponse(Exception exception)
        {
            RestResponse response = new();
            Exception = exception.GetBaseException();

            response.ResponseStatus = ResponseStatus.Error;
            if (exception is WebException webException && webException.Status == WebExceptionStatus.Timeout)
            {
                response.ResponseStatus = ResponseStatus.Timeout;
            }
        }

        #endregion



        public override string ToString()
        {
            StringBuilder s = new();
            s.Append("REST Response:");
            if (ResponseStatus != ResponseStatus.Success)
            {
                s.Append("  Error").AppendLine().AppendLine();
                s.AppendLine($"Status Code        : {StatusCode}");
                s.AppendLine($"Status Description : {StatusDescription}");
                s.AppendLine($"Error Message      : {Exception?.Message}");
                return s.ToString();
            }
            else
            {
                s.Append("  Success").AppendLine().AppendLine();
                s.AppendLine($"Status Code        : {StatusCode}");
                s.AppendLine($"Status Description : {StatusDescription}");

                s.AppendLine($"Content Encoding   : {ContentEncoding}");
                s.AppendLine($"ContentType        : {ContentType}");
                s.AppendLine($"ResponseUrl        : {ResponseUrl}");
                s.AppendLine($"Content Length     : {ContentLength}");
                if (ResponseStream is Stream stream)
                {
                    s.AppendLine($"Stream Length      : {stream.Length}");
                }
                s.AppendLine();
            }



            int maxLength = Headers.Count > 0 ? Headers.Max(i => i.Key.Length) : 0;
            int maxLength2 = Cookies.Count > 0 ? Cookies.Max(i => i.Name.Length) : 0;
            maxLength = Math.Max(maxLength, maxLength2);
            if (Headers.Count > 0)
            {
                s.AppendLine("Headers:");
                foreach (KeyValuePair<string, string> header in Headers)
                {
                    s.AppendLine($"    {header.Key.PadRight(maxLength)} : {header.Value}");
                }

                s.AppendLine();
            }

            if (Cookies.Count > 0)
            {
                s.AppendLine("Cookies:");
                foreach (Slight.Core.Cookie cookies in Cookies)
                {
                    s.AppendLine($"    {cookies.Name.PadRight(maxLength)} : {cookies.Value}");
                }

                s.AppendLine();
            }



            return s.ToString();
        }


        private class FileWriter : IFileWriter
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly Stream stream;

            public FileWriter(Stream stream)
            {
                Length = stream.Length;
                this.stream = stream;
            }

            public long Length { get; }

            public long WriteTo(Stream targetStream)
            {
                if (targetStream is null)
                {
                    throw new ArgumentNullException(nameof(targetStream));
                }

                if (!targetStream.CanWrite)
                {
                    throw new ArgumentException("targetStream can not write");
                }

                stream.CopyTo(targetStream);
                stream.Seek(0, SeekOrigin.Begin);
                targetStream.Seek(0, SeekOrigin.Begin);
                return stream.Length;
            }

            public override string ToString()
            {
                return $"Buffer Size:{Length}";
            }

            public byte[] ToArray()
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Seek(0, SeekOrigin.Begin);
                return buffer;
            }

            public Stream ToStream()
            {
                MemoryStream targetStream = new();
                stream.CopyTo(targetStream);
                stream.Seek(0, SeekOrigin.Begin);
                targetStream.Seek(0, SeekOrigin.Begin);


                //byte[] buffer = new byte[stream.Length];
                //stream.Read(buffer, 0, buffer.Length);
                //stream.Seek(0, SeekOrigin.Begin);
                //MemoryStream st = new MemoryStream(buffer);
                return targetStream;

            }
        }
    }
}