
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;
using System;

namespace Artifacts
{
    internal class CrafterLoop
    {
        private Character _character;
        private Random _random = Random.Shared;

        public CrafterLoop(Character character)
        {
            _character = character;
        }

        internal async Task RunAsync()
        {
            Console.WriteLine($"Starting crafter loop");

            while (true)
            {
                var skills = new List<string>([
                    "weaponcrafting",
                    "gearcrafting",
                    "jewelrycrafting",
                ]);

                // Choose a craft skill
                var skill = skills.ElementAt(_random.Next(skills.Count));

                int craftAmount = 1;
                switch (skill)
                {
                    case "weaponcrafting":
                    case "gearcrafting":
                    case "jewelrycrafting":
                        craftAmount = 1;
                        break;
                    default:
                        throw new Exception($"Unexpected skill {skill}");

                }

                var craftSkill = Utils.GetSkillCraft(skill);
                var level = Utils.GetSkillLevel(skill);
                Console.WriteLine($"Crafter chose skill {skill} at level {level} with {craftSkill}");

                var items = await Items.Instance.GetItems(skill: craftSkill, minLevel: 0, maxLevel: level);
                Console.WriteLine($"{items.Data.Count} potential things to craft");

                var bankItems = await Bank.Instance.GetItems();

                int total = await CraftItems(craftAmount, level, items, bankItems);
                if (total == 0)
                {
                    throw new Exception($"Nothing we can craft for skill {skill}");
                }

                // Go deposit the results in the bank
                await _character.MoveTo(MapContentType.Bank);
                await _character.DepositAllItems();

                // Recycle leftovers
                await Recycle();
            }
        }

        private async Task<int> CraftItems(int craftAmount, int level, DataPageItemSchema items, List<SimpleItemSchema> bankItems)
        {
            int total = 0;

            Console.WriteLine("Make the best gear we can");
            total = await CraftItemsFromLevelDown(craftAmount, level, items, bankItems);
            if (total == 0)
            {
                total = await CraftItemsFromLevelUp(craftAmount, level, items, bankItems);
            }

            return total;
        }

        private async Task<int> CraftItemsFromLevelUp(int craftAmount, int level, DataPageItemSchema items, List<SimpleItemSchema> bankItems)
        {
            var minLevel = Math.Max(level - 10, 1);
            Console.WriteLine($"Crafting {craftAmount} items from {minLevel} up");

            int total = 0;
            for (int targetLevel = minLevel; targetLevel < level; targetLevel++)
            {
                total = await CraftItemsAtLevel(targetLevel, craftAmount, items.Data, bankItems, ignoreBankCheck: true);
                if (total > 0)
                {
                    break;
                }
            }

            return total;
        }

        private async Task<int> CraftItemsFromLevelDown(int craftAmount, int level, DataPageItemSchema items, List<SimpleItemSchema> bankItems)
        {
            Console.WriteLine($"Crafting {craftAmount} items from {level} down");

            int total = 0;
            for (int targetLevel = level; targetLevel > 0; targetLevel--)
            {
                total = await CraftItemsAtLevel(targetLevel, craftAmount, items.Data, bankItems);
                if (total > 0)
                {
                    break;
                }
            }

            return total;
        }

        private async Task<int> CraftItemsAtLevel(int targetLevel, int craftAmount, List<ItemSchema> items, List<SimpleItemSchema> bankItems, bool ignoreBankCheck = false)
        {
            const int TooMany = 5;
            var total = 0;
            var itemsAtLevel = items.Where(x => x.Level == targetLevel);
            Console.WriteLine($"{itemsAtLevel.Count()} potential things to craft at level {targetLevel}");
            if (!itemsAtLevel.Any())
            {
                return 0;
            }

            var itemsList = new List<ItemSchema>(itemsAtLevel);
            var characters = await _character.GetAllCharacters();
            while (itemsList.Any())
            {
                var item = itemsList.ElementAt(_random.Next(itemsList.Count()));

                if (!ignoreBankCheck)
                {
                    var currentAmount = await CountAmountEverywhere(item.Code, characters, bankItems);

                    if (currentAmount >= TooMany)
                    {
                        Console.WriteLine($"We already have enough ({currentAmount}) {item.Code}");
                        itemsList.Remove(item);
                        continue;
                    }

                    Console.WriteLine($"We will craft {item.Code}, current inventory {currentAmount}");
                }

                total = await _character.CraftItems(item, craftAmount);
                if (total == 0)
                {
                    itemsList.Remove(item);
                    continue;
                }
                break;
            }

            if (total == 0)
            {
                Console.WriteLine($"Already have enough items at this level, crafting without bank check");
                return await CraftItemsAtLevel(targetLevel, craftAmount, items, bankItems, ignoreBankCheck: true);
            }

            return total;
        }

        private async Task<int> CountAmountEverywhere(string code, List<CharacterSchema> characters, List<SimpleItemSchema> bankItems)
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
                    var amount = await _character.WithdrawItems(bankItem.Code, recycleQuantity);
                    if (amount > 0)
                    {
                        try
                        {
                            var item = await Items.Instance.GetItem(bankItem.Code);

                            // Go to relevant workshop
                            await _character.MoveClosest(MapContentType.Workshop, item.Craft.Skill.Value.ToString());

                            await _character.Recycle(item.Code, amount);

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

        private async Task<int> CalculateRecycleQuantity(List<SimpleItemSchema> bankItems, List<CharacterSchema> characters, SimpleItemSchema bankItem)
        {
            // If there are 5 of a better item in every way we can recycle all of them, if not we need to keep 5
            var item = await Items.Instance.GetItem(bankItem.Code);

            // Can it be recycled?
            if (item.Craft == null ||
                (item.Craft.Skill != CraftSkill.Gearcrafting &&
                item.Craft.Skill != CraftSkill.Jewelrycrafting &&
                item.Craft.Skill != CraftSkill.Weaponcrafting))
            {
                return 0;
            }

            var currentAmount = await CountAmountEverywhere(bankItem.Code, characters, bankItems);

            // Recycle any more than 5
            if (currentAmount > 5)
            {
                Console.WriteLine($"Recycle {currentAmount - 5} {bankItem.Code} because we have {currentAmount} in total");
                return currentAmount - 5;
            }

            List<ItemSchema> comparables = new List<ItemSchema>();
            foreach (var otherBankItem in bankItems)
            {
                var comparable = await Items.Instance.GetItem(otherBankItem.Code);
                if (comparable.Type != item.Type)
                {
                    continue;
                }

                if (comparable.Code == item.Code)
                {
                    continue;
                }

                var bankItemAmount = await CountAmountEverywhere(otherBankItem.Code, characters, bankItems);
                if (bankItemAmount < 5)
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