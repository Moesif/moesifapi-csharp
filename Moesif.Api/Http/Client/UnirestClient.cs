#define MOESIF_INSTRUMENT

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Moesif.Api.Http.Request;
using Moesif.Api.Http.Response;

#if NET6_0_OR_GREATER

// using unirest_net.http;
// using UniHttpRequest = unirest_net.request.HttpRequest;
using UniHttp = System.Net.Http;
using HttpRequestMessage = System.Net.Http.HttpRequestMessage;
using HttpResponseMessage = System.Net.Http.HttpResponseMessage;
// using System.Net.Http;

// dotnet add package Microsoft.Extensions.Http
// using Microsoft.Extensions.Http;

namespace Moesif.Api.Http.Client
{
    public class UnirestClient: IHttpClient
    {
        // private readonly IHttpClientFactory _httpClientFactory;
        //
        // public UnirestClient(IHttpClientFactory httpClientFactory)
        // {
        //     _httpClientFactory = httpClientFactory;
        // }

        public static IHttpClient SharedClient { get; set; }

        private static string Version;
        // private readonly HttpClient _httpClient = null;
        private static UniHttp.HttpClientHandler handler = new UniHttp.HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = UniHttp.HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
        private static readonly UniHttp.HttpClient httpClient = new UniHttp.HttpClient(handler);

        static UnirestClient() {
            SharedClient = new UnirestClient();
            Version = Assembly.GetExecutingAssembly().FullName;
            InitClientDefaults(httpClient, Configuration.BaseUri);
        }

        public static void InitClientDefaults(UniHttp.HttpClient client, string baseUrl)
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("ContentType", "application/json; charset=utf-8");
            client.DefaultRequestHeaders.Add("X-Moesif-Application-Id", Configuration.ApplicationId);
            // Add other default headers as needed
            // Set timeout, retries.
        }

        #region Execute methods

        // public HttpResponse ExecuteAsString(HttpRequest request)
        // {
        //     //raise the on before request event
        //     raiseOnBeforeHttpRequestEvent(request);
        //
        //     UniHttpRequest uniRequest = ConvertRequest(request);
        //     HttpResponse response = ConvertResponse(uniRequest.asString());
        //
        //     //raise the on after response event
        //     raiseOnAfterHttpResponseEvent(response);
        //     return response;
        // }
        //
        public HttpResponse ExecuteAsString(HttpRequest request)
        {
            try
            {
                // Use Task.Run to execute the async method and wait for it to complete
                return Task.Run(() => ExecuteAsStringAsync(request)).GetAwaiter().GetResult();
            }
            catch (AggregateException ae)
            {
                // Handle or rethrow the inner exception
                throw ae.InnerException;
            }
        }

        // public Task<HttpResponse> ExecuteAsStringAsync(HttpRequest request)
        // {
        //     return Task.Factory.StartNew(() => ExecuteAsString(request));
        // }
        //
        public async Task<HttpResponse> ExecuteAsStringAsync(HttpRequest request, bool waitForResponse = true)
        {
            HttpStringResponse httpResponse;
#if MOESIF_INSTRUMENT
            var logStage = true;
            var perfMetrics = new PerformanceMetrics("ExecuteAsStringAsync", logStage);
            perfMetrics.Start("raiseOnBefore");
            Console.WriteLine($"ExecuteAsStringAsync / waitForResponse = {waitForResponse}");
#endif
            raiseOnBeforeHttpRequestEvent(request);

#if MOESIF_INSTRUMENT
            perfMetrics.StopPreviousStartNew("ExecuteRequestAsync");            
#endif

            if (waitForResponse)
            {
                var response = await ExecuteRequestAsync(request);
#if MOESIF_INSTRUMENT
                perfMetrics.StopPreviousStartNew("ReadAsStringAsync");            
#endif
                var stringResponse = await response.Content.ReadAsStringAsync();
                httpResponse = new HttpStringResponse
                {
                    Headers = response.Headers.ToDictionary(h => h.Key, h => string.Join(",", h.Value)),
                    Body = stringResponse,
                    StatusCode = (int)response.StatusCode
                };
            }
            else
            {
                ExecuteRequestAsync(request);
#if MOESIF_INSTRUMENT
                perfMetrics.StopPreviousStartNew("ReadAsStringAsync");            
#endif
                httpResponse  = new HttpStringResponse();
            }

#if MOESIF_INSTRUMENT
            perfMetrics.StopPreviousStartNew("raiseOnAfter");            
#endif

            raiseOnAfterHttpResponseEvent(httpResponse);

#if MOESIF_INSTRUMENT
            perfMetrics.Stop();
            perfMetrics.PrintMetrics(Console.WriteLine);
#endif
            return httpResponse;
        }

        // public HttpResponse ExecuteAsBinary(HttpRequest request)
        // {
        //     //raise the on before request event
        //     raiseOnBeforeHttpRequestEvent(request);
        //
        //     UniHttpRequest uniRequest = ConvertRequest(request);
        //     HttpResponse response = ConvertResponse(uniRequest.asBinary());
        //
        //     //raise the on after response event
        //     raiseOnAfterHttpResponseEvent(response);
        //     return response;
        // }
        public HttpResponse ExecuteAsBinary(HttpRequest request)
        {
            try
            {
                // Use Task.Run to execute the async method and wait for it to complete
                return Task.Run(() => ExecuteAsBinaryAsync(request)).GetAwaiter().GetResult();
            }
            catch (AggregateException ae)
            {
                // Handle or rethrow the inner exception
                throw ae.InnerException;
            }
        }
        //
        // public Task<HttpResponse> ExecuteAsBinaryAsync(HttpRequest request)
        // {
        //     return Task.Factory.StartNew(() => ExecuteAsString(request));
        // }
        public async Task<HttpResponse> ExecuteAsBinaryAsync(HttpRequest request)
        {
            raiseOnBeforeHttpRequestEvent(request);

            var response = await ExecuteRequestAsync(request);
            var binaryResponse = await response.Content.ReadAsByteArrayAsync();

            var httpResponse = new HttpResponse
            {
                Headers = response.Headers.ToDictionary(h => h.Key, h => string.Join(",", h.Value)),
                RawBody = new MemoryStream(binaryResponse),
                StatusCode = (int)response.StatusCode
            };

            raiseOnAfterHttpResponseEvent(httpResponse);
            return httpResponse;
        }

        private async Task<HttpResponseMessage> ExecuteRequestAsync(HttpRequest request)
        {
            var httpRequestMessage = new HttpRequestMessage(new System.Net.Http.HttpMethod(request.HttpMethod.ToString()), request.QueryUrl);

            if (request.Headers != null)
            {
                foreach (var header in request.Headers)
                {
                    httpRequestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            if (request.Body != null)
            {
                httpRequestMessage.Content = new UniHttp.StringContent(request.Body);
            }
            else if (request.FormParameters != null)
            {
                if (request.FormParameters.Any(p => p.Value is Stream || p.Value is FileStreamInfo))
                {
                    var content = new UniHttp.MultipartFormDataContent();
                    foreach (var kvp in request.FormParameters)
                    {
                        if (kvp.Value is FileStreamInfo fileInfo)
                        {
                            content.Add(new UniHttp.StreamContent(fileInfo.FileStream), kvp.Key, fileInfo.FileName);
                        }
                        else
                        {
                            content.Add(new UniHttp.StringContent(kvp.Value.ToString()), kvp.Key);
                        }
                    }
                    httpRequestMessage.Content = content;
                }
                else
                {
                    var content = new UniHttp.FormUrlEncodedContent(request.FormParameters.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString()));
                    httpRequestMessage.Content = content;
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                var byteArray = System.Text.Encoding.ASCII.GetBytes($"{request.Username}:{request.Password}");
                httpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            }

            return await httpClient.SendAsync(httpRequestMessage);
        }

        #endregion


        #region Http request and response events

        public event OnBeforeHttpRequestEventHandler OnBeforeHttpRequestEvent;
        public event OnAfterHttpResponseEventHandler OnAfterHttpResponseEvent;

        // private void raiseOnBeforeHttpRequestEvent(HttpRequest request)
        // {
        //     if ((null != OnBeforeHttpRequestEvent) && (null != request))
        //         OnBeforeHttpRequestEvent(this, request);
        // }
        private void raiseOnBeforeHttpRequestEvent(HttpRequest request)
        {
            OnBeforeHttpRequestEvent?.Invoke(this, request);
        }

        // private void raiseOnAfterHttpResponseEvent(HttpResponse response)
        // {
        //     if ((null != OnAfterHttpResponseEvent) && (null != response))
        //         OnAfterHttpResponseEvent(this, response);
        // }
        private void raiseOnAfterHttpResponseEvent(HttpResponse response)
        {
            OnAfterHttpResponseEvent?.Invoke(this, response);
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

        private static UniHttp.HttpMethod ConvertHttpMethod(HttpMethod method)
        {
            return method switch
            {
                HttpMethod.GET => System.Net.Http.HttpMethod.Get,
                HttpMethod.POST => System.Net.Http.HttpMethod.Post,
                HttpMethod.PUT => System.Net.Http.HttpMethod.Put,
                HttpMethod.PATCH => System.Net.Http.HttpMethod.Patch,
                HttpMethod.DELETE => System.Net.Http.HttpMethod.Delete,
                _ => throw new ArgumentOutOfRangeException("Unkown method" + method.ToString())
            };
        }

        // private static UniHttpRequest ConvertRequest(HttpRequest request)
        // {
        //     var uniMethod = ConvertHttpMethod(request.HttpMethod);
        //     var queryUrl = request.QueryUrl;
        //
        //     //instantiate unirest request object
        //     HttpRequest uniRequest = new HttpRequest(uniMethod,queryUrl);
        //     uniRequest.TimeOut = TimeSpan.FromSeconds(10);
        //
        //     //set request payload
        //     if (request.Body != null)
        //     {
        //         uniRequest.body(request.Body);
        //     }
        //     else if (request.FormParameters != null)
        //     {
        //         if (request.FormParameters.Any(p => p.Value is Stream || p.Value is FileStreamInfo))
        //         {
        //             //multipart
        //             foreach (var kvp in request.FormParameters)
        //             {
        //                 if (kvp.Value is FileStreamInfo){
        //                     var fileInfo = (FileStreamInfo) kvp.Value;
        //                     uniRequest.field(kvp.Key, fileInfo.FileStream, fileInfo.FileName, fileInfo.ContentType);
        //                     continue;
        //                 }
        //                 uniRequest.field(kvp.Key,kvp.Value);
        //             }
        //         }
        //         else
        //         {
        //             //URL Encode params
        //             var paramsString = string.Join("&",
        //                 request.FormParameters.Select(kvp =>
        //                 string.Format("{0}={1}", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value.ToString()))));
        //             uniRequest.body(paramsString);
        //             uniRequest.header("Content-Type", "application/x-www-form-urlencoded");
        //         }
        //     }
        //
        //     //set request headers
        //     Dictionary<string, Object> headers = request.Headers.ToDictionary(item=> item.Key,item=> (Object) item.Value);
        //     uniRequest.headers(headers);
        //     if (Configuration.UserAgentString != null)
        //     {
        //         uniRequest.header("user-agent", Configuration.UserAgentString);
        //     }
        //     else
        //     {
        //         uniRequest.header("user-agent", "moesifapi-csharp/" + Version);
        //     }
        //
        //     //Set basic auth credentials if any
        //     if (!string.IsNullOrWhiteSpace(request.Username))
        //     {
        //         uniRequest.basicAuth(request.Username, request.Password);
        //     }
        //
        //     return uniRequest;
        // }
        //
        // private static HttpResponse ConvertResponse(HttpResponse<Stream> binaryResponse)
        // {
        //     return new HttpResponse
        //     {
        //         Headers = binaryResponse.Headers,
        //         RawBody = binaryResponse.Body,
        //         StatusCode = binaryResponse.Code
        //     };
        // }
        //
        // private static HttpResponse ConvertResponse(HttpResponse<string> stringResponse)
        // {
        //     return new HttpStringResponse
        //     {
        //         Headers = stringResponse.Headers,
        //         RawBody = stringResponse.Raw,
        //         Body = stringResponse.Body,
        //         StatusCode = stringResponse.Code
        //     };
        // }

        #endregion
    }
}

#else

//using System.Linq;
//using System.Threading.Tasks;
//using Moesif.Api;
//using Moesif.Api.Http.Request;
//using Moesif.Api.Http.Response;
using unirest_net.http;
using UniHttpRequest = unirest_net.request.HttpRequest;
using UniHttpMethod = System.Net.Http.HttpMethod;
//using System.Reflection;


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

        public Task<HttpResponse> ExecuteAsStringAsync(HttpRequest request, bool waitForResponse = true)
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
            if (Configuration.UserAgentString != null)
            {
                uniRequest.header("user-agent", Configuration.UserAgentString);
            }
            else
            {
                uniRequest.header("user-agent", "moesifapi-csharp/" + Version);
            }

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
#endif
