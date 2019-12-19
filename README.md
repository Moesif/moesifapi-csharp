# MoesifApi Lib for C#


[Source Code on GitHub](https://github.com/moesif/moesifapi-csharp)

[Nuget Package](https://www.nuget.org/packages/Moesif.Api/)

__Check out Moesif's [Developer Documentation](https://www.moesif.com/docs) and [C# API Reference](https://www.moesif.com/docs/api?csharp) to learn more__


## How to Install:

Install the Nuget Package:

```bash
	Install-Package Moesif.Api
```

## How to Use:

Your Moesif Application Id can be found in the [_Moesif Portal_](https://www.moesif.com/).
After signing up for a Moesif account, your Moesif Application Id will be displayed during the onboarding steps. 

You can always find your Moesif Application Id at any time by logging 
into the [_Moesif Portal_](https://www.moesif.com/), click on the top right menu,
 and then clicking _Installation_.

### Create Event

```csharp
using System;
using System.Collections.Generic;
using Moesif.Api;
using Moesif.Api.Models;
using Moesif.Api.Exceptions;
using Moesif.Api.Controllers;
using System.Threading.Tasks;

// Create client instance using your ApplicationId
var client = new MoesifApiClient("My Application Id");
var apiClient = client.Api;

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
rspHeaders.Add("Date", "Tue, 23 Nov 2019 23:46:49 GMT");
rspHeaders.Add("Vary", "Accept-Encoding");
rspHeaders.Add("Pragma", "no-cache");
rspHeaders.Add("Expires", "-1");
rspHeaders.Add("Content-Type", "application/json; charset=utf-8");
rspHeaders.Add("Cache-Control", "no-cache");

var rspBody = ApiHelper.JsonDeserialize<object>(@" {
	    ""Title"": ""Hello"",
	    ""Message"": ""Hello World""
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

Dictionary<string, string> metadata = new Dictionary<string, string>
	{
		{ "id", "123456789" },
		{ "datacenter", "West US" },
		{ "image", "123" }
	};

var eventModel = new EventModel()
{
Request = eventReq,
Response = eventRsp,
UserId = "12345",
CompanyId = "67890",
SessionToken = "23jdf0owekfmcn4u3qypxg09w4d8ayrcdx8nu2ng]s98y18cx98q3yhwmnhcfx43f",
Metadata = metadata
};

// Perform API call

try
{
await apiClient.CreateEventAsync(eventModel);
}
catch(APIException) {};
```

## Update a Single User

Create or update a user profile in Moesif.
The metadata field can be any customer demographic or other info you want to store.
Only the `userId` field is required.
For details, visit the [C# API Reference](https://www.moesif.com/docs/api?csharp#update-a-user).

```csharp
var apiClient = new MoesifApiClient("YOUR_COLLECTOR_APPLICATION_ID").Api;;

// Campaign object is optional, but useful if you want to track ROI of acquisition channels
// See https://www.moesif.com/docs/api#users for campaign schema
var campaign = new CampaignModel()
{
    UtmSource = "google",
    UtmMedium = "cpc"
  UtmCampaign = "adwords"
    UtmTerm = "api+tooling"
    UtmContent = "landing"
};

// metadata can be any custom dictionary
var metadata = new Dictionary<string, object>
{
    {"email", "john@acmeinc.com"},
    {"first_name", "John"},
    {"last_name", "Doe"},
    {"title", "Software Engineer"},
    {"sales_info", new Dictionary<string, string> {
        {"stage", "Customer"},
        {"lifetime_value", 24000},
        {"account_owner", "mary@contoso.com"}
    }
};

// Only user_id is required
var user = new UserModel()
{
    UserId = "12345",
  CompanyId = "67890",
  Campaign = campaign,
    Metadata = metadata
};

// Update the user asynchronously
await apiClient.UpdateUserAsync(user);

// Update the user synchronously
apiClient.UpdateUser(user);
```

## Update Users in Batch
Similar to UpdateUser, but used to update a list of users in one batch. 
Only the `UserId` field is required.
For details, visit the [C# API Reference](https://www.moesif.com/docs/api?csharp#update-users-in-batch).

```csharp
var apiClient = new MoesifApiClient("YOUR_COLLECTOR_APPLICATION_ID").Api;;

var users = new List<UserModel>();

var metadataA = new Dictionary<string, object>
{
    {"email", "john@acmeinc.com"},
    {"first_name", "John"},
    {"last_name", "Doe"},
    {"title", "Software Engineer"},
    {"sales_info", new Dictionary<string, string> {
        {"stage", "Customer"},
        {"lifetime_value", 24000},
        {"account_owner", "mary@contoso.com"}
    }
};

// Only user_id is required
var userA = new UserModel()
{
    UserId = "12345",
  CompanyId = "67890", // If set, associate user with a company object
    Metadata = metadataA
};

var metadataB = new Dictionary<string, object>
{
    {"email", "mary@acmeinc.com"},
    {"first_name", "Mary"},
    {"last_name", "Jane"},
    {"title", "Software Engineer"},
    {"sales_info", new Dictionary<string, string> {
        {"stage", "Customer"},
        {"lifetime_value", 24000},
        {"account_owner", "mary@contoso.com"}
    }
};

// Only user_id is required
var userB = new UserModel()
{
    UserId = "54321",
  CompanyId = "67890",
    Metadata = metadataA
};


users.Add(userA);
users.Add(userB);

// Update the users asynchronously
await apiClient.UpdateUsersBatchAsync(users);

// Update the users synchronously
apiClient.UpdateUsersBatch(users);
```

## Update a Single Company

Create or update a company profile in Moesif.
The metadata field can be any company demographic or other info you want to store.
Only the `company_id` field is required.
For details, visit the [C# API Reference](https://www.moesif.com/docs/api?csharp#update-a-company).

```csharp
var apiClient = new MoesifApiClient("YOUR_COLLECTOR_APPLICATION_ID").Api;;

// Campaign object is optional, but useful if you want to track ROI of acquisition channels
// See https://www.moesif.com/docs/api#companies for campaign schema
var campaign = new CampaignModel()
{
    UtmSource = "google",
    UtmMedium = "cpc"
  UtmCampaign = "adwords"
    UtmTerm = "api+tooling"
    UtmContent = "landing"
};

// metadata can be any custom dictionary
var metadata = new Dictionary<string, object>
{
    {"org_name", "Acme, Inc"},
    {"plan_name", "Free"},
    {"deal_stage", "Lead"},
    {"mrr", 24000},
    {"demographics", new Dictionary<string, string> {
        {"alexa_ranking", 500000},
        {"employee_count", 47}
    }
};

// Only company id is required
var company = new CompanyModel()
{
  CompanyId = "67890",
  CompanyDomain = "acmeinc.com", // If domain is set, Moesif will enrich your profiles with publicly available info 
  Campaign = campaign,
    Metadata = metadata
};

// Update the company asynchronously
await apiClient.UpdateCompanyAsync(company);

// Update the company synchronously
apiClient.UpdateCompany(company);
```

## Update Companies in Batch

Similar to updateCompany, but used to update a list of companies in one batch. 
Only the `company_id` field is required.
For details, visit the [C# API Reference](https://www.moesif.com/docs/api?csharp#update-companies-in-batch).

```csharp
var apiClient = new MoesifApiClient("YOUR_COLLECTOR_APPLICATION_ID").Api;;

var companies = new List<CompanyModel>();

// Campaign object is optional, but useful if you want to track ROI of acquisition channels
// See https://www.moesif.com/docs/api#companies for campaign schema
var campaignA = new CampaignModel()
{
    UtmSource = "google",
    UtmMedium = "cpc"
  UtmCampaign = "adwords"
    UtmTerm = "api+tooling"
    UtmContent = "landing"
};

// metadata can be any custom dictionary
var metadataA = new Dictionary<string, object>
{
    {"org_name", "Acme, Inc"},
    {"plan_name", "Free"},
    {"deal_stage", "Lead"},
    {"mrr", 24000},
    {"demographics", new Dictionary<string, string> {
        {"alexa_ranking", 500000},
        {"employee_count", 47}
    }
};

// Only company id is required
var companyA = new CompanyModel()
{
  CompanyId = "67890",
  CompanyDomain = "acmeinc.com", // If domain is set, Moesif will enrich your profiles with publicly available info 
  Campaign = campaign,
    Metadata = metadata
};

// metadata can be any custom dictionary
var metadataB = new Dictionary<string, object>
{
    {"org_name", "Contoso, Inc"},
    {"plan_name", "Paid"},
    {"deal_stage", "Lead"},
    {"mrr", 48000},
    {"demographics", new Dictionary<string, string> {
        {"alexa_ranking", 500000},
        {"employee_count", 53}
    }
};

// Only company id is required
var companyB = new CompanyModel()
{
  CompanyId = "09876",
  CompanyDomain = "contoso.com", // If domain is set, Moesif will enrich your profiles with publicly available info 
  Campaign = campaign,
    Metadata = metadata
};


companies.Add(companyA);
companies.Add(companyB);

// Update the companies asynchronously
await apiClient.UpdateCompaniesBatchAsync(companies);

// Update the companies synchronously
apiClient.UpdateCompaniesBatch(companies);
```

## How To Test:

The SDK also contain tests, which are contained in the Moesif.Api.Tests project.
In order to invoke these test cases, you will need *NUnit 3.0 Test Adapter Extension for Visual Studio*.
Once the SDK is complied, the test cases should appear in the Test Explorer window.
Here, you can click *Run All* to execute these test cases.

## Compatibility:

The build process generates a portable class library, which can be used like
a normal class library. The generated library is compatible with Windows Forms,
Windows RT, Windows Phone 8, Silverlight 5, Xamarin iOS, Xamarin Android and
Mono. More information on how to use can be found at the following link.

http://msdn.microsoft.com/en-us/library/vstudio/gg597391(v=vs.100).aspx
