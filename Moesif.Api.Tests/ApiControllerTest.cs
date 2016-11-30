/*
 * MoesifAPI.Tests
 *

 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using Moesif.Api;
using Moesif.Api.Controllers;
using Moesif.Api.Models;
using Moesif.Api.Exceptions;
using Moesif.Api.Http.Client;
using Moesif.Api.Helpers;

namespace Moesif.Api
{
    [TestFixture]
    public class ApiControllerTest : ControllerTestBase
    {
        /// <summary>
        /// Controller instance (for all tests)
        /// </summary>
        private static ApiController controller;

        /// <summary>
        /// Setup test class
        /// </summary>
        [SetUp]
        public static void SetUpClass()
        {
            controller = GetClient().Api;
        }

        /// <summary>
        /// Add Single Event via Injestion API
        /// </summary>
        [Test]
        public async Task TestAddEvent() 
        {
            // Parameters for the API call
            var reqHeaders = new Dictionary<string, string>();
            reqHeaders.Add("Host", "api.acmeinc.com");
            reqHeaders.Add("Accept", "*/*");
            reqHeaders.Add("Connection", "Keep-Alive");
            reqHeaders.Add("User-Agent", "Dalvik/2.1.0 (Linux; U; Android 5.0.2; C6906 Build/14.5.A.0.242)");
            reqHeaders.Add("Content-Type", "application/json");
            reqHeaders.Add("Content-Length", "126");
            reqHeaders.Add("Accept-Encoding", "gzip");

            var reqBody = ApiHelper.JsonDeserialize<object>(@" {
    			    ""items"": [
    				    {
    					    ""type"": 1,
    					    ""id"": ""fwfrf""
                        },
    				    {
    					    ""type"": 2,
    					    ""id"": ""d43d3f""
    				    }
    			    ]
    		    }");

            var rspHeaders = new Dictionary<string, string>();
            rspHeaders.Add("Date", "Tue, 23 Nov 2016 23:46:49 GMT");
            rspHeaders.Add("Vary", "Accept-Encoding");
            rspHeaders.Add("Pragma", "no-cache");
            rspHeaders.Add("Expires", "-1");
            rspHeaders.Add("Content-Type", "application/json; charset=utf-8");
            rspHeaders.Add("Cache-Control", "no-cache");

            var rspBody = ApiHelper.JsonDeserialize<object>(@" {
    			    ""Error"": ""InvalidArgumentException"",
    			    ""Message"": ""Missing field field_a""
    		    }");


            var eventReq = new EventRequestModel()
            {
                Time = DateTime.Now.AddSeconds(-1),
                Uri = "https://api.acmeinc.com/items/reviews/",
                Verb = "PATCH",
                ApiVersion = "1.1.0",
                IpAddress = "61.48.220.123",
                Headers = reqHeaders,
                Body = reqBody
            };

            var eventRsp = new EventResponseModel()
            {
                Time = DateTime.Now,
                Status = 500,
                Headers = rspHeaders,
                Body = rspBody
            };

            var eventModel = new EventModel()
            {
                Request = eventReq,
                Response = eventRsp,
                UserId = "my_user_id",
                SessionToken = "23jdf0owekfmcn4u3qypxg09w4d8ayrcdx8nu2ng]s98y18cx98q3yhwmnhcfx43f"
            };

            // Perform API call

            try
            {
                await controller.CreateEventAsync(eventModel);
            }
            catch(APIException) {};

            // Test response code
            Assert.AreEqual(201, httpCallBackHandler.Response.StatusCode,
                    "Status should be 201");
        }

        /// <summary>
        /// Add Batched Events via Ingestion API
        /// </summary>
        [Test]
        public async Task TestAddBatchedEvents() 
        {
            // Parameters for the API call
            List<EventModel> body = ApiHelper.JsonDeserialize<List<EventModel>>("[{ 					\"request\": { 						\"time\": \"2016-09-09T04:45:42.914\", 						\"uri\": \"https://api.acmeinc.com/items/reviews/\", 						\"verb\": \"PATCH\", 						\"api_version\": \"1.1.0\", 						\"ip_address\": \"61.48.220.123\", 						\"headers\": { 							\"Host\": \"api.acmeinc.com\", 							\"Accept\": \"*/*\", 							\"Connection\": \"Keep-Alive\", 							\"User-Agent\": \"Dalvik/2.1.0 (Linux; U; Android 5.0.2; C6906 Build/14.5.A.0.242)\", 							\"Content-Type\": \"application/json\", 							\"Content-Length\": \"126\", 							\"Accept-Encoding\": \"gzip\" 						}, 						\"body\": { 							\"items\": [ 								{ 									\"direction_type\": 1, 									\"discovery_id\": \"fwfrf\", 									\"liked\": false 								}, 								{ 									\"direction_type\": 2, 									\"discovery_id\": \"d43d3f\", 									\"liked\": true 								} 							] 						} 					}, 					\"response\": { 						\"time\": \"2016-09-09T04:45:42.914\", 						\"status\": 500, 						\"headers\": { 							\"Date\": \"Tue, 23 Aug 2016 23:46:49 GMT\", 							\"Vary\": \"Accept-Encoding\", 							\"Pragma\": \"no-cache\", 							\"Expires\": \"-1\", 							\"Content-Type\": \"application/json; charset=utf-8\", 							\"X-Powered-By\": \"ARR/3.0\", 							\"Cache-Control\": \"no-cache\", 							\"Arr-Disable-Session-Affinity\": \"true\" 						}, 						\"body\": { 							\"Error\": \"InvalidArgumentException\", 							\"Message\": \"Missing field field_a\" 						} 					}, 					\"user_id\": \"mndug437f43\", 					\"session_token\": \"23jdf0owekfmcn4u3qypxg09w4d8ayrcdx8nu2ng]s98y18cx98q3yhwmnhcfx43f\" 					 }, { 					\"request\": { 						\"time\": \"2016-09-09T04:46:42.914\", 						\"uri\": \"https://api.acmeinc.com/items/reviews/\", 						\"verb\": \"PATCH\", 						\"api_version\": \"1.1.0\", 						\"ip_address\": \"61.48.220.123\", 						\"headers\": { 							\"Host\": \"api.acmeinc.com\", 							\"Accept\": \"*/*\", 							\"Connection\": \"Keep-Alive\", 							\"User-Agent\": \"Dalvik/2.1.0 (Linux; U; Android 5.0.2; C6906 Build/14.5.A.0.242)\", 							\"Content-Type\": \"application/json\", 							\"Content-Length\": \"126\", 							\"Accept-Encoding\": \"gzip\" 						}, 						\"body\": { 							\"items\": [ 								{ 									\"direction_type\": 1, 									\"discovery_id\": \"fwfrf\", 									\"liked\": false 								}, 								{ 									\"direction_type\": 2, 									\"discovery_id\": \"d43d3f\", 									\"liked\": true 								} 							] 						} 					}, 					\"response\": { 						\"time\": \"2016-09-09T04:46:42.914\", 						\"status\": 500, 						\"headers\": { 							\"Date\": \"Tue, 23 Aug 2016 23:46:49 GMT\", 							\"Vary\": \"Accept-Encoding\", 							\"Pragma\": \"no-cache\", 							\"Expires\": \"-1\", 							\"Content-Type\": \"application/json; charset=utf-8\", 							\"X-Powered-By\": \"ARR/3.0\", 							\"Cache-Control\": \"no-cache\", 							\"Arr-Disable-Session-Affinity\": \"true\" 						}, 						\"body\": { 							\"Error\": \"InvalidArgumentException\", 							\"Message\": \"Missing field field_a\" 						} 					}, 					\"user_id\": \"mndug437f43\", 					\"session_token\": \"23jdf0owekfmcn4u3qypxg09w4d8ayrcdx8nu2ng]s98y18cx98q3yhwmnhcfx43f\" 					 }, { 					\"request\": { 						\"time\": \"2016-09-09T04:47:42.914\", 						\"uri\": \"https://api.acmeinc.com/items/reviews/\", 						\"verb\": \"PATCH\", 						\"api_version\": \"1.1.0\", 						\"ip_address\": \"61.48.220.123\", 						\"headers\": { 							\"Host\": \"api.acmeinc.com\", 							\"Accept\": \"*/*\", 							\"Connection\": \"Keep-Alive\", 							\"User-Agent\": \"Dalvik/2.1.0 (Linux; U; Android 5.0.2; C6906 Build/14.5.A.0.242)\", 							\"Content-Type\": \"application/json\", 							\"Content-Length\": \"126\", 							\"Accept-Encoding\": \"gzip\" 						}, 						\"body\": { 							\"items\": [ 								{ 									\"direction_type\": 1, 									\"discovery_id\": \"fwfrf\", 									\"liked\": false 								}, 								{ 									\"direction_type\": 2, 									\"discovery_id\": \"d43d3f\", 									\"liked\": true 								} 							] 						} 					}, 					\"response\": { 						\"time\": \"2016-09-09T04:47:42.914\", 						\"status\": 500, 						\"headers\": { 							\"Date\": \"Tue, 23 Aug 2016 23:46:49 GMT\", 							\"Vary\": \"Accept-Encoding\", 							\"Pragma\": \"no-cache\", 							\"Expires\": \"-1\", 							\"Content-Type\": \"application/json; charset=utf-8\", 							\"X-Powered-By\": \"ARR/3.0\", 							\"Cache-Control\": \"no-cache\", 							\"Arr-Disable-Session-Affinity\": \"true\" 						}, 						\"body\": { 							\"Error\": \"InvalidArgumentException\", 							\"Message\": \"Missing field field_a\" 						} 					}, 					\"user_id\": \"mndug437f43\", 					\"session_token\": \"23jdf0owekfmcn4u3qypxg09w4d8ayrcdx8nu2ng]s98y18cx98q3yhwmnhcfx43f\" 					 }, { 					\"request\": { 						\"time\": \"2016-09-09T04:48:42.914\", 						\"uri\": \"https://api.acmeinc.com/items/reviews/\", 						\"verb\": \"PATCH\", 						\"api_version\": \"1.1.0\", 						\"ip_address\": \"61.48.220.123\", 						\"headers\": { 							\"Host\": \"api.acmeinc.com\", 							\"Accept\": \"*/*\", 							\"Connection\": \"Keep-Alive\", 							\"User-Agent\": \"Dalvik/2.1.0 (Linux; U; Android 5.0.2; C6906 Build/14.5.A.0.242)\", 							\"Content-Type\": \"application/json\", 							\"Content-Length\": \"126\", 							\"Accept-Encoding\": \"gzip\" 						}, 						\"body\": { 							\"items\": [ 								{ 									\"direction_type\": 1, 									\"discovery_id\": \"fwfrf\", 									\"liked\": false 								}, 								{ 									\"direction_type\": 2, 									\"discovery_id\": \"d43d3f\", 									\"liked\": true 								} 							] 						} 					}, 					\"response\": { 						\"time\": \"2016-09-09T04:48:42.914\", 						\"status\": 500, 						\"headers\": { 							\"Date\": \"Tue, 23 Aug 2016 23:46:49 GMT\", 							\"Vary\": \"Accept-Encoding\", 							\"Pragma\": \"no-cache\", 							\"Expires\": \"-1\", 							\"Content-Type\": \"application/json; charset=utf-8\", 							\"X-Powered-By\": \"ARR/3.0\", 							\"Cache-Control\": \"no-cache\", 							\"Arr-Disable-Session-Affinity\": \"true\" 						}, 						\"body\": { 							\"Error\": \"InvalidArgumentException\", 							\"Message\": \"Missing field field_a\" 						} 					}, 					\"user_id\": \"mndug437f43\", 					\"session_token\": \"exfzweachxjgznvKUYrxFcxv]s98y18cx98q3yhwmnhcfx43f\" 					 }, { 					\"request\": { 						\"time\": \"2016-09-09T04:49:42.914\", 						\"uri\": \"https://api.acmeinc.com/items/reviews/\", 						\"verb\": \"PATCH\", 						\"api_version\": \"1.1.0\", 						\"ip_address\": \"61.48.220.123\", 						\"headers\": { 							\"Host\": \"api.acmeinc.com\", 							\"Accept\": \"*/*\", 							\"Connection\": \"Keep-Alive\", 							\"User-Agent\": \"Dalvik/2.1.0 (Linux; U; Android 5.0.2; C6906 Build/14.5.A.0.242)\", 							\"Content-Type\": \"application/json\", 							\"Content-Length\": \"126\", 							\"Accept-Encoding\": \"gzip\" 						}, 						\"body\": { 							\"items\": [ 								{ 									\"direction_type\": 1, 									\"discovery_id\": \"fwfrf\", 									\"liked\": false 								}, 								{ 									\"direction_type\": 2, 									\"discovery_id\": \"d43d3f\", 									\"liked\": true 								} 							] 						} 					}, 					\"response\": { 						\"time\": \"2016-09-09T04:49:42.914\", 						\"status\": 500, 						\"headers\": { 							\"Date\": \"Tue, 23 Aug 2016 23:46:49 GMT\", 							\"Vary\": \"Accept-Encoding\", 							\"Pragma\": \"no-cache\", 							\"Expires\": \"-1\", 							\"Content-Type\": \"application/json; charset=utf-8\", 							\"X-Powered-By\": \"ARR/3.0\", 							\"Cache-Control\": \"no-cache\", 							\"Arr-Disable-Session-Affinity\": \"true\" 						}, 						\"body\": { 							\"Error\": \"InvalidArgumentException\", 							\"Message\": \"Missing field field_a\" 						} 					}, 					\"user_id\": \"mndug437f43\", 					\"session_token\": \"23jdf0owekfmcn4u3qypxg09w4d8ayrcdx8nu2ng]s98y18cx98q3yhwmnhcfx43f\" 					 }, { 					\"request\": { 						\"time\": \"2016-09-09T04:50:42.914\", 						\"uri\": \"https://api.acmeinc.com/items/reviews/\", 						\"verb\": \"PATCH\", 						\"api_version\": \"1.1.0\", 						\"ip_address\": \"61.48.220.123\", 						\"headers\": { 							\"Host\": \"api.acmeinc.com\", 							\"Accept\": \"*/*\", 							\"Connection\": \"Keep-Alive\", 							\"User-Agent\": \"Dalvik/2.1.0 (Linux; U; Android 5.0.2; C6906 Build/14.5.A.0.242)\", 							\"Content-Type\": \"application/json\", 							\"Content-Length\": \"126\", 							\"Accept-Encoding\": \"gzip\" 						}, 						\"body\": { 							\"items\": [ 								{ 									\"direction_type\": 1, 									\"discovery_id\": \"fwfrf\", 									\"liked\": false 								}, 								{ 									\"direction_type\": 2, 									\"discovery_id\": \"d43d3f\", 									\"liked\": true 								} 							] 						} 					}, 					\"response\": { 						\"time\": \"2016-09-09T04:50:42.914\", 						\"status\": 500, 						\"headers\": { 							\"Date\": \"Tue, 23 Aug 2016 23:46:49 GMT\", 							\"Vary\": \"Accept-Encoding\", 							\"Pragma\": \"no-cache\", 							\"Expires\": \"-1\", 							\"Content-Type\": \"application/json; charset=utf-8\", 							\"X-Powered-By\": \"ARR/3.0\", 							\"Cache-Control\": \"no-cache\", 							\"Arr-Disable-Session-Affinity\": \"true\" 						}, 						\"body\": { 							\"Error\": \"InvalidArgumentException\", 							\"Message\": \"Missing field field_a\" 						} 					}, 					\"user_id\": \"recvreedfef\", 					\"session_token\": \"xcvkrjmcfghwuignrmcmhxdhaaezse4w]s98y18cx98q3yhwmnhcfx43f\" 					 } ]");

            // Perform API call

            try
            {
                await controller.CreateEventsBatchAsync(body);
            }
            catch(APIException) {};

            // Test response code
            Assert.AreEqual(201, httpCallBackHandler.Response.StatusCode,
                    "Status should be 201");
        }

    }
}
