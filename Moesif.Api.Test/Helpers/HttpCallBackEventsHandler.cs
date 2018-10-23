/*
 * MoesifAPI.Tests
 *

 */
using Moesif.Api.Http.Client;
using Moesif.Api.Http.Request;
using Moesif.Api.Http.Response;

namespace Moesif.Api.Test.Helpers
{
    public class HttpCallBackEventsHandler
    {
        public HttpRequest Request { get; private set; }

        public HttpResponse Response { get; private set; }

        public void OnBeforeHttpRequestEventHandler(IHttpClient source, HttpRequest request)
        {
            this.Request = request;
        }

        public void OnAfterHttpResponseEventHandler(IHttpClient source, HttpResponse response)
        {
            this.Response = response;
        }
    }
}
