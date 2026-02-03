using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;
using StackExchange.Redis;
using System.Text;

namespace Artifacts
{
    internal class Character
    {
        internal MyCharactersApi _api;
        internal string Name { get; }

        internal static readonly Dictionary<string, ItemSlot> Slots = new()
        {
            { Utils.Details.WeaponSlot, ItemSlot.Weapon },
            { Utils.Details.ShieldSlot, ItemSlot.Shield },
            { Utils.Details.HelmetSlot, ItemSlot.Helmet },
            { Utils.Details.BodyArmorSlot, ItemSlot.BodyArmor },
            { Utils.Details.LegArmorSlot, ItemSlot.LegArmor },
            { Utils.Details.BootsSlot, ItemSlot.Boots },
            { Utils.Details.Ring1Slot, ItemSlot.Ring1 },
            { Utils.Details.AmuletSlot, ItemSlot.Amulet },
            { Utils.Details.Artifact1Slot, ItemSlot.Artifact1 },
            { Utils.Details.Artifact2Slot, ItemSlot.Artifact2 },
            { Utils.Details.Artifact3Slot, ItemSlot.Artifact3 },
            { Utils.Details.Utility1Slot, ItemSlot.Utility1 },
            { Utils.Details.Utility2Slot, ItemSlot.Utility2 },
            { Utils.Details.BagSlot, ItemSlot.Bag },
            { Utils.Details.RuneSlot, ItemSlot.Rune },
        };

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
            Console.WriteLine($"Turning in {quantity} of {code} for character {Name}");
            await Utils.ApiCall(async () =>
                {
                    var item = new SimpleItemSchema(code, quantity);
                    return await _api.ActionTaskTradeMyNameActionTaskTradePostAsync(Name, item);
                });
        }

        internal async Task<int> CraftItems(ItemSchema item, int remaining)
        {
            Console.WriteLine($"Crafting {remaining} of {item.Code} for character {Name}");
            item.PrintCraftComponents();
            //z z

            var minSkillLevel = item.Craft.Level;
            var skill = item.Craft.Skill;

            var ownedItems = new Dictionary<string, int>();
            var gatherQuantities = new Dictionary<string, int>();

            foreach (var component in item.Craft.Items)
            {
                var ownedItem = Utils.Details.Inventory.FirstOrDefault(i => i.Code.Equals(component.Code, StringComparison.OrdinalIgnoreCase));
                var ownedQuantity = ownedItem != null ? ownedItem.Quantity : 0;
                ownedItems[component.Code] = ownedQuantity;
                gatherQuantities[component.Code] = 0;
            }

            // We'll make a few at a time
            // TODO: smarter calculation to maximize inventory usage
            // TODO: batching for large amounts

            var craftQuantity = remaining;
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
            var itemsCrafted = 0;
            for (int index = 0; index < craftQuantity; index++)
            {
                try
                {
                    await Utils.ApiCall(async () =>
                    {
                        Console.WriteLine($"Crafting 1 {item.Code} for character {Name}");
                        var response = await _api.ActionCraftingMyNameActionCraftingPostAsync(Name, new CraftingSchema(item.Code, 1));
                        return response;
                    });
                }
                catch (ApiException ex)
                {
                    if (ex.ErrorCode == 497)
                    {
                        Console.WriteLine($"Inventory full for character {Name}, crafted {itemsCrafted} of {item.Code}");
                    }
                    else if (ex.ErrorCode == 478)
                    {
                        Console.WriteLine($"Ran out of components creating items");
                        item.PrintCraftComponents();
                    }

                    break;
                }
                itemsCrafted++;
            }

            Console.WriteLine($"Crafted {itemsCrafted} of {item.Code} for character {Name}");
            return itemsCrafted;
        }

        private async Task MoveClosest(MapContentType contentType, string code)
        {
            Console.WriteLine($"Moving to closest {contentType} with code {code} for character {Name}");
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

        internal async Task<int> GatherItems(string code, int remaining, bool skipBank = false)
        {
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

            Console.WriteLine($"Gathering {remaining} of {code} for character {Name}");

            var resourceCode = await Resources.Instance.GetResourceDrop(code);
            if (resourceCode == null)
            {
                Console.WriteLine($"Way to gather, craft, or hunt for {code}, we cannot craft this\n");
                return 0;
            }

            var skill = await Resources.Instance.GetResourceSkill(item);
            await GearUpSkill(skill);

            await MoveClosest(MapContentType.Resource, resourceCode);

            var gathered = 0;
            for (var index = 0; index < remaining; index++)
            {
                Console.WriteLine($"Gather 1 {code}");
                try
                {
                    await Utils.ApiCall(async () =>
                    {
                        var response = await _api.ActionGatheringMyNameActionGatheringPostAsync(Name);
                        gathered++;
                        return response;
                    });
                }
                catch (ApiException ex)
                {
                    if (ex.ErrorCode == 497)
                    {
                        Console.WriteLine("Inventory full, be right back");
                        await MoveTo(MapContentType.Bank);
                        await DepositAllItems();
                        return await GatherItems(code, remaining - gathered);
                    }

                    throw;
                }
            }

            return gathered;
        }

        private async Task<int> FightDrops(string monster, string code, int remaining)
        {
            await GearUp(monster);

            var lostLastFight = 0;

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
                    Console.WriteLine($"Fight results: {result.Data.Fight.ToString()}");
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
                    var items = new List<SimpleItemSchema>();
                    items.Add(new SimpleItemSchema(item.Code, item.Quantity));
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

            var bankItem = bankItems.FirstOrDefault(b => b.Code == code);
            if (bankItem == null)
            {
                return 0;
            }

            var withdrawQuantity = quantity;
            if (withdrawQuantity == 0)
            {
                withdrawQuantity = Math.Min(GetFreeInventorySpace(), bankItem.Quantity);
            }
            else
            {
                withdrawQuantity = Math.Min(quantity, bankItem.Quantity);
                withdrawQuantity = Math.Min(withdrawQuantity, GetFreeInventorySpace());
            }

            Console.WriteLine($"Try to withdraw {withdrawQuantity} of {code}");

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
        private int GetFreeInventorySpace()
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
                    Console.WriteLine($"Fight results: {result.Data.Fight.ToString()}");
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
            Console.WriteLine("Rest");

            if (await EatSomething())
            {
                Console.WriteLine("No time for rest, we ate!");
                return;
            }

            await Utils.ApiCall(async () =>
            {
                return await _api.ActionRestMyNameActionRestPostAsync(Name);
            });
        }

        private async Task<bool> EatSomething()
        {
            foreach (var inventoryItem in Utils.Details.Inventory)
            {
                if (string.IsNullOrEmpty(inventoryItem.Code) || inventoryItem.Quantity == 0)
                {
                    continue;
                }

                var item = await Items.Instance.GetItem(inventoryItem.Code);
                if (item.Type == "consumable")
                {
                    while (Utils.Details.Hp < Utils.Details.MaxXp)
                    {
                        try
                        {
                            await Utils.ApiCall(async () =>
                            {
                                return await _api.ActionUseItemMyNameActionUsePostAsync(Name, new SimpleItemSchema(item.Code, 1));
                            });
                        }
                        catch(ApiException ex)
                        {
                            Console.WriteLine($"Ran out of food: {ex.ErrorContent}");
                            break;
                        }
                    }
                }
            }

            return Utils.Details.Hp >= Utils.Details.MaxXp;
        }

        private async Task<CharacterFightResponseSchema> Fight()
        {
            Console.WriteLine("Fight!!");

            var result = await Utils.ApiCall(async () =>
            {
                return await _api.ActionFightMyNameActionFightPostAsync(Name);
            });

            return (CharacterFightResponseSchema)result;
        }

        private async Task GearUpSkill(string skill)
        {
            if (skill == null)
            {
                return;
            }

            var bankItems = await Bank.Instance.GetItems();
            await EquipBestForSkill(Utils.Details.WeaponSlot, ItemSlot.Weapon, skill, bankItems);
            await EquipBestForSkill(Utils.Details.ShieldSlot, ItemSlot.Shield, skill, bankItems);
            await EquipBestForSkill(Utils.Details.HelmetSlot, ItemSlot.Helmet, skill, bankItems);
            await EquipBestForSkill(Utils.Details.BodyArmorSlot, ItemSlot.BodyArmor, skill, bankItems);
            await EquipBestForSkill(Utils.Details.LegArmorSlot, ItemSlot.LegArmor, skill, bankItems);
            await EquipBestForSkill(Utils.Details.BootsSlot, ItemSlot.Boots, skill, bankItems);
            await EquipBestForSkill(Utils.Details.Ring1Slot, ItemSlot.Ring1, skill, bankItems);
            await EquipBestForSkill(Utils.Details.AmuletSlot, ItemSlot.Amulet, skill, bankItems);
            await EquipBestForSkill(Utils.Details.Artifact1Slot, ItemSlot.Artifact1, skill, bankItems);
            await EquipBestForSkill(Utils.Details.Artifact2Slot, ItemSlot.Artifact2, skill, bankItems);
            await EquipBestForSkill(Utils.Details.Artifact3Slot, ItemSlot.Artifact3, skill, bankItems);
            await EquipBestForSkill(Utils.Details.Utility1Slot, ItemSlot.Utility1, skill, bankItems);
            await EquipBestForSkill(Utils.Details.Utility2Slot, ItemSlot.Utility2, skill, bankItems);
            await EquipBestForSkill(Utils.Details.BagSlot, ItemSlot.Bag, skill, bankItems);
            await EquipBestForSkill(Utils.Details.RuneSlot, ItemSlot.Rune, skill, bankItems);
        }

        private async Task EquipBestForSkill(string currentEquipped, ItemSlot slotType, string skill, List<SimpleItemSchema> bankItems)
        {
            ItemSchema currentItem = null;
            var currentValue = 0.0;
            if (!string.IsNullOrEmpty(currentEquipped))
            {
                currentItem = await Items.Instance.GetItem(currentEquipped);
                currentValue = Items.Instance.CalculateItemValueSkill(currentItem, skill);
            }

            var bestInventoryItem = await GetBestItemFromInventorySkill(slotType, skill);
            var inventoryValue = bestInventoryItem != null ? Items.Instance.CalculateItemValueSkill(bestInventoryItem, skill) : 0;

            var bestBankItem = await GetBestItemFromBankSkill(slotType, skill, bankItems);
            var bankValue = bestBankItem != null ? Items.Instance.CalculateItemValueSkill(bestBankItem, skill) : 0;

            var max = Math.Max(currentValue, Math.Max(bankValue, inventoryValue));
            if (max == currentValue)
            {
                if (currentItem != null)
                {
                    Console.WriteLine($"Already equipped {currentItem.Code} is highest value at {currentValue}");
                }
            }
            else if (max == inventoryValue)
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

        private async Task<ItemSchema> GetBestItemFromBankSkill(ItemSlot slotType, string skill, List<SimpleItemSchema> bankItems)
        {
            ItemSchema bestItem = null;
            foreach (var bankItem in bankItems)
            {
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

        private async Task<ItemSchema> GetBestItemFromInventorySkill(ItemSlot slotType, string skill)
        {
            ItemSchema bestItem = null;
            foreach (var inventoryItem in Utils.Details.Inventory)
            {
                if (string.IsNullOrEmpty(inventoryItem.Code))
                {
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
            // Compare our equipment to the equiment in the bank currently. We can't do anything
            // about bank equipment being taken before we get to it, so we will check for that.

            var monster = await Monsters.Instance.GetMonster(monsterCode);
            
            Console.WriteLine("Check inventory for better items");

            await EquipBestInInventory(Utils.Details.WeaponSlot, ItemSlot.Weapon, monster);
            await EquipBestInInventory(Utils.Details.ShieldSlot, ItemSlot.Shield, monster);
            await EquipBestInInventory(Utils.Details.HelmetSlot, ItemSlot.Helmet, monster);
            await EquipBestInInventory(Utils.Details.BodyArmorSlot, ItemSlot.BodyArmor, monster);
            await EquipBestInInventory(Utils.Details.LegArmorSlot, ItemSlot.LegArmor, monster);
            await EquipBestInInventory(Utils.Details.BootsSlot, ItemSlot.Boots, monster);
            await EquipBestInInventory(Utils.Details.Ring1Slot, ItemSlot.Ring1, monster);
            await EquipBestInInventory(Utils.Details.AmuletSlot, ItemSlot.Amulet, monster);
            await EquipBestInInventory(Utils.Details.Artifact1Slot, ItemSlot.Artifact1, monster);
            await EquipBestInInventory(Utils.Details.Artifact2Slot, ItemSlot.Artifact2, monster);
            await EquipBestInInventory(Utils.Details.Artifact3Slot, ItemSlot.Artifact3, monster);
            await EquipBestInInventory(Utils.Details.Utility1Slot, ItemSlot.Utility1, monster);
            await EquipBestInInventory(Utils.Details.Utility2Slot, ItemSlot.Utility2, monster);
            await EquipBestInInventory(Utils.Details.BagSlot, ItemSlot.Bag, monster);
            await EquipBestInInventory(Utils.Details.RuneSlot, ItemSlot.Rune, monster);
            
            Console.WriteLine("Check bank for better items");
            
            var bankItems = await Bank.Instance.GetItems();
            var deposit = await EquipBestInBank(Utils.Details.WeaponSlot, ItemSlot.Weapon, monster, bankItems);
            deposit |= await EquipBestInBank(Utils.Details.ShieldSlot, ItemSlot.Shield, monster, bankItems);
            deposit |= await EquipBestInBank(Utils.Details.HelmetSlot, ItemSlot.Helmet, monster, bankItems);
            deposit |= await EquipBestInBank(Utils.Details.BodyArmorSlot, ItemSlot.BodyArmor, monster, bankItems);
            deposit |= await EquipBestInBank(Utils.Details.LegArmorSlot, ItemSlot.LegArmor, monster, bankItems);
            deposit |= await EquipBestInBank(Utils.Details.BootsSlot, ItemSlot.Boots, monster, bankItems);
            deposit |= await EquipBestInBank(Utils.Details.Ring1Slot, ItemSlot.Ring1, monster, bankItems);
            deposit |= await EquipBestInBank(Utils.Details.AmuletSlot, ItemSlot.Amulet, monster, bankItems);
            deposit |= await EquipBestInBank(Utils.Details.Artifact1Slot, ItemSlot.Artifact1, monster, bankItems);
            deposit |= await EquipBestInBank(Utils.Details.Artifact2Slot, ItemSlot.Artifact2, monster, bankItems);
            deposit |= await EquipBestInBank(Utils.Details.Artifact3Slot, ItemSlot.Artifact3, monster, bankItems);
            deposit |= await EquipBestInBank(Utils.Details.Utility1Slot, ItemSlot.Utility1, monster, bankItems);
            deposit |= await EquipBestInBank(Utils.Details.Utility2Slot, ItemSlot.Utility2, monster, bankItems);
            deposit |= await EquipBestInBank(Utils.Details.BagSlot, ItemSlot.Bag, monster, bankItems);
            deposit |= await EquipBestInBank(Utils.Details.RuneSlot, ItemSlot.Rune, monster, bankItems);
            if (deposit)
            {
                await DepositAllItems();
            }
        }

        private async Task<bool> EquipBestInBank(string currentEquipped, ItemSlot slotType, MonsterSchema monster, List<SimpleItemSchema> bankItems)
        {
            ItemSchema bestItem = await GetBestItemFromBank(currentEquipped, slotType, monster, bankItems);

            if (bestItem != null && bestItem.Code != currentEquipped)
            {
                await MoveTo(MapContentType.Bank);
                int withdrawn = 0;
                if (slotType == ItemSlot.Utility1 || slotType == ItemSlot.Utility2)
                {
                    // Special case for potions
                    withdrawn = await WithdrawItems(bestItem.Code, 1);
                }
                else 
                {
                    withdrawn = await WithdrawItems(bestItem.Code, 1);
                }

                if (withdrawn > 0)
                {
                    if (!string.IsNullOrEmpty(currentEquipped))
                    {
                        await Unequip(slotType);
                    }

                    await Equip(bestItem.Code, slotType);
                }
                else
                {
                    Console.WriteLine($"Cannot find {bestItem.Code} in bank");
                }

                return true;
            }

            return false;
        }

        private async Task<ItemSchema> GetBestItemFromBank(string currentEquipped, ItemSlot slotType, MonsterSchema monster, List<SimpleItemSchema> bankItems)
        {
            ItemSchema bestItem = null;
            if (!string.IsNullOrEmpty(currentEquipped))
            {
                bestItem = await Items.Instance.GetItem(currentEquipped);
            }

            foreach (var bankItem in bankItems)
            {
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

        private async Task EquipBestInInventory(string currentEquipped, ItemSlot slotType, MonsterSchema monster)
        {
            ItemSchema bestItem = await GetBestItemFromInventory(currentEquipped, slotType, monster);

            if (bestItem != null && bestItem.Code != currentEquipped)
            {
                if (!string.IsNullOrEmpty(currentEquipped))
                {
                    await Unequip(slotType);
                }

                await Equip(bestItem.Code, slotType);
            }
        }

        private static async Task<ItemSchema> GetBestItemFromInventory(string currentEquipped, ItemSlot slotType, MonsterSchema monster)
        {
            ItemSchema bestItem = null;
            if (!string.IsNullOrEmpty(currentEquipped))
            {
                bestItem = await Items.Instance.GetItem(currentEquipped);
            }

            foreach (var inventoryItem in Utils.Details.Inventory)
            {
                if (string.IsNullOrEmpty(inventoryItem.Code))
                {
                    continue;
                }

                var item = await Items.Instance.GetItem(inventoryItem.Code);
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

        private async Task Unequip(ItemSlot slotType)
        {
            await Utils.ApiCall(async () =>
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
                    return _api.ActionUnequipItemMyNameActionUnequipPostAsync(Name, new UnequipSchema(slotType, quantity));
                }
                catch (ApiException ex)
                {
                    Console.WriteLine($"Unequip error: {ex.ErrorContent}");
                    throw;
                }
        });
        }

        private async Task Equip(string code, ItemSlot slotType, int quantity = 0)
        {
            await Utils.ApiCall(async () =>
            {
                try
                {
                    var quantity = 1;
                    if (slotType == ItemSlot.Utility1 || slotType == ItemSlot.Utility2)
                    {
                        quantity = Utils.Details.Inventory.Where(x => x.Code == code).Sum(x => x.Quantity);
                    }

                    Console.WriteLine($"Equip {quantity} {code} in {slotType}");
                    var response = await _api.ActionEquipItemMyNameActionEquipPostAsync(Name, new EquipSchema(code, slotType, quantity));
                    Console.WriteLine($"Equip response: {response.Data.Item.Code} equipped in slot {response.Data.Slot}");
                    return response;
                }
                catch(ApiException ex)
                {
                    Console.WriteLine($"Equip error: {ex.ErrorContent}");
                    throw;
                }
            });
        }

        internal async Task Recycle(string code, int recycleQuantity)
        {
            Console.WriteLine($"Recycling {recycleQuantity} of {code}");

            await Utils.ApiCall(async () =>
            {
                return _api.ActionRecyclingMyNameActionRecyclingPostAsync(Name, new RecyclingSchema(code, recycleQuantity));
            });
        }
    }
}
