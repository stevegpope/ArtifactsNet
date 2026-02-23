# ArtifactsMmoClient.Model.AccountAchievementSchema
Full achievement data with progress - for API responses.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Name** | **string** | Name of the achievement. | 
**Code** | **string** | Code of the achievement. | 
**Description** | **string** | Description of the achievement. | 
**Points** | **int** | Points of the achievement. Used for the leaderboard. | 
**Objectives** | [**List&lt;AccountAchievementObjectiveSchema&gt;**](AccountAchievementObjectiveSchema.md) | List of objectives with progress. | 
**Rewards** | [**AchievementRewardsSchema**](AchievementRewardsSchema.md) | Rewards. | 
**CompletedAt** | **DateTimeOffset** | Completion timestamp. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

