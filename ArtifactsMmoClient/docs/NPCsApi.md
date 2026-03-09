# ArtifactsMmoClient.Api.NPCsApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetAllNpcsItemsNpcsItemsGet**](NPCsApi.md#getallnpcsitemsnpcsitemsget) | **GET** /npcs/items | Get All Npcs Items |
| [**GetAllNpcsNpcsDetailsGet**](NPCsApi.md#getallnpcsnpcsdetailsget) | **GET** /npcs/details | Get All Npcs |
| [**GetNpcItemsNpcsItemsCodeGet**](NPCsApi.md#getnpcitemsnpcsitemscodeget) | **GET** /npcs/items/{code} | Get Npc Items |
| [**GetNpcNpcsDetailsCodeGet**](NPCsApi.md#getnpcnpcsdetailscodeget) | **GET** /npcs/details/{code} | Get Npc |

<a id="getallnpcsitemsnpcsitemsget"></a>
# **GetAllNpcsItemsNpcsItemsGet**
> StaticDataPageNPCItem GetAllNpcsItemsNpcsItemsGet (string? code = null, string? npc = null, string? currency = null, int? page = null, int? size = null)

Get All Npcs Items

Retrieve the list of all NPC items.

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
    public class GetAllNpcsItemsNpcsItemsGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new NPCsApi(httpClient, config, httpClientHandler);
            var code = "code_example";  // string? | Item code. (optional) 
            var npc = "npc_example";  // string? | NPC code. (optional) 
            var currency = "currency_example";  // string? | Currency code. (optional) 
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get All Npcs Items
                StaticDataPageNPCItem result = apiInstance.GetAllNpcsItemsNpcsItemsGet(code, npc, currency, page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling NPCsApi.GetAllNpcsItemsNpcsItemsGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAllNpcsItemsNpcsItemsGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get All Npcs Items
    ApiResponse<StaticDataPageNPCItem> response = apiInstance.GetAllNpcsItemsNpcsItemsGetWithHttpInfo(code, npc, currency, page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling NPCsApi.GetAllNpcsItemsNpcsItemsGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string?** | Item code. | [optional]  |
| **npc** | **string?** | NPC code. | [optional]  |
| **currency** | **string?** | Currency code. | [optional]  |
| **page** | **int?** | Page number | [optional] [default to 1] |
| **size** | **int?** | Page size | [optional] [default to 50] |

### Return type

[**StaticDataPageNPCItem**](StaticDataPageNPCItem.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched NPC items. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getallnpcsnpcsdetailsget"></a>
# **GetAllNpcsNpcsDetailsGet**
> StaticDataPageNPCSchema GetAllNpcsNpcsDetailsGet (string? name = null, NPCType? type = null, string? currency = null, string? item = null, int? page = null, int? size = null)

Get All Npcs

Fetch NPCs details.

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
    public class GetAllNpcsNpcsDetailsGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new NPCsApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string? | NPC name. (optional) 
            var type = new NPCType?(); // NPCType? | Type of NPCs. (optional) 
            var currency = "currency_example";  // string? | Currency code to filter NPCs that trade with this currency. (optional) 
            var item = "item_example";  // string? | Item code to filter NPCs that trade this item. (optional) 
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get All Npcs
                StaticDataPageNPCSchema result = apiInstance.GetAllNpcsNpcsDetailsGet(name, type, currency, item, page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling NPCsApi.GetAllNpcsNpcsDetailsGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAllNpcsNpcsDetailsGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get All Npcs
    ApiResponse<StaticDataPageNPCSchema> response = apiInstance.GetAllNpcsNpcsDetailsGetWithHttpInfo(name, type, currency, item, page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling NPCsApi.GetAllNpcsNpcsDetailsGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string?** | NPC name. | [optional]  |
| **type** | [**NPCType?**](NPCType?.md) | Type of NPCs. | [optional]  |
| **currency** | **string?** | Currency code to filter NPCs that trade with this currency. | [optional]  |
| **item** | **string?** | Item code to filter NPCs that trade this item. | [optional]  |
| **page** | **int?** | Page number | [optional] [default to 1] |
| **size** | **int?** | Page size | [optional] [default to 50] |

### Return type

[**StaticDataPageNPCSchema**](StaticDataPageNPCSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched NPCs. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getnpcitemsnpcsitemscodeget"></a>
# **GetNpcItemsNpcsItemsCodeGet**
> StaticDataPageNPCItem GetNpcItemsNpcsItemsCodeGet (string code, int? page = null, int? size = null)

Get Npc Items

Retrieve the items list of a NPC. If the NPC has items to buy, sell or trade, they will be displayed.

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
    public class GetNpcItemsNpcsItemsCodeGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new NPCsApi(httpClient, config, httpClientHandler);
            var code = "code_example";  // string | The code of the NPC.
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get Npc Items
                StaticDataPageNPCItem result = apiInstance.GetNpcItemsNpcsItemsCodeGet(code, page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling NPCsApi.GetNpcItemsNpcsItemsCodeGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetNpcItemsNpcsItemsCodeGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Npc Items
    ApiResponse<StaticDataPageNPCItem> response = apiInstance.GetNpcItemsNpcsItemsCodeGetWithHttpInfo(code, page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling NPCsApi.GetNpcItemsNpcsItemsCodeGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | The code of the NPC. |  |
| **page** | **int?** | Page number | [optional] [default to 1] |
| **size** | **int?** | Page size | [optional] [default to 50] |

### Return type

[**StaticDataPageNPCItem**](StaticDataPageNPCItem.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched NPC items. |  -  |
| **404** | NPC items not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getnpcnpcsdetailscodeget"></a>
# **GetNpcNpcsDetailsCodeGet**
> NPCResponseSchema GetNpcNpcsDetailsCodeGet (string code)

Get Npc

Retrieve the details of a NPC.

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
    public class GetNpcNpcsDetailsCodeGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new NPCsApi(httpClient, config, httpClientHandler);
            var code = "code_example";  // string | The code of the NPC.

            try
            {
                // Get Npc
                NPCResponseSchema result = apiInstance.GetNpcNpcsDetailsCodeGet(code);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling NPCsApi.GetNpcNpcsDetailsCodeGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetNpcNpcsDetailsCodeGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Npc
    ApiResponse<NPCResponseSchema> response = apiInstance.GetNpcNpcsDetailsCodeGetWithHttpInfo(code);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling NPCsApi.GetNpcNpcsDetailsCodeGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | The code of the NPC. |  |

### Return type

[**NPCResponseSchema**](NPCResponseSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched NPC. |  -  |
| **404** | NPC not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

