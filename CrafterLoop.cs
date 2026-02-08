
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;

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
                    "jewelrycrafting"
                    //"alchemy",
                    //"cooking",
                ]);

                var miningSkill = Utils.GetSkillLevel("mining");
                if (miningSkill >= 20)
                {
                    // Gems are available, include them
                    skills.Add("mining");
                }

                // Choose a craft skill
                var skill = skills.ElementAt(_random.Next(skills.Count));

                int craftAmount = 1;
                switch (skill)
                {
                    case "weaponcrafting":
                    case "gearcrafting":
                    case "jewelrycrafting":
                    case "mining":
                        craftAmount = 1;
                        break;
                    case "alchemy":
                        craftAmount = 8;
                        break;
                    case "cooking":
                        craftAmount = 8;
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

            // One in ten crafting attempts to make best gear, the rest is training
            if (_random.Next(10) <= 10)
            {
                // For now, all highest level gear for the extra xp
                Console.WriteLine("Make the best gear we can");
                total = await CraftItemsFromLevelDown(craftAmount, level, items, bankItems);
            }
            else
            {
                Console.WriteLine("Craft training");
                total = await CraftTraining(craftAmount, level, items, bankItems);
            }

            if (total == 0)
            {
                total = await CraftItemsFromLevelUp(craftAmount, level, items, bankItems);
            }

            return total;
        }

        private async Task<int> CraftTraining(int craftAmount, int level, DataPageItemSchema items, List<SimpleItemSchema> bankItems)
        {
            var minLevel = Math.Max(level - 10, 1);
            Console.WriteLine($"Crafting training {craftAmount} items from {minLevel} up");

            int total = 0;
            for (int targetLevel = minLevel; targetLevel < level; targetLevel++)
            {
                var itemsAtLevel = items.Data.Where(x => x.Level == targetLevel);
                Console.WriteLine($"{itemsAtLevel.Count()} potential things to craft at level {targetLevel}");
                var itemsList = new List<ItemSchema>(itemsAtLevel);
                while (itemsList.Any())
                {
                    var item = itemsList.MinBy(item => item.Craft.Items.Sum(c => c.Quantity));
                    Console.WriteLine($"Item with the least components is {item.Code}");

                    total = await _character.CraftItems(item, craftAmount);
                    if (total == 0)
                    {
                        itemsList.Remove(item);
                        continue;
                    }
                    return total;
                }
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
            var itemsList = new List<ItemSchema>(itemsAtLevel);
            while (itemsList.Any())
            {
                var item = itemsList.ElementAt(_random.Next(itemsList.Count()));

                if (!ignoreBankCheck)
                {
                    var currentAmount = await CountAmountOnCharacters(item.Code);
                    currentAmount += bankItems.Where(x => x.Code == item.Code).Sum(x => x.Quantity);
                    if (currentAmount > TooMany)
                    {
                        itemsList.Remove(item);
                        continue;
                    }
                }

                total = await _character.CraftItems(item, craftAmount);
                if (total == 0)
                {
                    itemsList.Remove(item);
                    continue;
                }
                break;
            }

            return total;
        }

        private async Task<int> CountAmountOnCharacters(string code)
        {
            var characters = await _character.GetAllCharacters();
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

            return total;
        }

        private async Task Recycle()
        {
            const int TooMany = 5;

            // If there are too many of this item, try to recycle some
            var bankItems = await Bank.Instance.GetItems();
            foreach (var bankItem in bankItems)
            {
                if (bankItem.Quantity > TooMany)
                {
                    var item = await Items.Instance.GetItem(bankItem.Code);

                    // Can we recycle it?
                    if (item.Craft != null && 
                        (item.Craft.Skill == CraftSkill.Gearcrafting || 
                        item.Craft.Skill == CraftSkill.Jewelrycrafting ||
                        item.Craft.Skill == CraftSkill.Weaponcrafting))
                    {
                        var recycleQuantity = bankItem.Quantity - TooMany;
                        var amount = await _character.WithdrawItems(bankItem.Code, recycleQuantity);
                        if (amount > 0)
                        {
                            try
                            {
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
        }
    }
}
