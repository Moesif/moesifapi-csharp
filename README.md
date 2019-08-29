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

### Update User:

The api also let you update a user profile with custom metadata. The `UserId` is a required field, all other fields are optional.

```csharp
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
	UserId = "12345",
	Metadata = metadata,
	ModifiedTime = DateTime.UtcNow
};

// Perform API call
try
{
	// Create client instance using your ApplicationId
	var client = new MoesifApiClient("your_moesif_application_id");
	var apiClient = client.Api;
	apiClient.UpdateUserAsync(userModel);
}
catch (APIException) { };
```

### Update Company:

The api also let you update a company profile with custom metadata. The `CompanyId` is a required field, all other fields are optional.

```csharp
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
	CompanyId = "67890",
	Metadata = metadata,
	ModifiedTime = DateTime.UtcNow
};

// Perform API call
try
{
	// Create client instance using your ApplicationId
	var client = new MoesifApiClient("your_moesif_application_id");
	var apiClient = client.Api;
	apiClient.UpdateCompanyAsync(companyModel);
}
catch (APIException) { };
```

Your Moesif Application Id can be found in the [_Moesif Portal_](https://www.moesif.com/).
After signing up for a Moesif account, your Moesif Application Id will be displayed during the onboarding steps. 

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
