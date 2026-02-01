# ArtifactsMmoClient.Api.ItemsApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetAllItemsItemsGet**](ItemsApi.md#getallitemsitemsget) | **GET** /items | Get All Items |
| [**GetItemItemsCodeGet**](ItemsApi.md#getitemitemscodeget) | **GET** /items/{code} | Get Item |

<a id="getallitemsitemsget"></a>
# **GetAllItemsItemsGet**
> DataPageItemSchema GetAllItemsItemsGet (string? name = null, int? minLevel = null, int? maxLevel = null, ItemType? type = null, CraftSkill? craftSkill = null, string? craftMaterial = null, int? page = null, int? size = null)

Get All Items

Fetch items details.

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
    public class GetAllItemsItemsGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new ItemsApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string? | Name of the item. (optional) 
            var minLevel = 56;  // int? | Minimum level. (optional) 
            var maxLevel = 56;  // int? | Maximum level. (optional) 
            var type = new ItemType?(); // ItemType? | Type of items. (optional) 
            var craftSkill = new CraftSkill?(); // CraftSkill? | Skill to craft items. (optional) 
            var craftMaterial = "craftMaterial_example";  // string? | Item code of items used as material for crafting. (optional) 
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get All Items
                DataPageItemSchema result = apiInstance.GetAllItemsItemsGet(name, minLevel, maxLevel, type, craftSkill, craftMaterial, page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ItemsApi.GetAllItemsItemsGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAllItemsItemsGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get All Items
    ApiResponse<DataPageItemSchema> response = apiInstance.GetAllItemsItemsGetWithHttpInfo(name, minLevel, maxLevel, type, craftSkill, craftMaterial, page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ItemsApi.GetAllItemsItemsGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string?** | Name of the item. | [optional]  |
| **minLevel** | **int?** | Minimum level. | [optional]  |
| **maxLevel** | **int?** | Maximum level. | [optional]  |
| **type** | [**ItemType?**](ItemType?.md) | Type of items. | [optional]  |
| **craftSkill** | [**CraftSkill?**](CraftSkill?.md) | Skill to craft items. | [optional]  |
| **craftMaterial** | **string?** | Item code of items used as material for crafting. | [optional]  |
| **page** | **int?** | Page number | [optional] [default to 1] |
| **size** | **int?** | Page size | [optional] [default to 50] |

### Return type

[**DataPageItemSchema**](DataPageItemSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched items. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getitemitemscodeget"></a>
# **GetItemItemsCodeGet**
> ItemResponseSchema GetItemItemsCodeGet (string code)

Get Item

Retrieve the details of a item.

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
    public class GetItemItemsCodeGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new ItemsApi(httpClient, config, httpClientHandler);
            var code = "code_example";  // string | The code of the item.

            try
            {
                // Get Item
                ItemResponseSchema result = apiInstance.GetItemItemsCodeGet(code);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ItemsApi.GetItemItemsCodeGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetItemItemsCodeGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Item
    ApiResponse<ItemResponseSchema> response = apiInstance.GetItemItemsCodeGetWithHttpInfo(code);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ItemsApi.GetItemItemsCodeGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | The code of the item. |  |

### Return type

[**ItemResponseSchema**](ItemResponseSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched item. |  -  |
| **404** | item not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

