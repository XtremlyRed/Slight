using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;

namespace Slight.Core
{
    internal partial class RestRequest
    {
        public IRestRequest UseFileWriter(Action<IFileWriter> fileWriterFunc)
        {


            if (fileWriterFunc is null)
            {
                throw new ArgumentNullException(nameof(fileWriterFunc));
            }

            FileWriter = fileWriterFunc;

            return this;
        }


        public IRestRequest UseUrl(string url)
        {
            if (url is null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            RequestUrl = url;
            return this;
        }


        public IRestRequest UseAlwaysMultipartFormData(bool alwaysMultipartFormData)
        {
            AlwaysMultipartFormData = alwaysMultipartFormData;

            return this;
        }


        private HttpWebRequest BuildWebClient()
        {
            string baseUri = BuildURI();

            HttpWebRequest client = (HttpWebRequest)WebRequest.Create(baseUri);

            client.Method = Method.GET.ToString().ToUpper();
            client.AllowAutoRedirect = true;
            client.AllowWriteStreamBuffering = true;
            client.Timeout = MillisecondsTimeout;

            Invoker.TryRun(() =>
            {
                client.ServicePoint.Expect100Continue = false;
                client.ServicePoint.UseNagleAlgorithm = false;
            }, e => { });

            AssemblyName named = Assembly.GetExecutingAssembly().GetName();
            client.UserAgent = $"{named.Name} v{named.Version}";
            AddConfig(client);


            Invoker.TryRun(() =>
            {
                // var dict=Enum.GetValues(typeof(HttpRequestHeader)).Cast<HttpRequestHeader>().ToDictionary(i => i, i => i.ToString());

                client.Headers[HttpRequestHeader.Accept] = "*/*";

                List<Parameter> parameters = Parameters.Where(i => i.ParameterType == ParameterType.HttpHeader)
                    .ToList();
                foreach (Parameter header in parameters)
                {
                    try
                    {
                        client.Headers.Add(header.Name, header.Value?.ToString() ?? "");
                    }
                    catch
                    {

                    }
                }
            }, e => { });

            client.CookieContainer ??= new CookieContainer();
            string uri = HostUri;
            List<Parameter> cookies = Parameters.Where(i => i.ParameterType == ParameterType.Cookie).ToList();
            foreach (Parameter cookie in cookies)
            {
                client.CookieContainer.Add(new System.Net.Cookie
                {
                    Name = cookie.Name,
                    Value = cookie.Value?.ToString() ?? "",
                    Domain = uri
                });
            }

            Invoker.TryRun(() =>
            {
                client.Proxy ??= WebRequest.DefaultWebProxy ?? WebRequest.GetSystemWebProxy();
            }, e => { });

            return client;
        }

        private string BuildURI()
        {
            string targetUrl = RequestUrl;


            if (string.IsNullOrWhiteSpace(targetUrl))
            {
                throw new ArgumentNullException(nameof(targetUrl));
            }

            string baseUri = HostUri;

            baseUri = baseUri.EndsWith("/") && targetUrl.StartsWith("/")
                ? $"{baseUri}{targetUrl.Substring(1)}"
                : baseUri.EndsWith("/") || targetUrl.StartsWith("/") ? $"{baseUri}{targetUrl}" : $"{HostUri}/{targetUrl}";

            ICollection<Parameter> parameters = GetQueryStringParameters();

            if (parameters.Count == 0)
            {
                return baseUri;
            }


            string @params = string.Join("&", parameters.Select(i => $"{i.Name}={i.Value}").ToArray());

            baseUri = $"{baseUri}?{@params}";

            return baseUri;

            ICollection<Parameter> GetQueryStringParameters()
            {
                Method method = Method;

                bool flag = method is not Method.POST and not Method.PUT and not Method.PATCH;

                return flag
                    ? Parameters.Where(
                        p => p.ParameterType is ParameterType.GetOrPost or
                             ParameterType.QueryString or
                             ParameterType.QueryStringWithoutEncode
                    ).ToArray()
                    : (ICollection<Parameter>)Parameters
                    .Where(
                        p => p.ParameterType is ParameterType.QueryString or
                             ParameterType.QueryStringWithoutEncode
                    ).ToArray();
            }
        }


        private RestResponse GetResponse(HttpWebRequest webRequest)
        {
            RestResponse resp = new()
            {
                ResponseStream = null,
                FileWriterException = null,
                CanGetResult = false,
                Decoder = Decoder,
                Encoding = Encoding,
                Deserializer = Deserializer,
                HttpWebResponse = null,
                FileWriterFunc = FileWriter
            };



            try
            {
                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
                resp.SuccessResponse(response);
                return resp;
            }
            catch (WebException webEx)
            {
                if (webEx.Response is HttpWebResponse webResp)
                {
                    resp.SuccessResponse(webResp);
                    return resp;
                }

                resp.ErrorResponse(webEx);

            }
            catch (Exception ex)
            {
                resp.ErrorResponse(ex);
            }

            return resp;
        }
    }
}