# ArtifactsMmoClient.Api.CharactersApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateCharacterCharactersCreatePost**](CharactersApi.md#createcharactercharacterscreatepost) | **POST** /characters/create | Create Character |
| [**DeleteCharacterCharactersDeletePost**](CharactersApi.md#deletecharactercharactersdeletepost) | **POST** /characters/delete | Delete Character |
| [**GetActiveCharactersCharactersActiveGet**](CharactersApi.md#getactivecharacterscharactersactiveget) | **GET** /characters/active | Get Active Characters |
| [**GetCharacterCharactersNameGet**](CharactersApi.md#getcharactercharactersnameget) | **GET** /characters/{name} | Get Character |

<a id="createcharactercharacterscreatepost"></a>
# **CreateCharacterCharactersCreatePost**
> CharacterResponseSchema CreateCharacterCharactersCreatePost (AddCharacterSchema addCharacterSchema)

Create Character

Create new character on your account. You can create up to 5 characters.

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
    public class CreateCharacterCharactersCreatePostExample
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
            var apiInstance = new CharactersApi(httpClient, config, httpClientHandler);
            var addCharacterSchema = new AddCharacterSchema(); // AddCharacterSchema | 

            try
            {
                // Create Character
                CharacterResponseSchema result = apiInstance.CreateCharacterCharactersCreatePost(addCharacterSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling CharactersApi.CreateCharacterCharactersCreatePost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateCharacterCharactersCreatePostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create Character
    ApiResponse<CharacterResponseSchema> response = apiInstance.CreateCharacterCharactersCreatePostWithHttpInfo(addCharacterSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling CharactersApi.CreateCharacterCharactersCreatePostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **addCharacterSchema** | [**AddCharacterSchema**](AddCharacterSchema.md) |  |  |

### Return type

[**CharacterResponseSchema**](CharacterResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully created character. |  -  |
| **494** | This name is already in use. |  -  |
| **495** | You have reached the maximum number of characters on your account. |  -  |
| **550** | You cannot choose this skin because you do not own it. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="deletecharactercharactersdeletepost"></a>
# **DeleteCharacterCharactersDeletePost**
> CharacterResponseSchema DeleteCharacterCharactersDeletePost (DeleteCharacterSchema deleteCharacterSchema)

Delete Character

Delete character on your account.

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
    public class DeleteCharacterCharactersDeletePostExample
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
            var apiInstance = new CharactersApi(httpClient, config, httpClientHandler);
            var deleteCharacterSchema = new DeleteCharacterSchema(); // DeleteCharacterSchema | 

            try
            {
                // Delete Character
                CharacterResponseSchema result = apiInstance.DeleteCharacterCharactersDeletePost(deleteCharacterSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling CharactersApi.DeleteCharacterCharactersDeletePost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteCharacterCharactersDeletePostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete Character
    ApiResponse<CharacterResponseSchema> response = apiInstance.DeleteCharacterCharactersDeletePostWithHttpInfo(deleteCharacterSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling CharactersApi.DeleteCharacterCharactersDeletePostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **deleteCharacterSchema** | [**DeleteCharacterSchema**](DeleteCharacterSchema.md) |  |  |

### Return type

[**CharacterResponseSchema**](CharacterResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully deleted character. |  -  |
| **498** | Character not found. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getactivecharacterscharactersactiveget"></a>
# **GetActiveCharactersCharactersActiveGet**
> DataPageActiveCharacterSchema GetActiveCharactersCharactersActiveGet (int? page = null, int? size = null)

Get Active Characters

Fetch active characters details.

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
    public class GetActiveCharactersCharactersActiveGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new CharactersApi(httpClient, config, httpClientHandler);
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get Active Characters
                DataPageActiveCharacterSchema result = apiInstance.GetActiveCharactersCharactersActiveGet(page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling CharactersApi.GetActiveCharactersCharactersActiveGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetActiveCharactersCharactersActiveGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Active Characters
    ApiResponse<DataPageActiveCharacterSchema> response = apiInstance.GetActiveCharactersCharactersActiveGetWithHttpInfo(page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling CharactersApi.GetActiveCharactersCharactersActiveGetWithHttpInfo: " + e.Message);
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

[**DataPageActiveCharacterSchema**](DataPageActiveCharacterSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched active characters. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getcharactercharactersnameget"></a>
# **GetCharacterCharactersNameGet**
> CharacterResponseSchema GetCharacterCharactersNameGet (string name)

Get Character

Retrieve the details of a character.

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
    public class GetCharacterCharactersNameGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new CharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | The name of the character.

            try
            {
                // Get Character
                CharacterResponseSchema result = apiInstance.GetCharacterCharactersNameGet(name);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling CharactersApi.GetCharacterCharactersNameGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetCharacterCharactersNameGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Character
    ApiResponse<CharacterResponseSchema> response = apiInstance.GetCharacterCharactersNameGetWithHttpInfo(name);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling CharactersApi.GetCharacterCharactersNameGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | The name of the character. |  |

### Return type

[**CharacterResponseSchema**](CharacterResponseSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched character. |  -  |
| **404** | character not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

