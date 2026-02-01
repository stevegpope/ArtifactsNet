# ArtifactsMmoClient.Api.AchievementsApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetAchievementAchievementsCodeGet**](AchievementsApi.md#getachievementachievementscodeget) | **GET** /achievements/{code} | Get Achievement |
| [**GetAllAchievementsAchievementsGet**](AchievementsApi.md#getallachievementsachievementsget) | **GET** /achievements | Get All Achievements |

<a id="getachievementachievementscodeget"></a>
# **GetAchievementAchievementsCodeGet**
> AchievementResponseSchema GetAchievementAchievementsCodeGet (string code)

Get Achievement

Retrieve the details of an achievement.

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
    public class GetAchievementAchievementsCodeGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new AchievementsApi(httpClient, config, httpClientHandler);
            var code = "code_example";  // string | The code of the achievement.

            try
            {
                // Get Achievement
                AchievementResponseSchema result = apiInstance.GetAchievementAchievementsCodeGet(code);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AchievementsApi.GetAchievementAchievementsCodeGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAchievementAchievementsCodeGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Achievement
    ApiResponse<AchievementResponseSchema> response = apiInstance.GetAchievementAchievementsCodeGetWithHttpInfo(code);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AchievementsApi.GetAchievementAchievementsCodeGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | The code of the achievement. |  |

### Return type

[**AchievementResponseSchema**](AchievementResponseSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched achievement. |  -  |
| **404** | achievement not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getallachievementsachievementsget"></a>
# **GetAllAchievementsAchievementsGet**
> DataPageAchievementSchema GetAllAchievementsAchievementsGet (AchievementType? type = null, int? page = null, int? size = null)

Get All Achievements

List of all achievements.

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
    public class GetAllAchievementsAchievementsGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new AchievementsApi(httpClient, config, httpClientHandler);
            var type = new AchievementType?(); // AchievementType? | Type of achievements. (optional) 
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get All Achievements
                DataPageAchievementSchema result = apiInstance.GetAllAchievementsAchievementsGet(type, page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AchievementsApi.GetAllAchievementsAchievementsGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAllAchievementsAchievementsGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get All Achievements
    ApiResponse<DataPageAchievementSchema> response = apiInstance.GetAllAchievementsAchievementsGetWithHttpInfo(type, page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AchievementsApi.GetAllAchievementsAchievementsGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **type** | [**AchievementType?**](AchievementType?.md) | Type of achievements. | [optional]  |
| **page** | **int?** | Page number | [optional] [default to 1] |
| **size** | **int?** | Page size | [optional] [default to 50] |

### Return type

[**DataPageAchievementSchema**](DataPageAchievementSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched achievements. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

