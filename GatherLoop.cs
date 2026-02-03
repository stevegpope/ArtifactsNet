
using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Model;
using System.Net.Security;

namespace Artifacts
{
    internal class GatherLoop
    {
        private Character _character;
        private Random _random = Random.Shared;

        public GatherLoop(Character character)
        {
            _character = character;
        }

        internal async Task RunAsync()
        {
            Console.WriteLine($"Starting gather loop");

            while (true)
            {
                // Choose a craft skill
                var skills = new[]
                {
                    "alchemy",
                    //"woodcutting",
                    //"mining",
                };

                var skill = skills.ElementAt(_random.Next(skills.Length));
                Console.WriteLine($"Gather chose skill {skill}");

                int level = 0;
                CraftSkill craftSkill;
                switch (skill)
                {
                    case "alchemy":
                        level = Utils.Details.AlchemyLevel;
                        craftSkill = CraftSkill.Alchemy;
                        break;
                    case "woodcutting":
                        level = Utils.Details.WoodcuttingLevel;
                        craftSkill = CraftSkill.Woodcutting;
                        break;
                    case "mining":
                        level = Utils.Details.MiningLevel;
                        craftSkill = CraftSkill.Mining;
                        break;
                    default:
                        throw new Exception($"Unexpected skill {skill}");

                }

                // Choose the highest level thing we can gather

                var items = await Items.Instance.GetAllItems();
                var gatherItems = items.Where(i => i.Value.Craft == null && i.Value.Subtype == craftSkill.ToString().ToLower());
                ItemSchema item = null;

                for (int targetLevel = level; targetLevel >= 0; targetLevel--)
                {
                    var itemsAtLevel = gatherItems.Where(x => x.Value.Level == targetLevel).Select(x => x.Value);
                    var itemsList = new List<ItemSchema>(itemsAtLevel);
                    while (itemsList.Any())
                    {
                        item = itemsList.ElementAt(_random.Next(itemsList.Count()));
                        var total = await _character.GatherItems(item.Code, 50, skipBank: true);
                        if (total == 0)
                        {
                            itemsList.Remove(item);
                        }
                        break;
                    }
                }

                // Go deposit the results in the bank
                await _character.MoveTo(MapContentType.Bank);
                await _character.DepositAllItems();
            }
        }
    }
}
