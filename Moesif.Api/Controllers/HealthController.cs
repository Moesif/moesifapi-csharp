/*
 * MoesifAPI.PCL
 *

 */
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moesif.Api;
using Moesif.Api.Http.Request;
using Moesif.Api.Http.Response;
using Moesif.Api.Http.Client;
using Moesif.Api.Exceptions;
using Moesif.Api.Models;

namespace Moesif.Api.Controllers
{
    public partial class HealthController: BaseController
    {
        #region Singleton Pattern

        //private static variables for the singleton pattern
        private static object syncObject = new object();
        private static HealthController instance = null;

        /// <summary>
        /// Singleton pattern implementation
        /// </summary>
        internal static HealthController Instance
        {
            get
            {
                lock (syncObject)
                {
                    if (null == instance)
                    {
                        instance = new HealthController();
                    }
                }
                return instance;
            }
        }

        #endregion Singleton Pattern

        /// <summary>
        /// Health Probe
        /// </summary>
        /// <return>Returns the ModelsStatusModel response from the API call</return>
        public StatusModel GetHealthProbe()
        {
            Task<StatusModel> t = GetHealthProbeAsync();
            Task.WaitAll(t);
            return t.Result;
        }

        /// <summary>
        /// Health Probe
        /// </summary>
        /// <return>Returns the ModelsStatusModel response from the API call</return>
        public async Task<StatusModel> GetHealthProbeAsync()
        {
            //the base uri for api requestss
            string _baseUri = Configuration.BaseUri;

            //prepare query string for API call
            StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            _queryBuilder.Append("/health/probe");


            //validate and preprocess url
            string _queryUrl = ApiHelper.CleanUrl(_queryBuilder);

            //append request with appropriate headers and parameters
            var _headers = new Dictionary<string,string>()
            {
                { "accept", "application/json" }
            };
            _headers.Add("X-Moesif-Application-Id", Configuration.ApplicationId);

            //prepare the API call request to fetch the response
            HttpRequest _request = ClientInstance.Get(_queryUrl,_headers);

            //invoke request and get response
            HttpStringResponse _response = (HttpStringResponse) await ClientInstance.ExecuteAsStringAsync(_request);
            HttpContext _context = new HttpContext(_request,_response);
            //handle errors defined at the API level
            base.ValidateResponse(_response, _context);

            try
            {
                return ApiHelper.JsonDeserialize<StatusModel>(_response.Body);
            }
            catch (Exception _ex)
            {
                throw new APIException("Failed to parse the response: " + _ex.Message, _context);
            }
        }

    }
} 