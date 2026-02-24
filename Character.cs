using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using static StackExchange.Redis.Role;

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

        internal Character(
            MyCharactersApi api,
            string name
            )
        {
            _api = api;
            Name = name;
        }

        internal async Task Init()
        {
            var response = await _api.GetMyCharactersMyCharactersGetAsync();
            var character = response.Data.First(c => c.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));
            Utils.Details[Name] = character;
        }

        internal async Task<List<CharacterSchema>> GetAllCharacters()
        {
            var response = await _api.GetMyCharactersMyCharactersGetAsync();
            return response.Data;
        }

        internal async Task Move(int x, int y)
        {
            if (x == Utils.Details[Name].X && y == Utils.Details[Name].Y)
            {
                Console.WriteLine("Already at location");
                return;
            }

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
                    MapSchema location = GetClosest(response.Data);
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
            try
            {
                await Utils.ApiCall(async () =>
                {
                    var item = new SimpleItemSchema(code, quantity);
                    return await _api.ActionTaskTradeMyNameActionTaskTradePostAsync(Name, item);
                });
            }
            catch(ApiException ex)
            {
                Console.WriteLine($"Error trading in items {ex.ErrorContent}");
                if (ex.ErrorCode == 478)
                {
                    // Inventory dump
                    Console.WriteLine("Inventory dump");
                    foreach(var item in Utils.Details[Name].Inventory)
                    {
                        if (item.Quantity > 0)
                        {
                            Console.WriteLine($"{item.Quantity} {item.Code}");
                        }
                    }
                }

                throw;
            }
        }

        internal async Task<int> CraftItems(ItemSchema item, int remaining, bool bankOnly = false, bool ignoreBank = false)
        {
            Console.WriteLine($"Crafting {remaining} {item.Code} for character {Name}, bankOnly {bankOnly}, ignoreBank {ignoreBank}");
            Console.WriteLine($"{item.Code}: {remaining}");
            Console.WriteLine($"{item.Code}: {remaining}");
            Console.WriteLine($"{item.Code}: {remaining}");
            item.PrintCraftComponents();

            var minSkillLevel = item.Craft.Level;
            var skill = item.Craft.Skill;
            var totalAmount = remaining;

            // We'll make a few at a time
            var perItemInventorySpace = item.Craft.Items.Sum(x => x.Quantity);
            var batchSize = CalculateCraftingBatchSize(item.Craft);
            var craftQuantity = Math.Min(batchSize, remaining);
            if (craftQuantity <= 0)
            {
                Console.WriteLine($"No {item.Code} to craft");
                return 0;
            }

            return await CraftBatch(item, craftQuantity, bankOnly, ignoreBank);
        }

        private async Task<int> CraftBatch(ItemSchema item, int craftQuantity, bool bankOnly, bool ignoreBank)
        {
            Console.WriteLine($"Craft batch of {craftQuantity} {item.Code}, bankOnly {bankOnly}, ignoreBank {ignoreBank}");

            var gatherQuantities = CalculateGatherQuantities(item.Craft, craftQuantity);

            var found = await FetchCraftingItems(item, gatherQuantities, bankOnly, ignoreBank);
            if (found == 0)
            {
                await PerformTask();
                return 0;
            }

            // Gear up for crafting to get more xp
            var skillLevel = GetSkillLevel(item.Craft.Skill.ToString());
            if (item.Level > skillLevel - 10)
            {
                await GearUpSkill("crafting", null);
            }

            // Go to the crafting location
            await MoveClosest(MapContentType.Workshop, item.Craft.Skill.Value.ToString());

            // Craft the items one at a time until we can't make any more. We may be using inventory items from before, or we may run out of gathered items.
            var itemsCrafted = 0;
            for (int index = 0; index < craftQuantity; index++)
            {
                try
                {
                    var craftResponse = await Utils.ApiCall(async () =>
                    {
                        var leftToGet = craftQuantity - itemsCrafted;
                        var estimatedTime = new TimeSpan(hours: 0, minutes: 0, seconds: leftToGet * 5);
                        Console.WriteLine($"Craft {itemsCrafted + 1}/{craftQuantity} {item.Code} ETA: {estimatedTime}");
                        var response = await _api.ActionCraftingMyNameActionCraftingPostAsync(Name, new CraftingSchema(item.Code, 1));
                        Console.WriteLine($"{Name} Xp: {response.Data.Details.Xp}");
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
            
            Console.WriteLine($"Crafted {itemsCrafted} {item.Code} for character {Name}");
            return itemsCrafted;
        }

        private async Task<int> FetchCraftingItems(ItemSchema item, Dictionary<string, int> gatherQuantities, bool bankOnly, bool ignoreBank)
        {
            // Get the items required for crafting
            var index = 0;
            var gathered = 0;
            foreach (var component in item.Craft.Items)
            {
                gathered = 0;

                if (index > 0)
                {
                    await DropOffNonComponents(item);
                }

                index++;

                var componentQuantity = 0;
                var gatherQuantity = gatherQuantities[component.Code];
                while (componentQuantity < gatherQuantity)
                {
                    var current = await GatherItems(component.Code, gatherQuantity, bankOnly: bankOnly, ignoreBank: ignoreBank);
                    gathered += current;
                    componentQuantity += current;

                    Console.WriteLine($"Gathered {componentQuantity}/{gatherQuantity} {component.Code}");
                    if (current == 0)
                    {
                        Console.WriteLine($"We cannot gather any more {component.Code}!");
                        return gathered;
                    }
                }
            }

            return gathered;
        }

        private async Task DropOffNonComponents(ItemSchema item)
        {
            var itemsToExclude = new HashSet<string>();
            AddComponents(item, ref itemsToExclude);

            await DepositExcept(itemsToExclude.ToList());
        }

        private void AddComponents(ItemSchema item, ref HashSet<string> components)
        {
            components.Add(item.Code);

            if (item.Craft != null)
            {
                foreach (var craftItem in item.Craft.Items)
                {
                    var craft = Items.Instance.GetItem(craftItem.Code);
                    AddComponents(craft, ref components);
                }
            }
        }

        private async Task PerformTask()
        {
            await ExchangeCoins();

            if (string.IsNullOrEmpty(Utils.Details[Name].Task))
            {
                // Go to task master
                await AcceptNewTask();
            };

            Console.WriteLine($"Current task:\n + " +
                $"task : {Utils.Details[Name].Task}\n" +
                $"task type : {Utils.Details[Name].TaskType}\n" +
                $"task progress : {Utils.Details[Name].TaskProgress}/{Utils.Details[Name].TaskTotal}");

            await DoItemTask();
            await MoveTo(MapContentType.Bank);
            await DepositAllItems();
            await ExchangeCoins();
        }

        private Dictionary<string, int> CalculateGatherQuantities(CraftSchema craft, int craftQuantity)
        {
            var gatherQuantities = new Dictionary<string, int>();
            var ownedItems = new Dictionary<string, int>();

            foreach (var component in craft.Items)
            {
                var ownedItem = Utils.Details[Name].Inventory.FirstOrDefault(i => i.Code.Equals(component.Code, StringComparison.OrdinalIgnoreCase));
                var ownedQuantity = ownedItem != null ? ownedItem.Quantity : 0;
                ownedItems[component.Code] = ownedQuantity;
                gatherQuantities[component.Code] = 0;
            }

            for (var index = 0; index < craftQuantity; index++)
            {
                foreach (var component in craft.Items)
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

            return gatherQuantities;
        }

        private async Task DoItemTask()
        {
            await HandleItemsTask();
            await MoveTo(MapContentType.TasksMaster, "items");
            await FinishTask();
        }

        private async Task FinishTask()
        {
            List<SimpleItemSchema> rewards = new List<SimpleItemSchema>();

            await Utils.ApiCall(async () =>
            {
                var result = await _api.ActionCompleteTaskMyNameActionTaskCompletePostAsync(Name);
                Console.WriteLine($"Finished task! {result.Data.Rewards.Gold} Gold");
                foreach (var item in result.Data.Rewards.Items)
                {
                    Console.WriteLine($"drop: {item.Quantity} {item.Code}");
                }

                rewards = result.Data.Rewards.Items;

                return result;
            });

            if (rewards.Any())
            {
                await ConsumeGold(rewards);
            }
        }

        private async Task HandleItemsTask()
        {
            while (Utils.Details[Name].TaskProgress < Utils.Details[Name].TaskTotal)
            {
                var remaining = Utils.Details[Name].TaskTotal - Utils.Details[Name].TaskProgress;
                Console.WriteLine($"{remaining} {Utils.Details[Name].Task} left for task");

                // Go to bank and deposit all items to make room
                await MoveTo(MapContentType.Bank);
                await DepositAllItems();

                var withdrawn = await WithdrawItems(Utils.Details[Name].Task, remaining);
                if (withdrawn > 0)
                {
                    await ExchangeItems(Utils.Details[Name].Task, withdrawn, remaining);
                    remaining -= withdrawn;
                }

                if (remaining > 0)
                {
                    // Go gather, craft, or hunt down the item
                    Console.WriteLine($"Fetching item {Utils.Details[Name].Task}");

                    var item = Items.Instance.GetItem(Utils.Details[Name].Task);
                    var perItemInventorySpace = 1;
                    if (item.Craft != null)
                    {
                        perItemInventorySpace = item.Craft.Items.Sum(x => x.Quantity);
                    }
                    var gathered = await GatherItems(Utils.Details[Name].Task, remaining);
                    await ExchangeItems(Utils.Details[Name].Task, gathered, remaining);
                }
            }
        }

        private async Task HandleMonstersTask()
        {
            while (Utils.Details[Name].TaskProgress < Utils.Details[Name].TaskTotal)
            {
                var remaining = Utils.Details[Name].TaskTotal - Utils.Details[Name].TaskProgress;
                var monster = Utils.Details[Name].Task;
                await FightLoop(remaining, monster);
            }
        }

        private async Task ExchangeItems(string code, int quantity, int remaining)
        {
            // Go to task master to turn in items
            Console.WriteLine($"Moving to task master to turn in {quantity} items from inventory");
            await MoveTo(MapContentType.TasksMaster, "items");
            await TurnInItems(code, Math.Min(quantity, remaining));
        }

        private async Task AcceptNewTask()
        {
            // Get new task
            await MoveTo(MapContentType.TasksMaster, code: "items");
            Console.WriteLine($"Getting new task");
            await Utils.ApiCall(async () =>
            {
                var result = await _api.ActionAcceptNewTaskMyNameActionTaskNewPostAsync(Name);
                Console.WriteLine($"new task: " + result.Data.Task.ToJson());
                return result;
            });
        }

        private async Task ExchangeCoins()
        {
            var bankItems = await Bank.Instance.GetItems();
            foreach (var item in bankItems)
            {
                if (item.Code == "tasks_coin" && item.Quantity >= 6)
                {
                    await MoveTo(MapContentType.Bank);
                    var amount = await WithdrawItems("tasks_coin");
                    if (amount >= 6)
                    {
                        Console.WriteLine($"Got {amount} coins, going to exchange");
                        await MoveTo(MapContentType.TasksMaster);

                        while (amount >= 6)
                        {
                            Console.WriteLine($"Exchange task coins");
                            await Utils.ApiCall(async () =>
                            {
                                var result = await _api.ActionTaskExchangeMyNameActionTaskExchangePostAsync(Name);
                                Console.WriteLine($"Got rewards: {result.Data.Rewards.Gold} gold, {string.Join(',', result.Data.Rewards.Items.Select(item => item.Code))}");
                                return result;
                            });

                            amount -= 6;
                        }
                    }
                }
            }
        }

        private int CalculateCraftingBatchSize(CraftSchema craft)
        {
            var space = GetFreeInventorySpace() * .8;
            var quantity = 0;
            while(space > 0)
            {
                foreach(var component in craft.Items)
                {
                    space -= component.Quantity;
                }

                quantity++;
            }

            return quantity - 1;
        }

        internal async Task MoveClosest(MapContentType contentType, string code)
        {
            Console.WriteLine($"Moving to closest {code} for character {Name}");
            var response = await Map.Instance.GetMapLayer(contentType, code);
            if (response.Data != null && response.Data.Any())
            {
                var locations = response.Data;

                var location = GetClosest(locations);
                await Move(location.X, location.Y);
            }
            else
            {
                throw new Exception($"No locations found for type {contentType} and code {code}");
            }
        }

        internal async Task<int> GatherItems(string code, int total, bool ignoreBank = false, bool bankOnly = false, ItemSchema doNotUse = null)
        {
            Console.WriteLine($"Gather {total} {code}");
            var withdrawn = 0;

            if (!ignoreBank)
            {
                // Check the bank first
                withdrawn = await GatherFromBank(code, total);
                if (withdrawn < total)
                {
                    Console.WriteLine($"Did not get enough from the bank, still need {total - withdrawn}");
                }
                else
                {
                    Console.WriteLine($"Got enough from the bank");
                    return withdrawn;
                }
            }

            if (bankOnly)
            {
                Console.WriteLine($"Bank only, got {withdrawn} {code}");
                return withdrawn;
            }

            var remaining = total - withdrawn;

            var item = Items.Instance.GetItem(code);
            if (item.Craft != null)
            {
                Console.WriteLine($"Need to craft {code}");
                var crafted = await CraftItems(item, remaining, bankOnly, ignoreBank);
                return crafted + withdrawn;
            }

            var resource = await Resources.Instance.GetResourceDrop(code);
            if (resource != null)
            {
                var gathered = await GatherResources(code, doNotUse, remaining, item, resource);
                return gathered + withdrawn;
            }

            var monsters = Monsters.Instance.GetMonsters(maxLevel: 100, dropCode: item.Code);
            if (monsters.Data.Any())
            {
                var foughtFor = await GatherFromMonsters(code, remaining, monsters);
                return foughtFor + withdrawn;
            }

            int amountFromNpcs = await GatherFromNpc(code, remaining);
            if (amountFromNpcs > 0)
            {
                return amountFromNpcs + withdrawn;
            }

            int amountFromMarket = await GatherFromMarket(code, remaining);
            if (amountFromMarket > 0)
            {
                return amountFromMarket + withdrawn;
            }

            Console.WriteLine($"No way to gather, craft, or hunt for {code}, we cannot craft this\n");
            return 0;
        }

        private async Task<int> GatherFromMarket(string code, int total)
        {
            Console.WriteLine($"Check market for {total} {code}");
            var orders = await Bank.Instance.GetExchangeOrders(code);
            if (!orders.Any())
            {
                return 0;
            }

            Console.WriteLine($"There are orders for {code}, going to exchange");

            var cheapest = orders.OrderBy(x => x.Price);
            var gold = await FetchGoldForOrders(code, total, cheapest);
            if (gold == 0)
            {
                return 0;
            }

            await MoveTo(MapContentType.Bank);

            if (await WithdrawGold(gold) != gold)
            {
                Console.WriteLine($"Not enough gold in the bank!");
                return 0;
            }

            await MoveTo(MapContentType.GrandExchange);

            // Orders changed?
            orders = await Bank.Instance.GetExchangeOrders(code);
            if (!orders.Any())
            {
                return 0;
            }


            var remaining = total;
            var gotten = 0;
            cheapest = orders.OrderBy(x => x.Price);

            while (remaining > 0)
            {
                if (!cheapest.Any()) break;

                var order = cheapest.First();
                var amount = Math.Min(remaining, order.Quantity);

                try
                {
                    var exchange = await BuyExchangeOrder(order.Id, amount);
                    gotten += exchange.Data.Order.Quantity;
                    remaining -= exchange.Data.Order.Quantity;
                }
                catch (ApiException ex)
                {
                    Console.WriteLine($"GE transaction error: {ex.ErrorContent}");
                    return gotten;
                }
            }

            return gotten;
        }

        private async Task<int> FetchGoldForOrders(string code, int total, IOrderedEnumerable<GEOrderSchema> cheapest)
        {
            var remaining = total;
            var gold = 0;
            foreach (var order in cheapest)
            {
                var amount = Math.Min(remaining, order.Quantity);
                gold += amount * order.Price;
                remaining -= amount;

                if (remaining <= 0)
                {
                    return gold;
                }
            }

            return 0;
        }

        private async Task<GETransactionResponseSchema> BuyExchangeOrder(string orderId, int quantity)
        {
            Console.WriteLine($"Order {quantity} from {orderId}");
            return await Utils.ApiCall(async () =>
            {
                return await _api.ActionGeBuyItemMyNameActionGrandexchangeBuyPostAsync(Name, new GEBuyOrderSchema(orderId, quantity));
            }) as GETransactionResponseSchema;
        }

        private async Task<int> GatherFromNpc(string code, int total)
        {
            var item = await Npcs.Instance.FindNpcItem(code);
            if (item != null && item?.BuyPrice > 0)
            {
                if (item.Npc == "tasks_trader") return 0;

                Console.WriteLine($"Try to buy {total} {code} from NPC");
                var maps = await Map.Instance.GetMapLayer(MapContentType.Npc, item.Npc);
                while (maps.Data.Any())
                {
                    var map = maps.Data.First();
                    maps.Data.Remove(map);

                    if (map.Layer == MapLayer.Overworld)
                    {
                        try
                        {
                            // Double check that we can move to him
                            Console.WriteLine($"Try to move to NPC {item.Npc}");
                            await Move(map.X, map.Y);
                        }
                        catch (Exception ex)
                        {
                            // We cannot move to the npc!
                            Console.WriteLine($"Cannot move to {item.Npc}: {ex.Message}");
                            continue;
                        }

                        var remaining = item.BuyPrice * total;

                        while (remaining > 0)
                        {
                            Console.WriteLine($"Gather {item.BuyPrice * total} {item.Currency} to buy from NPC");
                            var gathered = await GatherItems(item.Currency, item.BuyPrice.Value * total);
                            if (gathered > 0)
                            {
                                remaining -= gathered;
                            }
                            else
                            {
                                Console.WriteLine($"Could not gather {item.Currency}!");
                                return 0;
                            }
                        }

                        Console.WriteLine($"Move to NPC {item.Npc}");
                        await Move(map.X, map.Y);
                        await BuyNpcItem(code, total);
                        return total;
                    }
                }
            }

            return 0;
        }

        private async Task<NpcMerchantTransactionResponseSchema> BuyNpcItem(string code, int remaining)
        {
            return await Utils.ApiCall(async () =>
            {
                Console.WriteLine($"Buy {remaining} {code} from NPC");
                return await _api.ActionNpcBuyItemMyNameActionNpcBuyPostAsync(Name, new NpcMerchantBuySchema(code, remaining));
            }) as NpcMerchantTransactionResponseSchema;
        }

        private async Task<int> GatherResources(string code, ItemSchema doNotUse, int remaining, ItemSchema item, ResourceSchema resource)
        {
            Console.WriteLine($"Gathering {remaining} {code} for character {Name}");
            var skill = await Resources.Instance.GetResourceSkill(item);

            await GearUpSkill(skill, doNotUse);
            await MoveClosest(MapContentType.Resource, resource.Code);

            var gathered = 0;
            var leftToGet = remaining - gathered;
            while (leftToGet > 0)
            {
                var estimatedTime = new TimeSpan(hours: 0, minutes: 0, seconds: leftToGet * Utils.LastCooldown);
                Console.WriteLine($"{gathered}/{remaining} {code} ETA: {estimatedTime}");

                try
                {
                    var result = await Utils.ApiCall(async () =>
                    {
                        return await _api.ActionGatheringMyNameActionGatheringPostAsync(Name);
                    });

                    var schema = result as SkillResponseSchema;
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
                catch (ApiException ex)
                {
                    if (ex.ErrorCode == 497)
                    {
                        Console.WriteLine("Inventory full, done for now");
                        return gathered;
                    }
                    else if (ex.ErrorCode == 493)
                    {
                        Console.WriteLine($"Not skilled enough to get {code}, going training");
                        await TrainSkill(skill, item.Level);

                        Console.WriteLine($"Back from training, gathering {leftToGet} {code}");
                        return await GatherResources(code, doNotUse, remaining, item, resource);
                    }

                    throw;
                }
            }

            return gathered;
        }

        private async Task<int> GatherFromMonsters(string code, int remaining, DataPageMonsterSchema monsters)
        {
            var monster = monsters.Data.MinBy(x => x.Level);
            Console.WriteLine($"Need to fight for {code}, chasing {monster}");

            if (monster.Level > Utils.Details[Name].Level || monster.Type == MonsterType.Boss)
            {
                Console.WriteLine($"We can't beat {monster.Code}, training");
                await TrainFighting(monster.Level + 1);
            }

            var drops = await FightDrops(monster.Code, code, remaining);
            return drops;
        }

        private async Task TrainFighting(int level)
        {
            var maxLevel = Math.Max(1, Utils.Details[Name].Level - 1);
            var monsters = Monsters.Instance.GetMonsters(maxLevel);
            var fightList = monsters.Data.OrderBy(x => x.Level).ToList();
            var lastXp = 0;

            while (Utils.Details[Name].Level < level)
            {
                var monster = fightList.Last();
                Console.WriteLine($"Train fighting with {monster.Code} from level {Utils.Details[Name].Level}/{level}");

                await GearUpMonster(monster.Code);

                var losses = 0;
                for (int index = 0; index < 25; index++)
                {
                    // If we are healthy enough fight right away
                    var needsRest = Utils.Details[Name].Hp < Utils.Details[Name].MaxHp * .6;
                    if (needsRest)
                    {
                        if (await Rest())
                        {
                            Console.WriteLine("We cooked food, gear up again");
                            await GearUpMonster(monster.Code);
                        }
                    }

                    try
                    {
                        // Assume we are not at the monster
                        await MoveTo(MapContentType.Monster, code: monster.Code);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Cannot move to monster {monster.Code}: {ex.Message}");
                        fightList.Remove(monster);
                        break;
                    }

                    try
                    {
                        Console.WriteLine($"Training run {index + 1}/25");

                        var hp = Utils.Details[Name].Hp;
                        var result = await Fight();
                        if (result.Data.Fight.Result == FightResult.Loss)
                        {
                            const int Limit = 3;
                            losses++;
                            Console.WriteLine($"loss {losses}/{Limit}");
                            if (losses >= Limit)
                            {
                                Console.WriteLine($"We Lost! Giving up on training with monster {monster.Code}");
                                fightList.Remove(monster);
                                break;
                            }
                        }
                        else
                        {
                            // Reset losses, we can beat him!
                            losses = 0;

                            if (Utils.Details[Name].Level >= level)
                            {
                                return;
                            }

                            if (lastXp == 0)
                            {
                                lastXp = result.Data.Fight.Characters.First().Xp;
                            }
                            else if (result.Data.Fight.Characters.First().Xp < lastXp)
                            {
                                Console.WriteLine($"Less XP from {monster.Code}, getting new monsters");
                                lastXp = 0;
                                monsters = Monsters.Instance.GetMonsters(Utils.Details[Name].Level - 1);
                                fightList = monsters.Data.Where(x => x.Level > Utils.Details[Name].Level - 10).OrderBy(x => x.Level).ToList();
                                break;
                            }
                        }
                    }
                    catch (ApiException ex)
                    {
                        if (ex.ErrorCode == 497)
                        {
                            Console.WriteLine("Inventory full, cannot fight");
                            await MoveTo(MapContentType.Bank);
                            await DepositAllItems();
                        }

                        Console.WriteLine($"Fight error: {ex.ErrorContent}");
                        throw;
                    }
                }
            }
        }

        private async Task<int> GatherFromBank(string code, int total)
        {
            var bankItems = await Bank.Instance.GetItems();
            var bankItemAmount = bankItems.Where(x => x.Code == code).Sum(x => x.Quantity);
            if (bankItemAmount > 0)
            {
                Console.WriteLine($"{bankItemAmount} {code} in bank, get {total} of them");
                await MoveTo(MapContentType.Bank);
                var quantity = Math.Min(total, bankItemAmount);
                return await WithdrawItems(code, quantity);
            }

            return 0;
        }

        private async Task TrainSkill(string skill, int levelGoal, ItemSchema doNotUse = null)
        {
            var currentLevel = GetSkillLevel(skill);
            Console.WriteLine($"Train {skill} from {currentLevel} to {levelGoal}");

            while (currentLevel < levelGoal)
            {
                await TrainGathering(currentLevel, skill, doNotUse);
                currentLevel = GetSkillLevel(skill);
                Console.WriteLine($"Training at {currentLevel}/{levelGoal}");
            }
        }

        internal async Task TrainGathering(int level, string skill, ItemSchema doNotUse = null)
        {
            Console.WriteLine($"Train {skill} to level {level}");
            var items = Items.Instance.GetAllItems();
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

                    total = await GatherItems(item, 50, ignoreBank: true, bankOnly: false, doNotUse);
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
            await GearUpMonster(monster);
            var monsterSchema = await Monsters.Instance.GetMonster(monster);

            var lostLastFight = 0;
            var losses = 0;

            var drops = 0;
            while (drops < remaining)
            {
                // If we are healthy enough fight right away
                var needsRest = Utils.Details[Name].Hp < Utils.Details[Name].MaxHp * .6;
                if (needsRest)
                {
                    if (await Rest())
                    {
                        Console.WriteLine("We cooked food, gear up again");
                        await GearUpMonster(monster);
                    }
                }

                try
                {
                    // Assume we are not at the monster
                    await MoveTo(MapContentType.Monster, code: monster);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Cannot move to monster {monster}: {ex.Message}");
                    return drops;
                }

                // If this is a boss fight we will wait until there are 3 characters present before we fight.
                // While checking for events, we also check to see if any characters are waiting at a boss.
                List<string> participants = null;
                if (monsterSchema.Type == MonsterType.Boss)
                {
                    participants = await WaitForBackup(Name);
                }

                try
                {
                    var hp = Utils.Details[Name].Hp;
                    var result = await Fight(participants);
                    lostLastFight = hp - Utils.Details[Name].Hp;
                    if (result.Data.Fight.Result == FightResult.Loss)
                    {
                        const int Limit = 3;
                        losses++;
                        Console.WriteLine($"loss {losses}/{Limit}");
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

                    Console.WriteLine($"Got {drops}/{remaining} {code} from the {monster}");
                }
                catch (ApiException ex)
                {
                    if (ex.ErrorCode == 497)
                    {
                        Console.WriteLine("Inventory full, cannot fight");
                        await MoveTo(MapContentType.Bank);
                        await DepositExcept(new List<string>([code]));
                        break;
                    }

                    Console.WriteLine($"Fight error: {ex.ErrorContent}");
                    throw;
                }
            }

            return drops;
        }

        private async Task<List<string>> WaitForBackup(string name)
        {
            var participants = new HashSet<string>();
            var waited = 0;
            while(true)
            {
                var characters = await GetCharacters();
                foreach (var character in characters)
                {
                    if (character.Name == name)
                    {
                        continue;
                    }

                    if (character.X == Utils.Details[Name].X &&
                        character.Y == Utils.Details[Name].Y &&
                        character.Layer == Utils.Details[Name].Layer)
                    {
                        participants.Add(character.Name);
                    }
                    else
                    {
                        participants.Remove(character.Name);
                    }

                    if (participants.Count >= 2)
                    {
                        Console.WriteLine($"Returning first 2 of participants: {string.Join(',', participants)}");
                        return participants.Take(2).ToList();
                    }
                }

                Console.WriteLine($"Wait 10/{waited}s so far for boss");
                Console.WriteLine($"Present so far: {string.Join(',', participants)}");
                waited += 10;
                await Task.Delay(10000);
            }
        }

        internal async Task<List<CharacterSchema>> GetCharacters()
        {
            var response = await _api.GetMyCharactersMyCharactersGetAsync();

            foreach (var character in response.Data)
            {
                Utils.Details[character.Name] = character;
            }

            return response.Data;
        }

        internal async Task DepositAllItems()
        {
            Console.WriteLine($"Depositing all items for character {Name}");
            await DepositGold();

            foreach (var item in Utils.Details[Name].Inventory)
            {
                if (string.IsNullOrEmpty(item.Code) || item.Quantity == 0)
                {
                    continue;
                }

                await DepositItem(item.Code, item.Quantity);
            }
        }

        internal async Task DepositItem(string code, int quantity)
        {
            try
            {
                Console.WriteLine($"Deposit {code} {quantity}");
                await Utils.ApiCall(async () =>
                {
                    var items = new List<SimpleItemSchema>
                    {
                        new SimpleItemSchema(code, quantity)
                    };

                    return await _api.ActionDepositBankItemMyNameActionBankDepositItemPostAsync(Name, items);
                });
            }
            catch (ApiException ex)
            {
                if (ex.ErrorCode == 478)
                {
                    // We don't have enough of this item??
                    Console.WriteLine($"We do not have {quantity} {code}");
                }
                else if (ex.ErrorCode == 461)
                {
                    // Already in transaction? Try again
                    Console.WriteLine($"Item {code} already in transaction");
                    await DepositItem(code, quantity);
                }
                else if (ex.ErrorCode == 462)
                {
                    Console.WriteLine("Bank is full, trying to expand");
                    await ExpandBank();
                    await DepositItem(code, quantity);
                    return;
                }

                throw;
            }
        }

        private async Task ExpandBank()
        {
            var bankDetails = await Bank.Instance.GetBankDetails();
            var withdrawn = await WithdrawGold(bankDetails.NextExpansionCost);

            while (withdrawn == bankDetails.NextExpansionCost)
            {
                Console.WriteLine($"{Name} buying bank expansion for {bankDetails.NextExpansionCost}");
                await Utils.ApiCall(async () =>
                {
                    return await _api.ActionBuyBankExpansionMyNameActionBankBuyExpansionPostAsync(Name);
                });

                bankDetails = await Bank.Instance.GetBankDetails();
                withdrawn = await WithdrawGold(bankDetails.NextExpansionCost);
            }
        }

        internal async Task DepositExcept(List<string> excludeCodes)
        {
            Console.WriteLine($"Depositing all items except {string.Join(", ", excludeCodes)} for character {Name}");
            await DepositGold();

            var items = new List<SimpleItemSchema>();
            foreach (var item in Utils.Details[Name].Inventory)
            {
                if (!string.IsNullOrEmpty(item.Code) && !excludeCodes.Contains(item.Code, StringComparer.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Deposit {item.Quantity} {item.Code}");
                    items.Add(new SimpleItemSchema(item.Code, item.Quantity));
                }
            }

            if (items.Count == 0)
            {
                Console.WriteLine($"No items to deposit for character {Name}");
                return;
            }

            await MoveTo(MapContentType.Bank);

            try
            {
                await Utils.ApiCall(async () =>
                {
                    var response = await _api.ActionDepositBankItemMyNameActionBankDepositItemPostAsync(Name, items);
                    return response;
                });
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"Deposit error: {ex.ErrorContent}"); 
            }
        }

        internal async Task DepositGold()
        {
            if (Utils.Details[Name].Gold <= 0)
            {
                return;
            }

            await MoveTo(MapContentType.Bank);

            await Utils.ApiCall(async () =>
            {
                Console.WriteLine($"Depositing {Utils.Details[Name].Gold} gold for character {Name}");
                var response = await _api.ActionDepositBankGoldMyNameActionBankDepositGoldPostAsync(Name, new DepositWithdrawGoldSchema(quantity: Utils.Details[Name].Gold));
                return response;
            });
        }

        internal async Task<int> WithdrawGold(int quantity)
        {
            try
            {
                await Utils.ApiCall(async () =>
                {
                    Console.WriteLine($"Withdrawing {quantity} gold for character {Name}");
                    var response = await _api.ActionWithdrawBankGoldMyNameActionBankWithdrawGoldPostAsync(Name, new DepositWithdrawGoldSchema(quantity: quantity));
                    return response;
                });

                return quantity;
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"Error trying to get gold {ex.ErrorContent}");
            }

            return 0;
        }

        internal async Task<int> WithdrawItems(string code, int quantity = 0)
        {
            var inventorySpace = GetFreeInventorySpace();
            Console.WriteLine($"Customer wants to withdraw {quantity} {code}. Free space {inventorySpace}");

            var bankItems = await Bank.Instance.GetItems();

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
                Console.WriteLine($"Invalid quantity {withdrawQuantity}");
                return 0;
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
            var amount = Utils.Details[Name].InventoryMaxItems;
            foreach(var item in Utils.Details[Name].Inventory)
            {
                amount -= item.Quantity;
            }

            return amount;
        }

        internal async Task FightLoop(int total, string monster)
        {
            var monstersKilled = 0;
            var lostLastFight = 0;
            var losses = 0;

            await GearUpMonster(monster);

            while(monstersKilled < total)
            {

                // If we are healthy enough fight right away
                var needsRest = false;
                if (lostLastFight == 0 && Utils.Details[Name].Hp < Utils.Details[Name].MaxXp * .75)
                {
                    needsRest = true;
                }
                else if (Utils.Details[Name].Hp < lostLastFight)
                {
                    needsRest |= true;
                }

                if (needsRest)
                {
                    if (await Rest())
                    {
                        Console.WriteLine("We cooked food, gear up again");
                        await GearUpMonster(monster);
                    }
                }

                // Assume we are not at the monster
                await MoveTo(MapContentType.Monster, code: monster);

                try
                {
                    var hp = Utils.Details[Name].Hp;
                    var result = await Fight();
                    lostLastFight = hp - Utils.Details[Name].Hp;
                    if (result.Data.Fight.Result == FightResult.Win)
                    {
                        monstersKilled++;
                        // Reset losses, we can beat him!
                        losses = 0;
                    }
                    else if (result.Data.Fight.Result == FightResult.Loss)
                    {
                        const int Limit = 3;
                        losses++;
                        Console.WriteLine($"loss {losses}/{Limit}");
                        if (losses >= Limit)
                        {
                            Console.WriteLine($"We Lost! Giving up on monster {monster}");
                            return;
                        }
                    }
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

                    return;
                }
            }
        }

        internal async Task<bool> Rest()
        {
            if (await EatSomething())
            {
                Console.WriteLine("No time for rest, we ate!");
                return false;
            }

            // No resting, go get food!
            var gotFood = await GetFood();
            if (gotFood)
            {
                await EatSomething();
                return true;
            }

            return false;
        }

        private async Task<bool> EatSomething()
        {
            if (Utils.Details[Name].MaxHp == Utils.Details[Name].Hp)
            {
                // Nothing to do
                return true;
            }

            var amountToHeal = Utils.Details[Name].MaxHp - Utils.Details[Name].Hp;
            Console.WriteLine($"Check for food, we need {amountToHeal} hp");

            foreach (var inventoryItem in Utils.Details[Name].Inventory)
            {
                if (string.IsNullOrEmpty(inventoryItem.Code) || inventoryItem.Quantity == 0)
                {
                    continue;
                }

                var item = Items.Instance.GetItem(inventoryItem.Code);
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

                    Console.WriteLine($"Eat {quantity} {item.Code} to heal {currentAmount}/{amountToHeal}");
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

        private async Task ConsumeGold(List<SimpleItemSchema> items)
        {
            foreach (var simpleItem in items)
            {
                var item = Items.Instance.GetItem(simpleItem.Code);
                if (item.Type == "consumable" && item.Subtype == "bag")
                {
                    await Consume(item.Code, simpleItem.Quantity);
                }
            }
        }

        internal async Task<CharacterFightResponseSchema> Fight(List<string> participants = null)
        {
            Console.WriteLine("Fight!!");

            var result = await Utils.ApiCall(async () =>
            {
                var response = await _api.ActionFightMyNameActionFightPostAsync(Name, new FightRequestSchema(participants));
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

        internal async Task GearUpSkill(string skill, ItemSchema doNotUse)
        {
            Console.WriteLine($"Gear up for {skill}");

            if (skill == null)
            {
                return;
            }

            var bankItems = await Bank.Instance.GetItems();
            await EquipBestForSkill(Utils.Details[Name].WeaponSlot, ItemSlot.Weapon, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details[Name].ShieldSlot, ItemSlot.Shield, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details[Name].HelmetSlot, ItemSlot.Helmet, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details[Name].BodyArmorSlot, ItemSlot.BodyArmor, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details[Name].LegArmorSlot, ItemSlot.LegArmor, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details[Name].BootsSlot, ItemSlot.Boots, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details[Name].Ring1Slot, ItemSlot.Ring1, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details[Name].Ring2Slot, ItemSlot.Ring2, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details[Name].AmuletSlot, ItemSlot.Amulet, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details[Name].Artifact1Slot, ItemSlot.Artifact1, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details[Name].Artifact2Slot, ItemSlot.Artifact2, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details[Name].Artifact3Slot, ItemSlot.Artifact3, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details[Name].Utility1Slot, ItemSlot.Utility1, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details[Name].Utility2Slot, ItemSlot.Utility2, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details[Name].BagSlot, ItemSlot.Bag, skill, bankItems, doNotUse);
            await EquipBestForSkill(Utils.Details[Name].RuneSlot, ItemSlot.Rune, skill, bankItems, doNotUse);
        }

        private async Task EquipBestForSkill(string currentEquipped, ItemSlot slotType, string skill, List<SimpleItemSchema> bankItems, ItemSchema doNotUse)
        {
            ItemSchema currentItem = null;
            var currentValue = 0.0;
            if (!string.IsNullOrEmpty(currentEquipped))
            {
                currentItem = Items.Instance.GetItem(currentEquipped);
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
                    continue;
                }

                var item = Items.Instance.GetItem(bankItem.Code);
                if (!MeetsConditions(item.Conditions))
                {
                    continue;
                }

                if (MatchesUtilitySlot(slotType, bankItem.Code))
                {
                    continue;
                }

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
            foreach (var inventoryItem in Utils.Details[Name].Inventory)
            {
                if (string.IsNullOrEmpty(inventoryItem.Code))
                {
                    continue;
                }

                if (doNotUse != null && inventoryItem.Code == doNotUse.Code)
                {
                    continue;
                }

                if (MatchesUtilitySlot(slotType, inventoryItem.Code))
                {
                    continue;
                }

                var item = Items.Instance.GetItem(inventoryItem.Code);
                if (!MeetsConditions(item.Conditions))
                {
                    continue;
                }

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

        internal async Task GearUpMonster(string monsterCode)
        {
            if (monsterCode == null)
            {
                return;
            }

            var monster = await Monsters.Instance.GetMonster(monsterCode);
            var bankItems = await Bank.Instance.GetItems();

            // The damage bonuses of supplementary equipment do not count if the weapon does not have the damage type
            var weapon = await EquipBestForMonster(Utils.Details[Name].WeaponSlot, ItemSlot.Weapon, monster, bankItems, null);

            await EquipBestForMonster(Utils.Details[Name].ShieldSlot, ItemSlot.Shield, monster, bankItems, weapon);
            await EquipBestForMonster(Utils.Details[Name].HelmetSlot, ItemSlot.Helmet, monster, bankItems, weapon);
            await EquipBestForMonster(Utils.Details[Name].BodyArmorSlot, ItemSlot.BodyArmor, monster, bankItems, weapon);
            await EquipBestForMonster(Utils.Details[Name].LegArmorSlot, ItemSlot.LegArmor, monster, bankItems, weapon);
            await EquipBestForMonster(Utils.Details[Name].BootsSlot, ItemSlot.Boots, monster, bankItems, weapon);
            await EquipBestForMonster(Utils.Details[Name].Ring1Slot, ItemSlot.Ring1, monster, bankItems, weapon);
            await EquipBestForMonster(Utils.Details[Name].Ring2Slot, ItemSlot.Ring2, monster, bankItems, weapon);
            await EquipBestForMonster(Utils.Details[Name].AmuletSlot, ItemSlot.Amulet, monster, bankItems, weapon);
            await EquipBestForMonster(Utils.Details[Name].Artifact1Slot, ItemSlot.Artifact1, monster, bankItems, weapon);
            await EquipBestForMonster(Utils.Details[Name].Artifact2Slot, ItemSlot.Artifact2, monster, bankItems, weapon);
            await EquipBestForMonster(Utils.Details[Name].Artifact3Slot, ItemSlot.Artifact3, monster, bankItems, weapon);
            await EquipBestForMonster(Utils.Details[Name].Utility1Slot, ItemSlot.Utility1, monster, bankItems, weapon);
            await EquipBestForMonster(Utils.Details[Name].Utility2Slot, ItemSlot.Utility2, monster, bankItems, weapon);
            await EquipBestForMonster(Utils.Details[Name].BagSlot, ItemSlot.Bag, monster, bankItems, weapon);
            await EquipBestForMonster(Utils.Details[Name].RuneSlot, ItemSlot.Rune, monster, bankItems, weapon);

            if (await GetFood())
            {
                await GearUpMonster(monster.Code);
            }
        }

        internal int GetSkillLevel(string skill)
        {
            switch (skill.ToLower())
            {
                case "weaponcrafting":
                    return Utils.Details[Name].WeaponcraftingLevel;
                case "gearcrafting":
                    return Utils.Details[Name].GearcraftingLevel;
                case "jewelrycrafting":
                    return Utils.Details[Name].JewelrycraftingLevel;
                case "cooking":
                    return Utils.Details[Name].CookingLevel;
                case "alchemy":
                    return Utils.Details[Name].AlchemyLevel;
                case "woodcutting":
                    return Utils.Details[Name].WoodcuttingLevel;
                case "mining":
                    return Utils.Details[Name].MiningLevel;
                case "fishing":
                    return Utils.Details[Name].FishingLevel;
                default:
                    throw new Exception($"Unexpected skill {skill}");
            }
        }

        private async Task<bool> GetFood()
        {
            const int Threshold = 50;

            var found = CountFoodInInventory();

            if (found < Threshold)
            {
                found += await GetFoodFromBank(Threshold - found);
            }

            if (found < Threshold)
            {
                Console.WriteLine("Not enough food left, now we are the chef");
                await ChefRun(found);
                return true;
            }

            return false;
        }

        private int CountFoodInInventory()
        {
            var total = 0;
            foreach(var inventoryItem in Utils.Details[Name].Inventory)
            {
                if (inventoryItem.Quantity == 0) continue;

                var item = Items.Instance.GetItem(inventoryItem.Code);
                if (item.Type == "consumable")
                {
                    total += inventoryItem.Quantity;
                }
            }

            return total;
        }

        internal async Task<int> GetFoodFromBank(int quantity)
        {
            // Save room, just in case of inventory issues
            var space = GetFreeInventorySpace() / 2;
            if (space <= 0)
            {
                Console.WriteLine("No room for food!");
                return 0;
            }

            var found = 0;
            var bankItems = await Bank.Instance.GetItems();
            foreach (var bankItem in bankItems)
            {
                if (bankItem.Quantity > 0)
                {
                    var item = Items.Instance.GetItem(bankItem.Code);
                    if (item.Type == "consumable" && item.Level <= Utils.Details[Name].Level)
                    {
                        var amount = Math.Min(space, bankItem.Quantity);

                        // Don't be greedy
                        amount = Math.Min(amount, 50);
                        amount = Math.Min(quantity, amount);

                        if (amount <= 0)
                        {
                            return found;
                        }

                        await MoveTo(MapContentType.Bank);

                        var withdrawn = await WithdrawItems(item.Code, amount);
                        if (withdrawn > 0)
                        {
                            found += withdrawn;
                        }

                        space -= withdrawn;
                        quantity -= withdrawn;

                        if (space <= 0)
                        {
                            return found;
                        }
                    }
                }
            }

            return found;
        }

        internal async Task ChefRun(int inventoryAmount)
        {
            const int Threshold = 50;

            // We already have enough
            if (inventoryAmount >= Threshold) return;

            // Cook everything in the bank
            var bankAmount = await CookBankFood();
            if (bankAmount > 0)
            {
                // Get whatever we need or can from the bank
                var amount = Math.Min(bankAmount, Threshold - inventoryAmount);
                bankAmount = await GetFoodFromBank(amount);
            }

            if (inventoryAmount + bankAmount < Threshold)
            {
                Console.WriteLine($"{inventoryAmount + bankAmount}/{Threshold} cooked, lets get fish to cook");
                var recipes = GetRecipes("cooking");
                var topDown = recipes.OrderByDescending(x => x.Level);
                foreach (var recipe in topDown)
                {
                    if (recipe.Craft.Items.Count > 1) continue;

                    var component = recipe.Craft.Items.First();
                    if (component.Quantity > 1) continue;

                    var item = Items.Instance.GetItem(component.Code);
                    if (item.Subtype == "fishing")
                    {
                        var gatherAmount = Threshold - inventoryAmount + bankAmount;
                        Console.WriteLine($"Chef going for {item.Code}");
                        if (await GatherItems(recipe.Code, gatherAmount) > 0)
                        {
                            return;
                        }
                    }
                }
            }
        }

        private IEnumerable<ItemSchema> GetRecipes(string skill)
        {
            var items = Items.Instance.GetAllItems();

            var craftSkill = Utils.GetSkillCraft(skill);
            var skillLevel = GetSkillLevel(skill);

            var recipes = items.Values.Where(i => i.Craft != null &&
                i.Craft.Skill == craftSkill &&
                i.Craft.Level <= skillLevel);

            return recipes;
        }

        internal MapSchema GetClosest(List<MapSchema> data)
        {
            if (data == null || data.Count == 0)
            {
                throw new ArgumentException("No map data");
            }

            MapSchema closest = null;
            double minDistance = double.MaxValue;

            foreach (var map in data)
            {
                var currentDistance = Utils.CalculateManhattanDistance(Utils.Details[Name].X, Utils.Details[Name].Y, map.X, map.Y);
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    closest = map;
                }
            }

            return closest;
        }


        private async Task<int> CookBankFood()
        {
            List<SimpleItemSchema> bankItems = await Bank.Instance.GetItems();

            // See if we can cook something in the bank
            var recipes = GetRecipes("cooking").OrderByDescending(x => x.Level);

            foreach (var recipe in recipes)
            {
                var quantity = int.MaxValue;
                foreach(var component in recipe.Craft.Items)
                {
                    var bankItem = bankItems.FirstOrDefault(x => x.Code == component.Code);
                    if (bankItem != null)
                    {
                        quantity = Math.Min(quantity, bankItem.Quantity / component.Quantity);
                    }
                    else
                    {
                        quantity = 0;
                        break;
                    }
                }

                if (quantity != 0 && quantity != int.MaxValue)
                {
                    var batch = await CraftItems(recipe, quantity, bankOnly: true);
                    Console.WriteLine($"Chef Cooked {batch} {recipe.Code}");
                    if (batch > 0)
                    {
                        await MoveTo(MapContentType.Bank);
                        await DepositItem(recipe.Code, batch);

                        // Cook more if we can
                        return batch + await CookBankFood();
                    }
                }
            }

            return 0;
        }

        private async Task<ItemSchema> EquipBestForMonster(string currentEquipped, ItemSlot slotType, MonsterSchema monster, List<SimpleItemSchema> bankItems, ItemSchema weapon)
        {
            ItemSchema currentItem = null;
            var currentValue = 0.0;

            if (!string.IsNullOrEmpty(currentEquipped))
            {
                currentItem = Items.Instance.GetItem(currentEquipped);
                currentValue = Items.Instance.CalculateItemValue(currentItem, monster, weapon, Utils.Details[Name].Level);
            }

            var bestInventoryItem = await GetBestItemFromInventory(slotType, monster, weapon);
            var inventoryValue = bestInventoryItem != null ? Items.Instance.CalculateItemValue(bestInventoryItem, monster, weapon, Utils.Details[Name].Level) : 0;

            var bestBankItem = await GetBestItemFromBank(slotType, monster, bankItems, weapon);
            var bankValue = bestBankItem != null ? Items.Instance.CalculateItemValue(bestBankItem, monster, weapon, Utils.Details[Name].Level) : 0;

            var max = Math.Max(currentValue, Math.Max(bankValue, inventoryValue));
            Console.WriteLine($"Equipped {currentValue}, inventory {inventoryValue}, bank {bankValue}");

            if (max == currentValue)
            {
                if ((slotType == ItemSlot.Utility1 || slotType == ItemSlot.Utility2) && 
                    currentItem == null && monster.Type == MonsterType.Boss)
                {
                    ItemSchema potion = ChoosePotion(monster, slotType, weapon);
                    var gathered = await GatherItems(potion.Code, 25);
                    if (gathered > 0)
                    {
                        await Equip(potion.Code, slotType, gathered);
                    }
                }

                // Special case for utilities. If the value is 0 unequip it
                if (slotType == ItemSlot.Utility1 || slotType == ItemSlot.Utility2)
                {
                    if (currentItem != null && currentValue == 0.0)
                    {
                        var quantity = 0;
                        if (slotType == ItemSlot.Utility1)
                        {
                            quantity = Utils.Details[Name].Utility1SlotQuantity;
                        }
                        else if (slotType == ItemSlot.Utility2)
                        {
                            quantity = Utils.Details[Name].Utility2SlotQuantity;
                        }

                        var response = await Unequip(slotType);
                        await DepositItem(response.Data.Item.Code, quantity);
                    }
                }

                // Nothing to change
                Console.WriteLine($"Keep equipped {currentItem?.Code}");
                return currentItem;
            }

            if (max == inventoryValue)
            {
                Console.WriteLine($"Inventory item {bestInventoryItem.Code} is highest value at {inventoryValue}");
                if (currentItem != null)
                {
                    await Unequip(slotType);
                }

                await Equip(bestInventoryItem.Code, slotType);
                return bestInventoryItem;
            }
            else
            {
                Console.WriteLine($"Bank item {bestBankItem.Code} is highest value at {bankValue}");

                var quantity = 1;

                // Special case for utility slots, take 25 max
                if (slotType == ItemSlot.Utility1 || slotType == ItemSlot.Utility2)
                {
                    var bankItem = bankItems.First(x => x.Code == bestBankItem.Code);
                    quantity = Math.Min(25, bankItem.Quantity);
                }

                await MoveTo(MapContentType.Bank);
                var withdrawn = await WithdrawItems(bestBankItem.Code, quantity);
                if (withdrawn > 0)
                {
                    if (currentItem != null)
                        await Unequip(slotType);

                    await Equip(bestBankItem.Code, slotType, withdrawn);
                }

                return bestBankItem;
            }
        }

        private bool MeetsConditions(List<ConditionSchema> conditions)
        {
            if (conditions == null || conditions.Count == 0)
            {
                return true;
            }

            foreach (ConditionSchema condition in conditions)
            {
                if (condition.Code == "level")
                {
                    if (condition.Operator == ConditionOperator.Gt)
                    {
                        if (Utils.Details[Name].Level <= condition.Value)
                        {
                            return false;
                        }
                    }
                    else if (condition.Operator == ConditionOperator.Eq)
                    {
                        if (Utils.Details[Name].Level != condition.Value)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        internal ItemSchema ChoosePotion(MonsterSchema monster, ItemSlot slotType, ItemSchema weapon)
        {
            var potions = GetRecipes("alchemy").Where(x => x.Level < Utils.Details[Name].Level);

            if (slotType == ItemSlot.Utility1)
            {
                // Put healing potions in slot 1
                var potionChoices = potions.Where(x => x.Effects.Any(y => y.Code == "restore"));
                var usablePotions = potionChoices.OrderByDescending(x => x.Level);
                return usablePotions.First();
            }
            else if (slotType == ItemSlot.Utility2)
            {
                // Put damage or poison potions in slot 2
                if (monster.Effects.Any(x => x.Code == "poison"))
                {
                    var potionChoices = potions.Where(x => x.Effects.Any(y => y.Code == "antipoison"));
                    var usablePotions = potionChoices.OrderByDescending(x => x.Level);
                    return usablePotions.First();
                }
                else
                {
                    var max = 0.0;
                    ItemSchema potion = null;
                    foreach (var potionElement in potions)
                    {
                        var value = potionElement.Effects.Sum(x => Items.AddBoost(weapon, monster, 10, x));
                        if (value > max)
                        {
                            potion = potionElement;
                            max = value;
                        }
                    }

                    return potion;
                }
            }

            throw new ArgumentException($"Cannot choose potion for slot {slotType}");
        }

        private async Task<ItemSchema> GetBestItemFromBank(ItemSlot slotType, MonsterSchema monster, List<SimpleItemSchema> bankItems, ItemSchema weapon)
        {
            ItemSchema bestItem = null;
            foreach (var bankItem in bankItems)
            {
                if (MatchesUtilitySlot(slotType, bankItem.Code))
                {
                    continue;
                }

                var item = Items.Instance.GetItem(bankItem.Code);
                if (!MeetsConditions(item.Conditions))
                {
                    continue;
                }

                if (Items.Instance.ItemTypeMatchesSlot(item.Type, slotType))
                {
                    if (bestItem == null)
                    {
                        bestItem = item;
                    }
                    else if (Items.Instance.IsBetterItem(bestItem, item, monster, weapon, Utils.Details[Name].Level))
                    {
                        bestItem = item;
                    }
                }
            }

            return bestItem;
        }

        private async Task<ItemSchema> GetBestItemFromInventory(ItemSlot slotType, MonsterSchema monster, ItemSchema weapon)
        {
            ItemSchema bestItem = null;
            foreach (var inventoryItem in Utils.Details[Name].Inventory)
            {
                if (string.IsNullOrEmpty(inventoryItem.Code))
                {
                    continue;
                }

                if (MatchesUtilitySlot(slotType, inventoryItem.Code))
                {
                    // Special case: we cannot have the same item in both utility slots
                    continue;
                }

                var item = Items.Instance.GetItem(inventoryItem.Code);
                if (item.Level > Utils.Details[Name].Level)
                {
                    // Too high level for us
                    continue;
                }

                if (!MeetsConditions(item.Conditions))
                {
                    continue;
                }

                if (Items.Instance.ItemTypeMatchesSlot(item.Type, slotType))
                {
                    if (bestItem == null)
                    {
                        bestItem = item;
                    }
                    else if (Items.Instance.IsBetterItem(bestItem, item, monster, weapon, Utils.Details[Name].Level))
                    {
                        bestItem = item;
                    }
                }
            }
            return bestItem;
        }

        private bool MatchesUtilitySlot(ItemSlot slotType, string code)
        {
            // Special case: we cannot have the same item in both utility slots
            if (slotType == ItemSlot.Utility2 && code == Utils.Details[Name].Utility1Slot)
            {
                return true;
            }
            else if (slotType == ItemSlot.Utility1 && code == Utils.Details[Name].Utility2Slot)
            {
                return true;
            }

            // Special case: we cannot have the same item in any artifact slots
            if (slotType == ItemSlot.Artifact1 && 
                (code == Utils.Details[Name].Artifact2Slot|| code == Utils.Details[Name].Artifact3Slot))
            {
                return true;
            }
            else if (slotType == ItemSlot.Artifact2 &&
                (code == Utils.Details[Name].Artifact1Slot || code == Utils.Details[Name].Artifact3Slot))
            {
                // Special case: we cannot have the same item in both utility slots
                return true;
            }
            else if (slotType == ItemSlot.Artifact3 &&
                (code == Utils.Details[Name].Artifact1Slot || code == Utils.Details[Name].Artifact2Slot))
            {
                return true; 
            }

            return false;
        }

        internal async Task<EquipmentResponseSchema> Unequip(ItemSlot slotType)
        {
            return await Utils.ApiCall(async () =>
            {
                try
                {
                    var quantity = 1;
                    if (slotType == ItemSlot.Utility1)
                    {
                        quantity = Utils.Details[Name].Utility1SlotQuantity;
                    }
                    else if (slotType == ItemSlot.Utility2)
                    {
                        quantity = Utils.Details[Name].Utility2SlotQuantity;
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
                    var item = Items.Instance.GetItem(code);
                    foreach (var condition in item.Conditions)
                    {
                        await MeetCondition(condition, item);
                    }
                    return;
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

        internal async Task SellNpc(string code, int quantity)
        {
            try
            {
                Console.WriteLine($"Selling {quantity} {code} to Npc");
                await Utils.ApiCall(async () =>
                {
                    return await _api.ActionNpcSellItemMyNameActionNpcSellPostAsync(Name, new NpcMerchantBuySchema(code, quantity));
                });
            }
            catch (ApiException ex)
            {
                Console.WriteLine(ex.ErrorContent);
            }
        }

        internal async Task<ClaimPendingItemDataSchema> ClaimItems(string id)
        {
            Console.WriteLine($"Claiming pending items");
            var result = await Utils.ApiCall(async () => await _api.ActionClaimPendingItemMyNameActionClaimItemIdPostAsync(Name, id));
            var claim = result as ClaimPendingItemResponseSchema;
            return claim.Data;
        }

        internal async Task DepositPendings()
        {
            var pendingItems = await Bank.Instance.GetPendingItems();
            var claimed = false;
            foreach (var item in pendingItems)
            {
                if (!item.ClaimedAt.HasValue)
                {
                    try
                    {
                        var claimedStuff = await ClaimItems(item.Id);
                        Console.WriteLine($"Claimed {claimedStuff.Item.Gold}gp");
                        foreach (var claimedItem in claimedStuff.Item.Items)
                        {
                            Console.WriteLine($"{claimedItem.Quantity} {claimedItem.Code}");
                        }

                        claimed = true;
                    }
                    catch (ApiException ex)
                    {
                        Console.WriteLine($"Error claiming: {ex.ErrorContent}");
                    }
                }
            }

            if (claimed)
            {
                await MoveTo(MapContentType.Bank);
                await DepositAllItems();
            }
        }
    }
}
