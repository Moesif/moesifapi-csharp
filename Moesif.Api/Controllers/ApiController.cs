/*
 * MoesifAPI.PCL
 *

 */
//#define MOESIF_INSTRUMENT

using System;
using System.Collections.Generic;
// using System.Dynamic;
// using System.Globalization;
// using System.IO;
// using System.Linq;
using System.Text;
using System.Threading.Tasks;
// using Moesif.Api;
using Moesif.Api.Http.Request;
using Moesif.Api.Http.Response;
using Moesif.Api.Http.Client;
// using Moesif.Api.Exceptions;
using Moesif.Api.Models;

#if MOESIF_INSTRUMENT
using System.Diagnostics;
#endif

namespace Moesif.Api.Controllers
{
    public partial class ApiController : BaseController
    {
        #region Singleton Pattern

        //private static variables for the singleton pattern
        private static object syncObject = new object();
        private static ApiController instance = null;
        private static string configUrl       = "/v1/config";
        private static string rulesUrl        = "/v1/rules";
        private static string eventUrl        = "/v1/events";
        private static string eventBatchUrl   = "/v1/events/batch";
        private static string companyUrl      = "/v1/companies";
        private static string companyBatchUrl = "/v1/companies/batch";
        private static string userUrl         = "/v1/users";
        private static string userBatchUrl    = "/v1/users/batch";

        /// <summary>
        /// Singleton pattern implementation
        /// </summary>
        internal static ApiController Instance
        {
            get
            {
                lock (syncObject)
                {
                    if (null == instance)
                    {
                        instance = new ApiController();
                        configUrl       = ApiHelper.CleanUrl(new StringBuilder(Configuration.BaseUri).Append("/v1/config"));
                        rulesUrl        = ApiHelper.CleanUrl(new StringBuilder(Configuration.BaseUri).Append("/v1/rules"));
                        eventUrl        = ApiHelper.CleanUrl(new StringBuilder(Configuration.BaseUri).Append("/v1/events"));
                        eventBatchUrl   = ApiHelper.CleanUrl(new StringBuilder(Configuration.BaseUri).Append("/v1/events/batch"));
                        companyUrl      = ApiHelper.CleanUrl(new StringBuilder(Configuration.BaseUri).Append("/v1/companies"));
                        companyBatchUrl = ApiHelper.CleanUrl(new StringBuilder(Configuration.BaseUri).Append("/v1/companies/batch"));
                        userUrl         = ApiHelper.CleanUrl(new StringBuilder(Configuration.BaseUri).Append("/v1/users"));
                        userBatchUrl    = ApiHelper.CleanUrl(new StringBuilder(Configuration.BaseUri).Append("/v1/users/batch"));
                    }
                }
                return instance;
            }
        }

        #endregion Singleton Pattern

        /// <summary>
        /// Add Single API Event Call
        /// </summary>
        /// <param name="body">Required parameter: Example: </param>
        /// <return>Returns the void response from the API call</return>
        public void CreateEvent(EventModel body)
        {
            Task t = CreateEventAsync(body);
            Task.WaitAll(t);
        }
        
        /// <summary>
        /// Add Single API Event Call
        /// </summary>
        /// <param name="body">Required parameter: Example: </param>
        /// <param name="waitForResponse">Optional parameter: Default: true </param>
        /// <return>Returns the void response from the API call</return>
        public async Task<Dictionary<string, string>> CreateEventAsync(EventModel body, bool waitForResponse = true)
        {
            
#if MOESIF_INSTRUMENT
            var logStage = true;
            var perfMetrics = new PerformanceMetrics("CreateEventAsync", logStage);
            perfMetrics.Start("prepareReqUrlQueryHeaders");
#endif
            // //the base uri for api requests
            // string _baseUri = Configuration.BaseUri;
            //
            // //prepare query string for API call
            // StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            // _queryBuilder.Append("/v1/events");
            //
            //
            // //validate and preprocess url : REVIEW why it is not build / verified early
            // string _queryUrl = ApiHelper.CleanUrl(_queryBuilder);
            
            string _queryUrl = ApiController.eventUrl;

            //append request with appropriate headers and parameters : REVIEW is this constant? if so, create early?
            var _headers = new Dictionary<string, string>()
            {
                { "content-type", "application/json; charset=utf-8" }
            };
            _headers.Add("X-Moesif-Application-Id", Configuration.ApplicationId);
#if MOESIF_INSTRUMENT
            perfMetrics.StopPreviousStartNew("prepareReqBody");
#endif

            //append body params
            var _body = ApiHelper.JsonSerialize(body);
#if MOESIF_INSTRUMENT
            perfMetrics.StopPreviousStartNew("prepareReq");
#endif
            //prepare the API call request to fetch the response
            HttpRequest _request = ClientInstance.PostBody(_queryUrl, _headers, _body);
#if MOESIF_INSTRUMENT
            perfMetrics.StopPreviousStartNew("ExecuteAsStringAsync");
#endif

            //invoke request and get response if needed
            if (!waitForResponse)
            {
#if MOESIF_INSTRUMENT
                Console.WriteLine("Current UTC time BEFORE executeAsStringAsync: " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"));
#endif
                ClientInstance.ExecuteAsStringAsync(_request, waitForResponse); // FINDME : REVIEW
#if MOESIF_INSTRUMENT
                //await Task.Run(async () => Task.Delay(2000));
                perfMetrics.Stop();
                perfMetrics.PrintMetrics(Console.WriteLine);
#endif
                return new Dictionary<string, string>();
            }
            else
            {
                HttpStringResponse _response = (HttpStringResponse) await ClientInstance.ExecuteAsStringAsync(_request, waitForResponse);
                HttpContext _context = new HttpContext(_request, _response);
                //handle errors defined at the API level
                base.ValidateResponse(_response, _context);
#if MOESIF_INSTRUMENT
                perfMetrics.Stop();
                perfMetrics.PrintMetrics(Console.WriteLine);
#endif
                // Return response headers
                return _response.Headers;
            }
        }

        /// <summary>
        /// Update Single User API Call
        /// </summary>
        /// <param name="body">Required parameter: Example: </param>
        /// <return>Returns the void response from the API call</return>
        public void UpdateUser(UserModel body)
        {
            Task t = UpdateUserAsync(body);
            Task.WaitAll(t);
        }

        /// <summary>
        /// Update Single User API Call
        /// </summary>
        /// <param name="body">Required parameter: Example: </param>
        /// <return>Returns the void response from the API call</return>
        public async Task UpdateUserAsync(UserModel body)
        {
            // //the base uri for api requestss
            // string _baseUri = Configuration.BaseUri;
            //
            // //prepare query string for API call
            // StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            // _queryBuilder.Append("/v1/users");
            //
            // //validate and preprocess url
            // string _queryUrl = ApiHelper.CleanUrl(_queryBuilder);
            
            string _queryUrl = ApiController.userUrl;

            //append request with appropriate headers and parameters
            var _headers = new Dictionary<string, string>()
            {
                { "content-type", "application/json; charset=utf-8" }
            };
            _headers.Add("X-Moesif-Application-Id", Configuration.ApplicationId);

            //append body params
            var _body = ApiHelper.JsonSerialize(body);

            //prepare the API call request to fetch the response
            HttpRequest _request = ClientInstance.PostBody(_queryUrl, _headers, _body);

            //invoke request and get response
            HttpStringResponse _response = (HttpStringResponse)await ClientInstance.ExecuteAsStringAsync(_request);
            HttpContext _context = new HttpContext(_request, _response);
            //handle errors defined at the API level
            base.ValidateResponse(_response, _context);

        }

        /// <summary>
        /// Update multiple Users in a single batch (batch size must be less than 250kb)
        /// </summary>
        /// <param name="body">Required parameter: Example: </param>
        /// <return>Returns the void response from the API call</return>
        public void UpdateUsersBatch(List<UserModel> body)
        {
            Task t = UpdateUsersBatchAsync(body);
            Task.WaitAll(t);
        }

        /// <summary>
        /// Update multiple Users in a single batch (batch size must be less than 250kb)
        /// </summary>
        /// <param name="body">Required parameter: Example: </param>
        /// <return>Returns the void response from the API call</return>
        public async Task UpdateUsersBatchAsync(List<UserModel> body)
        {
            // //the base uri for api requestss
            // string _baseUri = Configuration.BaseUri;
            //
            // //prepare query string for API call
            // StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            // _queryBuilder.Append("/v1/users/batch");
            //
            // //validate and preprocess url
            // string _queryUrl = ApiHelper.CleanUrl(_queryBuilder);
            
            string _queryUrl = ApiController.userBatchUrl;

            //append request with appropriate headers and parameters
            var _headers = new Dictionary<string, string>()
            {
                { "content-type", "application/json; charset=utf-8" }
            };
            _headers.Add("X-Moesif-Application-Id", Configuration.ApplicationId);

            //append body params
            var _body = ApiHelper.JsonSerialize(body);

            //prepare the API call request to fetch the response
            HttpRequest _request = ClientInstance.PostBody(_queryUrl, _headers, _body);

            //invoke request and get response
            HttpStringResponse _response = (HttpStringResponse)await ClientInstance.ExecuteAsStringAsync(_request);
            HttpContext _context = new HttpContext(_request, _response);
            //handle errors defined at the API level
            base.ValidateResponse(_response, _context);

        }

        /// <summary>
        /// Add multiple API Events in a single batch (batch size must be less than 250kb)
        /// </summary>
        /// <param name="body">Required parameter: Example: </param>
        /// <return>Returns the void response from the API call</return>
        public void CreateEventsBatch(List<EventModel> body)
        {
            Task t = CreateEventsBatchAsync(body);
            Task.WaitAll(t);
        }

        /// <summary>
        /// Add multiple API Events in a single batch (batch size must be less than 250kb)
        /// </summary>
        /// <param name="body">Required parameter: Example: </param>
        /// <return>Returns the void response from the API call</return>
        public async Task<Dictionary<string, string>> CreateEventsBatchAsync(List<EventModel> body)
        {
            // //the base uri for api requestss
            // string _baseUri = Configuration.BaseUri;
            //
            // //prepare query string for API call
            // StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            // _queryBuilder.Append("/v1/events/batch");
            
            string _queryUrl = ApiController.eventBatchUrl;

            if (Configuration.Debug)
            {
                if (body == null)
                {
                    Console.WriteLine("Body before serialization is null");
                }
                else
                {
                    foreach (EventModel eventData in body)
                    {
                        if (eventData == null)
                        {
                            Console.WriteLine("Body before serialization contains null elements");
                            break;
                        }
                    }
                }
            }


            // //validate and preprocess url
            // string _queryUrl = ApiHelper.CleanUrl(_queryBuilder);

            //append request with appropriate headers and parameters
            var _headers = new Dictionary<string, string>()
            {
                { "content-type", "application/json; charset=utf-8" }
            };
            _headers.Add("X-Moesif-Application-Id", Configuration.ApplicationId);

            //append body params
            var _body = ApiHelper.JsonSerialize(body);

            if (Configuration.Debug)
            {
                Console.WriteLine("Serialized body before sending - " + _body);
            }

            //prepare the API call request to fetch the response
            HttpRequest _request = ClientInstance.PostBody(_queryUrl, _headers, _body);

            //invoke request and get response
            HttpStringResponse _response = (HttpStringResponse)await ClientInstance.ExecuteAsStringAsync(_request);
            HttpContext _context = new HttpContext(_request, _response);
            //handle errors defined at the API level
            base.ValidateResponse(_response, _context);

            // Return response headers
            return _response.Headers;
        }

        /// <summary>
        /// Get AppConfig
        /// </summary>
        /// <param name="">Required parameter: Example: </param>
        /// <return>Returns the response from the API call</return>
        public void GetAppConfig()
        {
            Task t = GetAppConfigAsync();
            Task.WaitAll(t);
        }

        /// <summary>
        /// Get AppConfig
        /// </summary>
        /// <param name="">Required parameter: Example: </param>
        /// <return>Returns the void response from the API call</return>
        public async Task<HttpStringResponse> GetAppConfigAsync()
        {
            // //the base uri for api requestss
            // string _baseUri = Configuration.BaseUri;
            //
            // //prepare query string for API call
            // StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            // _queryBuilder.Append("/v1/config");
            //
            // //validate and preprocess url
            // string _queryUrl = ApiHelper.CleanUrl(_queryBuilder);
            string _queryUrl = ApiController.configUrl;

            //append request with appropriate headers and parameters
            var _headers = new Dictionary<string, string>()
            {
                { "x-moesif-application-id", Configuration.ApplicationId }
            };

            //prepare the API call request to fetch the response
            HttpRequest _request = ClientInstance.Get(_queryUrl, _headers);

            //invoke request and get response
            HttpStringResponse _response = (HttpStringResponse)await ClientInstance.ExecuteAsStringAsync(_request);
            HttpContext _context = new HttpContext(_request, _response);
            //handle errors defined at the API level
            base.ValidateResponse(_response, _context);

            //return response
            return _response;
        }

        /// <summary>
        /// Get GovernanceRule
        /// </summary>
        /// <param name="">Required parameter: Example: </param>
        /// <return>Returns the response from the API call</return>
        public void GetGovernanceRule()
        {
            Task t = GetGovernanceRuleAsync();
            Task.WaitAll(t);
        }

        /// <summary>
        /// Get GoveranceRule
        /// </summary>
        /// <param name="">Required parameter: Example: </param>
        /// <return>Returns the void response from the API call</return>
        public async Task<HttpStringResponse> GetGovernanceRuleAsync()
        {
            // //the base uri for api requestss
            // string _baseUri = Configuration.BaseUri;
            //
            // //prepare query string for API call
            // StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            // _queryBuilder.Append("/v1/rules");
            //
            // //validate and preprocess url
            // string _queryUrl = ApiHelper.CleanUrl(_queryBuilder);
            string _queryUrl = ApiController.rulesUrl;

            //append request with appropriate headers and parameters
            var _headers = new Dictionary<string, string>()
            {
                { "x-moesif-application-id", Configuration.ApplicationId }
            };

            //prepare the API call request to fetch the response
            HttpRequest _request = ClientInstance.Get(_queryUrl, _headers);

            //invoke request and get response
            HttpStringResponse _response = (HttpStringResponse)await ClientInstance.ExecuteAsStringAsync(_request);
            HttpContext _context = new HttpContext(_request, _response);
            //handle errors defined at the API level
            base.ValidateResponse(_response, _context);

            //return response
            return _response;
        }

        /// <summary>
        /// Update Single Company API Call
        /// </summary>
        /// <param name="body">Required parameter: Example: </param>
        /// <return>Returns the void response from the API call</return>
        public void UpdateCompany(CompanyModel body)
        {
            Task t = UpdateCompanyAsync(body);
            Task.WaitAll(t);
        }

        /// <summary>
        /// Update Single Company API Call
        /// </summary>
        /// <param name="body">Required parameter: Example: </param>
        /// <return>Returns the void response from the API call</return>
        public async Task UpdateCompanyAsync(CompanyModel body)
        {
            // //the base uri for api requestss
            // string _baseUri = Configuration.BaseUri;
            //
            // //prepare query string for API call
            // StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            // _queryBuilder.Append("/v1/companies");
            //
            // //validate and preprocess url
            // string _queryUrl = ApiHelper.CleanUrl(_queryBuilder);
            string _queryUrl = ApiController.companyUrl;
#if MOESIF_INSTRUMENT
            Console.WriteLine("_queryUrl");
            Console.WriteLine(_queryUrl);
#endif
            //append request with appropriate headers and parameters
            var _headers = new Dictionary<string, string>()
            {
                { "content-type", "application/json; charset=utf-8" }
            };
            _headers.Add("X-Moesif-Application-Id", Configuration.ApplicationId);
#if MOESIF_INSTRUMENT
            Console.WriteLine("_headers");
            Console.WriteLine(_headers);
#endif
            //append body params
            var _body = ApiHelper.JsonSerialize(body);

            //prepare the API call request to fetch the response
            HttpRequest _request = ClientInstance.PostBody(_queryUrl, _headers, _body);

            //invoke request and get response
            HttpStringResponse _response = (HttpStringResponse)await ClientInstance.ExecuteAsStringAsync(_request);
            HttpContext _context = new HttpContext(_request, _response);
            //handle errors defined at the API level
            base.ValidateResponse(_response, _context);

        }

        /// <summary>
        /// Update multiple companies in a single batch (batch size must be less than 250kb)
        /// </summary>
        /// <param name="body">Required parameter: Example: </param>
        /// <return>Returns the void response from the API call</return>
        public void UpdateCompaniesBatch(List<CompanyModel> body)
        {
            Task t = UpdateCompaniesBatchAsync(body);
            Task.WaitAll(t);
        }

        /// <summary>
        /// Update multiple Users in a single batch (batch size must be less than 250kb)
        /// </summary>
        /// <param name="body">Required parameter: Example: </param>
        /// <return>Returns the void response from the API call</return>
        public async Task UpdateCompaniesBatchAsync(List<CompanyModel> body)
        {
            // //the base uri for api requestss
            // string _baseUri = Configuration.BaseUri;
            //
            // //prepare query string for API call
            // StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            // _queryBuilder.Append("/v1/companies/batch");
            //
            // //validate and preprocess url
            // string _queryUrl = ApiHelper.CleanUrl(_queryBuilder);
            
            string _queryUrl = ApiController.companyBatchUrl;

            //append request with appropriate headers and parameters
            var _headers = new Dictionary<string, string>()
            {
                { "content-type", "application/json; charset=utf-8" }
            };
            _headers.Add("X-Moesif-Application-Id", Configuration.ApplicationId);

            //append body params
            var _body = ApiHelper.JsonSerialize(body);

            //prepare the API call request to fetch the response
            HttpRequest _request = ClientInstance.PostBody(_queryUrl, _headers, _body);

            //invoke request and get response
            HttpStringResponse _response = (HttpStringResponse)await ClientInstance.ExecuteAsStringAsync(_request);
            HttpContext _context = new HttpContext(_request, _response);
            //handle errors defined at the API level
            base.ValidateResponse(_response, _context);

        }
    }
}
