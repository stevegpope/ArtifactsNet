# ArtifactsMmoClient.Model.ItemSchema

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Name** | **string** | Item name. | 
**Code** | **string** | Item code. This is the item&#39;s unique identifier (ID). | 
**Level** | **int** | Item level. | 
**Type** | **string** | Item type. | 
**Subtype** | **string** | Item subtype. | 
**Description** | **string** | Item description. | 
**Conditions** | [**List&lt;ConditionSchema&gt;**](ConditionSchema.md) | Item conditions. If applicable. Conditions for using or equipping the item. | [optional] 
**Effects** | [**List&lt;SimpleEffectSchema&gt;**](SimpleEffectSchema.md) | List of object effects. For equipment, it will include item stats. | [optional] 
**Craft** | [**CraftSchema**](CraftSchema.md) | Craft information. If applicable. | [optional] 
**Tradeable** | **bool** | Item tradeable status. A non-tradeable item cannot be exchanged or sold. | 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

