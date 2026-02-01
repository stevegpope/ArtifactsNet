# ArtifactsMmoClient.Api.MapsApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetAllMapsMapsGet**](MapsApi.md#getallmapsmapsget) | **GET** /maps | Get All Maps |
| [**GetLayerMapsMapsLayerGet**](MapsApi.md#getlayermapsmapslayerget) | **GET** /maps/{layer} | Get Layer Maps |
| [**GetMapByIdMapsIdMapIdGet**](MapsApi.md#getmapbyidmapsidmapidget) | **GET** /maps/id/{map_id} | Get Map By Id |
| [**GetMapByPositionMapsLayerXYGet**](MapsApi.md#getmapbypositionmapslayerxyget) | **GET** /maps/{layer}/{x}/{y} | Get Map By Position |

<a id="getallmapsmapsget"></a>
# **GetAllMapsMapsGet**
> DataPageMapSchema GetAllMapsMapsGet (MapLayer? layer = null, MapContentType? contentType = null, string? contentCode = null, bool? hideBlockedMaps = null, int? page = null, int? size = null)

Get All Maps

Fetch maps details.

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
    public class GetAllMapsMapsGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new MapsApi(httpClient, config, httpClientHandler);
            var layer = new MapLayer?(); // MapLayer? | Filter maps by layer. (optional) 
            var contentType = new MapContentType?(); // MapContentType? | Type of maps. (optional) 
            var contentCode = "contentCode_example";  // string? | Content code on the map. (optional) 
            var hideBlockedMaps = false;  // bool? | When true, excludes maps with access_type 'blocked' from the results. (optional)  (default to false)
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get All Maps
                DataPageMapSchema result = apiInstance.GetAllMapsMapsGet(layer, contentType, contentCode, hideBlockedMaps, page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MapsApi.GetAllMapsMapsGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAllMapsMapsGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get All Maps
    ApiResponse<DataPageMapSchema> response = apiInstance.GetAllMapsMapsGetWithHttpInfo(layer, contentType, contentCode, hideBlockedMaps, page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MapsApi.GetAllMapsMapsGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **layer** | [**MapLayer?**](MapLayer?.md) | Filter maps by layer. | [optional]  |
| **contentType** | [**MapContentType?**](MapContentType?.md) | Type of maps. | [optional]  |
| **contentCode** | **string?** | Content code on the map. | [optional]  |
| **hideBlockedMaps** | **bool?** | When true, excludes maps with access_type &#39;blocked&#39; from the results. | [optional] [default to false] |
| **page** | **int?** | Page number | [optional] [default to 1] |
| **size** | **int?** | Page size | [optional] [default to 50] |

### Return type

[**DataPageMapSchema**](DataPageMapSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched maps. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getlayermapsmapslayerget"></a>
# **GetLayerMapsMapsLayerGet**
> DataPageMapSchema GetLayerMapsMapsLayerGet (MapLayer layer, MapContentType? contentType = null, string? contentCode = null, bool? hideBlockedMaps = null, int? page = null, int? size = null)

Get Layer Maps

Fetch maps details.

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
    public class GetLayerMapsMapsLayerGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new MapsApi(httpClient, config, httpClientHandler);
            var layer = (MapLayer) "interior";  // MapLayer | The layer of the map (interior, overworld, underground).
            var contentType = new MapContentType?(); // MapContentType? | Type of maps. (optional) 
            var contentCode = "contentCode_example";  // string? | Content code on the map. (optional) 
            var hideBlockedMaps = false;  // bool? | When true, excludes maps with access_type 'blocked' from the results. (optional)  (default to false)
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get Layer Maps
                DataPageMapSchema result = apiInstance.GetLayerMapsMapsLayerGet(layer, contentType, contentCode, hideBlockedMaps, page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MapsApi.GetLayerMapsMapsLayerGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetLayerMapsMapsLayerGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Layer Maps
    ApiResponse<DataPageMapSchema> response = apiInstance.GetLayerMapsMapsLayerGetWithHttpInfo(layer, contentType, contentCode, hideBlockedMaps, page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MapsApi.GetLayerMapsMapsLayerGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **layer** | **MapLayer** | The layer of the map (interior, overworld, underground). |  |
| **contentType** | [**MapContentType?**](MapContentType?.md) | Type of maps. | [optional]  |
| **contentCode** | **string?** | Content code on the map. | [optional]  |
| **hideBlockedMaps** | **bool?** | When true, excludes maps with access_type &#39;blocked&#39; from the results. | [optional] [default to false] |
| **page** | **int?** | Page number | [optional] [default to 1] |
| **size** | **int?** | Page size | [optional] [default to 50] |

### Return type

[**DataPageMapSchema**](DataPageMapSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched maps. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getmapbyidmapsidmapidget"></a>
# **GetMapByIdMapsIdMapIdGet**
> MapResponseSchema GetMapByIdMapsIdMapIdGet (int mapId)

Get Map By Id

Retrieve the details of a map by its unique ID.

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
    public class GetMapByIdMapsIdMapIdGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new MapsApi(httpClient, config, httpClientHandler);
            var mapId = 56;  // int | The unique ID of the map.

            try
            {
                // Get Map By Id
                MapResponseSchema result = apiInstance.GetMapByIdMapsIdMapIdGet(mapId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MapsApi.GetMapByIdMapsIdMapIdGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetMapByIdMapsIdMapIdGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Map By Id
    ApiResponse<MapResponseSchema> response = apiInstance.GetMapByIdMapsIdMapIdGetWithHttpInfo(mapId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MapsApi.GetMapByIdMapsIdMapIdGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **mapId** | **int** | The unique ID of the map. |  |

### Return type

[**MapResponseSchema**](MapResponseSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched map. |  -  |
| **404** | map not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getmapbypositionmapslayerxyget"></a>
# **GetMapByPositionMapsLayerXYGet**
> MapResponseSchema GetMapByPositionMapsLayerXYGet (MapLayer layer, int x, int y)

Get Map By Position

Retrieve the details of a map by layer and coordinates.

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
    public class GetMapByPositionMapsLayerXYGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new MapsApi(httpClient, config, httpClientHandler);
            var layer = (MapLayer) "interior";  // MapLayer | The layer of the map (interior, overworld, underground).
            var x = 56;  // int | The position x of the map.
            var y = 56;  // int | The position y of the map.

            try
            {
                // Get Map By Position
                MapResponseSchema result = apiInstance.GetMapByPositionMapsLayerXYGet(layer, x, y);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MapsApi.GetMapByPositionMapsLayerXYGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetMapByPositionMapsLayerXYGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Map By Position
    ApiResponse<MapResponseSchema> response = apiInstance.GetMapByPositionMapsLayerXYGetWithHttpInfo(layer, x, y);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MapsApi.GetMapByPositionMapsLayerXYGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **layer** | **MapLayer** | The layer of the map (interior, overworld, underground). |  |
| **x** | **int** | The position x of the map. |  |
| **y** | **int** | The position y of the map. |  |

### Return type

[**MapResponseSchema**](MapResponseSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched map. |  -  |
| **404** | map not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

