# ArtifactsMmoClient.Model.PendingItemSchema
Schema for pending items that can be claimed by any character.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Id** | **string** | Pending item ID. | 
**Account** | **string** | Account username. | 
**Source** | **PendingItemSource** | Source of the pending item. | 
**SourceId** | **string** | ID reference for the source (e.g., achievement code, order id). | [optional] 
**Description** | **string** | Description for display. | 
**Gold** | **int** | Gold amount. | [optional] [default to 0]
**Items** | [**List&lt;SimpleItemSchema&gt;**](SimpleItemSchema.md) | List of items to be claimed. | [optional] 
**CreatedAt** | **DateTimeOffset** | When the pending item was created. | 
**ClaimedAt** | **DateTimeOffset** | When the pending item was claimed. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

