
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
                Console.WriteLine($"Crafter chose skill {skill}");

                int level = 0;
                int craftAmount = 1;
                CraftSkill craftSkill;
                switch (skill)
                {
                    case "weaponcrafting":
                        level = Utils.Details.WeaponcraftingLevel;
                        craftSkill = CraftSkill.Weaponcrafting;
                        break;
                    case "gearcrafting":
                        level = Utils.Details.GearcraftingLevel;
                        craftSkill = CraftSkill.Gearcrafting;
                        break;
                    case "jewelrycrafting":
                        level = Utils.Details.JewelrycraftingLevel;
                        craftSkill = CraftSkill.Jewelrycrafting;
                        break;
                    case "alchemy":
                        level = Utils.Details.AlchemyLevel;
                        craftSkill = CraftSkill.Alchemy;
                        craftAmount = 8;
                        break;
                    case "cooking":
                        level = Utils.Details.CookingLevel;
                        craftSkill = CraftSkill.Cooking;
                        craftAmount = 8;
                        break;
                    default:
                        throw new Exception($"Unexpected skill {skill}");

                }

                // Choose the highest level thing we can craft
                var items = await Items.Instance.GetItems(skill: craftSkill, minLevel: 0, maxLevel: level);
                ItemSchema item = null;

                var bankItems = await Bank.Instance.GetItems();

                var total = 0;
                ItemSchema itemCrafted = null;
                for (int targetLevel = level; targetLevel >= 0; targetLevel--)
                {
                    // For now, do not build things that we have too many
                    var itemsAtLevel = items.Data.Where(x => x.Level == targetLevel && !bankItems.Any(item => item.Code == x.Code && item.Quantity >= 50));
                    var itemsList = new List<ItemSchema>(itemsAtLevel);
                    while (itemsList.Any())
                    {
                        item = itemsList.ElementAt(_random.Next(itemsList.Count()));
                        total = await _character.CraftItems(item, craftAmount);
                        if (total == 0)
                        {
                            itemsList.Remove(item);
                        }
                        itemCrafted = item;
                        break;
                    }
                }

                if (total == 0)
                {
                    Console.WriteLine($"Nothing we can craft for skill {skill}");

                    // In case we loop
                    Thread.Sleep(5000);
                    continue;
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
                                await _character.Recycle(item.Code, amount);
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
