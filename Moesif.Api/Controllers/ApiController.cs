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
    public partial class ApiController: BaseController
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
        /// <return>Returns the void response from the API call</return>
        public async Task<Dictionary<string, string>> CreateEventAsync(EventModel body)
        {
            //the base uri for api requestss
            string _baseUri = Configuration.BaseUri;

            //prepare query string for API call
            StringBuilder _queryBuilder = new StringBuilder(_baseUri);
            _queryBuilder.Append("/v1/events");


            //validate and preprocess url
            string _queryUrl = ApiHelper.CleanUrl(_queryBuilder);

            //append request with appropriate headers and parameters
            var _headers = new Dictionary<string,string>()
            {
                { "content-type", "application/json; charset=utf-8" }
            };
            _headers.Add("X-Moesif-Application-Id", Configuration.ApplicationId);

            //append body params
            var _body = ApiHelper.JsonSerialize(body);

            //prepare the API call request to fetch the response
            HttpRequest _request = ClientInstance.PostBody(_queryUrl, _headers, _body);

            //invoke request and get response
            HttpStringResponse _response = (HttpStringResponse) await ClientInstance.ExecuteAsStringAsync(_request);
            HttpContext _context = new HttpContext(_request,_response);
            //handle errors defined at the API level
            base.ValidateResponse(_response, _context);

            // Return response headers
            return _response.Headers;
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
            HttpStringResponse _response = (HttpStringResponse) await ClientInstance.ExecuteAsStringAsync(_request);
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


            //validate and preprocess url
            string _queryUrl = ApiHelper.CleanUrl(_queryBuilder);

            //append request with appropriate headers and parameters
            var _headers = new Dictionary<string,string>()
            {
                { "content-type", "application/json; charset=utf-8" }
            };
            _headers.Add("X-Moesif-Application-Id", Configuration.ApplicationId);

            //append body params
            var _body = ApiHelper.JsonSerialize(body);

            //prepare the API call request to fetch the response
            HttpRequest _request = ClientInstance.PostBody(_queryUrl, _headers, _body);

            //invoke request and get response
            HttpStringResponse _response = (HttpStringResponse) await ClientInstance.ExecuteAsStringAsync(_request);
            HttpContext _context = new HttpContext(_request,_response);
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