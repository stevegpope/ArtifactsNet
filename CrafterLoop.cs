
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;
using System;

namespace Artifacts
{
    internal class CrafterLoop
    {
        private Character _character;
        private Random _random = Random.Shared;
        private string Name { get; set; }

        internal class CraftResult
        {
            internal CraftResult()
            {
                Code = null;
                Recycle = false;
                Quantity = 0;
            }

            internal CraftResult(string code, int quantity, bool recycle)
            {
                Code = code;
                Recycle = recycle;
                Quantity = quantity;
            }

            public string Code { get; set; }
            public bool Recycle { get; set; }
            public int Quantity { get; set; }
        }

        public CrafterLoop(Character character)
        {
            _character = character;
            Name = _character.Name;
        }

        internal async Task RunAsync()
        {
            while (true)
            {
                try
                {
                    // TEMP
                    await DeleteStuff();
                    await Recycle();

                    Console.WriteLine($"Starting crafter loop");
                    await CheckForGold();
                    await ProcessEvents();

                    var currentCraft = _character.GetCurrentCraft();
                    if (currentCraft != null)
                    {
                        try
                        {
                            Console.WriteLine("Continue crafting " + currentCraft.Code);
                            await _character.CraftItems(Items.GetItem(currentCraft.Code), currentCraft.Quantity);
                        }
                        finally
                        {
                            _character.FinishCraft();
                        }
                    }
                    else
                    {
                        Console.WriteLine("No current craft, starting fresh");
                    }

                    // Start at the bank with no inventory
                    await _character.DepositAllItems();

                    if (!string.IsNullOrEmpty(Utils.Details[Name].Task))
                    {
                        await _character.PerformTask();
                    }

                    await MakeJewels();
                    await NpcTrades();

                    if (_random.Next(10) <= 0)
                    {
                        await _character.PerformTask();
                    }

                    string skill = ChooseCraftingSkill();

                    int craftAmount;
                    switch (skill)
                    {
                        case "weaponcrafting":
                        case "gearcrafting":
                        case "jewelrycrafting":
                            craftAmount = 1;
                            break;
                        case "alchemy":
                            craftAmount = 5;
                            break;
                        default:
                            throw new Exception($"Unexpected skill {skill}");

                    }

                    var craftSkill = Utils.GetSkillCraft(skill);
                    var level = _character.GetSkillLevel(skill);
                    Console.WriteLine($"Crafter chose skill {skill} at level {level} with {craftSkill}");

                    var items = await Items.Instance.GetItems(skill: craftSkill, minLevel: 0, maxLevel: level);
                    var bankItems = await Bank.Instance.GetItems();

                    // Filter out items retrieved from bosses
                    items = FilterBossItems(items, bankItems);

                    // Filter out items that require a resource we can't get if we don't have the resource in the bank
                    items = FilterTaskItems(items, bankItems);

                    Console.WriteLine($"{items.Count} potential things to craft");

                    var result = await CraftItems(craftAmount, level, items, bankItems);
                    if (result.Recycle && result.Quantity == 1)
                    {
                        Console.WriteLine($"Recycle immediate {result.Quantity} {result.Code}");
                        await _character.Recycle(result.Code, result.Quantity);
                    }

                    // Nobody can craft anything until alchemy 5
                    if (result.Quantity == 0)
                    {
                        await _character.TrainGathering(5, skill);
                    }

                    // Go deposit the results in the bank
                    await _character.DepositAllItems();

                    // Recycle leftovers
                    await Recycle();

                    // Clean up things we no longer need
                    await DeleteStuff();
                }
                catch (Exception ex)
                {
                    File.AppendAllText("errors.txt", $"[{DateTime.UtcNow}] {ex}\n");
                }
            }
        }

        private async Task DeleteStuff()
        {
            var bankItems = await Bank.Instance.GetItems();
            var characters = await _character.GetCharacters();
            var items = Items.GetAllItems().Values;

            var minCharacterLevel = characters.Min(x => x.Level);

            foreach (var bankItem in bankItems)
            {
                var item = Items.GetItem(bankItem.Code);
                var recipes = items.Where(i => i.Craft != null && i.Craft.Items.Any(c => c.Code == bankItem.Code));
                if (!recipes.Any()) continue;

                if (recipes.Any(r => r.Level > minCharacterLevel - 15))
                {
                    continue;
                }

                Console.WriteLine($"All low level recipes, deleting {bankItem.Quantity} {bankItem.Code}");
                var amount = await _character.WithdrawItems(bankItem.Code);
                if (amount > 0)
                {
                    await _character.DeleteItem(bankItem.Code, amount);
                }
            }
        }

        private async Task NpcTrades()
        {
            var npcs = Npcs.GetAllNpcs();
            var bankItems = await Bank.Instance.GetItems();
            var characters = await _character.GetCharacters();

            foreach (var npc in npcs.Values)
            {
                foreach(var item in npc.Items)
                {
                    if (item.Currency == "gold" || item.Currency == "tasks_coin")
                    {
                        continue;
                    }

                    if (item.BuyPrice == null || item.BuyPrice.Value <= 0)
                    {
                        // We are buying, not selling
                        continue; 
                    }

                    var bankItem = bankItems.FirstOrDefault(x => x.Code == item.Currency);
                    if (bankItem != null && bankItem.Quantity >= item.BuyPrice)
                    {
                        var purchaseQuantity = bankItem.Quantity / item.BuyPrice.Value;

                        var itemDef = Items.GetItem(item.Code);
                        if (itemDef.Effects != null && itemDef.Effects.Count > 0)
                        {
                            var currentAmount = CountAmountEverywhere(item.Code, characters, bankItems);
                            var limit = itemDef.Type == "ring" ? 10 : 5;
                            purchaseQuantity = Math.Min(purchaseQuantity, limit - currentAmount);
                        }

                        Console.WriteLine($"We can buy {purchaseQuantity} {item.Code} from {npc.Code} with our {bankItem.Quantity} {item.Currency}");

                        while (purchaseQuantity > 0)
                        {
                            var inventorySpace = _character.GetFreeInventorySpace();
                            var amount = inventorySpace / item.BuyPrice.Value;

                            var batch = Math.Min(purchaseQuantity, amount);
                            batch = Math.Min(batch, 25);
                            Console.WriteLine($"Buying {batch} {item.Code} from {npc.Code} for {batch * item.BuyPrice} {item.Currency}");
                            var withdrawn = await _character.WithdrawItems(item.Currency, batch * item.BuyPrice.Value);
                            if (withdrawn < batch * item.BuyPrice.Value)
                            {
                                Console.WriteLine($"Couldn't withdraw enough {item.Currency} to buy {batch} {item.Code}, we got {withdrawn} but needed {batch * item.BuyPrice.Value}");
                                break;
                            }

                            await _character.MoveTo(MapContentType.Npc, npc.Code);
                            var bought = await _character.BuyNpcItem(item.Code, batch);
                            if (bought == 0)
                            {
                                Console.WriteLine($"Couldn't buy any {item.Code} from {npc.Code}, even though we had enough {item.Currency}");
                                break;
                            }

                            await _character.DepositAllItems();
                            purchaseQuantity -= batch;
                        }
                    }
                }
            }
        }

        private List<ItemSchema> FilterTaskItems(List<ItemSchema> items, List<SimpleItemSchema> bankItems)
        {
            var result = new List<ItemSchema>();
            var allItems = Items.GetAllItems();
            var taskItems = allItems.Values.Where(i => i.Type == "resource" && i.Subtype == "task");

            foreach (var item in items)
            {
                var taskComponent = item.Craft.Items.FirstOrDefault(i => taskItems.Any(t => t.Code == i.Code));
                if (taskComponent != null)
                {
                    var bankItem = bankItems.FirstOrDefault(x => x.Code == taskComponent.Code);
                    if (bankItem == null || bankItem.Quantity < taskComponent.Quantity)
                    {
                        Console.WriteLine($"Filtering out {item.Code} because it needs {taskComponent.Code}, and we have none");
                    }
                    else
                    {
                        result.Add(item);
                    }
                }
                else
                {
                    result.Add(item);
                }
            }

            return result;
        }

        private List<ItemSchema> FilterBossItems(List<ItemSchema> items, List<SimpleItemSchema> bankItems)
        {
            var result = new List<ItemSchema>();
            var monsters = Monsters.GetAllMonsters();
            var bosses = monsters.Values.Where(m => m.Type == MonsterType.Boss);
            var bossDrops = bosses.SelectMany(b => b.Drops);

            foreach (var item in items)
            {
                var component = item.Craft.Items.FirstOrDefault(i => bossDrops.Any(d => i.Code == d.Code));
                if (component != null)
                {
                    var bankItem = bankItems.FirstOrDefault(x => x.Code == component.Code);
                    if (bankItem == null || bankItem.Quantity < component.Quantity)
                    {
                        Console.WriteLine($"Filtering out {item.Code} because it drops from a boss, and we have none");
                    }
                    else
                    {
                        result.Add(item);
                    }
                }
                else
                {
                    result.Add(item);
                }
            }

            return result;
        }

        private async Task MakeJewels()
        {
            if (Utils.Details[Name].MiningLevel < 20)
            {
                return;
            }

            var bankItems = await Bank.Instance.GetItems();
            var recipes = _character.GetRecipes("mining");
            var crafted = false;

            foreach (var recipe in recipes)
            {
                if (recipe.Craft.Items.Count != 1)
                {
                    continue;
                }

                if (recipe.Code.EndsWith("_bar"))
                {
                    continue;
                }

                var component = recipe.Craft.Items.First();
                var bankItem = bankItems.FirstOrDefault(x => x.Code == component.Code);
                if (bankItem != null)
                {
                    var amountToCraft = bankItem.Quantity / component.Quantity;
                    if (amountToCraft > 0)
                    {
                        Console.WriteLine($"Crafting {amountToCraft} {recipe.Code} from {bankItem.Quantity} {component.Code}");
                        if (await _character.CraftItems(Items.GetItem(recipe.Code), amountToCraft, bankOnly: true) > 0)
                        {
                            crafted = true;
                        }
                    }
                }
            }

            if (crafted)
            {
                // Go deposit the results in the bank
                await _character.DepositAllItems();
            }
        }

        private async Task CheckForGold()
        {
            var bankItems = await Bank.Instance.GetItems();
            foreach (var bankItem in bankItems)
            {
                if (bankItem.Quantity > 0)
                {
                    var item = Items.GetItem(bankItem.Code);
                    if (item.Type == "consumable" && item.Subtype == "bag")
                    {
                        var amount = await _character.WithdrawItems(item.Code, bankItem.Quantity);
                        if (amount > 0)
                        {
                            Console.WriteLine("GOLD SWEET GOLD!!");
                            await _character.Consume(item.Code, amount);
                        }
                    }
                }
            }
        }

        private string ChooseCraftingSkill()
        {
            var skillsPerCharacter = new Dictionary<string, string[]>
            {
                { "baz1", new[] { "weaponcrafting", "alchemy" } },
                { "baz2", new[] { "weaponcrafting", "alchemy" } },
                { "baz3", new[] { "gearcrafting", "alchemy" } },
                { "baz4", new[] { "gearcrafting", "alchemy" } },
                { "baz5", new[] { "jewelrycrafting", "alchemy" } },
            };

            var skills = skillsPerCharacter[Name];

            var result = new List<string>();

            // If the main skill (the first one) is a multiple of 10, focus on that one until we level up
            var mainSkillLevel = _character.GetSkillLevel(skills[0]);
            if (mainSkillLevel % 5 == 0)
            {
                Console.WriteLine($"Main skill {skills[0]} is at a multiple of 10 ({mainSkillLevel}), focusing on that");
                return skills[0];
            }

            // Eliminate skills that are 5 or more levels higher than all the others
            foreach (var skill in skills)
            {
                var others = skills.Where(x => x != skill);
                var level = _character.GetSkillLevel(skill);
                if (others.All(x => _character.GetSkillLevel(x) >= level - 5))
                {
                    result.Add(skill);
                }
            }

            return result.ElementAt(_random.Next(result.Count));
        }

        private async Task ProcessEvents()
        {
            await _character.DepositPendings();

            var events = await Events.Instance.GetActiveEvents();
            foreach (var activeEvent in events)
            {
                if (activeEvent.Expiration < DateTime.UtcNow + TimeSpan.FromSeconds(240))
                {
                    Console.WriteLine($"Event {activeEvent.Code} expires too soon {activeEvent.Expiration}");
                    continue;
                }

                if (activeEvent.Code.EndsWith("_merchant"))
                {
                    await ProcessMerchant(activeEvent);
                }
                else if (activeEvent.Map?.Interactions?.Content?.Type == MapContentType.Monster)
                {
                    await ProcessMonsterEvent(activeEvent);
                }
                else if (activeEvent.Map?.Interactions?.Content?.Type == MapContentType.Resource)
                {
                    await ProcessResourceEvent(activeEvent);
                }
            }
        }

        private async Task ProcessResourceEvent(ActiveEventSchema activeEvent)
        {
            Console.WriteLine($"{activeEvent.Code} is present, trying to gather stuff");
            var resource = Resources.GetResource(activeEvent.Map.Interactions.Content.Code);
            if (resource == null)
            {
                Console.WriteLine($"No resources here");
                return;
            }

            var gatheringSkill = resource.Skill;
            var level = _character.GetSkillLevel(gatheringSkill.ToString());
            if (resource.Level > level)
            {
                Console.WriteLine($"{resource.Code} ({resource.Level}) is too high level for us ({level})");
                return;
            }

            await _character.GatherItems(resource.Code, 100, ignoreBank: true);
            await _character.DepositAllItems();
        }

        private async Task ProcessMonsterEvent(ActiveEventSchema activeEvent)
        {
            Console.WriteLine($"{activeEvent.Code} is present, trying to kill stuff");
            var monster = Monsters.GetMonster(activeEvent.Map.Interactions.Content.Code);
            if (monster == null)
            {
                Console.WriteLine($"No monsters here");
                return;
            }

            if (monster.Level >= Utils.Details[Name].Level - 10)
            {
                Console.WriteLine($"{monster.Code} ({monster.Level}) is too high level for us ({Utils.Details[Name].Level})");
                return;
            }

            await _character.FightLoop(100, monster.Code);
        }

        internal async Task ProcessMerchant(ActiveEventSchema activeEvent)
        {
            Console.WriteLine($"{activeEvent.Code} is present, trying to sell stuff");

            var items = Npcs.Instance.GetNpcItems(activeEvent.Code);
            var bankItems = await Bank.Instance.GetItems();
            var npcs = Npcs.GetAllNpcs().Values;
            foreach (var item in items)
            {
                if (item?.SellPrice == null || item.SellPrice < 100)
                {
                    continue;
                }

                var itemDetails = Items.GetItem(item.Code);

                var bankItem = bankItems.FirstOrDefault(x => x.Code == item.Code);
                if (bankItem != null)
                {
                    bankItems = await Bank.Instance.GetItems();
                    var characters = await _character.GetCharacters();
                    var amount = GetSellQuantity(itemDetails, bankItem.Quantity, characters, bankItems, npcs);
                    if (amount > 0)
                    {
                        var withdrawn = await _character.WithdrawItems(bankItem.Code);
                        if (withdrawn > 0)
                        {
                            var sellAmount = Math.Min(amount, withdrawn);
                            Console.WriteLine($"Going to sell {withdrawn} {bankItem.Code} to {activeEvent.Code}");
                            await _character.Move(activeEvent.Map.X, activeEvent.Map.Y);
                            await _character.SellNpc(bankItem.Code, sellAmount);
                        }
                    }
                }
            }
        }

        private int GetSellQuantity(ItemSchema itemDetails, int bankAmount, List<CharacterSchema> characters, List<SimpleItemSchema> bankItems, IEnumerable<NPCSchema> npcs)
        {
            var currentAmount = CountAmountEverywhere(itemDetails.Code, characters, bankItems);

            if (itemDetails.Effects != null && itemDetails.Effects.Count > 0)
            {
                // Leave 5 of an item, 10 of a ring
                var limit = itemDetails.Type == "ring" ? 10 : 5;
                var amountToSell = currentAmount - limit;
                if (currentAmount > limit)
                {
                    Console.WriteLine($"Have {currentAmount} {itemDetails.Code}, it has effects, sell {amountToSell}/{currentAmount}");
                    return amountToSell;
                }
                else
                {
                    return 0;
                }
            }

            var items = Items.GetAllItems();
            var recipe = items.Values.FirstOrDefault(i => i.Craft != null && i.Craft.Items.Any(c => c.Code == itemDetails.Code));
            if (recipe != null)
            {
                var component = recipe.Craft.Items.First(c => c.Code == itemDetails.Code);
                var amountToKeep = component.Quantity * 25;
                var amountToSell = Math.Min(currentAmount - amountToKeep, bankAmount);
                Console.WriteLine($"Have {bankAmount} {itemDetails.Code}, recipes need it, sell {amountToSell}/{currentAmount}");
                return amountToSell;
            }

            // Do we need them for NPC purchases?
            foreach (var npc in npcs)
            {
                var purchase = npc.Items.FirstOrDefault(i => i.Currency == itemDetails.Code && i.BuyPrice != null && i.BuyPrice.Value > 0);
                if (purchase != null)
                {
                    var amountToKeep = purchase.BuyPrice.Value * 25;
                    var amountToSell = Math.Min(currentAmount - amountToKeep, bankAmount);
                    Console.WriteLine($"Have {bankAmount} {itemDetails.Code}, NPCs need it, sell {amountToSell}/{currentAmount}");
                    return amountToSell;
                }
            }

            Console.WriteLine($"Have {bankAmount} {itemDetails.Code}, not needed, sell {bankAmount}");
            return bankAmount;
        }

        private async Task<CraftResult> CraftItems(int craftAmount, int level, List<ItemSchema> items, List<SimpleItemSchema> bankItems)
        {
            Console.WriteLine("Make the best gear we can");
            var result = await CraftItemsFromLevelDown(craftAmount, level, items, bankItems);
            if (result.Quantity == 0)
            {
                result = await CraftItemsFromLevelUp(craftAmount, level, items, bankItems);
            }

            return result;
        }

        private async Task<CraftResult> CraftItemsFromLevelUp(int craftAmount, int level, List<ItemSchema> items, List<SimpleItemSchema> bankItems)
        {
            var minLevel = Math.Max(level - 10, 1);
            Console.WriteLine($"Crafting {craftAmount} items from {minLevel} up");

            for (int targetLevel = minLevel; targetLevel < level; targetLevel++)
            {
                var result = await CraftItemsAtLevel(targetLevel, craftAmount, items, bankItems, ignoreBankCheck: true);
                if (result.Quantity > 0)
                {
                    break;
                }
            }

            return new CraftResult();
        }

        private async Task<CraftResult> CraftItemsFromLevelDown(int craftAmount, int level, List<ItemSchema> items, List<SimpleItemSchema> bankItems)
        {
            Console.WriteLine($"Crafting {craftAmount} items from {level} down");

            for (int targetLevel = level; targetLevel > 0; targetLevel--)
            {
                var result = await CraftItemsAtLevel(targetLevel, craftAmount, items, bankItems);
                if (result.Quantity > 0)
                {
                    return result;
                }
            }

            return new CraftResult();
        }

        private async Task<CraftResult> CraftItemsAtLevel(int targetLevel, int craftAmount, List<ItemSchema> items, List<SimpleItemSchema> bankItems, bool ignoreBankCheck = false)
        {
            var itemsAtLevel = items.Where(x => x.Level == targetLevel);
            Console.WriteLine($"{itemsAtLevel.Count()} potential things to craft at level {targetLevel}");
            if (!itemsAtLevel.Any())
            {
                return new CraftResult();
            }

            var itemsList = new List<ItemSchema>(itemsAtLevel);
            var characters = await _character.GetCharacters();
            while (itemsList.Count != 0)
            {
                var item = ChooseEasiestItem(itemsList);

                if (!ignoreBankCheck)
                {
                    var currentAmount = CountAmountEverywhere(item.Code, characters, bankItems);
                    var limit = item.Type == "ring" ? 10 : 5;
                    if (currentAmount >= limit)
                    {
                        Console.WriteLine($"We already have enough ({currentAmount}) {item.Code}");
                        itemsList.Remove(item);
                        continue;
                    }

                    Console.WriteLine($"We will craft {item.Code}, current inventory {currentAmount}");
                }

                _character.StartCraft(item.Code, craftAmount);

                try
                {
                    var total = await _character.CraftItems(item, craftAmount);
                    if (total == 0)
                    {
                        itemsList.Remove(item);
                        continue;
                    }
                    else
                    {
                        return new CraftResult(item.Code, total, false);
                    }
                }
                finally
                {
                    _character.FinishCraft();
                }
            }

            Console.WriteLine($"Already have enough items at this level, crafting without bank check");

            var newItems = items.Where(x => !RequiresTaskItems(x.Code)).ToList();

            await _character.DepositAllItems();
                    
            var result = await CraftItemsAtLevel(targetLevel, craftAmount, newItems, bankItems, ignoreBankCheck: true);
            return new CraftResult(result.Code, result.Quantity, true);
        }

        private ItemSchema ChooseEasiestItem(List<ItemSchema> itemsList)
        {
            var easiestItem = itemsList[0];
            var requiredLevel = CalculateRequiredLevel(easiestItem);
            foreach(var item in itemsList)
            {
                if (item.Code == easiestItem.Code)
                {
                    continue;
                }

                var itemRequiredLevel = CalculateRequiredLevel(item);
                if (itemRequiredLevel < requiredLevel)
                {
                    easiestItem = item;
                    requiredLevel = itemRequiredLevel;
                }
            }

            Console.WriteLine($"{easiestItem.Code} is easiest among\n {string.Join(',', itemsList.Select(i => i.Code))}");
            return easiestItem;
        }

        private int CalculateRequiredLevel(ItemSchema easiestItem)
        {
            var monsters = Monsters.GetAllMonsters();
            var requiredLevel = 0;

            foreach(var item in easiestItem.Craft.Items)
            {
                var monster = monsters.Values.FirstOrDefault(m => m.Drops.Any(d => d.Code == item.Code));
                if (monster != null)
                {
                    requiredLevel = Math.Max(requiredLevel, monster.Level);
                }
            }

            return requiredLevel;
        }

        private bool RequiresTaskItems(string code)
        {
            var item = Items.GetItem(code);

            if (item.Craft != null)
            {
                return item.Craft.Items.Any(c => RequiresTaskItems(c.Code));
            }

            var monsters = Monsters.GetAllMonsters();
            var bosses = monsters.Values.Where(m => m.Type == MonsterType.Boss);
            var bossDrops = bosses.SelectMany(b => b.Drops);

            var allItems = Items.GetAllItems();
            var taskItems = allItems.Values.Where(i => i.Type == "resource" && i.Subtype == "task");

            return bossDrops.Any(i => i.Code == code) || taskItems.Any(i => i.Code == code);
        }

        private static int CountAmountEverywhere(string code, List<CharacterSchema> characters, List<SimpleItemSchema> bankItems)
        {
            var total = 0;

            foreach (var character in characters)
            {
                if (code == character.AmuletSlot) total++;
                if (code == character.Artifact1Slot) total++;
                if (code == character.Artifact2Slot) total++;
                if (code == character.Artifact3Slot) total++;
                if (code == character.BagSlot) total++;
                if (code == character.BodyArmorSlot) total++;
                if (code == character.BootsSlot) total++;
                if (code == character.HelmetSlot) total++;
                if (code == character.LegArmorSlot) total++;
                if (code == character.Ring1Slot) total++;
                if (code == character.Ring2Slot) total++;
                if (code == character.ShieldSlot) total++;
                if (code == character.Utility1Slot) total += character.Utility1SlotQuantity;
                if (code == character.Utility2Slot) total += character.Utility2SlotQuantity;
                if (code == character.WeaponSlot) total++;

                total += character.Inventory.Where(x => x.Code == code).Sum(x => x.Quantity);
            }

            total += bankItems.Where(x => x.Code == code).Sum(x => x.Quantity);

            return total;
        }

        private async Task Recycle()
        {
            // If there are too many of this item, try to recycle some
            var bankItems = await Bank.Instance.GetItems();
            var characters = await _character.GetCharacters();
            foreach (var bankItem in bankItems)
            {
                var recycleQuantity = await CalculateRecycleQuantity(bankItems, characters, bankItem);

                if (recycleQuantity > 0)
                {
                    var amount = await _character.WithdrawItems(bankItem.Code);
                    if (amount >= recycleQuantity)
                    {
                        try
                        {
                            var item = Items.GetItem(bankItem.Code);

                            // Go to relevant workshop
                            await _character.MoveTo(MapContentType.Workshop, item.Craft.Skill.Value.ToString());

                            await _character.Recycle(item.Code, recycleQuantity);

                            // Bank to bank for deposit
                            await _character.DepositAllItems();

                            // Start again to recheck state of the bank etc
                            await Recycle();
                            return;
                        }
                        catch (ApiException ex)
                        {
                            Console.WriteLine(ex.ErrorContent);
                        }
                    }
                }
            }
        }

        private async Task<int> CalculateRecycleQuantity(List<SimpleItemSchema> bankItems, List<CharacterSchema> characters, SimpleItemSchema bankItem)
        {
            // If there are 5 of a better item in every way we can recycle all of them, if not we need to keep 5
            var item = Items.GetItem(bankItem.Code);

            // Can it be recycled?
            if (item.Craft == null ||
                (item.Craft.Skill != CraftSkill.Gearcrafting &&
                item.Craft.Skill != CraftSkill.Jewelrycrafting &&
                item.Craft.Skill != CraftSkill.Weaponcrafting))
            {
                return 0;
            }

            var currentAmount = CountAmountEverywhere(bankItem.Code, characters, bankItems);

            // Recycle any more than 5 (10 for rings)
            var limit = 5;
            if (item.Type == "ring")
            {
                limit = 10;
            }

            var monsters = Monsters.GetAllMonsters();
            var keep = WouldChooseItemSkill(item, bankItems, characters, limit);
            if (!keep)
            {
                foreach (var monster in monsters.Values)
                {
                    if (WouldChooseItem(item, monster, bankItems, characters, limit))
                    {
                        keep = true;
                        break;
                    }
                }
            }

            if (keep)
            {
                if (currentAmount > limit)
                {
                    Console.WriteLine($"Recycle {currentAmount - limit} {bankItem.Code} because we have {currentAmount}");
                    return currentAmount - limit;
                }

                return 0;
            }
            
            Console.WriteLine($"Recycle all {bankItem.Quantity} {bankItem.Code} because we have better gear");
            return currentAmount;
        }

        private bool WouldChooseItemSkill(ItemSchema item, List<SimpleItemSchema> bankItems, List<CharacterSchema> characters, int limit)
        {
            // Crafting values
            var skills = Enum.GetNames(typeof(CraftSkill));
            foreach (var skill in skills)
            {
                var itemValue = Items.CalculateItemValueSkill(item, skill.ToLower());
                if (itemValue == 0) continue;

                var bankItem = GetBestItemSkill(skill.ToLower(), bankItems, bankItems, characters, limit);
                var bankValue = Items.CalculateItemValueSkill(bankItem, skill.ToLower());
                if (bankValue > itemValue)
                {
                    continue;
                }

                var inventoryItems = Utils.Details[Name].Inventory.Where(i => i.Quantity > 0).Select(i => new SimpleItemSchema(i.Code, i.Quantity));
                var inventoryItem = GetBestItemSkill(skill.ToLower(), inventoryItems, bankItems, characters, limit);
                var inventoryValue = Items.CalculateItemValueSkill(bankItem, skill.ToLower());
                if (inventoryValue > itemValue)
                {
                    continue;
                }

                return true;
            }

            return false;
        }

        private bool WouldChooseItem(ItemSchema item, MonsterSchema monster, List<SimpleItemSchema> bankItems, List<CharacterSchema> characters, int limit)
        {
            int maxLevel = Utils.Details[Name].Level;

            if (item.Subtype == "tool")
            {
                return true;
            }
            else
            {
                if(!IsBestItem(item.Type, monster, bankItems, bankItems, characters, limit, item))
                {
                    return false;
                }

                var inventoryItems = Utils.Details[Name].Inventory.Where(i => i.Quantity > 0).Select(i => new SimpleItemSchema(i.Code, i.Quantity));
                if(!IsBestItem(item.Type, monster, inventoryItems, bankItems, characters, limit, item))
                {
                    return false;
                }

                return true;
            }
        }

        private ItemSchema GetBestItemSkill(string skill, IEnumerable<SimpleItemSchema> items, List<SimpleItemSchema> bankItems, List<CharacterSchema> characters, int limit)
        {
            int maxLevel = Utils.Details[Name].Level;
            double bestValue = 0.0;
            ItemSchema bestItem = null;

            foreach (var bankItem in items)
            {
                var itemSchema = Items.GetItem(bankItem.Code);

                if (itemSchema.Subtype != "tool") continue;

                var amount = CountAmountEverywhere(bankItem.Code, characters, bankItems);
                if (amount < limit) continue;

                var itemValue = Items.CalculateItemValueSkill(itemSchema, skill);
                if (itemValue > bestValue)
                {
                    bestValue = itemValue;
                    bestItem = itemSchema;
                }
            }

            return bestItem;
        }

        private bool IsBestItem(string type, MonsterSchema monster, IEnumerable<SimpleItemSchema> items, List<SimpleItemSchema> bankItems, List<CharacterSchema> characters, int limit, ItemSchema compare)
        {
            int maxLevel = Utils.Details[Name].Level;

            var weapons = new[]
            {
                "copper_dagger", // air
                "wooden_staff", // earth
                "fishing_net", // water
                "fire_staff", // fire
            };

            return bankItems.Any(b => ChooseBestItemForMonster(type, monster, bankItems, characters, limit, compare, maxLevel, weapons, b)?.Code == compare.Code);
        }

        private static ItemSchema ChooseBestItemForMonster(string type, MonsterSchema monster, List<SimpleItemSchema> bankItems, List<CharacterSchema> characters, int limit, ItemSchema compare, int maxLevel, string[] weapons, SimpleItemSchema bankItem)
        {
            var itemSchema = Items.GetItem(bankItem.Code);

            if (itemSchema.Type != type) return null;

            var amount = CountAmountEverywhere(bankItem.Code, characters, bankItems);
            if (amount < limit) return null;

            if (type == "weapon")
            {
                var compareValue = Items.CalculateItemValue(compare, monster, null, maxLevel);
                var itemValue = Items.CalculateItemValue(itemSchema, monster, null, maxLevel);
                if (itemValue > compareValue)
                {
                    return itemSchema;
                }
            }
            else
            {
                var better = true;
                foreach (var weapon in weapons)
                {
                    var weaponSchema = Items.GetItem(weapon);
                    var compareValue = Items.CalculateItemValue(compare, monster, weaponSchema, maxLevel);
                    var itemValue = Items.CalculateItemValue(itemSchema, monster, weaponSchema, maxLevel);
                    if (itemValue > compareValue)
                    {
                        better = false;
                        break;
                    }
                }

                if (better)
                {
                    return itemSchema;
                }
            }

            return compare;
        }
    }
}