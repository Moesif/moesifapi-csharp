/*
 * MoesifAPI.PCL
 *

 */
#define MOESIF_INSTRUMENT

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
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            long prepareReqUrlQueryHeaders = 0;
            long prepareReqBody = 0;
            long prepareReq = 0;
            long executeReq = 0;
#endif
            //the base uri for api requestss
            string _baseUri = Configuration.BaseUri;

            //prepare query string for API call
            StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            _queryBuilder.Append("/v1/events");


            //validate and preprocess url
            string _queryUrl = ApiHelper.CleanUrl(_queryBuilder);

            //append request with appropriate headers and parameters
            var _headers = new Dictionary<string, string>()
            {
                { "content-type", "application/json; charset=utf-8" }
            };
            _headers.Add("X-Moesif-Application-Id", Configuration.ApplicationId);
#if MOESIF_INSTRUMENT
            prepareReqUrlQueryHeaders = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();
#endif

            //append body params
            var _body = ApiHelper.JsonSerialize(body);
#if MOESIF_INSTRUMENT
            prepareReqBody = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();
#endif
            //prepare the API call request to fetch the response
            HttpRequest _request = ClientInstance.PostBody(_queryUrl, _headers, _body);
#if MOESIF_INSTRUMENT
            prepareReq = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();
#endif

            //invoke request and get response if needed
            if (!waitForResponse)
            {
#if MOESIF_INSTRUMENT
                Console.WriteLine("Current UTC time BEFORE executeAsStringAsync: " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"));
#endif
                await ClientInstance.ExecuteAsStringAsync(_request);
#if MOESIF_INSTRUMENT
                //await Task.Run(async () => Task.Delay(2000));
                Console.WriteLine("Current UTC time AFTER executeAsStringAsync: " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                executeReq = stopwatch.ElapsedMilliseconds;
                stopwatch.Stop();
                Console.WriteLine("Current UTC time BEFORE CreateEventAsync return: " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                string strHeader = string.Concat(
                            "CreateEventAsync,",
                            "prepareReqUrlQueryHeaders,",
                            "prepareReqBody,",
                            "prepareReq,",
                            "executeReq"
                            );
                string strTimes = string.Concat(
                            $"{prepareReqUrlQueryHeaders + prepareReqBody + prepareReq + executeReq + stopwatch.ElapsedMilliseconds},",
                            $"{prepareReqUrlQueryHeaders},",
                            $"{prepareReqBody},",
                            $"{prepareReq},",
                            $"{executeReq}"
                            );
                Console.WriteLine($@"
                    {strHeader}
                    {strTimes}
                ");
                // Console.WriteLine($@"
                //             Exiting CreateEventAsync with time: {prepareReqUrlQueryHeaders + prepareReqBody + prepareReq + executeReq + stopwatch.ElapsedMilliseconds} ms
                //             prepareReqUrlQueryHeaders took: {prepareReqUrlQueryHeaders} ms
                //             prepareReqBody took: {prepareReqBody} ms
                //             prepareReq took: {prepareReq} ms
                //             executeReq took: {executeReq} ms");
#endif
                return new Dictionary<string, string>();
            }
            else
            {
                HttpStringResponse _response = (HttpStringResponse)await ClientInstance.ExecuteAsStringAsync(_request);
                HttpContext _context = new HttpContext(_request, _response);
                //handle errors defined at the API level
                base.ValidateResponse(_response, _context);

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
            //the base uri for api requestss
            string _baseUri = Configuration.BaseUri;

            //prepare query string for API call
            StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            _queryBuilder.Append("/v1/users");

            //validate and preprocess url
            string _queryUrl = ApiHelper.CleanUrl(_queryBuilder);

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
            //the base uri for api requestss
            string _baseUri = Configuration.BaseUri;

            //prepare query string for API call
            StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            _queryBuilder.Append("/v1/users/batch");

            //validate and preprocess url
            string _queryUrl = ApiHelper.CleanUrl(_queryBuilder);

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
            //the base uri for api requestss
            string _baseUri = Configuration.BaseUri;

            //prepare query string for API call
            StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            _queryBuilder.Append("/v1/events/batch");

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


            //validate and preprocess url
            string _queryUrl = ApiHelper.CleanUrl(_queryBuilder);

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
            //the base uri for api requestss
            string _baseUri = Configuration.BaseUri;

            //prepare query string for API call
            StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            _queryBuilder.Append("/v1/config");

            //validate and preprocess url
            string _queryUrl = ApiHelper.CleanUrl(_queryBuilder);

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
            //the base uri for api requestss
            string _baseUri = Configuration.BaseUri;

            //prepare query string for API call
            StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            _queryBuilder.Append("/v1/rules");

            //validate and preprocess url
            string _queryUrl = ApiHelper.CleanUrl(_queryBuilder);

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
            //the base uri for api requestss
            string _baseUri = Configuration.BaseUri;

            //prepare query string for API call
            StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            _queryBuilder.Append("/v1/companies");

            //validate and preprocess url
            string _queryUrl = ApiHelper.CleanUrl(_queryBuilder);
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
            //the base uri for api requestss
            string _baseUri = Configuration.BaseUri;

            //prepare query string for API call
            StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            _queryBuilder.Append("/v1/companies/batch");

            //validate and preprocess url
            string _queryUrl = ApiHelper.CleanUrl(_queryBuilder);

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
