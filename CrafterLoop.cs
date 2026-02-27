
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
        private CurrentCraftManager _craftManager;

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
            _craftManager = new CurrentCraftManager(Name);
        }

        internal async Task RunAsync()
        {
            Console.WriteLine($"Starting crafter loop");

            var currentCraft = _craftManager.GetCurrentCraft();
            if (currentCraft != null)
            {
                try
                {
                    Console.WriteLine("Continue crafting " + currentCraft.Code);
                    await _character.CraftItems(Items.GetItem(currentCraft.Code), currentCraft.Quantity);
                }
                finally
                {
                    _craftManager.FinishCraft();
                }
            }
            else
            {
                Console.WriteLine("No current craft, starting fresh");
            }

            // Start at the bank with no inventory
            await _character.MoveTo(MapContentType.Bank);
            await _character.DepositAllItems();

            while (true)
            {
                await CheckForGold();
                await ProcessEvents();

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
                Console.WriteLine($"{items.Count} potential things to craft");

                var bankItems = await Bank.Instance.GetItems();

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
                await _character.MoveTo(MapContentType.Bank);
                await _character.DepositAllItems();

                // Recycle leftovers
                await Recycle();
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
            var skills = new List<string>([
                "weaponcrafting",
                "gearcrafting",
                "jewelrycrafting",
                "alchemy"
            ]);

            var result = new List<string>();

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
            }
        }

        private async Task ProcessMonsterEvent(ActiveEventSchema activeEvent)
        {
            Console.WriteLine($"{activeEvent.Code} is present, trying to kill stuff");
            var monster = await Monsters.GetMonster(activeEvent.Map.Interactions.Content.Code);
            if (monster == null)
            {
                Console.WriteLine($"No monsters here");
                return;
            }

            if (monster.Level >= Utils.Details[Name].Level)
            {
                Console.WriteLine($"{monster.Code} ({monster.Level}) is too high level for us ({Utils.Details[Name].Level})");
                return;
            }

            await _character.FightLoop(100, monster.Code);
        }

        private async Task ProcessMerchant(ActiveEventSchema activeEvent)
        {
            Console.WriteLine($"{activeEvent.Code} is present, trying to sell stuff");

            var items = await Npcs.Instance.GetNpcItems(activeEvent.Code);
            var bankItems = await Bank.Instance.GetItems();
            foreach (var item in items)
            {
                if (item?.SellPrice == null || item.SellPrice < 10)
                {
                    continue;
                }

                var bankItem = bankItems.FirstOrDefault(x => x.Code == item.Code);
                if (bankItem != null)
                {
                    await _character.MoveTo(MapContentType.Bank);
                    var withdrawn = await _character.WithdrawItems(bankItem.Code, 100);
                    if (withdrawn > 0)
                    {
                        Console.WriteLine($"Going to sell {withdrawn} {bankItem.Code} to {activeEvent.Code}");
                        await _character.Move(activeEvent.Map.X, activeEvent.Map.Y);
                        await _character.SellNpc(bankItem.Code, withdrawn);
                    }
                }
            }
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
            var characters = await _character.GetAllCharacters();
            while (itemsList.Count != 0)
            {
                var item = itemsList.ElementAt(_random.Next(itemsList.Count));

                if (!ignoreBankCheck)
                {
                    var currentAmount = await CountAmountEverywhere(item.Code, characters, bankItems);
                    var limit = item.Type == "ring" ? 10 : 5;
                    if (currentAmount >= limit)
                    {
                        Console.WriteLine($"We already have enough ({currentAmount}) {item.Code}");
                        itemsList.Remove(item);
                        continue;
                    }

                    Console.WriteLine($"We will craft {item.Code}, current inventory {currentAmount}");
                }

                _craftManager.StartCraft(item.Code, craftAmount);

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
                    _craftManager.FinishCraft();
                }
            }

            Console.WriteLine($"Already have enough items at this level, crafting without bank check");

            // Remove anything that isn't made of resources
            var newItems = items.Where(x => IsMadeOfBaseResources(x.Code)).ToList();

            await _character.MoveTo(MapContentType.Bank);
            await _character.DepositAllItems();
                    
            var result = await CraftItemsAtLevel(targetLevel, craftAmount, newItems, bankItems, ignoreBankCheck: true);
            return new CraftResult(result.Code, result.Quantity, true);
        }

        private bool IsMadeOfBaseResources(string code)
        {
            var item = Items.GetItem(code);

            if (item.Craft != null)
            {
                return item.Craft.Items.All(c => IsMadeOfBaseResources(c.Code));
            }

            return item.Type == "resource" && item.Subtype != "task";
        }

        private static async Task<int> CountAmountEverywhere(string code, List<CharacterSchema> characters, List<SimpleItemSchema> bankItems)
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
            var characters = await _character.GetAllCharacters();
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
                            await _character.MoveClosest(MapContentType.Workshop, item.Craft.Skill.Value.ToString());

                            await _character.Recycle(item.Code, recycleQuantity);

                            // Bank to bank for deposit
                            await _character.MoveTo(MapContentType.Bank);
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

        private static async Task<int> CalculateRecycleQuantity(List<SimpleItemSchema> bankItems, List<CharacterSchema> characters, SimpleItemSchema bankItem)
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

            var currentAmount = await CountAmountEverywhere(bankItem.Code, characters, bankItems);

            // Recycle any more than 5 (10 for rings)
            var limit = 5;
            if (item.Type == "ring")
            {
                limit = 10;
            }

            if (currentAmount > limit)
            {
                Console.WriteLine($"Recycle {currentAmount - limit} {bankItem.Code} because we have {currentAmount} in total");
                return currentAmount - limit;
            }

            List<ItemSchema> comparables = new List<ItemSchema>();
            foreach (var otherBankItem in bankItems)
            {
                var comparable = Items.GetItem(otherBankItem.Code);
                if (comparable.Type != item.Type)
                {
                    continue;
                }

                if (comparable.Code == item.Code)
                {
                    continue;
                }

                var bankItemAmount = await CountAmountEverywhere(otherBankItem.Code, characters, bankItems);
                if (bankItemAmount < limit)
                {
                    continue;
                }

                if (comparable.Effects != null)
                {
                    // Are the effects on the comparable better
                    var better = true;
                    foreach (var effect in item.Effects)
                    {
                        var comparableEffect = comparable.Effects.FirstOrDefault(x => x.Code == effect.Code);
                        if (comparableEffect == null)
                        {
                            better = false;
                            break;
                        }

                        if (comparableEffect.Value < effect.Value)
                        {
                            better = false;
                            break;
                        }
                    }

                    if (better)
                    {
                        // All effects are better on the new item, we can get rid of all this one
                        Console.WriteLine($"Recycle all {bankItem.Quantity} {bankItem.Code} because we have {bankItemAmount} {comparable.Code} in total");
                        return bankItem.Quantity;
                    }
                }
            }

            return 0;
        }
    }
}