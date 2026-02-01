# ArtifactsMmoClient.Api.GrandExchangeApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetGeSellHistoryGrandexchangeHistoryCodeGet**](GrandExchangeApi.md#getgesellhistorygrandexchangehistorycodeget) | **GET** /grandexchange/history/{code} | Get Ge Sell History |
| [**GetGeSellOrderGrandexchangeOrdersIdGet**](GrandExchangeApi.md#getgesellordergrandexchangeordersidget) | **GET** /grandexchange/orders/{id} | Get Ge Sell Order |
| [**GetGeSellOrdersGrandexchangeOrdersGet**](GrandExchangeApi.md#getgesellordersgrandexchangeordersget) | **GET** /grandexchange/orders | Get Ge Sell Orders |

<a id="getgesellhistorygrandexchangehistorycodeget"></a>
# **GetGeSellHistoryGrandexchangeHistoryCodeGet**
> DataPageGeOrderHistorySchema GetGeSellHistoryGrandexchangeHistoryCodeGet (string code, string? seller = null, string? buyer = null, int? page = null, int? size = null)

Get Ge Sell History

Fetch the sales history of the item for the last 7 days.

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
    public class GetGeSellHistoryGrandexchangeHistoryCodeGetExample
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
            var seller = "seller_example";  // string? | The seller (account name) of the item. (optional) 
            var buyer = "buyer_example";  // string? | The buyer (account name) of the item. (optional) 
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get Ge Sell History
                DataPageGeOrderHistorySchema result = apiInstance.GetGeSellHistoryGrandexchangeHistoryCodeGet(code, seller, buyer, page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling GrandExchangeApi.GetGeSellHistoryGrandexchangeHistoryCodeGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetGeSellHistoryGrandexchangeHistoryCodeGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Ge Sell History
    ApiResponse<DataPageGeOrderHistorySchema> response = apiInstance.GetGeSellHistoryGrandexchangeHistoryCodeGetWithHttpInfo(code, seller, buyer, page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling GrandExchangeApi.GetGeSellHistoryGrandexchangeHistoryCodeGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | The code of the item. |  |
| **seller** | **string?** | The seller (account name) of the item. | [optional]  |
| **buyer** | **string?** | The buyer (account name) of the item. | [optional]  |
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

<a id="getgesellordergrandexchangeordersidget"></a>
# **GetGeSellOrderGrandexchangeOrdersIdGet**
> GEOrderResponseSchema GetGeSellOrderGrandexchangeOrdersIdGet (string id)

Get Ge Sell Order

Retrieve the sell order of a item.

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
    public class GetGeSellOrderGrandexchangeOrdersIdGetExample
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
                // Get Ge Sell Order
                GEOrderResponseSchema result = apiInstance.GetGeSellOrderGrandexchangeOrdersIdGet(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling GrandExchangeApi.GetGeSellOrderGrandexchangeOrdersIdGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetGeSellOrderGrandexchangeOrdersIdGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Ge Sell Order
    ApiResponse<GEOrderResponseSchema> response = apiInstance.GetGeSellOrderGrandexchangeOrdersIdGetWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling GrandExchangeApi.GetGeSellOrderGrandexchangeOrdersIdGetWithHttpInfo: " + e.Message);
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

<a id="getgesellordersgrandexchangeordersget"></a>
# **GetGeSellOrdersGrandexchangeOrdersGet**
> DataPageGEOrderSchema GetGeSellOrdersGrandexchangeOrdersGet (string? code = null, string? seller = null, int? page = null, int? size = null)

Get Ge Sell Orders

Fetch all sell orders.

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
    public class GetGeSellOrdersGrandexchangeOrdersGetExample
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
            var seller = "seller_example";  // string? | The seller (account name) of the item. (optional) 
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get Ge Sell Orders
                DataPageGEOrderSchema result = apiInstance.GetGeSellOrdersGrandexchangeOrdersGet(code, seller, page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling GrandExchangeApi.GetGeSellOrdersGrandexchangeOrdersGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetGeSellOrdersGrandexchangeOrdersGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Ge Sell Orders
    ApiResponse<DataPageGEOrderSchema> response = apiInstance.GetGeSellOrdersGrandexchangeOrdersGetWithHttpInfo(code, seller, page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling GrandExchangeApi.GetGeSellOrdersGrandexchangeOrdersGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string?** | The code of the item. | [optional]  |
| **seller** | **string?** | The seller (account name) of the item. | [optional]  |
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

