# ArtifactsMmoClient.Api.MonstersApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetAllMonstersMonstersGet**](MonstersApi.md#getallmonstersmonstersget) | **GET** /monsters | Get All Monsters |
| [**GetMonsterMonstersCodeGet**](MonstersApi.md#getmonstermonsterscodeget) | **GET** /monsters/{code} | Get Monster |

<a id="getallmonstersmonstersget"></a>
# **GetAllMonstersMonstersGet**
> DataPageMonsterSchema GetAllMonstersMonstersGet (string? name = null, int? minLevel = null, int? maxLevel = null, string? drop = null, int? page = null, int? size = null)

Get All Monsters

Fetch monsters details.

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
    public class GetAllMonstersMonstersGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new MonstersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string? | Name of the monster. (optional) 
            var minLevel = 56;  // int? | Minimum level. (optional) 
            var maxLevel = 56;  // int? | Maximum level. (optional) 
            var drop = "drop_example";  // string? | Item code of the drop. (optional) 
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get All Monsters
                DataPageMonsterSchema result = apiInstance.GetAllMonstersMonstersGet(name, minLevel, maxLevel, drop, page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MonstersApi.GetAllMonstersMonstersGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAllMonstersMonstersGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get All Monsters
    ApiResponse<DataPageMonsterSchema> response = apiInstance.GetAllMonstersMonstersGetWithHttpInfo(name, minLevel, maxLevel, drop, page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MonstersApi.GetAllMonstersMonstersGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string?** | Name of the monster. | [optional]  |
| **minLevel** | **int?** | Minimum level. | [optional]  |
| **maxLevel** | **int?** | Maximum level. | [optional]  |
| **drop** | **string?** | Item code of the drop. | [optional]  |
| **page** | **int?** | Page number | [optional] [default to 1] |
| **size** | **int?** | Page size | [optional] [default to 50] |

### Return type

[**DataPageMonsterSchema**](DataPageMonsterSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched monsters. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getmonstermonsterscodeget"></a>
# **GetMonsterMonstersCodeGet**
> MonsterResponseSchema GetMonsterMonstersCodeGet (string code)

Get Monster

Retrieve the details of a monster.

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
    public class GetMonsterMonstersCodeGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new MonstersApi(httpClient, config, httpClientHandler);
            var code = "code_example";  // string | The code of the monster.

            try
            {
                // Get Monster
                MonsterResponseSchema result = apiInstance.GetMonsterMonstersCodeGet(code);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MonstersApi.GetMonsterMonstersCodeGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetMonsterMonstersCodeGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Monster
    ApiResponse<MonsterResponseSchema> response = apiInstance.GetMonsterMonstersCodeGetWithHttpInfo(code);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MonstersApi.GetMonsterMonstersCodeGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | The code of the monster. |  |

### Return type

[**MonsterResponseSchema**](MonsterResponseSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched monster. |  -  |
| **404** | monster not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

