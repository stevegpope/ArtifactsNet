# ArtifactsMmoClient.Api.SimulationApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**FightSimulationSimulationFightSimulationPost**](SimulationApi.md#fightsimulationsimulationfightsimulationpost) | **POST** /simulation/fight_simulation | Fight Simulation |

<a id="fightsimulationsimulationfightsimulationpost"></a>
# **FightSimulationSimulationFightSimulationPost**
> CombatSimulationResponseSchema FightSimulationSimulationFightSimulationPost (CombatSimulationRequestSchema combatSimulationRequestSchema)

Fight Simulation

Simulate combat with fake characters against a monster multiple times. Member or founder account required.

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
    public class FightSimulationSimulationFightSimulationPostExample
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
            var apiInstance = new SimulationApi(httpClient, config, httpClientHandler);
            var combatSimulationRequestSchema = new CombatSimulationRequestSchema(); // CombatSimulationRequestSchema | 

            try
            {
                // Fight Simulation
                CombatSimulationResponseSchema result = apiInstance.FightSimulationSimulationFightSimulationPost(combatSimulationRequestSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SimulationApi.FightSimulationSimulationFightSimulationPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the FightSimulationSimulationFightSimulationPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Fight Simulation
    ApiResponse<CombatSimulationResponseSchema> response = apiInstance.FightSimulationSimulationFightSimulationPostWithHttpInfo(combatSimulationRequestSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SimulationApi.FightSimulationSimulationFightSimulationPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **combatSimulationRequestSchema** | [**CombatSimulationRequestSchema**](CombatSimulationRequestSchema.md) |  |  |

### Return type

[**CombatSimulationResponseSchema**](CombatSimulationResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Combat simulation completed successfully. |  -  |
| **404** | Monster not found. |  -  |
| **451** | Access denied, you must be a member to do that. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

