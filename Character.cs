using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Artifacts
{
    internal class Character
    {
        internal MyCharactersApi _api;
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
            var batchSize = 5;
            var craftQuantity = Math.Min(remaining, batchSize);
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
                        Console.WriteLine("Ran out of components creating items");
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
                var location = locations.OrderBy(loc =>  Utils.CalculateManhattanDistance(loc.X, loc.Y, Utils.Details.X, Utils.Details.Y)).First();
                Console.WriteLine($"Closest location is at ({location.X}, {location.Y})");
                await Move(location.X, location.Y);
            }
            else
            {
                throw new Exception($"No locations found for type {contentType} and code {code}");
            }
        }

        internal async Task<int> GatherItems(string code, int remaining)
        {
            // Check the bank first
            var bankItems = await Bank.Instance.GetItems();
            var bankItemAmount = bankItems.Where(x => x.Code == code).Sum(x => x.Quantity);
            if (bankItemAmount > 0)
            {
                Console.WriteLine($"Found {bankItemAmount} {code} in the bank, going to get them");
                await MoveTo(MapContentType.Bank);
                var quantity = Math.Min(remaining, bankItemAmount);
                await WithdrawItems(code, quantity);
                if (quantity == remaining)
                {
                    // Found all we need in the bank
                    return quantity;
                }
            }

            var item = await Items.Instance.GetItem(code);
            if (item.Craft != null)
            {
                Console.WriteLine($"Cannot gather {code}, need to craft it");
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
                throw new Exception($"No resource found with drop {code}, we cannot craft this");
                return 0;
            }

            await MoveClosest(MapContentType.Resource, resourceCode);

            // TODO: gear up

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
                        await WithdrawItems(code);
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
            await Utils.ApiCall(async () =>
            {
                return await _api.ActionRestMyNameActionRestPostAsync(Name);
            });
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
                    withdrawn = await WithdrawItems(bestItem.Code);
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

            Console.WriteLine($"Best item in bank for {slotType} is {bestItem?.Code}");
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

            Console.WriteLine($"Best item in inventory for {slotType} is {bestItem?.Code}");
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
                    return _api.ActionEquipItemMyNameActionEquipPostAsync(Name, new EquipSchema(code, slotType, quantity));
                }
                catch(ApiException ex)
                {
                    Console.WriteLine($"Equip error: {ex.ErrorContent}");
                    throw;
                }
            });
        }
    }
}
