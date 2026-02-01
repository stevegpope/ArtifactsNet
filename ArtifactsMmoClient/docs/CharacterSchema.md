# ArtifactsMmoClient.Model.CharacterSchema

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Name** | **string** | Name of the character. | 
**Account** | **string** | Account name. | 
**Skin** | **CharacterSkin** | Character skin code. | 
**Level** | **int** | Combat level. | 
**Xp** | **int** | The current xp level of the combat level. | 
**MaxXp** | **int** | XP required to level up the character. | 
**Gold** | **int** | The numbers of gold on this character. | 
**Speed** | **int** | *Not available, on the roadmap. Character movement speed. | 
**MiningLevel** | **int** | Mining level. | 
**MiningXp** | **int** | The current xp level of the Mining skill. | 
**MiningMaxXp** | **int** | Mining XP required to level up the skill. | 
**WoodcuttingLevel** | **int** | Woodcutting level. | 
**WoodcuttingXp** | **int** | The current xp level of the Woodcutting skill. | 
**WoodcuttingMaxXp** | **int** | Woodcutting XP required to level up the skill. | 
**FishingLevel** | **int** | Fishing level. | 
**FishingXp** | **int** | The current xp level of the Fishing skill. | 
**FishingMaxXp** | **int** | Fishing XP required to level up the skill. | 
**WeaponcraftingLevel** | **int** | Weaponcrafting level. | 
**WeaponcraftingXp** | **int** | The current xp level of the Weaponcrafting skill. | 
**WeaponcraftingMaxXp** | **int** | Weaponcrafting XP required to level up the skill. | 
**GearcraftingLevel** | **int** | Gearcrafting level. | 
**GearcraftingXp** | **int** | The current xp level of the Gearcrafting skill. | 
**GearcraftingMaxXp** | **int** | Gearcrafting XP required to level up the skill. | 
**JewelrycraftingLevel** | **int** | Jewelrycrafting level. | 
**JewelrycraftingXp** | **int** | The current xp level of the Jewelrycrafting skill. | 
**JewelrycraftingMaxXp** | **int** | Jewelrycrafting XP required to level up the skill. | 
**CookingLevel** | **int** | The current xp level of the Cooking skill. | 
**CookingXp** | **int** | Cooking XP. | 
**CookingMaxXp** | **int** | Cooking XP required to level up the skill. | 
**AlchemyLevel** | **int** | Alchemy level. | 
**AlchemyXp** | **int** | Alchemy XP. | 
**AlchemyMaxXp** | **int** | Alchemy XP required to level up the skill. | 
**Hp** | **int** | Character actual HP. | 
**MaxHp** | **int** | Character max HP. | 
**Haste** | **int** | *Increase speed attack (reduce fight cooldown) | 
**CriticalStrike** | **int** | % Critical strike. Critical strikes adds 50% extra damage to an attack (1.5x). | 
**Wisdom** | **int** | Wisdom increases the amount of XP gained from fights and skills (1% extra per 10 wisdom). | 
**Prospecting** | **int** | Prospecting increases the chances of getting drops from fights and skills (1% extra per 10 PP). | 
**Initiative** | **int** | Initiative determines turn order in combat. Higher initiative goes first. | 
**Threat** | **int** | Threat level affects monster targeting in multi-character combat. | 
**AttackFire** | **int** | Fire attack. | 
**AttackEarth** | **int** | Earth attack. | 
**AttackWater** | **int** | Water attack. | 
**AttackAir** | **int** | Air attack. | 
**Dmg** | **int** | % Damage. Damage increases your attack in all elements. | 
**DmgFire** | **int** | % Fire damage. Damage increases your fire attack. | 
**DmgEarth** | **int** | % Earth damage. Damage increases your earth attack. | 
**DmgWater** | **int** | % Water damage. Damage increases your water attack. | 
**DmgAir** | **int** | % Air damage. Damage increases your air attack. | 
**ResFire** | **int** | % Fire resistance. Reduces fire attack. | 
**ResEarth** | **int** | % Earth resistance. Reduces earth attack. | 
**ResWater** | **int** | % Water resistance. Reduces water attack. | 
**ResAir** | **int** | % Air resistance. Reduces air attack. | 
**Effects** | [**List&lt;StorageEffectSchema&gt;**](StorageEffectSchema.md) | List of active effects on the character. | [optional] 
**X** | **int** | Character x coordinate. | 
**Y** | **int** | Character y coordinate. | 
**Layer** | **MapLayer** | Character current layer. | 
**MapId** | **int** | Character current map ID. | 
**Cooldown** | **int** | Cooldown in seconds. | 
**CooldownExpiration** | **DateTimeOffset** | Datetime Cooldown expiration. | [optional] 
**WeaponSlot** | **string** | Weapon slot. | 
**RuneSlot** | **string** | Rune slot. | 
**ShieldSlot** | **string** | Shield slot. | 
**HelmetSlot** | **string** | Helmet slot. | 
**BodyArmorSlot** | **string** | Body armor slot. | 
**LegArmorSlot** | **string** | Leg armor slot. | 
**BootsSlot** | **string** | Boots slot. | 
**Ring1Slot** | **string** | Ring 1 slot. | 
**Ring2Slot** | **string** | Ring 2 slot. | 
**AmuletSlot** | **string** | Amulet slot. | 
**Artifact1Slot** | **string** | Artifact 1 slot. | 
**Artifact2Slot** | **string** | Artifact 2 slot. | 
**Artifact3Slot** | **string** | Artifact 3 slot. | 
**Utility1Slot** | **string** | Utility 1 slot. | 
**Utility1SlotQuantity** | **int** | Utility 1 quantity. | 
**Utility2Slot** | **string** | Utility 2 slot. | 
**Utility2SlotQuantity** | **int** | Utility 2 quantity. | 
**BagSlot** | **string** | Bag slot. | 
**Task** | **string** | Task in progress. | 
**TaskType** | **string** | Task type. | 
**TaskProgress** | **int** | Task progression. | 
**TaskTotal** | **int** | Task total objective. | 
**InventoryMaxItems** | **int** | Inventory max items. | 
**Inventory** | [**List&lt;InventorySlot&gt;**](InventorySlot.md) | List of inventory slots. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

