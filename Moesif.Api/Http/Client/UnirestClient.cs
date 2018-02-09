using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Moesif.Api.Http.Request;
using Moesif.Api.Http.Response;
using unirest_net.http;
using UniHttpRequest = unirest_net.request.HttpRequest;
using UniHttpMethod = System.Net.Http.HttpMethod;
using System.Reflection;
using System.Text;

namespace Moesif.Api.Http.Client
{
    public class UnirestClient: IHttpClient
    {
        public static IHttpClient SharedClient { get; set; }

        private static string Version;

        static UnirestClient() {
            SharedClient = new UnirestClient();
            Version = Assembly.GetExecutingAssembly().FullName;
        }

        #region Execute methods

        public HttpResponse ExecuteAsString(HttpRequest request)
        {
            //raise the on before request event
            raiseOnBeforeHttpRequestEvent(request);

            UniHttpRequest uniRequest = ConvertRequest(request);
            HttpResponse response = ConvertResponse(uniRequest.asString());

            //raise the on after response event
            raiseOnAfterHttpResponseEvent(response);
            return response;
        }

        public Task<HttpResponse> ExecuteAsStringAsync(HttpRequest request)
        {
            return Task.Factory.StartNew(() => ExecuteAsString(request));
        }

        public HttpResponse ExecuteAsBinary(HttpRequest request)
        {
            //raise the on before request event
            raiseOnBeforeHttpRequestEvent(request);

            UniHttpRequest uniRequest = ConvertRequest(request);
            HttpResponse response = ConvertResponse(uniRequest.asBinary());

            //raise the on after response event
            raiseOnAfterHttpResponseEvent(response);
            return response;
        }

        public Task<HttpResponse> ExecuteAsBinaryAsync(HttpRequest request)
        {
            return Task.Factory.StartNew(() => ExecuteAsString(request));
        }

        #endregion


        #region Http request and response events

        public event OnBeforeHttpRequestEventHandler OnBeforeHttpRequestEvent;
        public event OnAfterHttpResponseEventHandler OnAfterHttpResponseEvent;

        private void raiseOnBeforeHttpRequestEvent(HttpRequest request)
        {
            if ((null != OnBeforeHttpRequestEvent) && (null != request))
                OnBeforeHttpRequestEvent(this, request);
        }

        private void raiseOnAfterHttpResponseEvent(HttpResponse response)
        {
            if ((null != OnAfterHttpResponseEvent) && (null != response))
                OnAfterHttpResponseEvent(this, response);
        }

        #endregion


        #region Http methods

        public HttpRequest Get(string queryUrl, Dictionary<string, string> headers, string username = null, string password = null)
        {
            return new HttpRequest(HttpMethod.GET, queryUrl, headers, username, password);
        }

        public HttpRequest Get(string queryUrl)
        {
            return new HttpRequest(HttpMethod.GET, queryUrl);
        }

        public HttpRequest Post(string queryUrl)
        {
            return new HttpRequest(HttpMethod.POST, queryUrl);
        }

        public HttpRequest Put(string queryUrl)
        {
            return new HttpRequest(HttpMethod.PUT, queryUrl);
        }

        public HttpRequest Delete(string queryUrl)
        {
            return new HttpRequest(HttpMethod.DELETE, queryUrl);
        }

        public HttpRequest Patch(string queryUrl)
        {
            return new HttpRequest(HttpMethod.PATCH, queryUrl);
        }

        public HttpRequest Post(string queryUrl, Dictionary<string, string> headers, Dictionary<string, object> formParameters, string username = null,
            string password = null)
        {
            return new HttpRequest(HttpMethod.POST, queryUrl, headers, formParameters, username, password);
        }

        public HttpRequest PostBody(string queryUrl, Dictionary<string, string> headers, string body, string username = null, string password = null)
        {
            return new HttpRequest(HttpMethod.POST, queryUrl, headers, body, username, password);
        }

        public HttpRequest Put(string queryUrl, Dictionary<string, string> headers, Dictionary<string, object> formParameters, string username = null,
            string password = null)
        {
            return new HttpRequest(HttpMethod.PUT, queryUrl, headers, formParameters, username, password);
        }

        public HttpRequest PutBody(string queryUrl, Dictionary<string, string> headers, string body, string username = null, string password = null)
        {
            return new HttpRequest(HttpMethod.PUT, queryUrl, headers, body, username, password);
        }

        public HttpRequest Patch(string queryUrl, Dictionary<string, string> headers, Dictionary<string, object> formParameters, string username = null,
            string password = null)
        {
            return new HttpRequest(HttpMethod.PATCH, queryUrl, headers, formParameters, username, password);
        }

        public HttpRequest PatchBody(string queryUrl, Dictionary<string, string> headers, string body, string username = null, string password = null)
        {
            return new HttpRequest(HttpMethod.PATCH, queryUrl, headers, body, username, password);
        }

        public HttpRequest Delete(string queryUrl, Dictionary<string, string> headers, Dictionary<string, object> formParameters, string username = null,
            string password = null)
        {
            return new HttpRequest(HttpMethod.DELETE, queryUrl, headers, formParameters, username, password);
        }

        public HttpRequest DeleteBody(string queryUrl, Dictionary<string, string> headers, string body, string username = null, string password = null)
        {
            return new HttpRequest(HttpMethod.DELETE, queryUrl, headers, body, username, password);
        }

        #endregion

        #region Helper methods

        private static UniHttpMethod ConvertHttpMethod(HttpMethod method)
        {
            switch (method)
            {
                case HttpMethod.GET:
                case HttpMethod.POST:
                case HttpMethod.PUT:
                case HttpMethod.PATCH:
                case HttpMethod.DELETE:
                    return new UniHttpMethod(method.ToString().ToUpperInvariant());

                default:
                    throw new ArgumentOutOfRangeException("Unkown method" + method.ToString());
            }
        }

        private static UniHttpRequest ConvertRequest(HttpRequest request)
        {
            var uniMethod = ConvertHttpMethod(request.HttpMethod);
            var queryUrl = request.QueryUrl;
            
            //instantiate unirest request object
            UniHttpRequest uniRequest = new UniHttpRequest(uniMethod,queryUrl);
            uniRequest.TimeOut = TimeSpan.FromSeconds(10);

            //set request payload
            if (request.Body != null)
            {
                uniRequest.body(request.Body);
            }
            else if (request.FormParameters != null)
            {
                if (request.FormParameters.Any(p => p.Value is Stream || p.Value is FileStreamInfo))
                {
                    //multipart
                    foreach (var kvp in request.FormParameters)
                    {
                        if (kvp.Value is FileStreamInfo){
                            var fileInfo = (FileStreamInfo) kvp.Value;
                            uniRequest.field(kvp.Key, fileInfo.FileStream, fileInfo.FileName, fileInfo.ContentType);
                            continue;
                        }
                        uniRequest.field(kvp.Key,kvp.Value);
                    }
                }
                else
                {
                    //URL Encode params
                    var paramsString = string.Join("&",
                        request.FormParameters.Select(kvp =>
                        string.Format("{0}={1}", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value.ToString()))));
                    uniRequest.body(paramsString);
                    uniRequest.header("Content-Type", "application/x-www-form-urlencoded");
                }
            }

            //set request headers
            Dictionary<string, Object> headers = request.Headers.ToDictionary(item=> item.Key,item=> (Object) item.Value);
            uniRequest.headers(headers);
            uniRequest.header("user-agent", "moesifapi-csharp/" + Version);

            //Set basic auth credentials if any
            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                uniRequest.basicAuth(request.Username, request.Password);
            }

            return uniRequest;
        }

        private static HttpResponse ConvertResponse(HttpResponse<Stream> binaryResponse)
        {
            return new HttpResponse
            {
                Headers = binaryResponse.Headers,
                RawBody = binaryResponse.Body,
                StatusCode = binaryResponse.Code
            };
        }

        private static HttpResponse ConvertResponse(HttpResponse<string> stringResponse)
        {
            return new HttpStringResponse
            {
                Headers = stringResponse.Headers,
                RawBody = stringResponse.Raw,
                Body = stringResponse.Body,
                StatusCode = stringResponse.Code
            };
        }

        #endregion
    }
}
