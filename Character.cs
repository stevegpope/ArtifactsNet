using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;

namespace Artifacts
{
    internal class Character
    {
        internal MyCharactersApi _api;
        private static readonly Random _random = Random.Shared;
        internal string Name { get; }

        internal Character(
            Configuration config,
            HttpClient httpClient,
            string name
            )
        {
            _api = new MyCharactersApi(httpClient, config);

            Name = name;
        }

        internal async Task Init()
        {
            var response = await _api.GetMyCharactersMyCharactersGetAsync();
            var character = response.Data.First(c => c.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));
            Utils.Details = character;
        }

        internal async Task<List<CharacterSchema>> GetAllCharacters()
        {
            var response = await _api.GetMyCharactersMyCharactersGetAsync();
            return response.Data;
        }

        internal async Task Move(int x, int y)
        {
            Console.WriteLine($"Move to {x},{y}");
            await Utils.ApiCall(async () =>
            {
                try
                {
                    var destinationSchema = new DestinationSchema()
                    {
                        X = x,
                        Y = y
                    };

                    // Hack for now
                    if (x == 0 || y == 0)
                    {
                        var location = await Map.Instance.GetMapPosition(x, y);
                        destinationSchema.MapId = location.MapId;
                    }

                    var response = await _api.ActionMoveMyNameActionMovePostAsync(Name, destinationSchema);
                    return response;
                }
                catch (ApiException ex)
                {
                    if (ex.ErrorCode == 490)
                    {
                        Console.WriteLine($"Character {Name} is already at location");
                        return null;
                    }

                    throw;
                }
            });
        }

        internal async Task MoveTo(MapContentType locationType, string code = null)
        {
            Console.WriteLine($"Moving {Name} to {locationType}, code {code}");
            var response = await Map.Instance.GetMapLayer(locationType, code);
            if (response.Data != null && response.Data.Any())
            {
                while(true)
                {
                    MapSchema location = Utils.GetClosest(response.Data);
                    try
                    {
                        await Move(location.X, location.Y);
                        break;
                    }
                    catch (ApiException ex)
                    {
                        Console.WriteLine($"Can't go to {location.X}, {location.Y}: {ex.ErrorContent}");
                        response.Data.Remove(location);
                    }
                }
            }
            else
            {
                throw new Exception($"No locations found for type {locationType}");
            }
        }

        internal async Task TurnInItems(string code, int quantity)
        {
            Console.WriteLine($"Turning in {quantity} {code} for character {Name}");
            await Utils.ApiCall(async () =>
                {
                    var item = new SimpleItemSchema(code, quantity);
                    return await _api.ActionTaskTradeMyNameActionTaskTradePostAsync(Name, item);
                });
        }

        internal async Task<int> CraftItems(ItemSchema item, int remaining)
        {
            Console.WriteLine($"Crafting {remaining} {item.Code} for character {Name}");
            Console.WriteLine($"{item.Code}: {remaining}");
            Console.WriteLine($"{item.Code}: {remaining}");
            Console.WriteLine($"{item.Code}: {remaining}");
            item.PrintCraftComponents();

            var minSkillLevel = item.Craft.Level;
            var skill = item.Craft.Skill;
            var totalAmount = remaining;

            // We'll make a few at a time
            var perItemInventorySpace = item.Craft.Items.Sum(x => x.Quantity);
            var batchSize = CalculateCraftingBatchSize(perItemInventorySpace);
            var craftQuantity = Math.Min(batchSize, remaining);
            var itemsCrafted = 0;
            while (craftQuantity > 0)
            {
                var ownedItems = new Dictionary<string, int>();
                var gatherQuantities = new Dictionary<string, int>();
                foreach (var component in item.Craft.Items)
                {
                    var ownedItem = Utils.Details.Inventory.FirstOrDefault(i => i.Code.Equals(component.Code, StringComparison.OrdinalIgnoreCase));
                    var ownedQuantity = ownedItem != null ? ownedItem.Quantity : 0;
                    ownedItems[component.Code] = ownedQuantity;
                    gatherQuantities[component.Code] = 0;
                }

                Console.WriteLine($"Craft batch of {craftQuantity} {item.Code}");
                for (var index = 0; index < craftQuantity; index++)
                {
                    foreach (var component in item.Craft.Items)
                    {
                        var ownedQuantity = ownedItems[component.Code];
                        if (ownedQuantity >= component.Quantity)
                        {
                            // We have enough of this component to craft this one
                            ownedItems[component.Code] -= component.Quantity;
                        }
                        else
                        {
                            // Need to gather more
                            gatherQuantities[component.Code] += component.Quantity - ownedQuantity;
                            ownedItems[component.Code] = 0;
                        }
                    }
                }

                // Get the items required for crafting
                foreach (var component in item.Craft.Items)
                {
                    var gatherQuantity = gatherQuantities[component.Code];
                    if (gatherQuantity > 0)
                    {
                        var gathered = await GatherItems(component.Code, gatherQuantity);
                        if (gathered == 0)
                        {
                            Console.WriteLine($"We cannot gather {component.Code}!");
                            return 0;
                        }
                    }
                }

                // Go to the crafting location
                await MoveClosest(MapContentType.Workshop, item.Craft.Skill.Value.ToString());

                // Craft the items one at a time until we can't make any more. We may be using inventory items from before, or we may run out of gathered items.
                for (int index = 0; index < craftQuantity; index++)
                {
                    try
                    {
                        var craftResponse = await Utils.ApiCall(async () =>
                        {
                            var leftToGet = totalAmount - itemsCrafted;
                            var estimatedTime = new TimeSpan(hours: 0, minutes: 0, seconds: leftToGet * 5);
                            Console.WriteLine($"Crafting {itemsCrafted + 1}/{totalAmount} {item.Code} for character {Name} ETA: {estimatedTime}");
                            var response = await _api.ActionCraftingMyNameActionCraftingPostAsync(Name, new CraftingSchema(item.Code, 1));
                            Console.WriteLine($"{Name} Crafted {item.Code}. Xp: {response.Data.Details.Xp}");
                            return response;
                        });
                    }
                    catch (ApiException ex)
                    {
                        if (ex.ErrorCode == 478)
                        {
                            Console.WriteLine($"Ran out of components creating items");
                            item.PrintCraftComponents();
                        }

                        break;
                    }
                    itemsCrafted++;
                }

                remaining -= itemsCrafted;
                craftQuantity = Math.Min(batchSize, remaining);

                if (craftQuantity > 0)
                {
                    Console.WriteLine($"{craftQuantity} more to make, go deposit first");
                    await MoveTo(MapContentType.Bank);
                    await DepositAllItems();
                }
            }

            Console.WriteLine($"Crafted {itemsCrafted} {item.Code} for character {Name}");
            return itemsCrafted;
        }

        private int CalculateCraftingBatchSize(int perItemInventorySpace)
        {
            var space = GetFreeInventorySpace();
            return space / perItemInventorySpace;
        }

        internal async Task MoveClosest(MapContentType contentType, string code)
        {
            Console.WriteLine($"Moving to closest {code} for character {Name}");
            var response = await Map.Instance.GetMapLayer(contentType, code);
            if (response.Data != null && response.Data.Any())
            {
                var locations = response.Data;

                // Find the closest location
                var location = Utils.GetClosest(locations);
                Console.WriteLine($"Closest location is at ({location.X}, {location.Y})");
                await Move(location.X, location.Y);
            }
            else
            {
                throw new Exception($"No locations found for type {contentType} and code {code}");
            }
        }

        internal async Task<int> GatherItems(string code, int remaining, bool skipBank = false, ItemSchema doNotUse = null)
        {
            Console.WriteLine($"Gather {remaining} {code}");

            if (!skipBank)
            {
                // Check the bank first
                var bankItems = await Bank.Instance.GetItems();
                var bankItemAmount = bankItems.Where(x => x.Code == code).Sum(x => x.Quantity);
                if (bankItemAmount > 0)
                {
                    Console.WriteLine($"Found {bankItemAmount} {code} in the bank, going to get them");
                    await MoveTo(MapContentType.Bank);
                    var quantity = Math.Min(remaining, bankItemAmount);
                    var withdrawn = await WithdrawItems(code, quantity);
                    Console.WriteLine($"Withdrew {withdrawn} {code} from the bank, compare to {remaining}");
                    if (withdrawn == remaining)
                    {
                        // Found all we need in the bank
                        return quantity;
                    }

                    remaining -= withdrawn;
                    Console.WriteLine($"Did not get enough from the bank, still need {remaining}");
                }
            }

            var item = await Items.Instance.GetItem(code);
            if (item.Craft != null)
            {
                Console.WriteLine($"Need to craft {code}");
                return await CraftItems(item, remaining);
            }


            var resource = await Resources.Instance.GetResourceDrop(code);
            if (resource == null)
            {
                var monsters = Monsters.Instance.GetMonsters(maxLevel: 100, dropCode: item.Code);
                if (monsters.Data.Any())
                {
                    var monster = monsters.Data.MinBy(x => x.Level);
                    Console.WriteLine($"Need to fight for {code}, chasing {monster}");
                    if (monster.Level > Utils.Details.Level)
                    {
                        Console.WriteLine("We can't beat this monster, bailing");
                        return 0;
                    }

                    return await FightDrops(monster.Code, code, remaining);
                }

                Console.WriteLine($"No way to gather, craft, or hunt for {code}, we cannot craft this\n");
                return 0;
            }

            Console.WriteLine($"Gathering {remaining} {code} for character {Name}");
            var skill = await Resources.Instance.GetResourceSkill(item);

            await GearUpSkill(skill, doNotUse);
            await MoveClosest(MapContentType.Resource, resource.Code);

            var gathered = 0;
            var leftToGet = remaining - gathered;
            while (leftToGet > 0)
            {
                var estimatedTime = new TimeSpan(hours: 0, minutes: 0, seconds: leftToGet * 27);
                Console.WriteLine($"Gather {gathered}/{remaining} {code}. ETA: {estimatedTime}");
                try
                {
                    var result = await Utils.ApiCall(async () =>
                    {
                        return await _api.ActionGatheringMyNameActionGatheringPostAsync(Name);
                    });
                    
                    var schema = result as SkillResponseSchema;
                    if (schema != null)
                    {
                        Console.WriteLine($"XP: {schema.Data.Details.Xp}");
                        foreach (var drop in schema.Data.Details.Items)
                        {
                            Console.WriteLine($"drop: {drop.Quantity} {drop.Code}");
                            if (drop.Code == code)
                            {
                                leftToGet -= drop.Quantity;
                                gathered += drop.Quantity;
                            }
                        }
                    }
                }
                catch (ApiException ex)
                {
                    if (ex.ErrorCode == 497)
                    {
                        Console.WriteLine("Inventory full, be right back");
                        await MoveTo(MapContentType.Bank);
                        await DepositAllItems();
                        return await GatherItems(code, leftToGet);
                    }
                    else if (ex.ErrorCode == 493)
                    {
                        Console.WriteLine($"Not skilled enough to get {code}, going training");
                        await TrainSkill(skill, item.Level);

                        Console.WriteLine($"Back from training, gathering {leftToGet} {code}");
                        await MoveTo(MapContentType.Bank);
                        await DepositAllItems();
                        return await GatherItems(code, leftToGet);
                    }

                    throw;
                }
            }

            return gathered;
        }

        private async Task TrainSkill(string skill, int levelGoal, ItemSchema doNotUse = null)
        {
            var currentLevel = Utils.GetSkillLevel(skill);
            Console.WriteLine($"Train {skill} from {currentLevel} to {levelGoal}");

            while (currentLevel < levelGoal)
            {
                await TrainGathering(currentLevel, skill, doNotUse);
                currentLevel = Utils.GetSkillLevel(skill);
                Console.WriteLine($"Training at {currentLevel}/{levelGoal}");
            }
        }

        internal async Task TrainGathering(int level, string skill, ItemSchema doNotUse = null)
        {
            var items = await Items.Instance.GetAllItems();
            var gatherItems = items.Where(i => i.Value.Type == "resource" && i.Value.Subtype == skill);

            int total = 0;
            for (int targetLevel = level; targetLevel >= 1; targetLevel--)
            {
                var itemsAtLevel = gatherItems.Where(x => x.Value.Level == targetLevel).Select(x => x.Value);
                Console.WriteLine($"{itemsAtLevel.Count()} items at level {targetLevel}");
                var itemsList = new List<ItemSchema>(itemsAtLevel);
                while (itemsList.Any())
                {
                    // Pick the one with the highest drop rate
                    ItemSchema selectedItem = null;
                    string item = null;
                    var highestRate = 1000;
                    foreach (var listItem in itemsList)
                    {
                        var drops = await Resources.Instance.GetResourceDrop(listItem.Code);
                        var drop = drops.Drops.FirstOrDefault(x => x.Code == listItem.Code);
                        if (drop.Rate < highestRate)
                        {
                            Console.WriteLine($"New highest rate drop {drop.Code} rate: {drop.Rate} (lower is better)");
                            highestRate = drop.Rate;
                            item = drop.Code;
                            selectedItem = listItem;
                        }
                    }

                    total = await GatherItems(item, 50, skipBank: true, doNotUse);
                    if (total == 0)
                    {
                        itemsList.Remove(selectedItem);
                    }
                    break;
                }

                if (total > 0)
                {
                    break;
                }
            }

            // Go deposit the results in the bank
            await MoveTo(MapContentType.Bank);
            await DepositAllItems();
        }

        private async Task<int> FightDrops(string monster, string code, int remaining)
        {
            await GearUp(monster);

            var lostLastFight = 0;
            var losses = 0;

            var drops = 0;
            while (drops < remaining)
            {
                // Assume we are not at the monster
                await MoveTo(MapContentType.Monster, code: monster);

                // If we are healthy enough fight right away
                var needsRest = false;
                if (lostLastFight == 0 && Utils.Details.Hp < Utils.Details.MaxXp * .75)
                {
                    needsRest = true;
                }
                else if (Utils.Details.Hp < lostLastFight)
                {
                    needsRest |= true;
                }

                if (needsRest)
                {
                    await Rest();
                }

                try
                {
                    var hp = Utils.Details.Hp;
                    var result = await Fight();
                    lostLastFight = hp - Utils.Details.Hp;
                    if (result.Data.Fight.Result == FightResult.Loss)
                    {
                        const int Limit = 3;
                        losses++;
                        Console.WriteLine($"loss {losses} of {Limit}");
                        if (losses >= Limit)
                        {
                            Console.WriteLine($"We Lost! Giving up on getting drops for monster {monster}");
                            return 0;
                        }
                    }
                    else
                    {
                        // Reset losses, we can beat him!
                        losses = 0;
                    }

                    foreach (var character in result.Data.Fight.Characters)
                    {
                        if (character.Drops != null && character.Drops.Any())
                        {
                            drops += character.Drops.Where(x => x.Code == code).Sum(x => x.Quantity);
                        }
                    }

                    Console.WriteLine($"Got {drops} of {remaining} {code} from the {monster}");
                }
                catch (ApiException ex)
                {
                    if (ex.ErrorCode == 497)
                    {
                        Console.WriteLine("Inventory full, cannot fight");
                        break;
                    }

                    Console.WriteLine($"Fight error: {ex.ErrorContent}");
                    throw;
                }
            }

            return drops;
        }

        internal async Task DepositAllItems()
        {
            Console.WriteLine($"Depositing all items for character {Name}");
            await DepositGold();

            foreach (var item in Utils.Details.Inventory)
            {
                if (string.IsNullOrEmpty(item.Code))
                {
                    continue;
                }

                await Utils.ApiCall(async () =>
                {
                    var items = new List<SimpleItemSchema>
                    {
                        new SimpleItemSchema(item.Code, item.Quantity)
                    };

                    try
                    {
                        Console.WriteLine($"Deposit {item.Code} {item.Quantity}");
                        return await _api.ActionDepositBankItemMyNameActionBankDepositItemPostAsync(Name, items);
                    }
                    catch (ApiException ex)
                    {
                        if (ex.ErrorCode == 478)
                        {
                            // We don't have enough of this item??
                            return null;
                        }
                        else if (ex.ErrorCode == 461)
                        {
                            // Already in transaction?
                            Console.WriteLine($"Item {item.Quantity} {item.Code} already in transaction");
                            return null;
                        }

                        throw;
                    }
                });
            }

        }

        internal async Task DepositExcept(List<string> excludeCodes)
        {
            Console.WriteLine($"Depositing all items except {string.Join(", ", excludeCodes)} for character {Name}");
            await DepositGold();

            var items = new List<SimpleItemSchema>();
            foreach (var item in Utils.Details.Inventory)
            {
                if (!string.IsNullOrEmpty(item.Code) && !excludeCodes.Contains(item.Code, StringComparer.OrdinalIgnoreCase))
                {
                    items.Add(new SimpleItemSchema(item.Code, item.Quantity));
                }
            }

            if (items.Count == 0)
            {
                Console.WriteLine($"No items to deposit for character {Name}");
                return;
            }

            await Utils.ApiCall(async () =>
            {
                var response = await _api.ActionDepositBankItemMyNameActionBankDepositItemPostAsync(Name, items);
                return response;
            });
        }

        internal async Task DepositGold()
        {
            if (Utils.Details.Gold <= 0)
            {
                return;
            }

            await Utils.ApiCall(async () =>
            {
                Console.WriteLine($"Depositing {Utils.Details.Gold} gold for character {Name}");
                var response = await _api.ActionDepositBankGoldMyNameActionBankDepositGoldPostAsync(Name, new DepositWithdrawGoldSchema(quantity: Utils.Details.Gold));
                return response;
            });
        }

        internal async Task<int> WithdrawItems(string code, int quantity = 0)
        {
            var bankItems = await Bank.Instance.GetItems();
            var inventorySpace = GetFreeInventorySpace();

            var bankItem = bankItems.FirstOrDefault(b => b.Code == code);
            if (bankItem == null)
            {
                return 0;
            }

            var withdrawQuantity = quantity;
            if (withdrawQuantity == 0)
            {
                withdrawQuantity = Math.Min(inventorySpace, bankItem.Quantity);
            }
            else
            {
                withdrawQuantity = Math.Min(quantity, bankItem.Quantity);
                withdrawQuantity = Math.Min(withdrawQuantity, inventorySpace);
            }

            Console.WriteLine($"Try to withdraw {withdrawQuantity} {code}, inventory space {inventorySpace}");
            if (withdrawQuantity <= 0)
            {
                throw new Exception($"Invalid quantity {withdrawQuantity}");
            }

            try
            {
                await Utils.ApiCall(async () =>
                {
                    var items = new List<SimpleItemSchema> { new SimpleItemSchema(code, withdrawQuantity) };
                    return await _api.ActionWithdrawBankItemMyNameActionBankWithdrawItemPostAsync(Name, items);
                });
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"Could not withdraw: {ex.ErrorContent}");
                return 0;
            }

            Console.WriteLine($"Withdrew {withdrawQuantity} {code}");
            return withdrawQuantity;
        }
        internal int GetFreeInventorySpace()
        {
            var amount = Utils.Details.InventoryMaxItems;
            foreach(var item in Utils.Details.Inventory)
            {
                amount -= item.Quantity;
            }

            return amount;
        }

        internal async Task FightLoop(int total, string monster)
        {
            var monstersKilled = 0;
            var lostLastFight = 0;

            await GearUp(monster);

            while(monstersKilled < total)
            {
                // Assume we are not at the monster
                await MoveTo(MapContentType.Monster, code: monster);

                // If we are healthy enough fight right away
                var needsRest = false;
                if (lostLastFight == 0 && Utils.Details.Hp < Utils.Details.MaxXp * .75)
                {
                    needsRest = true;
                }
                else if (Utils.Details.Hp < lostLastFight)
                {
                    needsRest |= true;
                }

                if (needsRest)
                {
                    await Rest();
                }

                try
                {
                    var hp = Utils.Details.Hp;
                    var result = await Fight();
                    lostLastFight = hp - Utils.Details.Hp;
                }
                catch (ApiException ex)
                {
                    Console.WriteLine($"Fight error: {ex.ErrorContent}");
                    if (ex.ErrorCode == 497)
                    {
                        Console.WriteLine("Inventory full, be right back");
                        await MoveTo(MapContentType.Bank);
                        await DepositAllItems();
                    }
                }
            }
        }

        private async Task Rest()
        {
            if (await EatSomething())
            {
                Console.WriteLine("No time for rest, we ate!");
                return;
            }

            Console.WriteLine("Rest");
            await Utils.ApiCall(async () =>
            {
                return await _api.ActionRestMyNameActionRestPostAsync(Name);
            });

            Console.WriteLine("Finished Resting, back to it");
        }

        private async Task<bool> EatSomething()
        {
            if (Utils.Details.MaxHp == Utils.Details.Hp)
            {
                // Nothing to do
                return true;
            }

            var amountToHeal = Utils.Details.MaxHp - Utils.Details.Hp;
            Console.WriteLine($"Check for food, we need {amountToHeal} hp");

            foreach (var inventoryItem in Utils.Details.Inventory)
            {
                if (string.IsNullOrEmpty(inventoryItem.Code) || inventoryItem.Quantity == 0)
                {
                    continue;
                }

                var item = await Items.Instance.GetItem(inventoryItem.Code);
                if (item.Type == "consumable")
                {
                    var heal = item.Effects.Where(x => x.Code == "heal").Sum(x => x.Value);
                    if (heal == 0)
                    {
                        Console.WriteLine($"WARNING!!! {item.Code} is not healing!");
                        continue;
                    }

                    var idealQuantity = 0;
                    var currentAmount = 0;
                    while (currentAmount < amountToHeal)
                    {
                        idealQuantity++;
                        currentAmount += heal;
                    }

                    var quantity = Math.Min(idealQuantity, inventoryItem.Quantity);

                    Console.WriteLine($"Eat {quantity} {item.Code} to heal {currentAmount} of {amountToHeal}");
                    await Consume(item.Code, quantity);

                    if (currentAmount >= amountToHeal)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        internal async Task<int> Consume(string code, int quantity)
        {
            try
            {
                Console.WriteLine($"{Name} consuming {quantity} {code}");
                await Utils.ApiCall(async () =>
                {
                    return await _api.ActionUseItemMyNameActionUsePostAsync(Name, new SimpleItemSchema(code, quantity));
                });

                return quantity;
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"Cannot eat {code}: {ex.ErrorContent}");
            }

            return 0;
        }

        private async Task<bool> ConsumeGold(ItemSchema item, int quantity)
        {
            if (item.Type == "consumable" && item.Subtype == "bag")
            {
                return await Consume(item.Code, quantity) > 0;
            }

            return false;
        }

        private async Task<CharacterFightResponseSchema> Fight()
        {
            Console.WriteLine("Fight!!");

            var result = await Utils.ApiCall(async () =>
            {
                var response = await _api.ActionFightMyNameActionFightPostAsync(Name);
                Console.WriteLine($"Fight {response.Data.Fight.Result}!!!");
                foreach (var characterResponse in response.Data.Fight.Characters)
                {
                    Console.WriteLine($"XP: {characterResponse.Xp}");
                    if (characterResponse.Drops != null)
                    {
                        foreach (var drop in characterResponse.Drops)
                        {
                            Console.WriteLine($"drop {drop.Quantity} {drop.Code}");
                        }
                    }
                }

                return response;
            });

            return (CharacterFightResponseSchema)result;
        }

        private async Task GearUpSkill(string skill, ItemSchema doNotUse)
        {
            if (skill == null)
            {
                return;
            }

            var bankItems = await Bank.Instance.GetItems();
            await EquipBestForSkill(Utils.Details.WeaponSlot, ItemSlot.Weapon, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details.ShieldSlot, ItemSlot.Shield, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details.HelmetSlot, ItemSlot.Helmet, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details.BodyArmorSlot, ItemSlot.BodyArmor, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details.LegArmorSlot, ItemSlot.LegArmor, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details.BootsSlot, ItemSlot.Boots, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details.Ring1Slot, ItemSlot.Ring1, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details.AmuletSlot, ItemSlot.Amulet, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details.Artifact1Slot, ItemSlot.Artifact1, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details.Artifact2Slot, ItemSlot.Artifact2, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details.Artifact3Slot, ItemSlot.Artifact3, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details.Utility1Slot, ItemSlot.Utility1, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details.Utility2Slot, ItemSlot.Utility2, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details.BagSlot, ItemSlot.Bag, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details.RuneSlot, ItemSlot.Rune, skill, bankItems, doNotUse);
        }

        private async Task EquipBestForSkill(string currentEquipped, ItemSlot slotType, string skill, List<SimpleItemSchema> bankItems, ItemSchema doNotUse)
        {
            ItemSchema currentItem = null;
            var currentValue = 0.0;
            if (!string.IsNullOrEmpty(currentEquipped))
            {
                currentItem = await Items.Instance.GetItem(currentEquipped);
                currentValue = Items.Instance.CalculateItemValueSkill(currentItem, skill);
            }

            var bestInventoryItem = await GetBestItemFromInventorySkill(slotType, skill, doNotUse);
            var inventoryValue = bestInventoryItem != null ? Items.Instance.CalculateItemValueSkill(bestInventoryItem, skill) : 0;

            var bestBankItem = await GetBestItemFromBankSkill(slotType, skill, bankItems, doNotUse);
            var bankValue = bestBankItem != null ? Items.Instance.CalculateItemValueSkill(bestBankItem, skill) : 0;

            var max = Math.Max(currentValue, Math.Max(bankValue, inventoryValue));
            if (max == currentValue)
            {
                // Nothing to change
                return;
            }
            
            if (max == inventoryValue)
            {
                Console.WriteLine($"Inventory item {bestInventoryItem.Code} is highest value at {inventoryValue}");
                if (currentItem != null)
                    await Unequip(slotType);

                await Equip(bestInventoryItem.Code, slotType);
            }
            else
            {
                Console.WriteLine($"Bank item {bestBankItem.Code} is highest value at {bankValue}");
                // Best item is in the bank
                await MoveTo(MapContentType.Bank);
                if (await WithdrawItems(bestBankItem.Code, 1) > 0)
                {
                    if (currentItem != null)
                        await Unequip(slotType);

                    await Equip(bestBankItem.Code, slotType);
                }
            }
        }

        private async Task<ItemSchema> GetBestItemFromBankSkill(ItemSlot slotType, string skill, List<SimpleItemSchema> bankItems, ItemSchema doNotUse)
        {
            ItemSchema bestItem = null;
            foreach (var bankItem in bankItems)
            {
                if (doNotUse != null && doNotUse.Code == bankItem.Code)
                {
                    Console.WriteLine($"Intentionally not choosing {doNotUse.Code} from bank");
                    continue;
                }

                var item = await Items.Instance.GetItem(bankItem.Code);
                if (Items.Instance.ItemTypeMatchesSlot(item.Type, slotType))
                {
                    if (bestItem == null)
                    {
                        bestItem = item;
                    }
                    else if (Items.Instance.IsBetterItemSkill(bestItem, item, skill))
                    {
                        bestItem = item;
                    }
                }
            }

            return bestItem;
        }

        private async Task<ItemSchema> GetBestItemFromInventorySkill(ItemSlot slotType, string skill, ItemSchema doNotUse)
        {
            ItemSchema bestItem = null;
            foreach (var inventoryItem in Utils.Details.Inventory)
            {
                if (string.IsNullOrEmpty(inventoryItem.Code))
                {
                    continue;
                }

                if (doNotUse != null && inventoryItem.Code == doNotUse.Code)
                {
                    Console.WriteLine($"Intentionally not choosing {doNotUse.Code} from inventory");
                    continue;
                }

                var item = await Items.Instance.GetItem(inventoryItem.Code);
                if (Items.Instance.ItemTypeMatchesSlot(item.Type, slotType))
                {
                    if (bestItem == null)
                    {
                        bestItem = item;
                    }
                    else if (Items.Instance.IsBetterItemSkill(bestItem, item, skill))
                    {
                        bestItem = item;
                    }
                }
            }

            return bestItem;
        }

        private async Task GearUp(string monsterCode)
        {
            if (monsterCode == null)
            {
                return;
            }

            var monster = await Monsters.Instance.GetMonster(monsterCode);
            var bankItems = await Bank.Instance.GetItems();
            await EquipBestForMonster(Utils.Details.WeaponSlot, ItemSlot.Weapon, monster, bankItems);
            await EquipBestForMonster(Utils.Details.ShieldSlot, ItemSlot.Shield, monster, bankItems);
            await EquipBestForMonster(Utils.Details.HelmetSlot, ItemSlot.Helmet, monster, bankItems);
            await EquipBestForMonster(Utils.Details.BodyArmorSlot, ItemSlot.BodyArmor, monster, bankItems);
            await EquipBestForMonster(Utils.Details.LegArmorSlot, ItemSlot.LegArmor, monster, bankItems);
            await EquipBestForMonster(Utils.Details.BootsSlot, ItemSlot.Boots, monster, bankItems);
            await EquipBestForMonster(Utils.Details.Ring1Slot, ItemSlot.Ring1, monster, bankItems);
            await EquipBestForMonster(Utils.Details.AmuletSlot, ItemSlot.Amulet, monster, bankItems);
            await EquipBestForMonster(Utils.Details.Artifact1Slot, ItemSlot.Artifact1, monster, bankItems);
            await EquipBestForMonster(Utils.Details.Artifact2Slot, ItemSlot.Artifact2, monster, bankItems);
            await EquipBestForMonster(Utils.Details.Artifact3Slot, ItemSlot.Artifact3, monster, bankItems);
            await EquipBestForMonster(Utils.Details.Utility1Slot, ItemSlot.Utility1, monster, bankItems);
            await EquipBestForMonster(Utils.Details.Utility2Slot, ItemSlot.Utility2, monster, bankItems);
            await EquipBestForMonster(Utils.Details.BagSlot, ItemSlot.Bag, monster, bankItems);
            await EquipBestForMonster(Utils.Details.RuneSlot, ItemSlot.Rune, monster, bankItems);

            await GetFood(bankItems);
        }

        private async Task GetFood(List<SimpleItemSchema> bankItems)
        {
            // Save a little room, just in case of item swaps
            var space = GetFreeInventorySpace() - 10;

            var found = false;
            foreach (var bankItem in bankItems)
            {
                if (bankItem.Quantity > 0)
                {
                    var item = await Items.Instance.GetItem(bankItem.Code);
                    if (item.Type == "consumable" && item.Level <= Utils.Details.Level)
                    {
                        var amount = Math.Min(space, bankItem.Quantity);

                        // Don't be greedy
                        amount = Math.Min(amount, 20);

                        await MoveTo(MapContentType.Bank);

                        var withdrawn = await WithdrawItems(item.Code, amount);
                        if (withdrawn > 0)
                        {
                            found = true;
                        }

                        space -= withdrawn;
                        if (space <= 0)
                        {
                            return;
                        }
                    }
                }
            }

            if (!found)
            {
                Console.WriteLine("No food left, now we are the chef");
                await CookBankFood(bankItems);
            }
        }

        private async Task CookBankFood(List<SimpleItemSchema> bankItems)
        {
            // See if we can cook something in the bank
            var items = await Items.Instance.GetAllItems();
            var cookingLevel = Utils.GetSkillLevel("cooking");
            var recipes = items.Values.Where(i => i.Craft != null &&
                i.Craft.Skill == CraftSkill.Cooking &&
                i.Craft.Level <= cookingLevel);

            foreach (var recipe in recipes)
            {
                foreach(var component in recipe.Craft.Items)
                {

                }
            }
        }

        private async Task EquipBestForMonster(string currentEquipped, ItemSlot slotType, MonsterSchema monster, List<SimpleItemSchema> bankItems)
        {
            ItemSchema currentItem = null;
            var currentValue = 0.0;
            if (!string.IsNullOrEmpty(currentEquipped))
            {
                currentItem = await Items.Instance.GetItem(currentEquipped);
                currentValue = Items.Instance.CalculateItemValue(currentItem, monster);
            }

            var bestInventoryItem = await GetBestItemFromInventory(slotType, monster);
            var inventoryValue = bestInventoryItem != null ? Items.Instance.CalculateItemValue(bestInventoryItem, monster) : 0;

            var bestBankItem = await GetBestItemFromBank(slotType, monster, bankItems);
            var bankValue = bestBankItem != null ? Items.Instance.CalculateItemValue(bestBankItem, monster) : 0;

            var max = Math.Max(currentValue, Math.Max(bankValue, inventoryValue));
            if (max == currentValue)
            {
                // Nothing to change
                return;
            }

            if (max == inventoryValue)
            {
                Console.WriteLine($"Inventory item {bestInventoryItem.Code} is highest value at {inventoryValue}");
                if (currentItem != null)
                {
                    await Unequip(slotType);
                }

                await Equip(bestInventoryItem.Code, slotType);
            }
            else
            {
                Console.WriteLine($"Bank item {bestBankItem.Code} is highest value at {bankValue}");

                var quantity = 1;

                // Special case for utility slots, take 10 max
                if (slotType == ItemSlot.Utility1 || slotType == ItemSlot.Utility2)
                {
                    var bankItem = bankItems.First(x => x.Code == bestBankItem.Code);
                    quantity = Math.Min(10, bankItem.Quantity);
                }

                await MoveTo(MapContentType.Bank);
                var withdrawn = await WithdrawItems(bestBankItem.Code, quantity);
                if (withdrawn > 0)
                {
                    if (currentItem != null)
                        await Unequip(slotType);

                    await Equip(bestBankItem.Code, slotType, withdrawn);
                }
            }
        }

        private async Task<ItemSchema> GetBestItemFromBank(ItemSlot slotType, MonsterSchema monster, List<SimpleItemSchema> bankItems)
        {
            ItemSchema bestItem = null;
            foreach (var bankItem in bankItems)
            {
                if (slotType == ItemSlot.Utility2 && bankItem.Code == Utils.Details.Utility1Slot)
                {
                    // Special case: we cannot have the same item in both utility slots
                    continue;
                }
                if (slotType == ItemSlot.Utility2 && bankItem.Code == Utils.Details.Utility1Slot)
                {
                    // Special case: we cannot have the same item in both utility slots
                    continue;
                }


                var item = await Items.Instance.GetItem(bankItem.Code);
                if (Items.Instance.ItemTypeMatchesSlot(item.Type, slotType))
                {
                    if (bestItem == null)
                    {
                        bestItem = item;
                    }
                    else if (Items.Instance.IsBetterItem(bestItem, item, monster))
                    {
                        bestItem = item;
                    }
                }
            }

            return bestItem;
        }

        private static async Task<ItemSchema> GetBestItemFromInventory(ItemSlot slotType, MonsterSchema monster)
        {
            ItemSchema bestItem = null;
            foreach (var inventoryItem in Utils.Details.Inventory)
            {
                if (string.IsNullOrEmpty(inventoryItem.Code))
                {
                    continue;
                }

                if (slotType == ItemSlot.Utility2 && inventoryItem.Code == Utils.Details.Utility1Slot)
                {
                    // Special case: we cannot have the same item in both utility slots
                    continue;
                }
                if (slotType == ItemSlot.Utility1 && inventoryItem.Code == Utils.Details.Utility2Slot)
                {
                    // Special case: we cannot have the same item in both utility slots
                    continue;
                }

                var item = await Items.Instance.GetItem(inventoryItem.Code);
                if (item.Level > Utils.Details.Level)
                {
                    // Too high level for us
                    continue;
                }

                if (Items.Instance.ItemTypeMatchesSlot(item.Type, slotType))
                {
                    if (bestItem == null)
                    {
                        bestItem = item;
                    }
                    else if (Items.Instance.IsBetterItem(bestItem, item, monster))
                    {
                        bestItem = item;
                    }
                }
            }
            return bestItem;
        }

        private async Task<EquipmentResponseSchema> Unequip(ItemSlot slotType)
        {
            return await Utils.ApiCall(async () =>
            {
                try
                {
                    var quantity = 1;
                    if (slotType == ItemSlot.Utility1)
                    {
                        quantity = Utils.Details.Utility1SlotQuantity;
                    }
                    else if (slotType == ItemSlot.Utility2)
                    {
                        quantity = Utils.Details.Utility2SlotQuantity;
                    }

                    Console.WriteLine($"Unquip {quantity} from {slotType}");
                    var response = await _api.ActionUnequipItemMyNameActionUnequipPostAsync(Name, new UnequipSchema(slotType, quantity));

                    return response;
                }
                catch (ApiException ex)
                {
                    if (ex.ErrorCode == 483)
                    {
                        // Not enough hp to unequip
                        Console.WriteLine($"Not enough HP to unequip {slotType}");
                        await Rest();
                        return await Unequip(slotType);
                    }
                    else
                    {
                        Console.WriteLine($"Unequip error: {ex.ErrorContent}");
                        throw;
                    }
                }
            }) as EquipmentResponseSchema;
        }

        private async Task Equip(string code, ItemSlot slotType, int quantity = 1)
        {
            try
            {
                await Utils.ApiCall(async () =>
                {
                    Console.WriteLine($"Equip {quantity} {code} in {slotType}");
                    var response = await _api.ActionEquipItemMyNameActionEquipPostAsync(Name, new EquipSchema(code, slotType, quantity));
                    Console.WriteLine($"Equip response: {response.Data.Item.Code} equipped in slot {response.Data.Slot}");
                    return response;
                });
            }
            catch (ApiException ex)
            {
                if (ex.ErrorCode == 496)
                {
                    var item = await Items.Instance.GetItem(code);
                    foreach (var condition in item.Conditions)
                    {
                        await MeetCondition(condition, item);
                    }
                    await Equip(code, slotType, quantity);
                }

                Console.WriteLine($"Equip error: {ex.ErrorContent}");
                throw;
            }
        }

        private async Task MeetCondition(ConditionSchema condition, ItemSchema item)
        {
            string skill = null;
            switch(condition.Code)
            {
                case "fishing_level":
                    skill = "fishing";
                    break;
                case "mining_level":
                    skill = "mining";
                    break;
                case "woodcutting_level":
                    skill = "woodcutting";
                    break;
                case "cooking_level":
                    skill = "cooking";
                    break;
                case "alchemy_level":
                    skill = "alchemy";
                    break;
            }

            if (skill != null)
            {
                var level = condition.Value;
                if (condition.Operator == ConditionOperator.Gt)
                {
                    level++;
                }

                await TrainSkill(skill, level, doNotUse: item);
            }
            else
            {
                Console.WriteLine($"Unknown skill for condition {condition}");
            }
        }

        internal async Task Recycle(string code, int recycleQuantity)
        {
            Console.WriteLine($"Recycling {recycleQuantity} {code}");

            await Utils.ApiCall(async () =>
            {
                return await _api.ActionRecyclingMyNameActionRecyclingPostAsync(Name, new RecyclingSchema(code, recycleQuantity));
            });
        }
    }
}
