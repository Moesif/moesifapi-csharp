using System;
using Xunit;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Moesif.Api;
using Moesif.Api.Controllers;
using Moesif.Api.Models;
using Moesif.Api.Exceptions;
using Moesif.Api.Http.Client;
using Moesif.Api.Test.Helpers;


namespace Moesif.Api.Test
{
    public class ApiControllerTest : ControllerTestBase
    {
        private static ApiController controller;

        [Fact]
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
                Time = DateTime.UtcNow.AddSeconds(-1),
                Uri = "https://api.acmeinc.com/items/reviews/",
                Verb = "PATCH",
                ApiVersion = "1.1.0",
                IpAddress = "61.48.220.123",
                Headers = reqHeaders,
                Body = reqBody
            };

            var eventRsp = new EventResponseModel()
            {
                Time = DateTime.UtcNow,
                Status = 200,
                Headers = rspHeaders,
                Body = rspBody
            };

            Dictionary<string, string> metadata = new Dictionary<string, string>
            {
                { "email", "abc@email.com" },
                { "name", "abcdef" },
                { "image", "123" }
            };

            var eventModel = new EventModel()
            {
                Request = eventReq,
                Response = eventRsp,
                UserId = "my_user_id",
                SessionToken = "23jdf0owekfmcn4u3qypxg09w4d8ayrcdx8nu2ng]s98y18cx98q3yhwmnhcfx43f",
                Metadata = metadata
            };

            Dictionary<string, string> rsp;
            // Perform API call
            try
            {
                controller = ControllerTestBase.GetClient().Api;
                rsp = await controller.CreateEventAsync(eventModel);
            }
            catch (APIException) { rsp = null; };

            // Test response code
            Assert.NotEqual(null, rsp);
        }

        /// <summary>
        /// Add Batched Events via Ingestion API
        /// </summary>
        [Fact]
        public async Task TestAddBatchedEvents()
        {
            // Parameters for the API call
            List<EventModel> body = ApiHelper.JsonDeserialize<List<EventModel>>("[{\"request\":{\"time\":\"2018-10-21T04:45:42.914\",\"uri\":\"https://api.acmeinc.com/items/reviews/testb\",\"verb\":\"PATCH\",\"api_version\":\"1.1.0\",\"ip_address\":\"61.48.220.123\",\"headers\":{\"Host\":\"api.acmeinc.com\",\"Accept\":\"*/*\",\"Connection\":\"Keep-Alive\",\"User-Agent\":\"Dalvik/2.1.0(Linux;U;Android5.0.2;C6906Build/14.5.A.0.242)\",\"Content-Type\":\"application/json\",\"Content-Length\":\"126\",\"Accept-Encoding\":\"gzip\"},\"body\":{\"items\":[{\"direction_type\":1,\"discovery_id\":\"fwfrf\",\"liked\":false},{\"direction_type\":2,\"discovery_id\":\"d43d3f\",\"liked\":true}]}},\"response\":{\"time\":\"2018-10-21T04:45:43.914\",\"status\":200,\"headers\":{\"Date\":\"Tue,23Aug201623:46:49GMT\",\"Vary\":\"Accept-Encoding\",\"Pragma\":\"no-cache\",\"Expires\":\"-1\",\"Content-Type\":\"application/json;charset=utf-8\",\"X-Powered-By\":\"ARR/3.0\",\"Cache-Control\":\"no-cache\",\"Arr-Disable-Session-Affinity\":\"true\"},\"body\":{\"Error\":\"InvalidArgumentException\",\"Message\":\"Missingfieldfield_a\"}},\"user_id\":\"mndug437f43\",\"session_token\":\"23jdf0owekfmcn4u3qypxg09w4d8ayrcdx8nu2ng]s98y18cx98q3yhwmnhcfx43f\"},{\"request\":{\"time\":\"2018-10-21T04:46:42.914\",\"uri\":\"https://api.acmeinc.com/items/reviews/testc\",\"verb\":\"PATCH\",\"api_version\":\"1.1.0\",\"ip_address\":\"61.48.220.123\",\"headers\":{\"Host\":\"api.acmeinc.com\",\"Accept\":\"*/*\",\"Connection\":\"Keep-Alive\",\"User-Agent\":\"Dalvik/2.1.0(Linux;U;Android5.0.2;C6906Build/14.5.A.0.242)\",\"Content-Type\":\"application/json\",\"Content-Length\":\"126\",\"Accept-Encoding\":\"gzip\"},\"body\":{\"items\":[{\"direction_type\":1,\"discovery_id\":\"fwfrf\",\"liked\":false},{\"direction_type\":2,\"discovery_id\":\"d43d3f\",\"liked\":true}]}},\"response\":{\"time\":\"2018-10-21T04:46:43.914\",\"status\":200,\"headers\":{\"Date\":\"Tue,23Aug201623:46:49GMT\",\"Vary\":\"Accept-Encoding\",\"Pragma\":\"no-cache\",\"Expires\":\"-1\",\"Content-Type\":\"application/json;charset=utf-8\",\"X-Powered-By\":\"ARR/3.0\",\"Cache-Control\":\"no-cache\",\"Arr-Disable-Session-Affinity\":\"true\"},\"body\":{\"Error\":\"InvalidArgumentException\",\"Message\":\"Missingfieldfield_a\"}},\"user_id\":\"mndug437f43\",\"session_token\":\"23jdf0owekfmcn4u3qypxg09w4d8ayrcdx8nu2ng]s98y18cx98q3yhwmnhcfx43f\"}]");

            Dictionary<string, string> rsp;
            // Perform API call
            try
            {
                controller = ControllerTestBase.GetClient().Api;
                rsp = await controller.CreateEventsBatchAsync(body);
            }
            catch (APIException) { rsp = null; };

            // Test response code
            Assert.NotEqual(null, rsp);
        }

        /// <summary>
        /// Get Application Config
        /// </summary>
        [Fact]
        public async Task TestGetAppConfig()
        {
            Http.Response.HttpStringResponse rsp;
            // Perform API call
            try
            {
                controller = ControllerTestBase.GetClient().Api;
                rsp = await controller.GetAppConfigAsync();
            }
            catch (APIException) { rsp = null; };

            // Test response code
            Assert.NotEqual(null, rsp);
        }

        [Fact]
        public async Task TestUpdateUser()
        {
            Boolean isUpdated = false;
            Dictionary<string, object> metadata = new Dictionary<string, object>
            {
                {"email", "johndoe@acmeinc.com"},
                {"string_field", "value_1"},
                {"number_field", 0},
                {"object_field", new Dictionary<string, string> {
                    {"field_a", "value_a"},
                    {"field_b", "value_b"}
                    }
                }
            };
         
            var userModel = new UserModel()
            {
                UserAgentString = "Dalvik/2.1.0 (Linux; U; Android 5.0.2; C6906 Build/14.5.A.0.242)",
                UserId = "my_user_id",
                Metadata = metadata,
                ModifiedTime = DateTime.UtcNow
            };

            // Perform API call
            try
            {
                controller = ControllerTestBase.GetClient().Api;
                await controller.UpdateUserAsync(userModel);
                isUpdated = true;
            }
            catch (APIException) { };

            // Test response code
            Assert.Equal(true, isUpdated);

        }

        [Fact]
        public async Task TestUpdateUsersBatch() 
        {
            Boolean isUpdated = false;
            // Parameters for the API call
            List<UserModel> body = ApiHelper.JsonDeserialize<List<UserModel>>("[{\"modified_time\":\"2018-10-30T21:35:06.432\",\"ip_address\":\"0:0:0:0:0:0:0:1\",\"session_token\":\"23jdf0oszfexfqe[lwjfiefovprewv4d8ayrcdx8nu2ng]zfeeadedefx43f\",\"user_id\":\"my_user_id\",\"user_agent_string\":\"\",\"metadata\":{\"email\":\"johndoe@acmeinc.com\",\"string_field\":\"value_1\",\"number_field\":0,\"object_field\":{\"field_a\":\"value_a\",\"field_b\":\"value_b\"}},\"org_id\":\"86:5\",\"app_id\":\"46:8\"},{\"modified_time\":\"2018-10-30T21:35:06.432\",\"ip_address\":\"0:0:0:0:0:0:0:1\",\"session_token\":\"23jdf0oszfexfqe[lwjfiefovprewv4d8ayrcdx8nu2ng]zfeeadedefx43f\",\"user_id\":\"abc_user\",\"user_agent_string\":\"\",\"metadata\":{\"email\":\"johndoe@acmeinc.com\",\"string_field\":\"value_1\",\"number_field\":0,\"object_field\":{\"field_a\":\"value_a\",\"field_b\":\"value_b\"}},\"org_id\":\"86:5\",\"app_id\":\"46:8\"}]");

            // Perform API call
            try
            {
                controller = ControllerTestBase.GetClient().Api;
                await controller.UpdateUsersBatchAsync(body);
                isUpdated = true;
            }
            catch (APIException) { };

            // Test response code
            Assert.Equal(true, isUpdated);
        }

        [Fact]
        public async Task TestUpdateCompany()
        {
            Boolean isUpdated = false;
            Dictionary<string, object> metadata = new Dictionary<string, object>
            {
                {"email", "johndoe@acmeinc.com"},
                {"string_field", "value_1"},
                {"number_field", 0},
                {"object_field", new Dictionary<string, string> {
                    {"field_a", "value_a"},
                    {"field_b", "value_b"}
                    }
                }
            };

            var companyModel = new CompanyModel()
            {
                CompanyId = "1",
                Metadata = metadata,
                ModifiedTime = DateTime.UtcNow
            };

            // Perform API call
            try
            {
                controller = ControllerTestBase.GetClient().Api;
                await controller.UpdateCompanyAsync(companyModel);
                isUpdated = true;
            }
            catch (APIException) { };

            // Test response code
            Assert.Equal(true, isUpdated);
        }

        [Fact]
        public async Task TestUpdateCompanies()
        {
            Boolean isUpdated = false;

            List<CompanyModel> body = new List<CompanyModel>();

            Dictionary<string, object> metadata = new Dictionary<string, object>
            {
                {"email", "johndoe@acmeinc.com"},
                {"string_field", "value_1"},
                {"number_field", 0},
                {"object_field", new Dictionary<string, string> {
                    {"field_a", "value_a"},
                    {"field_b", "value_b"}
                    }
                }
            };

            var companyModelA = new CompanyModel()
            {
                CompanyId = "1",
                Metadata = metadata,
                ModifiedTime = DateTime.UtcNow
            };


            var companyModelB = new CompanyModel()
            {
                CompanyId = "2",
                Metadata = metadata,
                CompanyDomain = "CompanyDomain",
                ModifiedTime = DateTime.UtcNow
            };

            body.Add(companyModelA);
            body.Add(companyModelB);

            // Perform API call
            try
            {
                controller = ControllerTestBase.GetClient().Api;
                await controller.UpdateCompaniesBatchAsync(body);
                isUpdated = true;
            }
            catch (APIException) { };

            // Test response code
            Assert.Equal(true, isUpdated);

        }
    }
}
