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

        private EventModel CreateEvent()
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
            rspHeaders.Add("Date", "Tue, 20 Aug 2019 23:46:49 GMT");
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
                CompanyId = "my_company_id",
                SessionToken = "23jdf0owekfmcn4u3qypxg09w4d8ayrcdx8nu2ng]s98y18cx98q3yhwmnhcfx43f",
                Metadata = metadata
            };

            return eventModel;
        }

        [Fact]
        public async Task TestAddEvent()
        {
            EventModel eventModel = CreateEvent();

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
            List<EventModel> body = new List<EventModel>();

            EventModel eventModel = CreateEvent();

            body.Add(eventModel);
            body.Add(eventModel);

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

            var campaign = new CampaignModel()
            {
                UtmSource = "Newsletter",
                UtmMedium = "Email"
            };

            var userModel = new UserModel()
            {
                UserAgentString = "Dalvik/2.1.0 (Linux; U; Android 5.0.2; C6906 Build/14.5.A.0.242)",
                UserId = "12345",
                CompanyId = "67890",
                Metadata = metadata,
                ModifiedTime = DateTime.UtcNow,
                Campaign = campaign
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
            List<UserModel> body = new List<UserModel>();

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

            var userModelA = new UserModel()
            {
                UserId = "12345",
                CompanyId = "67890",
                Metadata = metadata,
                ModifiedTime = DateTime.UtcNow
            };


            var userModelB = new UserModel()
            {
                UserId = "1234",
                CompanyId = "6789",
                Metadata = metadata,
                SessionToken = "23jdf0owekfmcn4u3qypxg09w4d8ayrcdx8nu2ng]s98y18cx98q3yhwmnhcfx43f",
                ModifiedTime = DateTime.UtcNow
            };

            body.Add(userModelA);
            body.Add(userModelB);


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

            var campaign = new CampaignModel()
            {
                UtmSource = "Adwords",
                UtmMedium = "Twitter"
            };

            var companyModel = new CompanyModel()
            {
                CompanyId = "12345",
                Metadata = metadata,
                ModifiedTime = DateTime.UtcNow,
                Campaign = campaign
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
                CompanyId = "12345",
                Metadata = metadata,
                ModifiedTime = DateTime.UtcNow
            };


            var companyModelB = new CompanyModel()
            {
                CompanyId = "67890",
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
