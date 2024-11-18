/*
 * MoesifAPI.PCL
 *

 */
using System;
using System.IO;
using System.Text;
using Moesif.Api.Http.Client;
using Moesif.Api.Http.Response;
using Moesif.Api.Exceptions;

#if NET6_0_OR_GREATER
    using System.Text.Json.Serialization;
#else
//    using Microsoft.AspNetCore.Mvc;
    using System.Net.Http;
    using System.Threading.Tasks;
#endif

namespace Moesif.Api.Controllers
{
    public partial class BaseController
    {
        #region shared http client instance
        private static object syncObject = new object();
        private static IHttpClient clientInstance = null;

        public static IHttpClient ClientInstance
        {
            get
            {
                lock(syncObject)
                {
                    if(null == clientInstance)
                    {
                        clientInstance = new UnirestClient();
                    }
                    return clientInstance;
                }
            }
            set
            {
                lock (syncObject)
                {
                    if (value is IHttpClient)
                    {
                        clientInstance = value;
                    }
                }
            }
        }
        #endregion shared http client instance
//#if NET451
//         protected readonly IHttpClientFactory _httpClientFactory;
//
//         public BaseController(IHttpClientFactory httpClientFactory)
//         {
//             _httpClientFactory = httpClientFactory;
//         }
//#endif

        /// <summary>
        /// Validates the response against HTTP errors defined at the API level
        /// </summary>
        /// <param name="_response">The response recieved</param>
        /// <param name="_context">Context of the request and the recieved response</param>
        internal void ValidateResponse(HttpResponse _response, HttpContext _context)
        {
            if ((_response.StatusCode < 200) || (_response.StatusCode > 206)) //[200,206] = HTTP OK
            {
                // Load the raw body from stream
                string bodyData = "";
                try
                {
                    using (Stream body = _response.RawBody)
                    {
                        Byte[] bytes = new byte[body.Length];
                        body.Position = 0;
                        body.Read(bytes, 0, (int)body.Length);
                        bodyData = Encoding.UTF8.GetString(bytes);
                    }
                }
                catch (Exception) { };

                throw new APIException($"HTTP Response Not OK [status: {_response.StatusCode}, error:{bodyData}]", _context);
            }
        }
    }
}