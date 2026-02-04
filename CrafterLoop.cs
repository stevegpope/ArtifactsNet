
using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;
using System.Net.Security;

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
                // Choose a craft skill
                var skills = new[]
                {
                    "weaponcrafting",
                    "gearcrafting",
                    "jewelrycrafting",
                    "alchemy",
                    "cooking",
                };

                var skill = skills.ElementAt(_random.Next(skills.Length));

                int craftAmount = 1;
                switch (skill)
                {
                    case "weaponcrafting":
                        break;
                    case "gearcrafting":
                        break;
                    case "jewelrycrafting":
                        break;
                    case "alchemy":
                        craftAmount = 8;
                        break;
                    case "cooking":
                        craftAmount = 8;
                        break;
                    case "mining":
                        craftAmount = 8;
                        break;
                    case "woodcutting":
                        craftAmount = 8;
                        break;
                    default:
                        throw new Exception($"Unexpected skill {skill}");

                }

                var craftSkill = Utils.GetSkillCraft(skill);
                var level = Utils.GetSkillLevel(skill);
                Console.WriteLine($"Crafter chose skill {skill} at level {level} with {craftSkill}");

                // Choose the highest level thing we can craft
                var items = await Items.Instance.GetItems(skill: craftSkill, minLevel: 0, maxLevel: level);
                Console.WriteLine($"{items.Data.Count} potential things to craft");
                ItemSchema item = null;

                var bankItems = await Bank.Instance.GetItems();

                var total = 0;
                ItemSchema itemCrafted = null;
                for (int targetLevel = level; targetLevel > 0; targetLevel--)
                {
                    var itemsAtLevel = items.Data.Where(x => x.Level == targetLevel);
                    Console.WriteLine($"{itemsAtLevel.Count()} potential things to craft at level {targetLevel}");
                    var itemsList = new List<ItemSchema>(itemsAtLevel);
                    while (itemsList.Any())
                    {
                        item = itemsList.ElementAt(_random.Next(itemsList.Count()));
                        total = await _character.CraftItems(item, craftAmount);
                        if (total == 0)
                        {
                            itemsList.Remove(item);
                            continue;
                        }
                        itemCrafted = item;
                        break;
                    }

                    if (total > 0)
                    {
                        break;
                    }
                }

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

        private async Task Recycle()
        {
            const int TooMany = 5;

            // If there are too many of this item, try to recycle some
            var bankItems = await Bank.Instance.GetItems();
            foreach (var bankItem in bankItems)
            {
                if (bankItem.Quantity > TooMany * 2)
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
