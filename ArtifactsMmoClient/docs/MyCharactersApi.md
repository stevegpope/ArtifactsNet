# ArtifactsMmoClient.Api.MyCharactersApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**ActionAcceptNewTaskMyNameActionTaskNewPost**](MyCharactersApi.md#actionacceptnewtaskmynameactiontasknewpost) | **POST** /my/{name}/action/task/new | Action Accept New Task |
| [**ActionBuyBankExpansionMyNameActionBankBuyExpansionPost**](MyCharactersApi.md#actionbuybankexpansionmynameactionbankbuyexpansionpost) | **POST** /my/{name}/action/bank/buy_expansion | Action Buy Bank Expansion |
| [**ActionChangeSkinMyNameActionChangeSkinPost**](MyCharactersApi.md#actionchangeskinmynameactionchangeskinpost) | **POST** /my/{name}/action/change_skin | Action Change Skin |
| [**ActionCompleteTaskMyNameActionTaskCompletePost**](MyCharactersApi.md#actioncompletetaskmynameactiontaskcompletepost) | **POST** /my/{name}/action/task/complete | Action Complete Task |
| [**ActionCraftingMyNameActionCraftingPost**](MyCharactersApi.md#actioncraftingmynameactioncraftingpost) | **POST** /my/{name}/action/crafting | Action Crafting |
| [**ActionDeleteItemMyNameActionDeletePost**](MyCharactersApi.md#actiondeleteitemmynameactiondeletepost) | **POST** /my/{name}/action/delete | Action Delete Item |
| [**ActionDepositBankGoldMyNameActionBankDepositGoldPost**](MyCharactersApi.md#actiondepositbankgoldmynameactionbankdepositgoldpost) | **POST** /my/{name}/action/bank/deposit/gold | Action Deposit Bank Gold |
| [**ActionDepositBankItemMyNameActionBankDepositItemPost**](MyCharactersApi.md#actiondepositbankitemmynameactionbankdeposititempost) | **POST** /my/{name}/action/bank/deposit/item | Action Deposit Bank Item |
| [**ActionEquipItemMyNameActionEquipPost**](MyCharactersApi.md#actionequipitemmynameactionequippost) | **POST** /my/{name}/action/equip | Action Equip Item |
| [**ActionFightMyNameActionFightPost**](MyCharactersApi.md#actionfightmynameactionfightpost) | **POST** /my/{name}/action/fight | Action Fight |
| [**ActionGatheringMyNameActionGatheringPost**](MyCharactersApi.md#actiongatheringmynameactiongatheringpost) | **POST** /my/{name}/action/gathering | Action Gathering |
| [**ActionGeBuyItemMyNameActionGrandexchangeBuyPost**](MyCharactersApi.md#actiongebuyitemmynameactiongrandexchangebuypost) | **POST** /my/{name}/action/grandexchange/buy | Action Ge Buy Item |
| [**ActionGeCancelSellOrderMyNameActionGrandexchangeCancelPost**](MyCharactersApi.md#actiongecancelsellordermynameactiongrandexchangecancelpost) | **POST** /my/{name}/action/grandexchange/cancel | Action Ge Cancel Sell Order |
| [**ActionGeCreateSellOrderMyNameActionGrandexchangeSellPost**](MyCharactersApi.md#actiongecreatesellordermynameactiongrandexchangesellpost) | **POST** /my/{name}/action/grandexchange/sell | Action Ge Create Sell Order |
| [**ActionGiveGoldMyNameActionGiveGoldPost**](MyCharactersApi.md#actiongivegoldmynameactiongivegoldpost) | **POST** /my/{name}/action/give/gold | Action Give Gold |
| [**ActionGiveItemsMyNameActionGiveItemPost**](MyCharactersApi.md#actiongiveitemsmynameactiongiveitempost) | **POST** /my/{name}/action/give/item | Action Give Items |
| [**ActionMoveMyNameActionMovePost**](MyCharactersApi.md#actionmovemynameactionmovepost) | **POST** /my/{name}/action/move | Action Move |
| [**ActionNpcBuyItemMyNameActionNpcBuyPost**](MyCharactersApi.md#actionnpcbuyitemmynameactionnpcbuypost) | **POST** /my/{name}/action/npc/buy | Action Npc Buy Item |
| [**ActionNpcSellItemMyNameActionNpcSellPost**](MyCharactersApi.md#actionnpcsellitemmynameactionnpcsellpost) | **POST** /my/{name}/action/npc/sell | Action Npc Sell Item |
| [**ActionRecyclingMyNameActionRecyclingPost**](MyCharactersApi.md#actionrecyclingmynameactionrecyclingpost) | **POST** /my/{name}/action/recycling | Action Recycling |
| [**ActionRestMyNameActionRestPost**](MyCharactersApi.md#actionrestmynameactionrestpost) | **POST** /my/{name}/action/rest | Action Rest |
| [**ActionTaskCancelMyNameActionTaskCancelPost**](MyCharactersApi.md#actiontaskcancelmynameactiontaskcancelpost) | **POST** /my/{name}/action/task/cancel | Action Task Cancel |
| [**ActionTaskExchangeMyNameActionTaskExchangePost**](MyCharactersApi.md#actiontaskexchangemynameactiontaskexchangepost) | **POST** /my/{name}/action/task/exchange | Action Task Exchange |
| [**ActionTaskTradeMyNameActionTaskTradePost**](MyCharactersApi.md#actiontasktrademynameactiontasktradepost) | **POST** /my/{name}/action/task/trade | Action Task Trade |
| [**ActionTransitionMyNameActionTransitionPost**](MyCharactersApi.md#actiontransitionmynameactiontransitionpost) | **POST** /my/{name}/action/transition | Action Transition |
| [**ActionUnequipItemMyNameActionUnequipPost**](MyCharactersApi.md#actionunequipitemmynameactionunequippost) | **POST** /my/{name}/action/unequip | Action Unequip Item |
| [**ActionUseItemMyNameActionUsePost**](MyCharactersApi.md#actionuseitemmynameactionusepost) | **POST** /my/{name}/action/use | Action Use Item |
| [**ActionWithdrawBankGoldMyNameActionBankWithdrawGoldPost**](MyCharactersApi.md#actionwithdrawbankgoldmynameactionbankwithdrawgoldpost) | **POST** /my/{name}/action/bank/withdraw/gold | Action Withdraw Bank Gold |
| [**ActionWithdrawBankItemMyNameActionBankWithdrawItemPost**](MyCharactersApi.md#actionwithdrawbankitemmynameactionbankwithdrawitempost) | **POST** /my/{name}/action/bank/withdraw/item | Action Withdraw Bank Item |
| [**GetAllCharactersLogsMyLogsGet**](MyCharactersApi.md#getallcharacterslogsmylogsget) | **GET** /my/logs | Get All Characters Logs |
| [**GetCharacterLogsMyLogsNameGet**](MyCharactersApi.md#getcharacterlogsmylogsnameget) | **GET** /my/logs/{name} | Get Character Logs |
| [**GetMyCharactersMyCharactersGet**](MyCharactersApi.md#getmycharactersmycharactersget) | **GET** /my/characters | Get My Characters |

<a id="actionacceptnewtaskmynameactiontasknewpost"></a>
# **ActionAcceptNewTaskMyNameActionTaskNewPost**
> TaskResponseSchema ActionAcceptNewTaskMyNameActionTaskNewPost (string name)

Action Accept New Task

Accepting a new task.

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
    public class ActionAcceptNewTaskMyNameActionTaskNewPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.

            try
            {
                // Action Accept New Task
                TaskResponseSchema result = apiInstance.ActionAcceptNewTaskMyNameActionTaskNewPost(name);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionAcceptNewTaskMyNameActionTaskNewPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionAcceptNewTaskMyNameActionTaskNewPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Accept New Task
    ApiResponse<TaskResponseSchema> response = apiInstance.ActionAcceptNewTaskMyNameActionTaskNewPostWithHttpInfo(name);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionAcceptNewTaskMyNameActionTaskNewPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |

### Return type

[**TaskResponseSchema**](TaskResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | New task successfully accepted. |  -  |
| **498** | Character not found. |  -  |
| **499** | The character is in cooldown. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **598** | Tasks Master not found on this map. |  -  |
| **489** | The character already has an assigned task. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actionbuybankexpansionmynameactionbankbuyexpansionpost"></a>
# **ActionBuyBankExpansionMyNameActionBankBuyExpansionPost**
> BankExtensionTransactionResponseSchema ActionBuyBankExpansionMyNameActionBankBuyExpansionPost (string name)

Action Buy Bank Expansion

Buy a 20 slots bank expansion.

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
    public class ActionBuyBankExpansionMyNameActionBankBuyExpansionPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.

            try
            {
                // Action Buy Bank Expansion
                BankExtensionTransactionResponseSchema result = apiInstance.ActionBuyBankExpansionMyNameActionBankBuyExpansionPost(name);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionBuyBankExpansionMyNameActionBankBuyExpansionPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionBuyBankExpansionMyNameActionBankBuyExpansionPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Buy Bank Expansion
    ApiResponse<BankExtensionTransactionResponseSchema> response = apiInstance.ActionBuyBankExpansionMyNameActionBankBuyExpansionPostWithHttpInfo(name);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionBuyBankExpansionMyNameActionBankBuyExpansionPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |

### Return type

[**BankExtensionTransactionResponseSchema**](BankExtensionTransactionResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Bank expansion successfully bought. |  -  |
| **598** | Bank not found on this map. |  -  |
| **498** | Character not found. |  -  |
| **499** | The character is in cooldown. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **492** | The character does not have enough gold. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actionchangeskinmynameactionchangeskinpost"></a>
# **ActionChangeSkinMyNameActionChangeSkinPost**
> ChangeSkinResponseSchema ActionChangeSkinMyNameActionChangeSkinPost (string name, ChangeSkinCharacterSchema changeSkinCharacterSchema)

Action Change Skin

Change the skin of your character.

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
    public class ActionChangeSkinMyNameActionChangeSkinPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.
            var changeSkinCharacterSchema = new ChangeSkinCharacterSchema(); // ChangeSkinCharacterSchema | 

            try
            {
                // Action Change Skin
                ChangeSkinResponseSchema result = apiInstance.ActionChangeSkinMyNameActionChangeSkinPost(name, changeSkinCharacterSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionChangeSkinMyNameActionChangeSkinPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionChangeSkinMyNameActionChangeSkinPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Change Skin
    ApiResponse<ChangeSkinResponseSchema> response = apiInstance.ActionChangeSkinMyNameActionChangeSkinPostWithHttpInfo(name, changeSkinCharacterSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionChangeSkinMyNameActionChangeSkinPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |
| **changeSkinCharacterSchema** | [**ChangeSkinCharacterSchema**](ChangeSkinCharacterSchema.md) |  |  |

### Return type

[**ChangeSkinResponseSchema**](ChangeSkinResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Skin successfully changed. |  -  |
| **499** | The character is in cooldown. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **550** | You cannot choose this skin because you do not own it. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actioncompletetaskmynameactiontaskcompletepost"></a>
# **ActionCompleteTaskMyNameActionTaskCompletePost**
> RewardDataResponseSchema ActionCompleteTaskMyNameActionTaskCompletePost (string name)

Action Complete Task

Complete a task.

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
    public class ActionCompleteTaskMyNameActionTaskCompletePostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.

            try
            {
                // Action Complete Task
                RewardDataResponseSchema result = apiInstance.ActionCompleteTaskMyNameActionTaskCompletePost(name);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionCompleteTaskMyNameActionTaskCompletePost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionCompleteTaskMyNameActionTaskCompletePostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Complete Task
    ApiResponse<RewardDataResponseSchema> response = apiInstance.ActionCompleteTaskMyNameActionTaskCompletePostWithHttpInfo(name);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionCompleteTaskMyNameActionTaskCompletePostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |

### Return type

[**RewardDataResponseSchema**](RewardDataResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The task has been successfully completed. |  -  |
| **498** | Character not found. |  -  |
| **499** | The character is in cooldown. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **598** | Tasks Master not found on this map. |  -  |
| **488** | The character has not completed the task. |  -  |
| **487** | The character has no task assigned. |  -  |
| **497** | The character&#39;s inventory is full. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actioncraftingmynameactioncraftingpost"></a>
# **ActionCraftingMyNameActionCraftingPost**
> SkillResponseSchema ActionCraftingMyNameActionCraftingPost (string name, CraftingSchema craftingSchema)

Action Crafting

Craft an item. The character must be on a map with a workshop.

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
    public class ActionCraftingMyNameActionCraftingPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.
            var craftingSchema = new CraftingSchema(); // CraftingSchema | 

            try
            {
                // Action Crafting
                SkillResponseSchema result = apiInstance.ActionCraftingMyNameActionCraftingPost(name, craftingSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionCraftingMyNameActionCraftingPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionCraftingMyNameActionCraftingPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Crafting
    ApiResponse<SkillResponseSchema> response = apiInstance.ActionCraftingMyNameActionCraftingPostWithHttpInfo(name, craftingSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionCraftingMyNameActionCraftingPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |
| **craftingSchema** | [**CraftingSchema**](CraftingSchema.md) |  |  |

### Return type

[**SkillResponseSchema**](SkillResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The item was successfully crafted. |  -  |
| **404** | Craft not found. |  -  |
| **598** | Workshop not found on this map. |  -  |
| **498** | Character not found. |  -  |
| **497** | The character&#39;s inventory is full. |  -  |
| **499** | The character is in cooldown. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **493** | The character&#39;s skill level is too low. |  -  |
| **478** | Missing required item(s). |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actiondeleteitemmynameactiondeletepost"></a>
# **ActionDeleteItemMyNameActionDeletePost**
> DeleteItemResponseSchema ActionDeleteItemMyNameActionDeletePost (string name, SimpleItemSchema simpleItemSchema)

Action Delete Item

Delete an item from your character's inventory.

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
    public class ActionDeleteItemMyNameActionDeletePostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.
            var simpleItemSchema = new SimpleItemSchema(); // SimpleItemSchema | 

            try
            {
                // Action Delete Item
                DeleteItemResponseSchema result = apiInstance.ActionDeleteItemMyNameActionDeletePost(name, simpleItemSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionDeleteItemMyNameActionDeletePost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionDeleteItemMyNameActionDeletePostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Delete Item
    ApiResponse<DeleteItemResponseSchema> response = apiInstance.ActionDeleteItemMyNameActionDeletePostWithHttpInfo(name, simpleItemSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionDeleteItemMyNameActionDeletePostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |
| **simpleItemSchema** | [**SimpleItemSchema**](SimpleItemSchema.md) |  |  |

### Return type

[**DeleteItemResponseSchema**](DeleteItemResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Item successfully deleted from your character. |  -  |
| **498** | Character not found. |  -  |
| **499** | The character is in cooldown. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **478** | Missing required item(s). |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actiondepositbankgoldmynameactionbankdepositgoldpost"></a>
# **ActionDepositBankGoldMyNameActionBankDepositGoldPost**
> BankGoldTransactionResponseSchema ActionDepositBankGoldMyNameActionBankDepositGoldPost (string name, DepositWithdrawGoldSchema depositWithdrawGoldSchema)

Action Deposit Bank Gold

Deposit gold in a bank on the character's map.

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
    public class ActionDepositBankGoldMyNameActionBankDepositGoldPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.
            var depositWithdrawGoldSchema = new DepositWithdrawGoldSchema(); // DepositWithdrawGoldSchema | 

            try
            {
                // Action Deposit Bank Gold
                BankGoldTransactionResponseSchema result = apiInstance.ActionDepositBankGoldMyNameActionBankDepositGoldPost(name, depositWithdrawGoldSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionDepositBankGoldMyNameActionBankDepositGoldPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionDepositBankGoldMyNameActionBankDepositGoldPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Deposit Bank Gold
    ApiResponse<BankGoldTransactionResponseSchema> response = apiInstance.ActionDepositBankGoldMyNameActionBankDepositGoldPostWithHttpInfo(name, depositWithdrawGoldSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionDepositBankGoldMyNameActionBankDepositGoldPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |
| **depositWithdrawGoldSchema** | [**DepositWithdrawGoldSchema**](DepositWithdrawGoldSchema.md) |  |  |

### Return type

[**BankGoldTransactionResponseSchema**](BankGoldTransactionResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Golds successfully deposited in your bank. |  -  |
| **598** | Bank not found on this map. |  -  |
| **492** | The character does not have enough gold. |  -  |
| **498** | Character not found. |  -  |
| **499** | The character is in cooldown. |  -  |
| **461** | Some of your items or your gold in the bank are already part of an ongoing transaction. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actiondepositbankitemmynameactionbankdeposititempost"></a>
# **ActionDepositBankItemMyNameActionBankDepositItemPost**
> BankItemTransactionResponseSchema ActionDepositBankItemMyNameActionBankDepositItemPost (string name, List<SimpleItemSchema> simpleItemSchema)

Action Deposit Bank Item

Deposit multiple items in a bank on the character's map. The cooldown will be 3 seconds multiplied by the number of different items deposited.

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
    public class ActionDepositBankItemMyNameActionBankDepositItemPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.
            var simpleItemSchema = new List<SimpleItemSchema>(); // List<SimpleItemSchema> | 

            try
            {
                // Action Deposit Bank Item
                BankItemTransactionResponseSchema result = apiInstance.ActionDepositBankItemMyNameActionBankDepositItemPost(name, simpleItemSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionDepositBankItemMyNameActionBankDepositItemPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionDepositBankItemMyNameActionBankDepositItemPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Deposit Bank Item
    ApiResponse<BankItemTransactionResponseSchema> response = apiInstance.ActionDepositBankItemMyNameActionBankDepositItemPostWithHttpInfo(name, simpleItemSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionDepositBankItemMyNameActionBankDepositItemPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |
| **simpleItemSchema** | [**List&lt;SimpleItemSchema&gt;**](SimpleItemSchema.md) |  |  |

### Return type

[**BankItemTransactionResponseSchema**](BankItemTransactionResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Items successfully deposited in your bank. |  -  |
| **598** | Bank not found on this map. |  -  |
| **404** | Item not found. |  -  |
| **461** | Some of your items or your gold in the bank are already part of an ongoing transaction. |  -  |
| **498** | Character not found. |  -  |
| **499** | The character is in cooldown. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **478** | Missing required item(s). |  -  |
| **462** | Your bank is full. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actionequipitemmynameactionequippost"></a>
# **ActionEquipItemMyNameActionEquipPost**
> EquipmentResponseSchema ActionEquipItemMyNameActionEquipPost (string name, EquipSchema equipSchema)

Action Equip Item

Equip an item on your character.

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
    public class ActionEquipItemMyNameActionEquipPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.
            var equipSchema = new EquipSchema(); // EquipSchema | 

            try
            {
                // Action Equip Item
                EquipmentResponseSchema result = apiInstance.ActionEquipItemMyNameActionEquipPost(name, equipSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionEquipItemMyNameActionEquipPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionEquipItemMyNameActionEquipPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Equip Item
    ApiResponse<EquipmentResponseSchema> response = apiInstance.ActionEquipItemMyNameActionEquipPostWithHttpInfo(name, equipSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionEquipItemMyNameActionEquipPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |
| **equipSchema** | [**EquipSchema**](EquipSchema.md) |  |  |

### Return type

[**EquipmentResponseSchema**](EquipmentResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The item has been successfully equipped on your character. |  -  |
| **404** | Item not found. |  -  |
| **498** | Character not found. |  -  |
| **483** | The character does not have enough HP to unequip this item. |  -  |
| **499** | The character is in cooldown. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **478** | Missing required item(s). |  -  |
| **496** | Conditions not met. |  -  |
| **491** | The equipment slot is not empty. |  -  |
| **485** | This item is already equipped. |  -  |
| **484** | The character cannot equip more than 100 utilities in the same slot. |  -  |
| **497** | The character&#39;s inventory is full. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actionfightmynameactionfightpost"></a>
# **ActionFightMyNameActionFightPost**
> CharacterFightResponseSchema ActionFightMyNameActionFightPost (string name, FightRequestSchema? fightRequestSchema = null)

Action Fight

Start a fight against a monster on the character's map. Add participants for multi-character fights (up to 3 characters, only for boss).

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
    public class ActionFightMyNameActionFightPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.
            var fightRequestSchema = new FightRequestSchema?(); // FightRequestSchema? |  (optional) 

            try
            {
                // Action Fight
                CharacterFightResponseSchema result = apiInstance.ActionFightMyNameActionFightPost(name, fightRequestSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionFightMyNameActionFightPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionFightMyNameActionFightPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Fight
    ApiResponse<CharacterFightResponseSchema> response = apiInstance.ActionFightMyNameActionFightPostWithHttpInfo(name, fightRequestSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionFightMyNameActionFightPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |
| **fightRequestSchema** | [**FightRequestSchema?**](FightRequestSchema?.md) |  | [optional]  |

### Return type

[**CharacterFightResponseSchema**](CharacterFightResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The fight ended successfully. |  -  |
| **498** | Character not found. |  -  |
| **499** | The character is in cooldown. |  -  |
| **598** | Monster not found on this map. |  -  |
| **486** | Only boss monsters can be fought by multiple characters. |  -  |
| **497** | The character&#39;s inventory is full. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actiongatheringmynameactiongatheringpost"></a>
# **ActionGatheringMyNameActionGatheringPost**
> SkillResponseSchema ActionGatheringMyNameActionGatheringPost (string name)

Action Gathering

Harvest a resource on the character's map.

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
    public class ActionGatheringMyNameActionGatheringPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.

            try
            {
                // Action Gathering
                SkillResponseSchema result = apiInstance.ActionGatheringMyNameActionGatheringPost(name);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionGatheringMyNameActionGatheringPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionGatheringMyNameActionGatheringPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Gathering
    ApiResponse<SkillResponseSchema> response = apiInstance.ActionGatheringMyNameActionGatheringPostWithHttpInfo(name);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionGatheringMyNameActionGatheringPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |

### Return type

[**SkillResponseSchema**](SkillResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The resource has been successfully gathered. |  -  |
| **498** | Character not found. |  -  |
| **499** | The character is in cooldown. |  -  |
| **598** | Resource not found on this map. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **493** | The character&#39;s skill level is too low. |  -  |
| **497** | The character&#39;s inventory is full. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actiongebuyitemmynameactiongrandexchangebuypost"></a>
# **ActionGeBuyItemMyNameActionGrandexchangeBuyPost**
> GETransactionResponseSchema ActionGeBuyItemMyNameActionGrandexchangeBuyPost (string name, GEBuyOrderSchema gEBuyOrderSchema)

Action Ge Buy Item

Buy an item at the Grand Exchange on the character's map.

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
    public class ActionGeBuyItemMyNameActionGrandexchangeBuyPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.
            var gEBuyOrderSchema = new GEBuyOrderSchema(); // GEBuyOrderSchema | 

            try
            {
                // Action Ge Buy Item
                GETransactionResponseSchema result = apiInstance.ActionGeBuyItemMyNameActionGrandexchangeBuyPost(name, gEBuyOrderSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionGeBuyItemMyNameActionGrandexchangeBuyPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionGeBuyItemMyNameActionGrandexchangeBuyPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Ge Buy Item
    ApiResponse<GETransactionResponseSchema> response = apiInstance.ActionGeBuyItemMyNameActionGrandexchangeBuyPostWithHttpInfo(name, gEBuyOrderSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionGeBuyItemMyNameActionGrandexchangeBuyPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |
| **gEBuyOrderSchema** | [**GEBuyOrderSchema**](GEBuyOrderSchema.md) |  |  |

### Return type

[**GETransactionResponseSchema**](GETransactionResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Item successfully bought from the Grand Exchange. |  -  |
| **598** | Grand Exchange not found on this map. |  -  |
| **498** | Character not found. |  -  |
| **497** | The character&#39;s inventory is full. |  -  |
| **499** | The character is in cooldown. |  -  |
| **436** | A transaction is already in progress for this order by another character. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **492** | The character does not have enough gold. |  -  |
| **434** | This offer does not contain that many items. |  -  |
| **435** | You cannot trade with yourself. |  -  |
| **404** | Order not found. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actiongecancelsellordermynameactiongrandexchangecancelpost"></a>
# **ActionGeCancelSellOrderMyNameActionGrandexchangeCancelPost**
> GETransactionResponseSchema ActionGeCancelSellOrderMyNameActionGrandexchangeCancelPost (string name, GECancelOrderSchema gECancelOrderSchema)

Action Ge Cancel Sell Order

Cancel a sell order at the Grand Exchange on the character's map.

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
    public class ActionGeCancelSellOrderMyNameActionGrandexchangeCancelPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.
            var gECancelOrderSchema = new GECancelOrderSchema(); // GECancelOrderSchema | 

            try
            {
                // Action Ge Cancel Sell Order
                GETransactionResponseSchema result = apiInstance.ActionGeCancelSellOrderMyNameActionGrandexchangeCancelPost(name, gECancelOrderSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionGeCancelSellOrderMyNameActionGrandexchangeCancelPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionGeCancelSellOrderMyNameActionGrandexchangeCancelPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Ge Cancel Sell Order
    ApiResponse<GETransactionResponseSchema> response = apiInstance.ActionGeCancelSellOrderMyNameActionGrandexchangeCancelPostWithHttpInfo(name, gECancelOrderSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionGeCancelSellOrderMyNameActionGrandexchangeCancelPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |
| **gECancelOrderSchema** | [**GECancelOrderSchema**](GECancelOrderSchema.md) |  |  |

### Return type

[**GETransactionResponseSchema**](GETransactionResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Your sell order has been successfully cancelled. |  -  |
| **598** | Grand Exchange not found on this map. |  -  |
| **498** | Character not found. |  -  |
| **497** | The character&#39;s inventory is full. |  -  |
| **499** | The character is in cooldown. |  -  |
| **436** | A transaction is already in progress for this order by another character. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **438** | You cannot cancel an order that is not yours. |  -  |
| **404** | Order not found. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actiongecreatesellordermynameactiongrandexchangesellpost"></a>
# **ActionGeCreateSellOrderMyNameActionGrandexchangeSellPost**
> GECreateOrderTransactionResponseSchema ActionGeCreateSellOrderMyNameActionGrandexchangeSellPost (string name, GEOrderCreationrSchema gEOrderCreationrSchema)

Action Ge Create Sell Order

Create a sell order at the Grand Exchange on the character's map.  Please note there is a 3% listing tax, charged at the time of posting, on the total price.

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
    public class ActionGeCreateSellOrderMyNameActionGrandexchangeSellPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.
            var gEOrderCreationrSchema = new GEOrderCreationrSchema(); // GEOrderCreationrSchema | 

            try
            {
                // Action Ge Create Sell Order
                GECreateOrderTransactionResponseSchema result = apiInstance.ActionGeCreateSellOrderMyNameActionGrandexchangeSellPost(name, gEOrderCreationrSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionGeCreateSellOrderMyNameActionGrandexchangeSellPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionGeCreateSellOrderMyNameActionGrandexchangeSellPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Ge Create Sell Order
    ApiResponse<GECreateOrderTransactionResponseSchema> response = apiInstance.ActionGeCreateSellOrderMyNameActionGrandexchangeSellPostWithHttpInfo(name, gEOrderCreationrSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionGeCreateSellOrderMyNameActionGrandexchangeSellPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |
| **gEOrderCreationrSchema** | [**GEOrderCreationrSchema**](GEOrderCreationrSchema.md) |  |  |

### Return type

[**GECreateOrderTransactionResponseSchema**](GECreateOrderTransactionResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The sell order has been successfully created. |  -  |
| **498** | Character not found. |  -  |
| **499** | The character is in cooldown. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **404** | Item not found. |  -  |
| **478** | Missing required item(s). |  -  |
| **492** | The character does not have enough gold. |  -  |
| **433** | You cannot create more than 100 orders at the same time. |  -  |
| **437** | This item cannot be sold. |  -  |
| **598** | Grand Exchange not found on this map. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actiongivegoldmynameactiongivegoldpost"></a>
# **ActionGiveGoldMyNameActionGiveGoldPost**
> GiveGoldResponseSchema ActionGiveGoldMyNameActionGiveGoldPost (string name, GiveGoldSchema giveGoldSchema)

Action Give Gold

Give gold to another character in your account on the same map.

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
    public class ActionGiveGoldMyNameActionGiveGoldPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.
            var giveGoldSchema = new GiveGoldSchema(); // GiveGoldSchema | 

            try
            {
                // Action Give Gold
                GiveGoldResponseSchema result = apiInstance.ActionGiveGoldMyNameActionGiveGoldPost(name, giveGoldSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionGiveGoldMyNameActionGiveGoldPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionGiveGoldMyNameActionGiveGoldPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Give Gold
    ApiResponse<GiveGoldResponseSchema> response = apiInstance.ActionGiveGoldMyNameActionGiveGoldPostWithHttpInfo(name, giveGoldSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionGiveGoldMyNameActionGiveGoldPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |
| **giveGoldSchema** | [**GiveGoldSchema**](GiveGoldSchema.md) |  |  |

### Return type

[**GiveGoldResponseSchema**](GiveGoldResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Gold given successfully. |  -  |
| **498** | Character not found. |  -  |
| **499** | The character is in cooldown. |  -  |
| **492** | The character does not have enough gold. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actiongiveitemsmynameactiongiveitempost"></a>
# **ActionGiveItemsMyNameActionGiveItemPost**
> GiveItemResponseSchema ActionGiveItemsMyNameActionGiveItemPost (string name, GiveItemsSchema giveItemsSchema)

Action Give Items

Give items to another character in your account on the same map. The cooldown will be 3 seconds multiplied by the number of different items given.

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
    public class ActionGiveItemsMyNameActionGiveItemPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.
            var giveItemsSchema = new GiveItemsSchema(); // GiveItemsSchema | 

            try
            {
                // Action Give Items
                GiveItemResponseSchema result = apiInstance.ActionGiveItemsMyNameActionGiveItemPost(name, giveItemsSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionGiveItemsMyNameActionGiveItemPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionGiveItemsMyNameActionGiveItemPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Give Items
    ApiResponse<GiveItemResponseSchema> response = apiInstance.ActionGiveItemsMyNameActionGiveItemPostWithHttpInfo(name, giveItemsSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionGiveItemsMyNameActionGiveItemPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |
| **giveItemsSchema** | [**GiveItemsSchema**](GiveItemsSchema.md) |  |  |

### Return type

[**GiveItemResponseSchema**](GiveItemResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Items given successfully. |  -  |
| **404** | Item not found. |  -  |
| **498** | Character not found. |  -  |
| **499** | The character is in cooldown. |  -  |
| **497** | The character&#39;s inventory is full. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **478** | Missing required item(s). |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actionmovemynameactionmovepost"></a>
# **ActionMoveMyNameActionMovePost**
> CharacterMovementResponseSchema ActionMoveMyNameActionMovePost (string name, DestinationSchema destinationSchema)

Action Move

Moves a character on the map using either the map's ID or X and Y position. Provide either 'map_id' or both 'x' and 'y' coordinates in the request body.

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
    public class ActionMoveMyNameActionMovePostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.
            var destinationSchema = new DestinationSchema(); // DestinationSchema | 

            try
            {
                // Action Move
                CharacterMovementResponseSchema result = apiInstance.ActionMoveMyNameActionMovePost(name, destinationSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionMoveMyNameActionMovePost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionMoveMyNameActionMovePostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Move
    ApiResponse<CharacterMovementResponseSchema> response = apiInstance.ActionMoveMyNameActionMovePostWithHttpInfo(name, destinationSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionMoveMyNameActionMovePostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |
| **destinationSchema** | [**DestinationSchema**](DestinationSchema.md) |  |  |

### Return type

[**CharacterMovementResponseSchema**](CharacterMovementResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The character has moved successfully. |  -  |
| **498** | Character not found. |  -  |
| **499** | The character is in cooldown. |  -  |
| **490** | The character is already at the destination. |  -  |
| **404** | Map not found. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **595** | No path available to the destination map. |  -  |
| **596** | The map is blocked and cannot be accessed. |  -  |
| **496** | Conditions not met. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actionnpcbuyitemmynameactionnpcbuypost"></a>
# **ActionNpcBuyItemMyNameActionNpcBuyPost**
> NpcMerchantTransactionResponseSchema ActionNpcBuyItemMyNameActionNpcBuyPost (string name, NpcMerchantBuySchema npcMerchantBuySchema)

Action Npc Buy Item

Buy an item from an NPC on the character's map.

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
    public class ActionNpcBuyItemMyNameActionNpcBuyPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.
            var npcMerchantBuySchema = new NpcMerchantBuySchema(); // NpcMerchantBuySchema | 

            try
            {
                // Action Npc Buy Item
                NpcMerchantTransactionResponseSchema result = apiInstance.ActionNpcBuyItemMyNameActionNpcBuyPost(name, npcMerchantBuySchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionNpcBuyItemMyNameActionNpcBuyPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionNpcBuyItemMyNameActionNpcBuyPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Npc Buy Item
    ApiResponse<NpcMerchantTransactionResponseSchema> response = apiInstance.ActionNpcBuyItemMyNameActionNpcBuyPostWithHttpInfo(name, npcMerchantBuySchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionNpcBuyItemMyNameActionNpcBuyPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |
| **npcMerchantBuySchema** | [**NpcMerchantBuySchema**](NpcMerchantBuySchema.md) |  |  |

### Return type

[**NpcMerchantTransactionResponseSchema**](NpcMerchantTransactionResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Item successfully bought from the NPC. |  -  |
| **598** | NPC not found on this map. |  -  |
| **498** | Character not found. |  -  |
| **497** | The character&#39;s inventory is full. |  -  |
| **499** | The character is in cooldown. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **492** | The character does not have enough gold. |  -  |
| **441** | This item is not available for purchase. |  -  |
| **478** | Missing required item(s). |  -  |
| **404** | Item not found. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actionnpcsellitemmynameactionnpcsellpost"></a>
# **ActionNpcSellItemMyNameActionNpcSellPost**
> NpcMerchantTransactionResponseSchema ActionNpcSellItemMyNameActionNpcSellPost (string name, NpcMerchantBuySchema npcMerchantBuySchema)

Action Npc Sell Item

Sell an item to an NPC on the character's map.

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
    public class ActionNpcSellItemMyNameActionNpcSellPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.
            var npcMerchantBuySchema = new NpcMerchantBuySchema(); // NpcMerchantBuySchema | 

            try
            {
                // Action Npc Sell Item
                NpcMerchantTransactionResponseSchema result = apiInstance.ActionNpcSellItemMyNameActionNpcSellPost(name, npcMerchantBuySchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionNpcSellItemMyNameActionNpcSellPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionNpcSellItemMyNameActionNpcSellPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Npc Sell Item
    ApiResponse<NpcMerchantTransactionResponseSchema> response = apiInstance.ActionNpcSellItemMyNameActionNpcSellPostWithHttpInfo(name, npcMerchantBuySchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionNpcSellItemMyNameActionNpcSellPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |
| **npcMerchantBuySchema** | [**NpcMerchantBuySchema**](NpcMerchantBuySchema.md) |  |  |

### Return type

[**NpcMerchantTransactionResponseSchema**](NpcMerchantTransactionResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Item successfully sold to the NPC. |  -  |
| **598** | NPC not found on this map. |  -  |
| **498** | Character not found. |  -  |
| **497** | The character&#39;s inventory is full. |  -  |
| **499** | The character is in cooldown. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **478** | Missing required item(s). |  -  |
| **442** | This item cannot be sold. |  -  |
| **404** | Item not found. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actionrecyclingmynameactionrecyclingpost"></a>
# **ActionRecyclingMyNameActionRecyclingPost**
> RecyclingResponseSchema ActionRecyclingMyNameActionRecyclingPost (string name, RecyclingSchema recyclingSchema)

Action Recycling

Recycling an item. The character must be on a map with a workshop (only for equipments and weapons).

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
    public class ActionRecyclingMyNameActionRecyclingPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.
            var recyclingSchema = new RecyclingSchema(); // RecyclingSchema | 

            try
            {
                // Action Recycling
                RecyclingResponseSchema result = apiInstance.ActionRecyclingMyNameActionRecyclingPost(name, recyclingSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionRecyclingMyNameActionRecyclingPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionRecyclingMyNameActionRecyclingPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Recycling
    ApiResponse<RecyclingResponseSchema> response = apiInstance.ActionRecyclingMyNameActionRecyclingPostWithHttpInfo(name, recyclingSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionRecyclingMyNameActionRecyclingPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |
| **recyclingSchema** | [**RecyclingSchema**](RecyclingSchema.md) |  |  |

### Return type

[**RecyclingResponseSchema**](RecyclingResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The items were successfully recycled. |  -  |
| **404** | Item not found. |  -  |
| **598** | Workshop not found on this map. |  -  |
| **498** | Character not found. |  -  |
| **497** | The character&#39;s inventory is full. |  -  |
| **499** | The character is in cooldown. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **493** | The character&#39;s skill level is too low. |  -  |
| **478** | Missing required item(s). |  -  |
| **473** | This item cannot be recycled. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actionrestmynameactionrestpost"></a>
# **ActionRestMyNameActionRestPost**
> CharacterRestResponseSchema ActionRestMyNameActionRestPost (string name)

Action Rest

Recovers hit points by resting. (1 second per 5 HP, minimum 3 seconds)

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
    public class ActionRestMyNameActionRestPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.

            try
            {
                // Action Rest
                CharacterRestResponseSchema result = apiInstance.ActionRestMyNameActionRestPost(name);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionRestMyNameActionRestPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionRestMyNameActionRestPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Rest
    ApiResponse<CharacterRestResponseSchema> response = apiInstance.ActionRestMyNameActionRestPostWithHttpInfo(name);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionRestMyNameActionRestPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |

### Return type

[**CharacterRestResponseSchema**](CharacterRestResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The character has rested successfully. |  -  |
| **498** | Character not found. |  -  |
| **499** | The character is in cooldown. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actiontaskcancelmynameactiontaskcancelpost"></a>
# **ActionTaskCancelMyNameActionTaskCancelPost**
> TaskCancelledResponseSchema ActionTaskCancelMyNameActionTaskCancelPost (string name)

Action Task Cancel

Cancel a task for 1 tasks coin.

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
    public class ActionTaskCancelMyNameActionTaskCancelPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.

            try
            {
                // Action Task Cancel
                TaskCancelledResponseSchema result = apiInstance.ActionTaskCancelMyNameActionTaskCancelPost(name);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionTaskCancelMyNameActionTaskCancelPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionTaskCancelMyNameActionTaskCancelPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Task Cancel
    ApiResponse<TaskCancelledResponseSchema> response = apiInstance.ActionTaskCancelMyNameActionTaskCancelPostWithHttpInfo(name);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionTaskCancelMyNameActionTaskCancelPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |

### Return type

[**TaskCancelledResponseSchema**](TaskCancelledResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The task has been successfully cancelled. |  -  |
| **498** | Character not found. |  -  |
| **499** | The character is in cooldown. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **598** | Tasks Master not found on this map. |  -  |
| **478** | Missing required item(s). |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actiontaskexchangemynameactiontaskexchangepost"></a>
# **ActionTaskExchangeMyNameActionTaskExchangePost**
> RewardDataResponseSchema ActionTaskExchangeMyNameActionTaskExchangePost (string name)

Action Task Exchange

Exchange 6 tasks coins for a random reward. Rewards are exclusive items or resources.

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
    public class ActionTaskExchangeMyNameActionTaskExchangePostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.

            try
            {
                // Action Task Exchange
                RewardDataResponseSchema result = apiInstance.ActionTaskExchangeMyNameActionTaskExchangePost(name);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionTaskExchangeMyNameActionTaskExchangePost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionTaskExchangeMyNameActionTaskExchangePostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Task Exchange
    ApiResponse<RewardDataResponseSchema> response = apiInstance.ActionTaskExchangeMyNameActionTaskExchangePostWithHttpInfo(name);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionTaskExchangeMyNameActionTaskExchangePostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |

### Return type

[**RewardDataResponseSchema**](RewardDataResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The tasks coins have been successfully exchanged. |  -  |
| **498** | Character not found. |  -  |
| **499** | The character is in cooldown. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **598** | Tasks Master not found on this map. |  -  |
| **478** | Missing required item(s). |  -  |
| **497** | The character&#39;s inventory is full. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actiontasktrademynameactiontasktradepost"></a>
# **ActionTaskTradeMyNameActionTaskTradePost**
> TaskTradeResponseSchema ActionTaskTradeMyNameActionTaskTradePost (string name, SimpleItemSchema simpleItemSchema)

Action Task Trade

Trading items with a Tasks Master.

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
    public class ActionTaskTradeMyNameActionTaskTradePostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.
            var simpleItemSchema = new SimpleItemSchema(); // SimpleItemSchema | 

            try
            {
                // Action Task Trade
                TaskTradeResponseSchema result = apiInstance.ActionTaskTradeMyNameActionTaskTradePost(name, simpleItemSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionTaskTradeMyNameActionTaskTradePost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionTaskTradeMyNameActionTaskTradePostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Task Trade
    ApiResponse<TaskTradeResponseSchema> response = apiInstance.ActionTaskTradeMyNameActionTaskTradePostWithHttpInfo(name, simpleItemSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionTaskTradeMyNameActionTaskTradePostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |
| **simpleItemSchema** | [**SimpleItemSchema**](SimpleItemSchema.md) |  |  |

### Return type

[**TaskTradeResponseSchema**](TaskTradeResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | You have successfully trade items to a Tasks Master. |  -  |
| **498** | Character not found. |  -  |
| **499** | The character is in cooldown. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **598** | Tasks Master not found on this map. |  -  |
| **475** | Task already completed or too many items submitted. |  -  |
| **474** | The character does not have this task. |  -  |
| **478** | Missing required item(s). |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actiontransitionmynameactiontransitionpost"></a>
# **ActionTransitionMyNameActionTransitionPost**
> CharacterTransitionResponseSchema ActionTransitionMyNameActionTransitionPost (string name)

Action Transition

Execute a transition from the current map to another layer. The character must be on a map that has a transition available.

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
    public class ActionTransitionMyNameActionTransitionPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.

            try
            {
                // Action Transition
                CharacterTransitionResponseSchema result = apiInstance.ActionTransitionMyNameActionTransitionPost(name);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionTransitionMyNameActionTransitionPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionTransitionMyNameActionTransitionPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Transition
    ApiResponse<CharacterTransitionResponseSchema> response = apiInstance.ActionTransitionMyNameActionTransitionPostWithHttpInfo(name);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionTransitionMyNameActionTransitionPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |

### Return type

[**CharacterTransitionResponseSchema**](CharacterTransitionResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The character has transitioned successfully. |  -  |
| **498** | Character not found. |  -  |
| **499** | The character is in cooldown. |  -  |
| **404** | Transition not found. |  -  |
| **492** | Insufficient gold for this transition. |  -  |
| **478** | Missing required item(s). |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **496** | Conditions not met. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actionunequipitemmynameactionunequippost"></a>
# **ActionUnequipItemMyNameActionUnequipPost**
> EquipmentResponseSchema ActionUnequipItemMyNameActionUnequipPost (string name, UnequipSchema unequipSchema)

Action Unequip Item

Unequip an item on your character.

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
    public class ActionUnequipItemMyNameActionUnequipPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.
            var unequipSchema = new UnequipSchema(); // UnequipSchema | 

            try
            {
                // Action Unequip Item
                EquipmentResponseSchema result = apiInstance.ActionUnequipItemMyNameActionUnequipPost(name, unequipSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionUnequipItemMyNameActionUnequipPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionUnequipItemMyNameActionUnequipPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Unequip Item
    ApiResponse<EquipmentResponseSchema> response = apiInstance.ActionUnequipItemMyNameActionUnequipPostWithHttpInfo(name, unequipSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionUnequipItemMyNameActionUnequipPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |
| **unequipSchema** | [**UnequipSchema**](UnequipSchema.md) |  |  |

### Return type

[**EquipmentResponseSchema**](EquipmentResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The item has been successfully unequipped and added in their inventory. |  -  |
| **404** | Item not found. |  -  |
| **498** | Character not found. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **491** | The equipment slot is empty. |  -  |
| **497** | The character&#39;s inventory is full. |  -  |
| **478** | Missing required item(s). |  -  |
| **483** | The character does not have enough HP to unequip this item. |  -  |
| **499** | The character is in cooldown. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actionuseitemmynameactionusepost"></a>
# **ActionUseItemMyNameActionUsePost**
> UseItemResponseSchema ActionUseItemMyNameActionUsePost (string name, SimpleItemSchema simpleItemSchema)

Action Use Item

Use an item as a consumable.

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
    public class ActionUseItemMyNameActionUsePostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.
            var simpleItemSchema = new SimpleItemSchema(); // SimpleItemSchema | 

            try
            {
                // Action Use Item
                UseItemResponseSchema result = apiInstance.ActionUseItemMyNameActionUsePost(name, simpleItemSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionUseItemMyNameActionUsePost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionUseItemMyNameActionUsePostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Use Item
    ApiResponse<UseItemResponseSchema> response = apiInstance.ActionUseItemMyNameActionUsePostWithHttpInfo(name, simpleItemSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionUseItemMyNameActionUsePostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |
| **simpleItemSchema** | [**SimpleItemSchema**](SimpleItemSchema.md) |  |  |

### Return type

[**UseItemResponseSchema**](UseItemResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The item has been successfully used. |  -  |
| **404** | Item not found. |  -  |
| **498** | Character not found. |  -  |
| **499** | The character is in cooldown. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **476** | This item is not a consumable. |  -  |
| **478** | Missing required item(s). |  -  |
| **496** | Conditions not met. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actionwithdrawbankgoldmynameactionbankwithdrawgoldpost"></a>
# **ActionWithdrawBankGoldMyNameActionBankWithdrawGoldPost**
> BankGoldTransactionResponseSchema ActionWithdrawBankGoldMyNameActionBankWithdrawGoldPost (string name, DepositWithdrawGoldSchema depositWithdrawGoldSchema)

Action Withdraw Bank Gold

Withdraw gold from your bank.

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
    public class ActionWithdrawBankGoldMyNameActionBankWithdrawGoldPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.
            var depositWithdrawGoldSchema = new DepositWithdrawGoldSchema(); // DepositWithdrawGoldSchema | 

            try
            {
                // Action Withdraw Bank Gold
                BankGoldTransactionResponseSchema result = apiInstance.ActionWithdrawBankGoldMyNameActionBankWithdrawGoldPost(name, depositWithdrawGoldSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionWithdrawBankGoldMyNameActionBankWithdrawGoldPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionWithdrawBankGoldMyNameActionBankWithdrawGoldPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Withdraw Bank Gold
    ApiResponse<BankGoldTransactionResponseSchema> response = apiInstance.ActionWithdrawBankGoldMyNameActionBankWithdrawGoldPostWithHttpInfo(name, depositWithdrawGoldSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionWithdrawBankGoldMyNameActionBankWithdrawGoldPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |
| **depositWithdrawGoldSchema** | [**DepositWithdrawGoldSchema**](DepositWithdrawGoldSchema.md) |  |  |

### Return type

[**BankGoldTransactionResponseSchema**](BankGoldTransactionResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Golds successfully withdraw from your bank. |  -  |
| **498** | Character not found. |  -  |
| **499** | The character is in cooldown. |  -  |
| **461** | Some of your items or your gold in the bank are already part of an ongoing transaction. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **598** | Bank not found on this map. |  -  |
| **460** | Insufficient gold in your bank. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="actionwithdrawbankitemmynameactionbankwithdrawitempost"></a>
# **ActionWithdrawBankItemMyNameActionBankWithdrawItemPost**
> BankItemTransactionResponseSchema ActionWithdrawBankItemMyNameActionBankWithdrawItemPost (string name, List<SimpleItemSchema> simpleItemSchema)

Action Withdraw Bank Item

Take items from your bank and put them in the character's inventory. The cooldown will be 3 seconds multiplied by the number of different items withdrawn.

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
    public class ActionWithdrawBankItemMyNameActionBankWithdrawItemPostExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.
            var simpleItemSchema = new List<SimpleItemSchema>(); // List<SimpleItemSchema> | 

            try
            {
                // Action Withdraw Bank Item
                BankItemTransactionResponseSchema result = apiInstance.ActionWithdrawBankItemMyNameActionBankWithdrawItemPost(name, simpleItemSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.ActionWithdrawBankItemMyNameActionBankWithdrawItemPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActionWithdrawBankItemMyNameActionBankWithdrawItemPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Action Withdraw Bank Item
    ApiResponse<BankItemTransactionResponseSchema> response = apiInstance.ActionWithdrawBankItemMyNameActionBankWithdrawItemPostWithHttpInfo(name, simpleItemSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.ActionWithdrawBankItemMyNameActionBankWithdrawItemPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |
| **simpleItemSchema** | [**List&lt;SimpleItemSchema&gt;**](SimpleItemSchema.md) |  |  |

### Return type

[**BankItemTransactionResponseSchema**](BankItemTransactionResponseSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Items successfully withdrawn from your bank. |  -  |
| **404** | Item not found. |  -  |
| **498** | Character not found. |  -  |
| **499** | The character is in cooldown. |  -  |
| **461** | Some of your items or your gold in the bank are already part of an ongoing transaction. |  -  |
| **486** | An action is already in progress for this character. |  -  |
| **497** | The character&#39;s inventory is full. |  -  |
| **598** | Bank not found on this map. |  -  |
| **478** | Missing required item(s). |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getallcharacterslogsmylogsget"></a>
# **GetAllCharactersLogsMyLogsGet**
> DataPageLogSchema GetAllCharactersLogsMyLogsGet (int? page = null, int? size = null)

Get All Characters Logs

History of the last 5000 actions of all your characters.

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
    public class GetAllCharactersLogsMyLogsGetExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get All Characters Logs
                DataPageLogSchema result = apiInstance.GetAllCharactersLogsMyLogsGet(page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.GetAllCharactersLogsMyLogsGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAllCharactersLogsMyLogsGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get All Characters Logs
    ApiResponse<DataPageLogSchema> response = apiInstance.GetAllCharactersLogsMyLogsGetWithHttpInfo(page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.GetAllCharactersLogsMyLogsGetWithHttpInfo: " + e.Message);
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

[**DataPageLogSchema**](DataPageLogSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched logs. |  -  |
| **404** | Logs not found. |  -  |
| **498** | Character not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getcharacterlogsmylogsnameget"></a>
# **GetCharacterLogsMyLogsNameGet**
> DataPageLogSchema GetCharacterLogsMyLogsNameGet (string name, int? page = null, int? size = null)

Get Character Logs

History of the last actions of your character.

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
    public class GetCharacterLogsMyLogsNameGetExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);
            var name = "name_example";  // string | Name of your character.
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get Character Logs
                DataPageLogSchema result = apiInstance.GetCharacterLogsMyLogsNameGet(name, page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.GetCharacterLogsMyLogsNameGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetCharacterLogsMyLogsNameGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Character Logs
    ApiResponse<DataPageLogSchema> response = apiInstance.GetCharacterLogsMyLogsNameGetWithHttpInfo(name, page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.GetCharacterLogsMyLogsNameGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | Name of your character. |  |
| **page** | **int?** | Page number | [optional] [default to 1] |
| **size** | **int?** | Page size | [optional] [default to 50] |

### Return type

[**DataPageLogSchema**](DataPageLogSchema.md)

### Authorization

[JWTBearer](../README.md#JWTBearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched logs. |  -  |
| **404** | Logs not found. |  -  |
| **498** | Character not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getmycharactersmycharactersget"></a>
# **GetMyCharactersMyCharactersGet**
> MyCharactersListSchema GetMyCharactersMyCharactersGet ()

Get My Characters

List of your characters.

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
    public class GetMyCharactersMyCharactersGetExample
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
            var apiInstance = new MyCharactersApi(httpClient, config, httpClientHandler);

            try
            {
                // Get My Characters
                MyCharactersListSchema result = apiInstance.GetMyCharactersMyCharactersGet();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MyCharactersApi.GetMyCharactersMyCharactersGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetMyCharactersMyCharactersGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get My Characters
    ApiResponse<MyCharactersListSchema> response = apiInstance.GetMyCharactersMyCharactersGetWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MyCharactersApi.GetMyCharactersMyCharactersGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**MyCharactersListSchema**](MyCharactersListSchema.md)

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

