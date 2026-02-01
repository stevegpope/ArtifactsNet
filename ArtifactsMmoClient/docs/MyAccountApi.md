# ArtifactsMmoClient.Api.MyAccountApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**ChangePasswordMyChangePasswordPost**](MyAccountApi.md#changepasswordmychangepasswordpost) | **POST** /my/change_password | Change Password |
| [**GetAccountDetailsMyDetailsGet**](MyAccountApi.md#getaccountdetailsmydetailsget) | **GET** /my/details | Get Account Details |
| [**GetBankDetailsMyBankGet**](MyAccountApi.md#getbankdetailsmybankget) | **GET** /my/bank | Get Bank Details |
| [**GetBankItemsMyBankItemsGet**](MyAccountApi.md#getbankitemsmybankitemsget) | **GET** /my/bank/items | Get Bank Items |
| [**GetGeSellHistoryMyGrandexchangeHistoryGet**](MyAccountApi.md#getgesellhistorymygrandexchangehistoryget) | **GET** /my/grandexchange/history | Get Ge Sell History |
| [**GetGeSellOrdersMyGrandexchangeOrdersGet**](MyAccountApi.md#getgesellordersmygrandexchangeordersget) | **GET** /my/grandexchange/orders | Get Ge Sell Orders |

<a id="changepasswordmychangepasswordpost"></a>
# **ChangePasswordMyChangePasswordPost**
> ResponseSchema ChangePasswordMyChangePasswordPost (ChangePassword changePassword)

Change Password

Change your account password. Changing the password reset the account token.

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
    public class ChangePasswordMyChangePasswordPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // Configure Bearer token for authorization: JWTBearer
            config.AccessToken = "YOUR_BEARER_TOKEN";

            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new MyAccountApi(httpClient, config, httpClientHandler);
            var changePassword = new ChangePassword(); // ChangePassword | 

            try
            {
                // Change Password
                ResponseSchema result = apiInstance.ChangePasswordMyChangePasswordPost(changePassword);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyAccountApi.ChangePasswordMyChangePasswordPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ChangePasswordMyChangePasswordPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Change Password
    ApiResponse<ResponseSchema> response = apiInstance.ChangePasswordMyChangePasswordPostWithHttpInfo(changePassword);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyAccountApi.ChangePasswordMyChangePasswordPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **changePassword** | [**ChangePassword**](ChangePassword.md) |  |  |

### Return type

[**ResponseSchema**](ResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Password changed successfully. |  -  |
| **458** | Please use a different password. |  -  |
| **459** | The current password you entered is invalid. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getaccountdetailsmydetailsget"></a>
# **GetAccountDetailsMyDetailsGet**
> MyAccountDetailsSchema GetAccountDetailsMyDetailsGet ()

Get Account Details

Fetch account details.

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
    public class GetAccountDetailsMyDetailsGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // Configure Bearer token for authorization: JWTBearer
            config.AccessToken = "YOUR_BEARER_TOKEN";

            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new MyAccountApi(httpClient, config, httpClientHandler);

            try
            {
                // Get Account Details
                MyAccountDetailsSchema result = apiInstance.GetAccountDetailsMyDetailsGet();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyAccountApi.GetAccountDetailsMyDetailsGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAccountDetailsMyDetailsGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Account Details
    ApiResponse<MyAccountDetailsSchema> response = apiInstance.GetAccountDetailsMyDetailsGetWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyAccountApi.GetAccountDetailsMyDetailsGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**MyAccountDetailsSchema**](MyAccountDetailsSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched data. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getbankdetailsmybankget"></a>
# **GetBankDetailsMyBankGet**
> BankResponseSchema GetBankDetailsMyBankGet ()

Get Bank Details

Fetch bank details.

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
    public class GetBankDetailsMyBankGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // Configure Bearer token for authorization: JWTBearer
            config.AccessToken = "YOUR_BEARER_TOKEN";

            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new MyAccountApi(httpClient, config, httpClientHandler);

            try
            {
                // Get Bank Details
                BankResponseSchema result = apiInstance.GetBankDetailsMyBankGet();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyAccountApi.GetBankDetailsMyBankGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetBankDetailsMyBankGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Bank Details
    ApiResponse<BankResponseSchema> response = apiInstance.GetBankDetailsMyBankGetWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyAccountApi.GetBankDetailsMyBankGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**BankResponseSchema**](BankResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched bank details. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getbankitemsmybankitemsget"></a>
# **GetBankItemsMyBankItemsGet**
> DataPageSimpleItemSchema GetBankItemsMyBankItemsGet (string? itemCode = null, int? page = null, int? size = null)

Get Bank Items

Fetch all items in your bank.

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
    public class GetBankItemsMyBankItemsGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // Configure Bearer token for authorization: JWTBearer
            config.AccessToken = "YOUR_BEARER_TOKEN";

            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new MyAccountApi(httpClient, config, httpClientHandler);
            var itemCode = "itemCode_example";  // string? | Item to search in your bank. (optional) 
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get Bank Items
                DataPageSimpleItemSchema result = apiInstance.GetBankItemsMyBankItemsGet(itemCode, page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyAccountApi.GetBankItemsMyBankItemsGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetBankItemsMyBankItemsGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Bank Items
    ApiResponse<DataPageSimpleItemSchema> response = apiInstance.GetBankItemsMyBankItemsGetWithHttpInfo(itemCode, page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyAccountApi.GetBankItemsMyBankItemsGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **itemCode** | **string?** | Item to search in your bank. | [optional]  |
| **page** | **int?** | Page number | [optional] [default to 1] |
| **size** | **int?** | Page size | [optional] [default to 50] |

### Return type

[**DataPageSimpleItemSchema**](DataPageSimpleItemSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched data. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getgesellhistorymygrandexchangehistoryget"></a>
# **GetGeSellHistoryMyGrandexchangeHistoryGet**
> DataPageGeOrderHistorySchema GetGeSellHistoryMyGrandexchangeHistoryGet (string? id = null, string? code = null, int? page = null, int? size = null)

Get Ge Sell History

Fetch your sales history of the last 7 days.

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
    public class GetGeSellHistoryMyGrandexchangeHistoryGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // Configure Bearer token for authorization: JWTBearer
            config.AccessToken = "YOUR_BEARER_TOKEN";

            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new MyAccountApi(httpClient, config, httpClientHandler);
            var id = "id_example";  // string? | Order ID to search in your history. (optional) 
            var code = "code_example";  // string? | Item to search in your history. (optional) 
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get Ge Sell History
                DataPageGeOrderHistorySchema result = apiInstance.GetGeSellHistoryMyGrandexchangeHistoryGet(id, code, page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyAccountApi.GetGeSellHistoryMyGrandexchangeHistoryGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetGeSellHistoryMyGrandexchangeHistoryGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Ge Sell History
    ApiResponse<DataPageGeOrderHistorySchema> response = apiInstance.GetGeSellHistoryMyGrandexchangeHistoryGetWithHttpInfo(id, code, page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyAccountApi.GetGeSellHistoryMyGrandexchangeHistoryGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string?** | Order ID to search in your history. | [optional]  |
| **code** | **string?** | Item to search in your history. | [optional]  |
| **page** | **int?** | Page number | [optional] [default to 1] |
| **size** | **int?** | Page size | [optional] [default to 50] |

### Return type

[**DataPageGeOrderHistorySchema**](DataPageGeOrderHistorySchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched data. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getgesellordersmygrandexchangeordersget"></a>
# **GetGeSellOrdersMyGrandexchangeOrdersGet**
> DataPageGEOrderSchema GetGeSellOrdersMyGrandexchangeOrdersGet (string? code = null, int? page = null, int? size = null)

Get Ge Sell Orders

Fetch your sell orders details.

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
    public class GetGeSellOrdersMyGrandexchangeOrdersGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // Configure Bearer token for authorization: JWTBearer
            config.AccessToken = "YOUR_BEARER_TOKEN";

            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new MyAccountApi(httpClient, config, httpClientHandler);
            var code = "code_example";  // string? | The code of the item. (optional) 
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get Ge Sell Orders
                DataPageGEOrderSchema result = apiInstance.GetGeSellOrdersMyGrandexchangeOrdersGet(code, page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyAccountApi.GetGeSellOrdersMyGrandexchangeOrdersGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetGeSellOrdersMyGrandexchangeOrdersGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Ge Sell Orders
    ApiResponse<DataPageGEOrderSchema> response = apiInstance.GetGeSellOrdersMyGrandexchangeOrdersGetWithHttpInfo(code, page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyAccountApi.GetGeSellOrdersMyGrandexchangeOrdersGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string?** | The code of the item. | [optional]  |
| **page** | **int?** | Page number | [optional] [default to 1] |
| **size** | **int?** | Page size | [optional] [default to 50] |

### Return type

[**DataPageGEOrderSchema**](DataPageGEOrderSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched data. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

