# ArtifactsMmoClient.Model.MonsterSchema

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Name** | **string** | Name of the monster. | 
**Code** | **string** | The code of the monster. This is the monster&#39;s unique identifier (ID). | 
**Level** | **int** | Monster level. | 
**Type** | **MonsterType** | Monster type. | 
**Hp** | **int** | Monster hit points. | 
**AttackFire** | **int** | Monster fire attack. | 
**AttackEarth** | **int** | Monster earth attack. | 
**AttackWater** | **int** | Monster water attack. | 
**AttackAir** | **int** | Monster air attack. | 
**ResFire** | **int** | Monster % fire resistance. | 
**ResEarth** | **int** | Monster % earth resistance. | 
**ResWater** | **int** | Monster % water resistance. | 
**ResAir** | **int** | Monster % air resistance. | 
**CriticalStrike** | **int** | Monster % critical strike. | 
**Initiative** | **int** | Monster initiative for turn order. | 
**Effects** | [**List&lt;SimpleEffectSchema&gt;**](SimpleEffectSchema.md) | List of effects. | [optional] 
**MinGold** | **int** | Monster minimum gold drop.  | 
**MaxGold** | **int** | Monster maximum gold drop.  | 
**Drops** | [**List&lt;DropRateSchema&gt;**](DropRateSchema.md) | Monster drops. This is a list of items that the monster drops after killing the monster.  | 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

