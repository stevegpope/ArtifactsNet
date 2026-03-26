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
        private DateTime LastDeposit = DateTime.MinValue;
        private CurrentCraftManager _craftManager;

        internal Character(
            Configuration config,
            HttpClient httpClient,
            string name
            )
        {
            _api = new MyCharactersApi(httpClient, config);
            Name = name;
            _craftManager = new CurrentCraftManager(Name);
        }

        internal Character(
            MyCharactersApi api,
            string name
            )
        {
            _api = api;
            Name = name;
            _craftManager = new CurrentCraftManager(Name);
        }

        internal async Task Init()
        {
            var response = await _api.GetMyCharactersMyCharactersGetAsync();
            var character = response.Data.First(c => c.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));
            Utils.Details[Name] = character;
        }

        internal async Task Move(int x, int y)
        {
            if (x == Utils.Details[Name].X && y == Utils.Details[Name].Y)
            {
                Console.WriteLine("Already at location");
                return;
            }

            Console.WriteLine($"Move to {x},{y}");
            await Utils.ApiCall(Name, async () =>
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
            var pathfinder = new PathFinder(Utils.Details[Name]);
            var path = await pathfinder.GetPath(locationType, code);
            if (path == null || path.Count == 0)
            {
                throw new Exception($"No locations found for type {locationType} and code {code}");
            }

            while(path.Count() > 1)
            {
                var location = path.First();
                await Move(location.X, location.Y);
                await TransitionToNewLayer();
                path.Remove(location);
            }

            var finalLocation = path.First();
            await Move(finalLocation.X, finalLocation.Y);
        }

        private async Task TransitionToNewLayer()
        {
            await Utils.ApiCall(Name, async () =>
            {
                Console.WriteLine("Transition to new layer");
                return await _api.ActionTransitionMyNameActionTransitionPostAsync(Name);
            });
        }

        internal async Task TurnInItems(string code, int quantity)
        {
            try
            {
                await Utils.ApiCall(Name, async () =>
                {
                    Console.WriteLine($"Turning in {quantity} {code} for character {Name}");
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
                return 0;
            }

            // Gear up for crafting to get more xp
            var skillLevel = GetSkillLevel(item.Craft.Skill.ToString());
            if (item.Level > skillLevel - 10)
            {
                try
                {
                    await GearUpSkill("crafting");
                }
                catch (ApiException ex)
                {
                    if (ex.ErrorCode == 497)
                    {
                        Console.WriteLine("Inventory full, cannot gear up");
                        await DropOffNonComponents(item);
                        await GearUpSkill("crafting");
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Go to the crafting location
            await MoveTo(MapContentType.Workshop, item.Craft.Skill.Value.ToString());

            var quantity = CalculateCraftQuantity(item.Craft, craftQuantity);

            try
            {
                await Utils.ApiCall(Name, async () =>
                {
                    Console.WriteLine($"Craft {quantity} {item.Code}");
                    var response = await _api.ActionCraftingMyNameActionCraftingPostAsync(Name, new CraftingSchema(item.Code, quantity));
                    var xp = GetSkillXp(item.Craft.Skill.ToString());
                    var maxXp = GetSkillMaxXp(item.Craft.Skill.ToString());
                    Console.WriteLine($"{Name} {item.Craft.Skill.ToString()} {response.Data.Details.Xp} Xp: {xp}/{maxXp}");
                    Console.WriteLine($"{Name} crafted {quantity} {item.Code}");
                    return response;
                });
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"Error crafting {item.Code}: {ex.ErrorContent}");

                if (ex.ErrorCode == 478)
                {
                    Console.WriteLine($"Ran out of components creating items");
                    item.PrintCraftComponents();
                }
            }
            
            return quantity;
        }

        private int CalculateCraftQuantity(CraftSchema craft, int craftQuantity)
        {
            // Calculate how many we can make with the current inventory
            var max = 0;
            foreach(var component in craft.Items)
            {
                var owned = Utils.Details[Name].Inventory.Where(i => i.Code == component.Code).Sum(i => i.Quantity);
                max = Math.Max(max, owned / component.Quantity);
            }

            return Math.Min(max, craftQuantity);
        }

        private async Task<int> FetchCraftingItems(ItemSchema item, Dictionary<string, int> gatherQuantities, bool bankOnly, bool ignoreBank)
        {
            // Get the items required for crafting
            var index = 0;
            var gathered = 0;
            foreach (var component in item.Craft.Items)
            {
                gathered = 0;

                // Do not drop off non-components for sub-items. We need to keep them in inventory to craft the main item
                if (index > 0 && item.Code == _craftManager.GetCurrentCraft()?.Code)
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

        private static void AddComponents(ItemSchema item, ref HashSet<string> components)
        {
            components.Add(item.Code);

            if (item.Craft != null)
            {
                foreach (var craftItem in item.Craft.Items)
                {
                    var craft = Items.GetItem(craftItem.Code);
                    AddComponents(craft, ref components);
                }
            }
        }

        internal async Task PerformTask()
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

            if (Utils.Details[Name].TaskType == "items")
            {
                await DoItemTask();
            }
            else
            {
                await DoMonstersTask();
            }

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

        private async Task DoMonstersTask()
        {
            await HandleMonstersTask();
            await MoveTo(MapContentType.TasksMaster, "monsters");
            await FinishTask();
        }

        private async Task FinishTask()
        {
            List<SimpleItemSchema> rewards = new List<SimpleItemSchema>();

            await Utils.ApiCall(Name, async () =>
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

            if (rewards.Count != 0)
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

                    var item = Items.GetItem(Utils.Details[Name].Task);
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
            var code = "items";
            await MoveTo(MapContentType.TasksMaster, code: code);

            Console.WriteLine($"Getting new task");
            await Utils.ApiCall(Name, async () =>
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
                    var amount = await WithdrawItems("tasks_coin");
                    if (amount >= 6)
                    {
                        Console.WriteLine($"Got {amount} coins, going to exchange");
                        await MoveTo(MapContentType.TasksMaster);

                        while (amount >= 6)
                        {
                            Console.WriteLine($"Exchange task coins");
                            await Utils.ApiCall(Name, async () =>
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

        internal async Task<int> GatherItems(string code, int total, bool ignoreBank = false, bool bankOnly = false)
        {
            Console.WriteLine($"Gather {total} {code}");
            var withdrawn = 0;

            if (!ignoreBank)
            {
                // Check the bank first
                withdrawn = await GatherFromBank(code, total);
                if (withdrawn < total)
                {
                    Console.WriteLine($"Did not get enough from the bank, still need {total - withdrawn} {code}");
                }
                else
                {
                    Console.WriteLine($"Got enough from the bank");
                    return withdrawn;
                }
            }

            if (code == "gold")
            {
                // No other way to get gold
                Console.WriteLine($"Only got {withdrawn} gold");
                return withdrawn;
            }

            if (bankOnly)
            {
                Console.WriteLine($"Bank only, got {withdrawn} {code}");
                return withdrawn;
            }

            var remaining = total - withdrawn;

            var item = Items.GetItem(code);
            if (item.Craft != null)
            {
                Console.WriteLine($"Need to craft {code}");
                var crafted = await CraftItems(item, remaining, bankOnly, ignoreBank);
                return crafted + withdrawn;
            }

            var resource = await Resources.Instance.GetResourceDrop(code);
            if (resource != null)
            {
                var gathered = await GatherResources(remaining, item, resource);
                return gathered + withdrawn;
            }

            var monsters = Monsters.Instance.GetMonsters(maxLevel: 100, dropCode: item.Code);
            if (monsters?.Count != 0)
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
            if (orders.Count == 0)
            {
                return 0;
            }

            Console.WriteLine($"There are orders for {code}, going to exchange");

            var cheapest = orders.OrderBy(x => x.Price);
            var gold = await FetchGoldForOrders(total, cheapest);
            if (gold == 0)
            {
                return 0;
            }

            if (await WithdrawGold(gold) != gold)
            {
                Console.WriteLine($"Not enough gold in the bank!");
                return 0;
            }

            await MoveTo(MapContentType.GrandExchange);

            // Orders changed?
            orders = await Bank.Instance.GetExchangeOrders(code);
            if (orders.Count == 0)
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
                    await BuyExchangeOrder(order.Id, amount);

                    gotten += amount;
                    remaining -= amount;
                }
                catch (ApiException ex)
                {
                    Console.WriteLine($"GE transaction error: {ex.ErrorContent}");
                    return gotten;
                }
            }

            return gotten;
        }

        private static async Task<int> FetchGoldForOrders(int total, IOrderedEnumerable<GEOrderSchema> cheapest)
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
            return await Utils.ApiCall(Name, async () =>
            {
                return await _api.ActionGeBuyItemMyNameActionGrandexchangeBuyPostAsync(Name, new GEBuyOrderSchema(orderId, quantity));
            }) as GETransactionResponseSchema;
        }

        private async Task<int> GatherFromNpc(string code, int total)
        {
            var npcs = Npcs.GetAllNpcs();
            var npc = npcs.FirstOrDefault(n => n.Value.Items.Any(i => i.Code == code)).Value;
            var item = npc?.Items.FirstOrDefault(i => i.Code == code);
            if (item != null && item?.BuyPrice > 0)
            {
                if (npc.Code == "tasks_trader") return 0;

                Console.WriteLine($"Try to buy {total} {code} from NPC");
                var maps = await Map.Instance.GetMapLayer(MapContentType.Npc, npc.Code);
                while (maps.Count != 0)
                {
                    var map = maps.First();
                    maps.Remove(map);

                    if (map.Layer == MapLayer.Overworld)
                    {
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

                        try
                        {
                            Console.WriteLine($"Move to NPC {npc.Code}");
                            await Move(map.X, map.Y);
                        }
                        catch (ApiException ex)
                        {
                            Console.WriteLine($"Cannot buy {code} from {npc.Code}: {ex.ErrorContent}");
                            continue;
                        }

                        return await BuyNpcItem(code, total);
                    }
                }
            }

            return 0;
        }

        internal async Task<int> BuyNpcItem(string code, int amountToPurchase)
        {
            var amount = amountToPurchase;
            var bought = 0;

            while (amount > 0)
            {
                // Max 100 purchase, we will use batches
                var quantity = Math.Min(amount, 100);

                try
                {
                    await Utils.ApiCall(Name, async () =>
                    {
                        Console.WriteLine($"Buy {amountToPurchase} {code} from NPC");
                        return await _api.ActionNpcBuyItemMyNameActionNpcBuyPostAsync(Name, new NpcMerchantBuySchema(code, quantity));
                    });

                    bought += quantity;
                    amount -= quantity;
                }
                catch(ApiException ex)
                {
                    Console.WriteLine($"Error buying {quantity} {code} from NPC: {ex.ErrorContent}");
                    break;
                }
            }

            return bought;
        }

        private async Task<int> GatherResources(int remaining, ItemSchema item, ResourceSchema resource)
        {
            Console.WriteLine($"Gathering {remaining} {item.Code} for character {Name}");
            var skill = await Resources.Instance.GetResourceSkill(item);

            try
            { 
                await GearUpSkill(skill);
            }
            catch (ApiException ex)
            {
                if (ex.ErrorCode == 497)
                {
                    Console.WriteLine("Inventory full, cannot gear up");
                    await DropOffNonComponents(item);
                    await GearUpSkill("crafting");
                }
                else
                {
                    throw;
                }
            }

            await MoveTo(MapContentType.Resource, resource.Code);

            var gathered = 0;
            var leftToGet = remaining - gathered;
            while (leftToGet > 0)
            {
                var estimatedTime = new TimeSpan(hours: 0, minutes: 0, seconds: (int)(leftToGet * Utils.LastCooldown(Name)));
                Console.WriteLine($"{gathered}/{remaining} {item.Code} ETA: {estimatedTime}");

                try
                {
                    var levelBefore = GetSkillLevel(skill);
                    var result = await Utils.ApiCall(Name, async () =>
                    {
                        return await _api.ActionGatheringMyNameActionGatheringPostAsync(Name);
                    });

                    if (result == null) continue;

                    var schema = result as SkillResponseSchema;
                    var xp = GetSkillXp(skill);
                    var maxXp = GetSkillMaxXp(skill);
                    Console.WriteLine($"{Name} {skill} {schema.Data.Details.Xp} Xp: {xp}/{maxXp}");
                    foreach (var drop in schema.Data.Details.Items)
                    {
                        Console.WriteLine($"drop: {drop.Quantity} {drop.Code}");
                        if (drop.Code == item.Code)
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
                        Console.WriteLine($"Not skilled enough to get {item.Code}, going training");
                        await TrainSkill(skill, item.Level);

                        Console.WriteLine($"Back from training, gathering {leftToGet} {item.Code}");
                        return await GatherResources(remaining, item, resource);
                    }
                    else if (ex.ErrorCode == 598)
                    {
                        Console.WriteLine($"Resource is no longer present. We're done");
                        return gathered;
                    }

                    throw;
                }
            }

            return gathered;
        }

        private async Task<int> GatherFromMonsters(string code, int remaining, List<MonsterSchema> monsters)
        {
            var monster = monsters.MinBy(x => x.Level);
            Console.WriteLine($"Need to fight for {code}, chasing {monster}");

            if (monster.Type == MonsterType.Boss)
            {
                Console.WriteLine($"We can't beat {monster.Code}");
                return 0;
            }

            if (monster.Level >= Utils.Details[Name].Level)
            {
                Console.WriteLine($"We can't beat {monster.Code}, training");
                await TrainFighting(monster.Level + 1);
                await GearUpMonster(monster.Code);
            }

            var drops = await FightDrops(monster.Code, code, remaining);
            return drops;
        }

        internal async Task TrainFighting(int level)
        {
            var maxLevel = Math.Max(1, Utils.Details[Name].Level - 3);
            var monsters = Monsters.Instance.GetMonsters(maxLevel).Where(x => x.Type != MonsterType.Boss);
            var fightList = monsters.OrderBy(x => x.Level).ToList();
            var lastXp = 0;

            while (Utils.Details[Name].Level < level)
            {
                var monster = fightList.Last();
                Console.WriteLine($"Train fighting with {monster.Code} from level {Utils.Details[Name].Level}/{level}");
                var maps = await Map.Instance.GetMapLayer(MapContentType.Monster, monster.Code);
                if (maps == null || !maps.Any())
                {
                    fightList.Remove(monster);
                    continue;
                }

                await GearUpMonster(monster.Code);

                var losses = 0;
                var fights = 0;
                while (Utils.Details[Name].Level < level)
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
                        Console.WriteLine($"Training run {fights++ + 1}");

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
                                monsters = Monsters.Instance.GetMonsters(Utils.Details[Name].Level - 1).Where(x => x.Type != MonsterType.Boss);
                                fightList = monsters.Where(x => x.Level > Utils.Details[Name].Level - 10).OrderBy(x => x.Level).ToList();
                                break;
                            }
                        }
                    }
                    catch (ApiException ex)
                    {
                        if (ex.ErrorCode == 497)
                        {
                            Console.WriteLine("Inventory full, cannot fight");
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
            if (code == "gold")
            {
                var details = await Bank.Instance.GetBankDetails();
                var gold = details.Gold;
                if (gold >= total)
                {
                    Console.WriteLine($"Withdraw {total} gold");
                    return await WithdrawGold(total);
                }
                else
                {
                    Console.WriteLine($"Not enough gold in the bank, only {gold} available");
                    return 0;
                }
            }

            var bankItems = await Bank.Instance.GetItems();
            var bankItemAmount = bankItems.Where(x => x.Code == code).Sum(x => x.Quantity);
            if (bankItemAmount > 0)
            {
                Console.WriteLine($"{bankItemAmount} {code} in bank, get {total} of them");
                var quantity = Math.Min(total, bankItemAmount);
                return await WithdrawItems(code, quantity);
            }

            return 0;
        }

        private async Task TrainSkill(string skill, int levelGoal)
        {
            var currentLevel = GetSkillLevel(skill);
            Console.WriteLine($"Train {skill} from {currentLevel} to {levelGoal}");

            while (currentLevel < levelGoal)
            {
                await TrainGathering(currentLevel, skill);
                currentLevel = GetSkillLevel(skill);
                Console.WriteLine($"Training at {currentLevel}/{levelGoal}");
            }
        }

        internal async Task TrainGathering(int level, string skill)
        {
            Console.WriteLine($"Train {skill} at level {level}");
            var items = Items.GetAllItems();
            var gatherItems = items.Where(i => i.Value.Type == "resource" && i.Value.Subtype == skill);

            int total = 0;
            for (int targetLevel = level; targetLevel >= 1; targetLevel--)
            {
                var itemsAtLevel = gatherItems.Where(x => x.Value.Level == targetLevel).Select(x => x.Value);
                Console.WriteLine($"{itemsAtLevel.Count()} items at level {targetLevel}");
                var itemsList = new List<ItemSchema>(itemsAtLevel);
                while (itemsList.Count != 0)
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

                    total = await GatherItems(item, 50, ignoreBank: true, bankOnly: false);
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
            await DepositAllItems();
        }

        private async Task<int> FightDrops(string monster, string code, int remaining)
        {
            await GearUpMonster(monster);
            var monsterSchema = Monsters.GetMonster(monster);

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
                        try
                        {
                            await GearUpMonster(monster);
                        }
                        catch (ApiException ex)
                        {
                            if (ex.ErrorCode == 497)
                            {
                                Console.WriteLine("Inventory full, cannot fight");
                                await DepositExcept(new List<string>([code]));
                                await GearUpMonster(monster);
                            }
                        }
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

                try
                {
                    var hp = Utils.Details[Name].Hp;
                    var result = await Fight();
                    if (result == null) continue;

                    lostLastFight = hp - Utils.Details[Name].Hp;
                    if (result.Data.Fight.Result == FightResult.Loss)
                    {
                        const int Limit = 3;
                        losses++;
                        Console.WriteLine($"loss {losses}/{Limit}");
                        if (losses >= Limit)
                        {
                            Console.WriteLine($"We Lost! Training for monster {monster}");
                            losses = 0;
                            await TrainFighting(Utils.Details[Name].Level + 1);
                            await GearUpMonster(monster);
                            continue;
                        }
                    }
                    else
                    {
                        // Reset losses, we can beat him!
                        losses = 0;
                    }

                    foreach (var character in result.Data.Fight.Characters)
                    {
                        if (character.Drops != null && character.Drops.Count != 0)
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
                        await DepositExcept(new List<string>([code]));
                        break;
                    }

                    Console.WriteLine($"Fight error: {ex.ErrorContent}");
                    throw;
                }
            }

            return drops;
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

            await DepositExcept(new List<string>());
        }

        internal async Task DepositItem(string code, int quantity)
        {
            await DepositItems(new List<SimpleItemSchema>() { new SimpleItemSchema(code, quantity) });
        }

        internal async Task DepositItems(List<SimpleItemSchema> items)
        { 
            try
            {
                await MoveTo(MapContentType.Bank);

                Console.WriteLine($"Deposit:");
                foreach (var item in items)
                {
                    Console.WriteLine($"{item.Quantity} {item.Code}");
                }

                await Utils.ApiCall(Name, async () =>
                {
                    return await _api.ActionDepositBankItemMyNameActionBankDepositItemPostAsync(Name, items);
                });
            }
            catch (ApiException ex)
            {
                Console.WriteLine(ex.ErrorContent);

                if (ex.ErrorCode == 461)
                {
                    // Already in transaction? Try again
                    Console.WriteLine($"Item already in transaction");
                    await DepositItems(items);
                }
                else if (ex.ErrorCode == 462)
                {
                    Console.WriteLine("Bank is full, trying to expand");
                    await ExpandBank();
                    await DepositItems(items);
                    return;
                }
                else if (ex.ErrorCode == 478)
                {
                    Console.WriteLine("Do not have items, must have worked already");
                    return;
                }

                throw;
            }
        }

        private async Task ExpandBank()
        {
            var bankDetails = await Bank.Instance.GetBankDetails();
            var withdrawn = await WithdrawGold(bankDetails.NextExpansionCost);

            if (withdrawn == bankDetails.NextExpansionCost)
            {
                Console.WriteLine($"{Name} buying bank expansion for {bankDetails.NextExpansionCost}");
                await Utils.ApiCall(Name, async () =>
                {
                    return await _api.ActionBuyBankExpansionMyNameActionBankBuyExpansionPostAsync(Name);
                });
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
                    items.Add(new SimpleItemSchema(item.Code, item.Quantity));
                }
            }

            if (items.Count == 0)
            {
                Console.WriteLine($"No items to deposit for character {Name}");
                return;
            }

            await DepositItems(items);
        }

        internal async Task DepositGold()
        {
            if (LastDeposit != DateTime.MinValue)
            {
                const int hours = 1;
                if (DateTime.UtcNow < LastDeposit + TimeSpan.FromHours(hours))
                {
                    Console.WriteLine($"Skip deposit gold until {LastDeposit + TimeSpan.FromHours(hours)}");
                    return;
                }
            }

            if (Utils.Details[Name].Gold <= 0)
            {
                return;
            }

            LastDeposit = DateTime.UtcNow;

            await MoveTo(MapContentType.Bank);

            try
            {
                await Utils.ApiCall(Name, async () =>
                {
                    Console.WriteLine($"Depositing {Utils.Details[Name].Gold} gold for character {Name}");
                    var response = await _api.ActionDepositBankGoldMyNameActionBankDepositGoldPostAsync(Name, new DepositWithdrawGoldSchema(quantity: Utils.Details[Name].Gold));
                    return response;
                });
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"Error trying to deposit gold {ex.ErrorContent}");
            }
        }

        internal async Task<int> WithdrawGold(int quantity)
        {
            await MoveTo(MapContentType.Bank);

            try
            {
                await Utils.ApiCall(Name, async () =>
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
            await MoveTo(MapContentType.Bank);

            var inventorySpace = GetFreeInventorySpace();
            Console.WriteLine($"Customer wants to withdraw {quantity} {code}. Free space {inventorySpace}");
            if (inventorySpace == 0)
            {
                throw new Exception($"Not allowed to withdraw when we have no space");
            }

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
                await Utils.ApiCall(Name, async () =>
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
            var losses = 0;

            var monsterSchema = Monsters.GetMonster(monster);
            await TrainFighting(monsterSchema.Level + 1);
            await GearUpMonster(monster);

            while(monstersKilled < total)
            {
                // If we are healthy enough fight right away
                var needsRest = Utils.Details[Name].Hp < Utils.Details[Name].MaxXp * .75;
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

                    var hp = Utils.Details[Name].Hp;
                    var result = await Fight();
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
                            Console.WriteLine($"Training up to level {Utils.Details[Name].Level + 1}");
                            await TrainFighting(Utils.Details[Name].Level + 1);
                            await GearUpMonster(monster);
                            continue;
                        }
                    }
                }
                catch (ApiException ex)
                {
                    Console.WriteLine($"Fight error: {ex.ErrorContent}");
                    if (ex.ErrorCode == 497)
                    {
                        Console.WriteLine("Inventory full, be right back");
                        await DepositAllItems();
                        continue;
                    }

                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in fight loop: {ex.Message}");
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

                var item = Items.GetItem(inventoryItem.Code);
                var heal = item?.Effects.Where(x => x.Code == "heal").Sum(x => x.Value);
                if (item.Type == "consumable" && heal > 0)
                {
                    var idealQuantity = 0;
                    var currentAmount = 0;
                    while (currentAmount < amountToHeal)
                    {
                        idealQuantity++;
                        currentAmount += heal.Value;
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
                await Utils.ApiCall(Name, async () =>
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
                var item = Items.GetItem(simpleItem.Code);
                if (item.Type == "consumable" && item.Subtype == "bag")
                {
                    await Consume(item.Code, simpleItem.Quantity);
                }
            }
        }

        internal async Task<CharacterFightResponseSchema> Fight(List<string> participants = null)
        {
            Console.WriteLine("Fight!!");

            var result = await Utils.ApiCallGet(async () =>
            {
                var response = await _api.ActionFightMyNameActionFightPostAsync(Name, new FightRequestSchema(participants));
                Console.WriteLine($"Fight {response.Data.Fight.Result}");
                foreach (var characterResponse in response.Data.Fight.Characters)
                {
                    Console.WriteLine($"{characterResponse.Xp} XP: {Utils.Details[Name].Xp}/{Utils.Details[Name].MaxXp}");
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

        internal async Task GearUpSkill(string skill)
        {
            Console.WriteLine($"Gear up for {skill}");

            if (skill == null)
            {
                return;
            }

            var bankItems = await Bank.Instance.GetItems();
            await EquipBestForSkill(Utils.Details[Name].WeaponSlot, ItemSlot.Weapon, skill, bankItems);
            await EquipBestForSkill(Utils.Details[Name].ShieldSlot, ItemSlot.Shield, skill, bankItems);
            await EquipBestForSkill(Utils.Details[Name].HelmetSlot, ItemSlot.Helmet, skill, bankItems);
            await EquipBestForSkill(Utils.Details[Name].BodyArmorSlot, ItemSlot.BodyArmor, skill, bankItems);
            await EquipBestForSkill(Utils.Details[Name].LegArmorSlot, ItemSlot.LegArmor, skill, bankItems);
            await EquipBestForSkill(Utils.Details[Name].BootsSlot, ItemSlot.Boots, skill, bankItems);
            await EquipBestForSkill(Utils.Details[Name].Ring1Slot, ItemSlot.Ring1, skill, bankItems);
            await EquipBestForSkill(Utils.Details[Name].Ring2Slot, ItemSlot.Ring2, skill, bankItems);
            await EquipBestForSkill(Utils.Details[Name].AmuletSlot, ItemSlot.Amulet, skill, bankItems);
            await EquipBestForSkill(Utils.Details[Name].Artifact1Slot, ItemSlot.Artifact1, skill, bankItems);
            await EquipBestForSkill(Utils.Details[Name].Artifact2Slot, ItemSlot.Artifact2, skill, bankItems);
            await EquipBestForSkill(Utils.Details[Name].Artifact3Slot, ItemSlot.Artifact3, skill, bankItems);
            await EquipBestForSkill(Utils.Details[Name].BagSlot, ItemSlot.Bag, skill, bankItems);
            await EquipBestForSkill(Utils.Details[Name].RuneSlot, ItemSlot.Rune, skill, bankItems);
        }

        private async Task EquipBestForSkill(string currentEquipped, ItemSlot slotType, string skill, List<SimpleItemSchema> bankItems)
        {
            ItemSchema currentItem = null;
            var currentValue = 0.0;
            if (!string.IsNullOrEmpty(currentEquipped))
            {
                currentItem = Items.GetItem(currentEquipped);
                currentValue = Items.CalculateItemValueSkill(currentItem, skill);
            }

            var bestInventoryItem = GetBestItemFromInventorySkill(slotType, skill);
            var inventoryValue = bestInventoryItem != null ? Items.CalculateItemValueSkill(bestInventoryItem, skill) : 0;

            var bestBankItem = GetBestItemFromBankSkill(slotType, skill, bankItems);
            var bankValue = bestBankItem != null ? Items.CalculateItemValueSkill(bestBankItem, skill) : 0;

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
                    await UnequipAndDeposit(slotType);

                await Equip(bestInventoryItem.Code, slotType);
            }
            else
            {
                Console.WriteLine($"Bank item {bestBankItem.Code} is highest value at {bankValue}");
                // Best item is in the bank
                if (await WithdrawItems(bestBankItem.Code, 1) > 0)
                {
                    if (currentItem != null)
                        await UnequipAndDeposit(slotType);

                    await Equip(bestBankItem.Code, slotType);
                }
                else
                {
                    bankItems = await Bank.Instance.GetItems();
                    await EquipBestForSkill(currentEquipped, slotType, skill, bankItems);
                }
            }
        }

        private ItemSchema GetBestItemFromBankSkill(ItemSlot slotType, string skill, List<SimpleItemSchema> bankItems)
        {
            ItemSchema bestItem = null;
            foreach (var bankItem in bankItems)
            {
                var item = Items.GetItem(bankItem.Code);
                if (!MeetsConditions(item.Conditions))
                {
                    continue;
                }

                if (MatchesUtilitySlot(slotType, bankItem.Code))
                {
                    continue;
                }

                if (Items.ItemTypeMatchesSlot(item.Type, slotType))
                {
                    if (bestItem == null)
                    {
                        bestItem = item;
                    }
                    else if (Items.IsBetterItemSkill(bestItem, item, skill))
                    {
                        bestItem = item;
                    }
                }
            }

            return bestItem;
        }

        private ItemSchema GetBestItemFromInventorySkill(ItemSlot slotType, string skill)
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
                    continue;
                }

                var item = Items.GetItem(inventoryItem.Code);
                if (!MeetsConditions(item.Conditions))
                {
                    continue;
                }

                if (Items.ItemTypeMatchesSlot(item.Type, slotType))
                {
                    if (bestItem == null)
                    {
                        bestItem = item;
                    }
                    else if (Items.IsBetterItemSkill(bestItem, item, skill))
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

            Console.WriteLine($"Gear up for monster {monsterCode}");

            var monster = Monsters.GetMonster(monsterCode);
            var bankItems = await Bank.Instance.GetItems();

            await GetFood();
            if (monster.Effects.Any(effect => effect.Code == "poison"))
            {
                // Do not fight a venomous monster without antidote
                //if (!Utils.Details[Name].Utility2Slot.Contains("antidote") || Utils.Details[Name].Utility2SlotQuantity <= 0)
                //{
                //    var antidote = ChooseAntidote();
                //    var amount = await GatherItems(antidote, 25);
                //    if (amount > 0)
                //    {
                //        await Equip(antidote, ItemSlot.Utility2, 25);
                //    }
                //    else
                //    {
                //        throw new Exception($"Cannot make or gather {antidote}");
                //    }
                //}
            }
           

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
        }

        private string ChooseAntidote()
        {
            var items = Items.GetAllItems();
            var skillLevel = GetSkillLevel("alchemy");

            ItemSchema bestItem = null;
            double bestValue = 0.0;

            foreach(var item in items.Values.Where(x => x.Craft != null && x.Craft.Skill == CraftSkill.Alchemy))
            {
                if (item.Level > Utils.Details[Name].Level || item.Level > skillLevel)
                {
                    continue;
                }

                var effect = item.Effects.FirstOrDefault(x => x.Code == "antipoison");
                if (effect != null && effect.Value > bestValue)
                {
                    bestItem = item;
                    bestValue = effect.Value;
                }
            }

            return bestItem.Code;
        }

        internal int GetSkillLevel(string skill)
        {
            return skill.ToLower() switch
            {
                "weaponcrafting" => Utils.Details[Name].WeaponcraftingLevel,
                "gearcrafting" => Utils.Details[Name].GearcraftingLevel,
                "jewelrycrafting" => Utils.Details[Name].JewelrycraftingLevel,
                "cooking" => Utils.Details[Name].CookingLevel,
                "alchemy" => Utils.Details[Name].AlchemyLevel,
                "woodcutting" => Utils.Details[Name].WoodcuttingLevel,
                "mining" => Utils.Details[Name].MiningLevel,
                "fishing" => Utils.Details[Name].FishingLevel,
                _ => throw new Exception($"Unexpected skill {skill}"),
            };
        }

        internal int GetSkillXp(string skill)
        {
            return skill.ToLower() switch
            {
                "weaponcrafting" => Utils.Details[Name].WeaponcraftingXp,
                "gearcrafting" => Utils.Details[Name].GearcraftingXp,
                "jewelrycrafting" => Utils.Details[Name].JewelrycraftingXp,
                "cooking" => Utils.Details[Name].CookingXp,
                "alchemy" => Utils.Details[Name].AlchemyXp,
                "woodcutting" => Utils.Details[Name].WoodcuttingXp,
                "mining" => Utils.Details[Name].MiningXp,
                "fishing" => Utils.Details[Name].FishingXp,
                _ => throw new Exception($"Unexpected skill {skill}"),
            };
        }

        internal int GetSkillMaxXp(string skill)
        {
            return skill.ToLower() switch
            {
                "weaponcrafting" => Utils.Details[Name].WeaponcraftingMaxXp,
                "gearcrafting" => Utils.Details[Name].GearcraftingMaxXp,
                "jewelrycrafting" => Utils.Details[Name].JewelrycraftingMaxXp,
                "cooking" => Utils.Details[Name].CookingMaxXp,
                "alchemy" => Utils.Details[Name].AlchemyMaxXp,
                "woodcutting" => Utils.Details[Name].WoodcuttingMaxXp,
                "mining" => Utils.Details[Name].MiningMaxXp,
                "fishing" => Utils.Details[Name].FishingMaxXp,
                _ => throw new Exception($"Unexpected skill {skill}"),
            };
        }

        internal async Task<bool> GetFood()
        {
            const int Threshold = 50;

            var found = CountFoodInInventory();

            if (found < Threshold)
            {
                found += await GetFoodFromBank(Threshold - found);
            }

            if (found < Threshold)
            {
                Console.WriteLine($"Not enough food left ({found/Threshold}), now we are the chef");
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

                var item = Items.GetItem(inventoryItem.Code);
                var heal = item?.Effects.Where(x => x.Code == "heal").Sum(x => x.Value);
                if (item.Type == "consumable" && heal > 0)
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
            Console.WriteLine($"{quantity} food wanted, {space} space");
            if (space <= quantity)
            {
                Console.WriteLine("No room for food");
                var craft = _craftManager.GetCurrentCraft();
                if (craft != null)
                {
                    var item = Items.GetItem(craft.Code);
                    await DropOffNonComponents(item);
                    return 0;
                }
                else
                {
                    await DepositAllItems();
                }

                return await GetFoodFromBank(quantity);
            }

            var found = 0;
            var bankItems = await Bank.Instance.GetItems();
            foreach (var bankItem in bankItems)
            {
                if (bankItem.Quantity > 0)
                {
                    var item = Items.GetItem(bankItem.Code);
                    var heal = item?.Effects.Where(x => x.Code == "heal").Sum(x => x.Value);
                    if (item.Type == "consumable" && heal > 0 && item.Level <= Utils.Details[Name].Level)
                    {
                        var amount = Math.Min(space, bankItem.Quantity);

                        // Don't be greedy
                        amount = Math.Min(amount, 50);
                        amount = Math.Min(quantity, amount);

                        if (amount <= 0)
                        {
                            return found;
                        }

                        var withdrawn = await WithdrawItems(item.Code, amount);
                        if (withdrawn > 0)
                        {
                            found += withdrawn;
                        }

                        space -= withdrawn;
                        quantity -= withdrawn;

                        if (space <= 0 || quantity == 0)
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

            // Cook enough
            var remaining = Threshold - inventoryAmount;
            var bankAmount = await CookBankFood(remaining);
            if (bankAmount == remaining)
            {
                return;
            }

            remaining -= bankAmount;

            if (remaining > 0)
            {
                await GatherFishAndCook(remaining);
            }
        }

        internal async Task<string> GatherFishAndCook(int remaining)
        {
            Console.WriteLine($"{remaining} food needed, lets get fish to cook");
            var recipes = GetRecipes("cooking");
            var topDown = recipes.Where(x => x.Level <= Utils.Details[Name].Level).OrderByDescending(x => x.Level);
            foreach (var recipe in topDown)
            {
                if (recipe.Craft.Items.Count > 1) continue;

                var component = recipe.Craft.Items.First();
                if (component.Quantity > 1) continue;

                var item = Items.GetItem(component.Code);
                if (item.Subtype == "fishing")
                {
                    Console.WriteLine($"Chef going for {remaining} {item.Code}");
                    if (await GatherItems(recipe.Code, remaining, ignoreBank: true) > 0)
                    {
                        return recipe.Code;
                    }
                }
            }

            return null;
        }

        internal IEnumerable<ItemSchema> GetRecipes(string skill)
        {
            var items = Items.GetAllItems();

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


        private async Task<int> CookBankFood(int amount)
        {
            if (amount <= 0)
            {
                return 0;
            }

            List<SimpleItemSchema> bankItems = await Bank.Instance.GetItems();

            // See if we can cook something in the bank
            var recipes = GetRecipes("cooking").OrderByDescending(x => x.Level);

            var total = 0;
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
                    quantity = Math.Min(quantity, amount - total);
                    var batch = await CraftItems(recipe, quantity, bankOnly: true);
                    Console.WriteLine($"Chef Cooked {batch} {recipe.Code}");
                    total += batch;
                    if (batch > 0)
                    {
                        await DepositItem(recipe.Code, batch);

                        // Cook more if we can
                        return total + await CookBankFood(amount - total);
                    }
                }
            }

            return total;
        }

        private async Task<ItemSchema> EquipBestForMonster(string currentEquipped, ItemSlot slotType, MonsterSchema monster, List<SimpleItemSchema> bankItems, ItemSchema weapon)
        {
            ItemSchema currentItem = null;
            var currentValue = 0.0;

            if (!string.IsNullOrEmpty(currentEquipped))
            {
                currentItem = Items.GetItem(currentEquipped);
                currentValue = Items.CalculateItemValue(currentItem, monster, weapon, Utils.Details[Name].Level);
            }

            var bestInventoryItem = GetBestItemFromInventory(slotType, monster, weapon);
            var inventoryValue = bestInventoryItem != null ? Items.CalculateItemValue(bestInventoryItem, monster, weapon, Utils.Details[Name].Level) : 0;

            var bestBankItem = GetBestItemFromBank(slotType, monster, bankItems, weapon);
            var bankValue = bestBankItem != null ? Items.CalculateItemValue(bestBankItem, monster, weapon, Utils.Details[Name].Level) : 0;

            var max = Math.Max(currentValue, Math.Max(bankValue, inventoryValue));

            if (max == currentValue)
            {
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

                        await UnequipAndDeposit(slotType, quantity);
                    }
                }

                // Nothing to change
                if (currentItem != null)
                {
                    Console.WriteLine($"Keep equipped {currentItem.Code} in {slotType}");
                }

                return currentItem;
            }

            if (max == inventoryValue)
            {
                Console.WriteLine($"Inventory item {bestInventoryItem.Code} is highest value at {inventoryValue} in {slotType}");
                if (currentItem != null)
                {
                    await UnequipAndDeposit(slotType);
                }

                await Equip(bestInventoryItem.Code, slotType);
                return bestInventoryItem;
            }
            else
            {
                Console.WriteLine($"Bank item {bestBankItem.Code} is highest value at {bankValue} in {slotType}");

                var quantity = 1;

                // Special case for utility slots, take 25 max
                if (slotType == ItemSlot.Utility1 || slotType == ItemSlot.Utility2)
                {
                    var bankItem = bankItems.First(x => x.Code == bestBankItem.Code);
                    quantity = Math.Min(25, bankItem.Quantity);
                }

                var withdrawn = await WithdrawItems(bestBankItem.Code, quantity);
                if (withdrawn > 0)
                {
                    if (currentItem != null)
                    {
                        await UnequipAndDeposit(slotType);
                    }

                    await Equip(bestBankItem.Code, slotType, withdrawn);
                }
                else
                {
                    Console.WriteLine($"Could not withdraw {quantity} {bestBankItem.Code}!");
                    bankItems = await Bank.Instance.GetItems();
                    return await EquipBestForMonster(currentEquipped, slotType, monster, bankItems, weapon);
                }

                return bestBankItem;
            }
        }

        private async Task UnequipAndDeposit(ItemSlot slotType, int quantity = 1)
        {
            var response = await Unequip(slotType);
            if (response != null)
            {
                await DepositItem(response.Data.Item.Code, quantity);
            }
        }

        internal bool MeetsConditions(List<ConditionSchema> conditions)
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
            var potions = GetRecipes("alchemy").Where(x => x.Level <= Utils.Details[Name].Level);

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

        internal ItemSchema GetBestItemFromBank(ItemSlot slotType, MonsterSchema monster, List<SimpleItemSchema> bankItems, ItemSchema weapon)
        {
            ItemSchema bestItem = null;
            foreach (var bankItem in bankItems)
            {
                if (MatchesUtilitySlot(slotType, bankItem.Code))
                {
                    continue;
                }

                var item = Items.GetItem(bankItem.Code);
                if (!MeetsConditions(item.Conditions))
                {
                    continue;
                }

                if (Items.ItemTypeMatchesSlot(item.Type, slotType))
                {
                    if (bestItem == null)
                    {
                        bestItem = item;
                    }
                    else if (Items.IsBetterItem(bestItem, item, monster, weapon, Utils.Details[Name].Level))
                    {
                        bestItem = item;
                    }
                }
            }

            return bestItem;
        }

        internal ItemSchema GetBestItemFromInventory(ItemSlot slotType, MonsterSchema monster, ItemSchema weapon)
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

                var item = Items.GetItem(inventoryItem.Code);
                if (item.Level > Utils.Details[Name].Level)
                {
                    // Too high level for us
                    continue;
                }

                if (!MeetsConditions(item.Conditions))
                {
                    continue;
                }

                if (Items.ItemTypeMatchesSlot(item.Type, slotType))
                {
                    if (bestItem == null)
                    {
                        bestItem = item;
                    }
                    else if (Items.IsBetterItem(bestItem, item, monster, weapon, Utils.Details[Name].Level))
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
            return await Utils.ApiCall(Name, async () =>
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
                        Console.WriteLine($"Not enough HP to unequip {slotType}, eat/rest");
                        if (!await EatSomething())
                        {
                            Console.WriteLine("Rest");
                            await Utils.ApiCall(Name, async () =>
                            {
                                return await _api.ActionRestMyNameActionRestPostAsync(Name);
                            });
                        }

                        return await Unequip(slotType);
                    }

                    throw;
                }
            }) as EquipmentResponseSchema;
        }

        private async Task Equip(string code, ItemSlot slotType, int quantity = 1)
        {
            try
            {
                await Utils.ApiCall(Name, async () =>
                {
                    Console.WriteLine($"Equip {quantity} {code} in {slotType}");
                    var response = await _api.ActionEquipItemMyNameActionEquipPostAsync(Name, new EquipSchema(code, slotType, quantity));
                    Console.WriteLine($"Equip response: {response.Data.Item.Code} equipped in slot {response.Data.Slot}");
                    return response;
                });
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"Equip error: {ex.ErrorContent}");
                throw;
            }
        }

        internal async Task Recycle(string code, int recycleQuantity)
        {
            Console.WriteLine($"Recycling {recycleQuantity} {code}");

            try
            {
                await Utils.ApiCall(Name, async () =>
                {
                    return await _api.ActionRecyclingMyNameActionRecyclingPostAsync(Name, new RecyclingSchema(code, recycleQuantity));
                });
            }
            catch(ApiException ex)
            {
                Console.WriteLine($"Error recycling: {ex.ErrorContent}");
                if (ex.ErrorCode == 478)
                {
                    Console.WriteLine($"Missing items to recycle {code}");
                    return;
                }

                throw;
            }
        }

        internal async Task SellNpc(string code, int quantity)
        {
            try
            {
                Console.WriteLine($"Selling {quantity} {code} to Npc");
                await Utils.ApiCall(Name, async () =>
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
            var result = await Utils.ApiCall(Name, async () => await _api.ActionClaimPendingItemMyNameActionClaimItemIdPostAsync(Name, id));
            var claim = result as ClaimPendingItemResponseSchema;
            return claim.Data;
        }

        internal async Task DepositPendings()
        {
            var pendingItems = await Bank.Instance.GetPendingItems();
            var claimed = false;
            foreach (var item in pendingItems)
            {
                try
                {
                    var claimedStuff = await ClaimItems(item.Id);
                    if (claimedStuff != null)
                    {
                        Console.WriteLine($"Claimed {claimedStuff.Item.Gold}gp");
                        foreach (var claimedItem in claimedStuff.Item.Items)
                        {
                            Console.WriteLine($"{claimedItem.Quantity} {claimedItem.Code}");
                        }
                    }

                    claimed = true;
                }
                catch (ApiException ex)
                {
                    Console.WriteLine($"Error claiming: {ex.ErrorContent}");
                }
            }

            if (claimed)
            {
                await DepositAllItems();
            }
        }

        internal CurrentCraftManager.CraftInfo GetCurrentCraft()
        {
            return _craftManager.GetCurrentCraft();
        }

        internal void FinishCraft()
        {
            _craftManager.FinishCraft();
        }

        internal void StartCraft(string code, int craftAmount)
        {
            _craftManager.StartCraft(code, craftAmount);
        }

        internal async Task DeleteItem(string code, int amount)
        {
            try
            {
                await Utils.ApiCall(Name, async () =>
                {
                    Console.WriteLine($"Delete {amount} {code}");
                    return await _api.ActionDeleteItemMyNameActionDeletePostAsync(Name, new SimpleItemSchema(code, amount));
                });
            }
            catch(ApiException ex)
            {
                Console.WriteLine($"Error deleting item {ex.ErrorContent}");
            }
        }
    }
}
