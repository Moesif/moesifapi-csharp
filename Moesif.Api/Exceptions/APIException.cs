/*
 * MoesifAPI.PCL
 *

 */
using System;
using System.IO;
// using Newtonsoft.Json;
// using System.Text;
using Moesif.Api.Http.Client;
using Moesif.Api.Models;

namespace Moesif.Api.Exceptions
{
    public class APIException : Exception
    {
        /// <summary>
        /// The HTTP response code from the API request
        /// </summary>
        public int ResponseCode
        {
            get { return this.HttpContext != null ? HttpContext.Response.StatusCode : -1; }
        }

        /// <summary>
        /// HttpContext stores the request and response
        /// </summary>
        public HttpContext HttpContext { get; set; }

        /// <summary>
        /// Initialization constructor
        /// </summary>
        /// <param name="reason"> The reason for throwing exception </param>
        /// <param name="context"> The HTTP context that encapsulates request and response objects </param>
        public APIException(string reason, HttpContext context)
            : base(reason)
        {
            this.HttpContext = context;

            //if a derived exception class is used, then perform deserialization of response body
            if ((this.GetType().Name.Equals("APIException", StringComparison.OrdinalIgnoreCase))
                || (context == null) || (context.Response == null)
                || (context.Response.RawBody == null)
                || (!context.Response.RawBody.CanRead))
                return;

            using (StreamReader reader = new StreamReader(context.Response.RawBody))
            {
                var responseBody = reader.ReadToEnd();
                if (!string.IsNullOrWhiteSpace(responseBody))
                {
                    try
                    {
                        // JsonConvert.PopulateObject(responseBody, this);
                        ApiHelper.JsonDeserialize<EventResponseModel>(responseBody);
                    }
                    catch { } //ignoring response body from deserailization
                }
            }
        }
    }
}