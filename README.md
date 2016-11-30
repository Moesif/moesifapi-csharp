MoesifApi Lib for C#
====================

[Source Code on GitHub](https://github.com/moesif/moesifapi-csharp)

__Check out Moesif's
[C# developer documentation](https://www.moesif.com/developer-documentation) to learn more__

(Documentation access requires an active account)

How to Install:
===============
Install the Nuget Package:

```bash
	Install-Package Moesif.Api
```

How to Use:
==========
```csharp
using Moesif.Api;
using Moesif.Api.Helpers;

// Create client instance using your ApplicationId
var client = new MoesifApiClient("my_application_id");
var apiClient = GetClient().Api;

// Parameters for the API call
var reqHeaders = new Dictionary<string, string>();
reqHeaders.Add("Host", "api.acmeinc.com");
reqHeaders.Add("Accept", "*/*");
reqHeaders.Add("Connection", "Keep-Alive");
reqHeaders.Add("User-Agent", "Dalvik/2.1.0 (Linux; U; Android 5.0.2; C6906 Build/14.5.A.0.242)");
reqHeaders.Add("Content-Type", "application/json");
reqHeaders.Add("Content-Length", "126");
reqHeaders.Add("Accept-Encoding", "gzip");

var reqBody = APIHelper.JsonDeserialize<object>(@" {
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

var rspBody = APIHelper.JsonDeserialize<object>(@" {
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
```


How To Test:
=============
The SDK also contain tests, which are contained in the Moesif.Api.Tests project.
In order to invoke these test cases, you will need *NUnit 3.0 Test Adapter Extension for Visual Studio*.
Once the SDK is complied, the test cases should appear in the Test Explorer window.
Here, you can click *Run All* to execute these test cases.

Compatibility:
==============
The build process generates a portable class library, which can be used like
a normal class library. The generated library is compatible with Windows Forms,
Windows RT, Windows Phone 8, Silverlight 5, Xamarin iOS, Xamarin Android and
Mono. More information on how to use can be found at the following link.

http://msdn.microsoft.com/en-us/library/vstudio/gg597391(v=vs.100).aspx
