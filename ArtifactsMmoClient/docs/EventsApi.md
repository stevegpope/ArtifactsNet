# ArtifactsMmoClient.Api.EventsApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetAllActiveEventsEventsActiveGet**](EventsApi.md#getallactiveeventseventsactiveget) | **GET** /events/active | Get All Active Events |
| [**GetAllEventsEventsGet**](EventsApi.md#getalleventseventsget) | **GET** /events | Get All Events |
| [**SpawnEventEventsSpawnPost**](EventsApi.md#spawneventeventsspawnpost) | **POST** /events/spawn | Spawn Event |

<a id="getallactiveeventseventsactiveget"></a>
# **GetAllActiveEventsEventsActiveGet**
> DataPageActiveEventSchema GetAllActiveEventsEventsActiveGet (int? page = null, int? size = null)

Get All Active Events

Fetch active events details.

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
    public class GetAllActiveEventsEventsActiveGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new EventsApi(httpClient, config, httpClientHandler);
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get All Active Events
                DataPageActiveEventSchema result = apiInstance.GetAllActiveEventsEventsActiveGet(page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EventsApi.GetAllActiveEventsEventsActiveGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAllActiveEventsEventsActiveGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get All Active Events
    ApiResponse<DataPageActiveEventSchema> response = apiInstance.GetAllActiveEventsEventsActiveGetWithHttpInfo(page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EventsApi.GetAllActiveEventsEventsActiveGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **page** | **int?** | Page number | [optional] [default to 1] |
| **size** | **int?** | Page size | [optional] [default to 50] |

### Return type

[**DataPageActiveEventSchema**](DataPageActiveEventSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched active events. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getalleventseventsget"></a>
# **GetAllEventsEventsGet**
> DataPageEventSchema GetAllEventsEventsGet (MapContentType? type = null, int? page = null, int? size = null)

Get All Events

Fetch events details.

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
    public class GetAllEventsEventsGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new EventsApi(httpClient, config, httpClientHandler);
            var type = new MapContentType?(); // MapContentType? | Type of events. (optional) 
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get All Events
                DataPageEventSchema result = apiInstance.GetAllEventsEventsGet(type, page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EventsApi.GetAllEventsEventsGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAllEventsEventsGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get All Events
    ApiResponse<DataPageEventSchema> response = apiInstance.GetAllEventsEventsGetWithHttpInfo(type, page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EventsApi.GetAllEventsEventsGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **type** | [**MapContentType?**](MapContentType?.md) | Type of events. | [optional]  |
| **page** | **int?** | Page number | [optional] [default to 1] |
| **size** | **int?** | Page size | [optional] [default to 50] |

### Return type

[**DataPageEventSchema**](DataPageEventSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched events. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="spawneventeventsspawnpost"></a>
# **SpawnEventEventsSpawnPost**
> ActiveEventResponseSchema SpawnEventEventsSpawnPost (SpawnEventRequest spawnEventRequest)

Spawn Event

Spawn a specific event by consuming 1 event token. Member or founder account required.

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
    public class SpawnEventEventsSpawnPostExample
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
            var apiInstance = new EventsApi(httpClient, config, httpClientHandler);
            var spawnEventRequest = new SpawnEventRequest(); // SpawnEventRequest | 

            try
            {
                // Spawn Event
                ActiveEventResponseSchema result = apiInstance.SpawnEventEventsSpawnPost(spawnEventRequest);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EventsApi.SpawnEventEventsSpawnPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the SpawnEventEventsSpawnPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Spawn Event
    ApiResponse<ActiveEventResponseSchema> response = apiInstance.SpawnEventEventsSpawnPostWithHttpInfo(spawnEventRequest);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EventsApi.SpawnEventEventsSpawnPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **spawnEventRequest** | [**SpawnEventRequest**](SpawnEventRequest.md) |  |  |

### Return type

[**ActiveEventResponseSchema**](ActiveEventResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched data. |  -  |
| **563** | Insufficient event tokens. You need at least 1 event token to spawn an event. |  -  |
| **564** | Event not found or already active. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

