# ArtifactsMmoClient.Api.AccountsApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateAccountAccountsCreatePost**](AccountsApi.md#createaccountaccountscreatepost) | **POST** /accounts/create | Create Account |
| [**ForgotPasswordAccountsForgotPasswordPost**](AccountsApi.md#forgotpasswordaccountsforgotpasswordpost) | **POST** /accounts/forgot_password | Forgot Password |
| [**GetAccountAccountsAccountGet**](AccountsApi.md#getaccountaccountsaccountget) | **GET** /accounts/{account} | Get Account |
| [**GetAccountAchievementsAccountsAccountAchievementsGet**](AccountsApi.md#getaccountachievementsaccountsaccountachievementsget) | **GET** /accounts/{account}/achievements | Get Account Achievements |
| [**GetAccountCharactersAccountsAccountCharactersGet**](AccountsApi.md#getaccountcharactersaccountsaccountcharactersget) | **GET** /accounts/{account}/characters | Get Account Characters |
| [**ResetPasswordAccountsResetPasswordPost**](AccountsApi.md#resetpasswordaccountsresetpasswordpost) | **POST** /accounts/reset_password | Reset Password |

<a id="createaccountaccountscreatepost"></a>
# **CreateAccountAccountsCreatePost**
> ResponseSchema CreateAccountAccountsCreatePost (AddAccountSchema addAccountSchema)

Create Account

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
    public class CreateAccountAccountsCreatePostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new AccountsApi(httpClient, config, httpClientHandler);
            var addAccountSchema = new AddAccountSchema(); // AddAccountSchema | 

            try
            {
                // Create Account
                ResponseSchema result = apiInstance.CreateAccountAccountsCreatePost(addAccountSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AccountsApi.CreateAccountAccountsCreatePost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateAccountAccountsCreatePostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create Account
    ApiResponse<ResponseSchema> response = apiInstance.CreateAccountAccountsCreatePostWithHttpInfo(addAccountSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AccountsApi.CreateAccountAccountsCreatePostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **addAccountSchema** | [**AddAccountSchema**](AddAccountSchema.md) |  |  |

### Return type

[**ResponseSchema**](ResponseSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Account created successfully. |  -  |
| **456** | This username is already taken. |  -  |
| **457** | This email is already in use. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="forgotpasswordaccountsforgotpasswordpost"></a>
# **ForgotPasswordAccountsForgotPasswordPost**
> PasswordResetResponseSchema ForgotPasswordAccountsForgotPasswordPost (PasswordResetRequestSchema passwordResetRequestSchema)

Forgot Password

Request a password reset.

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
    public class ForgotPasswordAccountsForgotPasswordPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new AccountsApi(httpClient, config, httpClientHandler);
            var passwordResetRequestSchema = new PasswordResetRequestSchema(); // PasswordResetRequestSchema | 

            try
            {
                // Forgot Password
                PasswordResetResponseSchema result = apiInstance.ForgotPasswordAccountsForgotPasswordPost(passwordResetRequestSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AccountsApi.ForgotPasswordAccountsForgotPasswordPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ForgotPasswordAccountsForgotPasswordPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Forgot Password
    ApiResponse<PasswordResetResponseSchema> response = apiInstance.ForgotPasswordAccountsForgotPasswordPostWithHttpInfo(passwordResetRequestSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AccountsApi.ForgotPasswordAccountsForgotPasswordPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **passwordResetRequestSchema** | [**PasswordResetRequestSchema**](PasswordResetRequestSchema.md) |  |  |

### Return type

[**PasswordResetResponseSchema**](PasswordResetResponseSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | If this email address is associated with an account, a reset link has been sent. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getaccountaccountsaccountget"></a>
# **GetAccountAccountsAccountGet**
> AccountDetailsSchema GetAccountAccountsAccountGet (string account)

Get Account

Retrieve the details of an account.

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
    public class GetAccountAccountsAccountGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new AccountsApi(httpClient, config, httpClientHandler);
            var account = "account_example";  // string | The name of the account.

            try
            {
                // Get Account
                AccountDetailsSchema result = apiInstance.GetAccountAccountsAccountGet(account);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AccountsApi.GetAccountAccountsAccountGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAccountAccountsAccountGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Account
    ApiResponse<AccountDetailsSchema> response = apiInstance.GetAccountAccountsAccountGetWithHttpInfo(account);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AccountsApi.GetAccountAccountsAccountGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **account** | **string** | The name of the account. |  |

### Return type

[**AccountDetailsSchema**](AccountDetailsSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched account. |  -  |
| **404** | account not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getaccountachievementsaccountsaccountachievementsget"></a>
# **GetAccountAchievementsAccountsAccountAchievementsGet**
> DataPageAccountAchievementSchema GetAccountAchievementsAccountsAccountAchievementsGet (string account, AchievementType? type = null, bool? completed = null, int? page = null, int? size = null)

Get Account Achievements

Retrieve the achievements of a account.

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
    public class GetAccountAchievementsAccountsAccountAchievementsGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new AccountsApi(httpClient, config, httpClientHandler);
            var account = "account_example";  // string | The name of the account.
            var type = new AchievementType?(); // AchievementType? | Type of achievements. (optional) 
            var completed = true;  // bool? | Filter by completed achievements. (optional) 
            var page = 1;  // int? | Page number (optional)  (default to 1)
            var size = 50;  // int? | Page size (optional)  (default to 50)

            try
            {
                // Get Account Achievements
                DataPageAccountAchievementSchema result = apiInstance.GetAccountAchievementsAccountsAccountAchievementsGet(account, type, completed, page, size);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AccountsApi.GetAccountAchievementsAccountsAccountAchievementsGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAccountAchievementsAccountsAccountAchievementsGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Account Achievements
    ApiResponse<DataPageAccountAchievementSchema> response = apiInstance.GetAccountAchievementsAccountsAccountAchievementsGetWithHttpInfo(account, type, completed, page, size);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AccountsApi.GetAccountAchievementsAccountsAccountAchievementsGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **account** | **string** | The name of the account. |  |
| **type** | [**AchievementType?**](AchievementType?.md) | Type of achievements. | [optional]  |
| **completed** | **bool?** | Filter by completed achievements. | [optional]  |
| **page** | **int?** | Page number | [optional] [default to 1] |
| **size** | **int?** | Page size | [optional] [default to 50] |

### Return type

[**DataPageAccountAchievementSchema**](DataPageAccountAchievementSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched achievements. |  -  |
| **404** | Account not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getaccountcharactersaccountsaccountcharactersget"></a>
# **GetAccountCharactersAccountsAccountCharactersGet**
> CharactersListSchema GetAccountCharactersAccountsAccountCharactersGet (string account)

Get Account Characters

Account character lists.

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
    public class GetAccountCharactersAccountsAccountCharactersGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new AccountsApi(httpClient, config, httpClientHandler);
            var account = "account_example";  // string | The name of the account.

            try
            {
                // Get Account Characters
                CharactersListSchema result = apiInstance.GetAccountCharactersAccountsAccountCharactersGet(account);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AccountsApi.GetAccountCharactersAccountsAccountCharactersGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAccountCharactersAccountsAccountCharactersGetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Account Characters
    ApiResponse<CharactersListSchema> response = apiInstance.GetAccountCharactersAccountsAccountCharactersGetWithHttpInfo(account);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AccountsApi.GetAccountCharactersAccountsAccountCharactersGetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **account** | **string** | The name of the account. |  |

### Return type

[**CharactersListSchema**](CharactersListSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successfully fetched account characters. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="resetpasswordaccountsresetpasswordpost"></a>
# **ResetPasswordAccountsResetPasswordPost**
> PasswordResetResponseSchema ResetPasswordAccountsResetPasswordPost (PasswordResetConfirmSchema passwordResetConfirmSchema)

Reset Password

Reset password with a token. Use /forgot_password to get a token by email.

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
    public class ResetPasswordAccountsResetPasswordPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost";
            // create instances of HttpClient, HttpClientHandler to be reused later with different Api classes
            HttpClient httpClient = new HttpClient();
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            var apiInstance = new AccountsApi(httpClient, config, httpClientHandler);
            var passwordResetConfirmSchema = new PasswordResetConfirmSchema(); // PasswordResetConfirmSchema | 

            try
            {
                // Reset Password
                PasswordResetResponseSchema result = apiInstance.ResetPasswordAccountsResetPasswordPost(passwordResetConfirmSchema);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AccountsApi.ResetPasswordAccountsResetPasswordPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ResetPasswordAccountsResetPasswordPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Reset Password
    ApiResponse<PasswordResetResponseSchema> response = apiInstance.ResetPasswordAccountsResetPasswordPostWithHttpInfo(passwordResetConfirmSchema);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AccountsApi.ResetPasswordAccountsResetPasswordPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **passwordResetConfirmSchema** | [**PasswordResetConfirmSchema**](PasswordResetConfirmSchema.md) |  |  |

### Return type

[**PasswordResetResponseSchema**](PasswordResetResponseSchema.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Password has been successfully reset. |  -  |
| **561** | The password reset token has expired. |  -  |
| **562** | This password reset token has already been used. |  -  |
| **560** | The password reset token is invalid. |  -  |
| **422** | Request could not be processed due to an invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

