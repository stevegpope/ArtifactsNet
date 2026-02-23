# ArtifactsMmoClient.Api.GrandExchangeApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetGeHistoryGrandexchangeHistoryCodeGet**](GrandExchangeApi.md#getgehistorygrandexchangehistorycodeget) | **GET** /grandexchange/history/{code} | Get Ge History |
| [**GetGeOrderGrandexchangeOrdersIdGet**](GrandExchangeApi.md#getgeordergrandexchangeordersidget) | **GET** /grandexchange/orders/{id} | Get Ge Order |
| [**GetGeOrdersGrandexchangeOrdersGet**](GrandExchangeApi.md#getgeordersgrandexchangeordersget) | **GET** /grandexchange/orders | Get Ge Orders |

<a id="getgehistorygrandexchangehistorycodeget"></a>
# **GetGeHistoryGrandexchangeHistoryCodeGet**
> DataPageGeOrderHistorySchema GetGeHistoryGrandexchangeHistoryCodeGet (string code, string? account = null, int? page = null, int? size = null)

Get Ge History

Fetch the transaction history of the item for the last 7 days (buy and sell orders).

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
    public class GetGeHistoryGrandexchangeHistoryCodeGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new GrandExchangeApi(httpClient, config, httpClientHandler);
            var code = "code_example";  // string | The code of the item.
            var account = "account_example";  // string? | Account involved in the transaction (matches either seller or buyer). (optional) 
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get Ge History
                DataPageGeOrderHistorySchema result = apiInstance.GetGeHistoryGrandexchangeHistoryCodeGet(code, account, page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling GrandExchangeApi.GetGeHistoryGrandexchangeHistoryCodeGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetGeHistoryGrandexchangeHistoryCodeGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Ge History
    ApiResponse<DataPageGeOrderHistorySchema> response = apiInstance.GetGeHistoryGrandexchangeHistoryCodeGetWithHttpInfo(code, account, page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling GrandExchangeApi.GetGeHistoryGrandexchangeHistoryCodeGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | The code of the item. |  |
| **account** | **string?** | Account involved in the transaction (matches either seller or buyer). | [optional]  |
| **page** | **int?** | Page number | [optional] [default to 1] |
| **size** | **int?** | Page size | [optional] [default to 50] |

### Return type

[**DataPageGeOrderHistorySchema**](DataPageGeOrderHistorySchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched item history. |  -  |
| **404** | item history not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getgeordergrandexchangeordersidget"></a>
# **GetGeOrderGrandexchangeOrdersIdGet**
> GEOrderResponseSchema GetGeOrderGrandexchangeOrdersIdGet (string id)

Get Ge Order

Retrieve a specific order by ID.

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
    public class GetGeOrderGrandexchangeOrdersIdGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new GrandExchangeApi(httpClient, config, httpClientHandler);
            var id = "id_example";  // string | The id of the order.

            try
            {
                // Get Ge Order
                GEOrderResponseSchema result = apiInstance.GetGeOrderGrandexchangeOrdersIdGet(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling GrandExchangeApi.GetGeOrderGrandexchangeOrdersIdGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetGeOrderGrandexchangeOrdersIdGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Ge Order
    ApiResponse<GEOrderResponseSchema> response = apiInstance.GetGeOrderGrandexchangeOrdersIdGetWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling GrandExchangeApi.GetGeOrderGrandexchangeOrdersIdGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | The id of the order. |  |

### Return type

[**GEOrderResponseSchema**](GEOrderResponseSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched GE order. |  -  |
| **404** | GE order not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getgeordersgrandexchangeordersget"></a>
# **GetGeOrdersGrandexchangeOrdersGet**
> DataPageGEOrderSchema GetGeOrdersGrandexchangeOrdersGet (string? code = null, string? account = null, GEOrderType? type = null, int? page = null, int? size = null)

Get Ge Orders

Fetch all orders (sell and buy orders).  Use the `type` parameter to filter by order type; when using `account`, `type` is required to decide whether to match seller or buyer.

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
    public class GetGeOrdersGrandexchangeOrdersGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new GrandExchangeApi(httpClient, config, httpClientHandler);
            var code = "code_example";  // string? | The code of the item. (optional) 
            var account = "account_example";  // string? | The account that sells or buys items. (optional) 
            var type = new GEOrderType?(); // GEOrderType? | Filter by order type (sell or buy). (optional) 
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get Ge Orders
                DataPageGEOrderSchema result = apiInstance.GetGeOrdersGrandexchangeOrdersGet(code, account, type, page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling GrandExchangeApi.GetGeOrdersGrandexchangeOrdersGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetGeOrdersGrandexchangeOrdersGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Ge Orders
    ApiResponse<DataPageGEOrderSchema> response = apiInstance.GetGeOrdersGrandexchangeOrdersGetWithHttpInfo(code, account, type, page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling GrandExchangeApi.GetGeOrdersGrandexchangeOrdersGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string?** | The code of the item. | [optional]  |
| **account** | **string?** | The account that sells or buys items. | [optional]  |
| **type** | [**GEOrderType?**](GEOrderType?.md) | Filter by order type (sell or buy). | [optional]  |
| **page** | **int?** | Page number | [optional] [default to 1] |
| **size** | **int?** | Page size | [optional] [default to 50] |

### Return type

[**DataPageGEOrderSchema**](DataPageGEOrderSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched GE orders. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

