# ArtifactsMmoClient.Api.LeaderboardApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetAccountsLeaderboardLeaderboardAccountsGet**](LeaderboardApi.md#getaccountsleaderboardleaderboardaccountsget) | **GET** /leaderboard/accounts | Get Accounts Leaderboard |
| [**GetCharactersLeaderboardLeaderboardCharactersGet**](LeaderboardApi.md#getcharactersleaderboardleaderboardcharactersget) | **GET** /leaderboard/characters | Get Characters Leaderboard |

<a id="getaccountsleaderboardleaderboardaccountsget"></a>
# **GetAccountsLeaderboardLeaderboardAccountsGet**
> DataPageAccountLeaderboardSchema GetAccountsLeaderboardLeaderboardAccountsGet (AccountLeaderboardType? sort = null, string? name = null, int? page = null, int? size = null)

Get Accounts Leaderboard

Fetch leaderboard details.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;

namespace Example
{
    public class GetAccountsLeaderboardLeaderboardAccountsGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new LeaderboardApi(httpClient, config, httpClientHandler);
            var sort = new AccountLeaderboardType?(); // AccountLeaderboardType? | Sort of account leaderboards. (optional) 
            var name = "name_example";  // string? | Account name. (optional) 
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get Accounts Leaderboard
                DataPageAccountLeaderboardSchema result = apiInstance.GetAccountsLeaderboardLeaderboardAccountsGet(sort, name, page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling LeaderboardApi.GetAccountsLeaderboardLeaderboardAccountsGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAccountsLeaderboardLeaderboardAccountsGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Accounts Leaderboard
    ApiResponse<DataPageAccountLeaderboardSchema> response = apiInstance.GetAccountsLeaderboardLeaderboardAccountsGetWithHttpInfo(sort, name, page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling LeaderboardApi.GetAccountsLeaderboardLeaderboardAccountsGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **sort** | [**AccountLeaderboardType?**](AccountLeaderboardType?.md) | Sort of account leaderboards. | [optional]  |
| **name** | **string?** | Account name. | [optional]  |
| **page** | **int?** | Page number | [optional] [default to 1] |
| **size** | **int?** | Page size | [optional] [default to 50] |

### Return type

[**DataPageAccountLeaderboardSchema**](DataPageAccountLeaderboardSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched leaderboard. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getcharactersleaderboardleaderboardcharactersget"></a>
# **GetCharactersLeaderboardLeaderboardCharactersGet**
> DataPageCharacterLeaderboardSchema GetCharactersLeaderboardLeaderboardCharactersGet (CharacterLeaderboardType? sort = null, string? name = null, int? page = null, int? size = null)

Get Characters Leaderboard

Fetch leaderboard details.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;

namespace Example
{
    public class GetCharactersLeaderboardLeaderboardCharactersGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new LeaderboardApi(httpClient, config, httpClientHandler);
            var sort = new CharacterLeaderboardType?(); // CharacterLeaderboardType? | Sort of character leaderboards. (optional) 
            var name = "name_example";  // string? | Character name. (optional) 
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get Characters Leaderboard
                DataPageCharacterLeaderboardSchema result = apiInstance.GetCharactersLeaderboardLeaderboardCharactersGet(sort, name, page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling LeaderboardApi.GetCharactersLeaderboardLeaderboardCharactersGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetCharactersLeaderboardLeaderboardCharactersGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Characters Leaderboard
    ApiResponse<DataPageCharacterLeaderboardSchema> response = apiInstance.GetCharactersLeaderboardLeaderboardCharactersGetWithHttpInfo(sort, name, page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling LeaderboardApi.GetCharactersLeaderboardLeaderboardCharactersGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **sort** | [**CharacterLeaderboardType?**](CharacterLeaderboardType?.md) | Sort of character leaderboards. | [optional]  |
| **name** | **string?** | Character name. | [optional]  |
| **page** | **int?** | Page number | [optional] [default to 1] |
| **size** | **int?** | Page size | [optional] [default to 50] |

### Return type

[**DataPageCharacterLeaderboardSchema**](DataPageCharacterLeaderboardSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched leaderboard. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

