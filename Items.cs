using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using static StackExchange.Redis.Role;

namespace Artifacts
{
    internal class Items
    {
        private ItemsApi _api;
        private TasksApi _tasks;
        private static Configuration _config;
        private static HttpClient _httpClient;
        private static Dictionary<string, ItemSchema> _cache = null;
        private List<DropRateSchema> _taskItems;

        internal static Items Instance => lazy.Value;

        const string air = "air";
        const string earth = "earth";
        const string water = "water";
        const string fire = "fire";

        internal static void Config(
            Configuration config,
            HttpClient httpClient
            )
        {
            _config = config;
            _httpClient = httpClient;
        }

        private static readonly Lazy<Items> lazy =
            new(() =>
            {
                if (_config == null || _httpClient == null)
                {
                    throw new InvalidOperationException("Items not configured. Call Items.Config() before accessing the Instance.");
                }
                return new Items(_config, _httpClient);
            });
            
        private Items(
            Configuration config,
            HttpClient httpClient
            )
        {
            _api = new ItemsApi(httpClient, config);
            _tasks = new TasksApi(httpClient, config);
        }

        internal async Task CacheItems()
        {
            if (_cache == null)
            {
                Console.WriteLine("Caching items");
                _cache = new Dictionary<string, ItemSchema>();
                var pageNum = 1;
                while (true)
                {
                    var page = await _api.GetAllItemsItemsGetAsync(page: pageNum);
                    foreach (var item in page.Data)
                    {
                        _cache[item.Code] = item;
                    }

                    pageNum++;
                    if (pageNum == page.Pages)
                    {
                        break;
                    }
                }

                var taskItems = await _tasks.GetAllTasksRewardsTasksRewardsGetAsync();
                _taskItems = taskItems.Data;
            }
        }

        internal async Task<List<DropRateSchema>> GetTaskItems()
        {
            return _taskItems;
        }

        internal ItemSchema GetItem(string code)
        {
            if (_cache.ContainsKey(code))
            { 
                return _cache[code]; 
            }

            throw new Exception($"Item with code {code} not found.");
        }

        internal Dictionary<string, ItemSchema> GetAllItems()
        {
            return _cache;
        }

        internal async Task<DataPageItemSchema> GetItems(CraftSkill skill, int minLevel, int maxLevel)
        {
            var items = await _api.GetAllItemsItemsGetAsync(minLevel: minLevel, maxLevel: maxLevel, craftSkill: skill);
            if (items != null)
            {
                return items;
            }
            else
            {
                throw new Exception($"Failed to find items");
            }
        }

        internal bool IsBetterItem(ItemSchema bestItem, ItemSchema item, MonsterSchema monster, ItemSchema weapon, int maxLevel)
        {
            // Optimization
            if (bestItem != null && item != null && bestItem.Code == item.Code)
            {
                return false;
            }

            var item1Value = CalculateItemValue(bestItem, monster, weapon, maxLevel);
            var item2Value = CalculateItemValue(item, monster, weapon, maxLevel);

            return item2Value > item1Value;
        }

        internal double CalculateItemValue(ItemSchema item, MonsterSchema monster, ItemSchema weapon, int maxLevel)
        {
            if (item.Effects == null || item.Effects.Count == 0)
            {
                return 0;
            }

            if (item.Level > maxLevel)
            {
                // Too high level for us
                Console.WriteLine($"{item.Code} is too high level for us at {item.Level}");
                return 0;
            }

            var value = 0.0;

            var estimatedRounds = monster.Level * 2;

            foreach (var effect in item.Effects)
            {
                switch (effect.Code)
                {
                    case "hp":
                    case "boost_hp":
                    case "threat":
                    case "inventory_space":
                    case "critical_strike":
                    case "wisdom":
                    case "prospecting":
                    case "haste":
                    case "initative":
                        value += effect.Value * 0.1;
                        continue;
                    case "dmg":
                    case "heal":
                    case "healing":
                    case "poison":
                    case "lifesteal":
                    case "reconstitution":
                    case "burn":
                    case "guard":
                    case "shell":
                    case "frenzy":
                    case "void_drain":
                    case "berserker_rage":
                    case "vampiric_strike":
                    case "barrier":
                    case "protective_bubble":
                    case "restore":
                        value += effect.Value * estimatedRounds;
                        continue;
                }

                value += AddBoost(weapon, monster, estimatedRounds, effect);
            }

            value += GetPoisonValue(item, monster, estimatedRounds);
            value += GetResistanceValue(item, monster, estimatedRounds);
            value += GetAttackValueForMonster(item, monster, estimatedRounds);

            Console.WriteLine($"{item.Code} {value} against {monster.Code}");
            return value;
        }

        private static double GetResistanceValue(ItemSchema item, MonsterSchema monster, int estimatedRounds)
        {
            double value = 0;

            // Specific elements count twice
            if (monster.AttackAir > 0)
            {
                value += item.Effects.Where(e => e.Code == "res_air").Sum(e => e.Value) * estimatedRounds;
            }
            if (monster.AttackEarth > 0)
            {
                value += item.Effects.Where(e => e.Code == "res_earth").Sum(e => e.Value) * estimatedRounds;
            }
            if (monster.AttackFire > 0)
            {
                value += item.Effects.Where(e => e.Code == "res_fire").Sum(e => e.Value) * estimatedRounds;
            }
            if (monster.AttackWater > 0)
            {
                value += item.Effects.Where(e => e.Code == "res_water").Sum(e => e.Value) * estimatedRounds;
            }

            return value;
        }

        private static double GetPoisonValue(ItemSchema item, MonsterSchema monster, int estimatedRounds)
        {
            double value = 0;
            if (monster.Effects != null && monster.Effects.Any())
            {
                if (monster.Effects.Any(x => x.Code == "poison"))
                {
                    value += item.Effects.Where(x => x.Code == "antipoison").Sum(x => x.Value) * estimatedRounds;
                }
            }

            return value;
        }

        private static double GetAttackValueForMonster(ItemSchema item, MonsterSchema monster, int estimatedRounds)
        {
            double value = 0;

            value += GetAttackValueWithResistance(monster.ResAir, item, monster, estimatedRounds, air);
            value += GetAttackValueWithResistance(monster.ResEarth, item, monster, estimatedRounds, earth);
            value += GetAttackValueWithResistance(monster.ResWater, item, monster, estimatedRounds, water);
            value += GetAttackValueWithResistance(monster.ResFire, item, monster, estimatedRounds, fire);

            return value;
        }

        private static double GetAttackValueWithResistance(int resValue, ItemSchema item, MonsterSchema monster, int estimatedRounds, string element)
        {
            // From the docs:
            // Round(Attack * Round(Resistance * 0.01))

            var attack = GetAttackValueForElement(item, element);
            var res = 1.0;
            if (resValue < 0)
            {
                // Negative res value means this element is better
                res = 1 + Math.Abs(resValue) * 0.01;
            }
            else if (resValue > 0)
            {
                // Positive res value means this element is resisted
                res = resValue * 0.01;
            }

            var result = attack * res;
            return result * estimatedRounds;
        }

        private static double GetAttackValueForElement(ItemSchema item, string element)
        {
            var prefix = "attack_";
            var code = prefix + element;
            return item.Effects.Where(e => e.Code == code).Sum(e => e.Value);
        }

        internal static double AddBoost(ItemSchema weapon, MonsterSchema monster, int estimatedRounds, SimpleEffectSchema effect)
        {
            var value = 0.0;

            if (weapon != null && (effect.Code.StartsWith("boost_") || effect.Code.StartsWith("dmg_")))
            {
                var element = effect.Code.Substring(effect.Code.LastIndexOf('_') + 1);
                if (!new[] { air, water, earth, fire }.Any(x => x == element))
                {
                    return value;
                }

                foreach (var weaponEffect in weapon.Effects)
                {
                    if (weaponEffect.Code.StartsWith("attack") || weaponEffect.Code.StartsWith("dmg_"))
                    {
                        var weaponElement = weaponEffect.Code.Substring(weaponEffect.Code.LastIndexOf('_') + 1);
                        if (weaponElement == element)
                        {
                            value += effect.Value * estimatedRounds;

                            switch (element)
                            {
                                case air:
                                    if (monster.ResAir < 0) value += effect.Value * estimatedRounds;
                                    break;
                                case water:
                                    if (monster.ResWater < 0) value += effect.Value * estimatedRounds;
                                    break;
                                case earth:
                                    if (monster.ResEarth < 0) value += effect.Value * estimatedRounds;
                                    break;
                                case fire:
                                    if (monster.ResFire < 0) value += effect.Value * estimatedRounds;
                                    break;
                            }
                        }
                    }
                }
            }

            return value;
        }

        internal bool ItemTypeMatchesSlot(string type, ItemSlot slotType)
        {
            switch (slotType)
            {
                case ItemSlot.Weapon:
                    return "weapon" == type;
                case ItemSlot.Boots:
                    return "boots" == type;
                case ItemSlot.Artifact1:
                case ItemSlot.Artifact2:
                case ItemSlot.Artifact3:
                    return "artifact" == type;
                case ItemSlot.Amulet:
                    return "amulet" == type;
                case ItemSlot.Bag:
                    return "bag" == type;
                case ItemSlot.BodyArmor:
                    return "body_armor" == type;
                case ItemSlot.Helmet:
                    return "helmet" == type;
                case ItemSlot.LegArmor:
                    return "leg_armor" == type;
                case ItemSlot.Ring1:
                case ItemSlot.Ring2:
                    return "ring" == type;
                case ItemSlot.Rune:
                    return "rune" == type;
                case ItemSlot.Shield:
                    return "shield" == type;
                case ItemSlot.Utility1:
                case ItemSlot.Utility2:
                    return "utility" == type;
                default:
                    throw new NotImplementedException();
            }
        }

        internal bool IsBetterItemSkill(ItemSchema bestItem, ItemSchema item, string skill)
        {
            // Optimization
            if (bestItem != null && item != null && bestItem.Code == item.Code)
            {
                return false;
            }

            var item1Value = CalculateItemValueSkill(bestItem, skill);
            var item2Value = CalculateItemValueSkill(item, skill);

            return item2Value > item1Value;
        }

        internal double CalculateItemValueSkill(ItemSchema item, string skill)
        {
            if (item == null)
            {
                return 0;
            }

            if (item.Effects == null || item.Effects.Count == 0)
            {
                return 0;
            }

            var value = 0.0;
            foreach (var effect in item.Effects)
            {
                // These effects have negative values!
                if (effect.Code == skill) { value -= effect.Value; }

                // These are normal, with prospecting being the most important for drops
                if (effect.Code == "wisdom") { value += effect.Value; }
                if (skill != "crafting")
                {
                    if (effect.Code == "prospecting") { value += effect.Value * 5; }
                }
            }

            return value;
        }
    }
}
