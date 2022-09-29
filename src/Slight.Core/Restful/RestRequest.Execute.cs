
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Slight.Core
{
    internal partial class RestRequest
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly IDictionary<string, PropertyInfo> ConfigPropertyInfos =
            new ConcurrentDictionary<string, PropertyInfo>();

        public Task<IRestResponse> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(Execute, cancellationToken, TaskCreationOptions.DenyChildAttach,
                TaskScheduler.Default);
        }

        public IRestResponse Execute()
        {
            SerializeRequestBody();

            HttpWebRequest httpClient = BuildWebClient();

            return Method switch
            {
                Method.COPY => ExecutePost(httpClient),
                Method.PUT => ExecutePost(httpClient),
                Method.PATCH => ExecutePost(httpClient),
                Method.MERGE => ExecutePost(httpClient),
                Method.POST => ExecutePost(httpClient),
                _ => ExecuteGet(httpClient)
            };
        }

        public Task<TType> ExecuteAsync<TType>(CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(Execute<TType>, cancellationToken, TaskCreationOptions.DenyChildAttach,
                TaskScheduler.Default);
        }

        public TType Execute<TType>()
        {
            IRestResponse r = Execute();

            return r.GetResult<TType>();
        }


        private IRestResponse ExecuteGet(HttpWebRequest webRequest)
        {
            Parameter parameter = Parameters.FirstOrDefault(i => i.ParameterType == ParameterType.RequestBody);
            if (parameter != null)
            {
                if (Method is Method.DELETE or Method.OPTIONS)
                {
                    webRequest.ContentType = parameter.ContentType;
                    string boundary = "---------" + DateTime.Now.Ticks.ToString("X");
                    WriteContent(webRequest, boundary);
                }
            }

            return GetResponse(webRequest);
        }

        private IRestResponse ExecutePost(HttpWebRequest webRequest)
        {
            string boundary = "---------" + DateTime.Now.Ticks.ToString("X");

            PrepareContent(webRequest, boundary);
            WriteContent(webRequest, boundary);
            return GetResponse(webRequest);
        }


        private void PrepareContent(HttpWebRequest webRequest, string boundary)
        {
            bool needsContentType = string.IsNullOrEmpty(webRequest.ContentType);

            if (RequestFiles.Count > 0 || AlwaysMultipartFormData)
            {
                if (needsContentType)
                {
                    webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
                }
                else if (!webRequest.ContentType.Contains("boundary"))
                {
                    webRequest.ContentType = webRequest.ContentType + "; boundary=" + boundary;
                }
            }
            else if (Parameters.Any(i => i.ParameterType == ParameterType.RequestBody))
            {
                Parameter f = Parameters.First(i => i.ParameterType == ParameterType.RequestBody);
                webRequest.ContentType = f.ContentType;
            }
            else if (Parameters.Count > 0)
            {
                webRequest.ContentType = "application/x-www-form-urlencoded";

                string content = string.Join("&", Parameters.Select(p => $"{p.Name}={p.Value}"));

                RequestBody = new RequestBody(webRequest.ContentType, "", content);

            }
        }


        private void WriteContent(WebRequest webRequest, string boundary)
        {
            Stream requestStream = null;

            try
            {
                if (RequestFiles.Count > 0 || AlwaysMultipartFormData)
                {
                    long length = 0L;
                    List<byte[]> buffers = CollectParameterBuffers(boundary);
                    ICollection<RequestFile> files = CollectFileBuffers(boundary);
                    byte[] endBuffer = Encoding.GetBytes($"--{boundary}--{LineBreak}");
                    length += buffers.Sum(i => i.Length);
                    length += files.Count > 0 ? files.Sum(i => i.BufferLength) : 0;
                    length += endBuffer.Length;
                    webRequest.ContentLength = length;

                    requestStream = webRequest.GetRequestStream();


                    foreach (byte[] buffer in buffers)
                    {
                        requestStream.Write(buffer, 0, buffer.Length);
                    }

                    buffers.Clear();

                    foreach (RequestFile file in files)
                    {
                        requestStream.Write(file.HeaderBuffer, 0, file.HeaderBuffer.Length);
                        file.FileStream.CopyTo(requestStream);
                        requestStream.Write(file.EndBuffer, 0, file.EndBuffer.Length);
                    }
                    files.ForEachAsync(i => i.Dispose()).NoAwaiter();

                    requestStream.Write(endBuffer, 0, endBuffer.Length);
                    endBuffer = null;

                    return;
                }

                if (RequestBody != null)
                {
                    byte[] buffer = Encoding.GetBytes(RequestBody.Value.ToString());
                    webRequest.ContentLength = buffer.Length;

                    requestStream = webRequest.GetRequestStream();

                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = null; ;
                }
            }
            finally
            {
                requestStream?.Dispose();
            }
        }


        private List<byte[]> CollectParameterBuffers(string boundary)
        {
            Encoding encoding = Encoding;
            List<byte[]> buffers = new();

            const string format1 = "--{0}{3}Content-Type: {4}{3}Content-Disposition: form-data; name=\"{1}\"{3}{3}{2}{3}";
            const string format2 = "--{0}{3}Content-Disposition: form-data; name=\"{1}\"{3}{3}{2}{3}";


            foreach (Parameter parameter in Parameters.OrderBy(i => i.Ticks).ToArray())
            {
                string format = parameter.ParameterType == ParameterType.RequestBody
                    ? format1
                    : format2;

                object value = parameter.Value;
                if (parameter.ParameterType == ParameterType.RequestBody)
                {
                    value = Serializer(parameter.Value);
                }

                format = string.Format(format, boundary, parameter.Name, value, LineBreak, parameter.Name);

                byte[] buffer = encoding.GetBytes(format);

                buffers.Add(buffer);
            }

            return buffers;
        }


        private ICollection<RequestFile> CollectFileBuffers(string boundary)
        {

            if (RequestFiles.Count <= 0)
            {
                return new List<RequestFile>();
            }

            Encoding encoding = Encoding;

            StringBuilder fileBuilder = new();
            foreach (RequestFile file in RequestFiles)
            {
                fileBuilder.Append("--")
                    .Append(boundary)
                    .Append(LineBreak)
                    .Append("Content-Disposition: form-data;")
                    .Append($" name=\"{Path.GetFileName(file.FileName)}\";")
                    .Append($" filename=\"{file.FileName}\"")
                    .Append(LineBreak)
                    .Append($"Content-Type: {file.ContentType}")
                    .Append(LineBreak)
                    .Append(LineBreak);

                file.HeaderBuffer = encoding.GetBytes(fileBuilder.ToString());

                file.EndBuffer = encoding.GetBytes(LineBreak);

                fileBuilder.Clear();
            }

            return RequestFiles;
        }


        private void AddConfig(HttpWebRequest request)
        {
            Type type = request.GetType();
            foreach (KeyValuePair<string, object> itemConfig in RequestConfigs)
            {
                if (!ConfigPropertyInfos.TryGetValue(itemConfig.Key, out PropertyInfo propertyInfo))
                {
                    ConfigPropertyInfos[itemConfig.Key] = propertyInfo = type.GetProperty(itemConfig.Key);
                }

                propertyInfo?.SetValue(request, itemConfig.Value);
            }
        }

        private void SerializeRequestBody()
        {

            Parameter body = Parameters.FirstOrDefault(i => i.ParameterType == ParameterType.RequestBody);

            if (body is null)
            {
                return;
            }

            string bodyValue = Serializer(body.Value);

            string contentType = body.ContentType;

            RequestBody = new RequestBody(contentType, contentType, bodyValue);

        }
    }
}