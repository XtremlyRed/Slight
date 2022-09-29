
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
namespace Slight.Core
{

    [DebuggerDisplay("{BuildURI()}   {Method}")]

    internal partial class RestRequest : IRestRequest
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal int MillisecondsTimeout;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal Encoding Encoding;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal string HostUri;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal Func<object, string> Serializer;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal Func<string, Type, object> Deserializer;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal Func<byte[], byte[]> Decoder;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal RequestBody RequestBody;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal bool AlwaysMultipartFormData;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] internal Action<IFileWriter> FileWriter;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private const string LineBreak = "\r\n";
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly List<Parameter> Parameters = new();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly List<RequestFile> RequestFiles = new();

        public Method Method { get; private set; } = Method.GET;
        public string RequestUrl { get; internal set; }
        // public string HostUri { get; internal set; }

        public IRestRequest AddParameter<TParam>(string key, TParam parameter)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (parameter is null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            Parameter p = new(key, parameter, ContentType.FromDataFormat[DataFormat.Json])
            {
                ParameterType = ParameterType.GetOrPost
            };

            Parameters.Add(p);
            return this;
        }

        public IRestRequest AddParameters(IEnumerable<KeyValuePair<string, object>> parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            foreach (KeyValuePair<string, object> item in parameters)
            {
                if (string.IsNullOrWhiteSpace(item.Key))
                {
                    throw new ArgumentNullException(nameof(item.Key));
                }

                Parameter p = new(item.Key, item.Value, ContentType.FromDataFormat[DataFormat.Json])
                {
                    ParameterType = ParameterType.GetOrPost
                };
                Parameters.Add(p);
            }


            return this;
        }

        public IRestRequest AddParameter<TParam>(TParam parameter, DataFormat dataFormat = DataFormat.Json)
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            Parameter p = new("", parameter, ContentType.FromDataFormat[dataFormat])
            {
                ParameterType = ParameterType.RequestBody
            };

            Parameters.Add(p);
            return this;
        }


        public IRestRequest AddFile(string fileName)
        {
            return string.IsNullOrWhiteSpace(fileName)
                ? throw new ArgumentNullException(nameof(fileName))
                : !File.Exists(fileName)
                ? throw new ArgumentException("file not exist", nameof(fileName))
                : AddFile(fileName, File.OpenRead(fileName));
        }

        public IRestRequest AddFiles(IEnumerable<string> filePaths)
        {
            if (filePaths is null)
            {
                throw new ArgumentNullException(nameof(filePaths));
            }

            foreach (string filePath in filePaths)
            {
                AddFile(filePath);
            }

            return this;
        }

        public IRestRequest AddFile(string fileName, Stream stream)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (!File.Exists(fileName))
            {
                throw new ArgumentException("file does not exist", nameof(fileName));
            }

            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (stream.CanRead == false)
            {
                throw new ArgumentException("Stream can not read", nameof(stream));
            }

            if (stream.CanSeek == false)
            {
                throw new ArgumentException("Stream can not seek", nameof(stream));
            }

            stream.Seek(0, SeekOrigin.Begin);

            RequestFiles.Add(new RequestFile
            {
                FileName = fileName,
                FileStream = stream
            });

            return this;
        }

        public IRestRequest AddHeader(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            Parameter p = new(name, value)
            {
                ParameterType = ParameterType.HttpHeader
            };
            Parameters.Add(p);
            return this;

        }

        public IRestRequest AddCookie(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }


            Parameter p = new(name, value)
            {
                ParameterType = ParameterType.Cookie
            };
            Parameters.Add(p);
            return this;
        }


        public void Dispose()
        {

        }

        ~RestRequest()
        {
            Dispose();
        }
    }
}