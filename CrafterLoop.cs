
using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Model;
using System.Net.Security;

namespace Artifacts
{
    internal class CrafterLoop
    {
        private Character _character;
        private Random _random = new Random();

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
                    "woodcutting",
                    "mining",
                };

                var skill = skills.ElementAt(_random.Next(skills.Length - 1));
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
                        craftAmount = 10;
                        break;
                    case "cooking":
                        level = Utils.Details.CookingLevel;
                        craftSkill = CraftSkill.Cooking;
                        craftAmount = 10;
                        break;
                    case "woodcutting":
                        level = Utils.Details.WoodcuttingLevel;
                        craftSkill = CraftSkill.Woodcutting;
                        craftAmount = 10;
                        break;
                    case "mining":
                        level = Utils.Details.MiningLevel;
                        craftSkill = CraftSkill.Mining;
                        craftAmount = 10;
                        break;
                    default:
                        throw new Exception($"Unexpected skill {skill}");

                }

                // Choose the highest level thing we can craft
                var items = await Items.Instance.GetItems(skill: craftSkill, minLevel: 0, maxLevel: level);
                ItemSchema item = null;

                for (int targetLevel = level; targetLevel >= 0; targetLevel--)
                {
                    var itemsAtLevel = items.Data.Where(x => x.Level == targetLevel);
                    if (itemsAtLevel.Any())
                    {
                        item = itemsAtLevel.ElementAt(_random.Next(itemsAtLevel.Count() - 1));
                        break;
                    }
                }

                if (item == null)
                {
                    Console.WriteLine($"Cannot craft any items between level {0} and {level}");
                    continue;
                }

                var total = await _character.CraftItems(item, craftAmount);
                if (total == 0)
                {
                    Console.WriteLine($"Failed to craft {item.Code}");
                }

                // Go deposit the results in the bank
                await _character.MoveTo(MapContentType.Bank);
                await _character.DepositAllItems();
            }
        }
    }
}
