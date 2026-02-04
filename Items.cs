using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;
using System.Threading.Tasks;

namespace Artifacts
{
    internal class Items
    {
        private ItemsApi _api;
        private static Configuration _config;
        private static HttpClient _httpClient;
        private static Dictionary<string, ItemSchema> _cache = null;

        internal static Items Instance => lazy.Value;

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
        }

        private async Task CacheItems()
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
            }
        }

        internal async Task<ItemSchema> GetItem(string code)
        {
            await CacheItems();
            if (_cache.ContainsKey(code))
            { 
                return _cache[code]; 
            }

            throw new Exception($"Item with code {code} not found.");
        }

        internal async Task<Dictionary<string, ItemSchema>> GetAllItems()
        {
            await CacheItems();
            return _cache;
        }

        internal async Task<DataPageItemSchema> GetItems(CraftSkill skill, int minLevel, int maxLevel)
        {
            await CacheItems();

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

        internal bool IsBetterItem(ItemSchema bestItem, ItemSchema item, MonsterSchema monster)
        {
            // Optimization
            if (bestItem != null && item != null && bestItem.Code == item.Code)
            {
                return false;
            }

            var item1Value = CalculateItemValue(bestItem, monster);
            var item2Value = CalculateItemValue(item, monster);

            return item2Value > item1Value;
        }

        internal double CalculateItemValue(ItemSchema item, MonsterSchema monster)
        {
            if (item.Effects == null || item.Effects.Count == 0)
            {
                return 0;
            }

            if (item.Level > Utils.Details.Level)
            {
                // Too high level for us
                return 0;
            }

            var value = 0.0;

            var estimatedRounds = monster.Level * 2;

            foreach (var effect in item.Effects)
            {
                switch(effect.Code)
                {
                    case "hp":
                    case "boost_hp":
                    case "threat":
                    case "critical_strike":
                        value += effect.Value;
                        break;
                    case "dmg":
                    case "dmg_earth":
                    case "dmg_water":
                    case "dmg_air":
                    case "dmg_fire":
                    case "attack_earth":
                    case "attack_water":
                    case "attack_air":
                    case "attack_fire":
                    case "heal":
                    case "restore":
                    case "boost_dmg_earth":
                    case "boost_dmg_water":
                    case "boost_dmg_fire":
                    case "boost_dmg_air":
                        value += effect.Value * estimatedRounds;
                        break;

                }
            }

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

            const string atk_air = "attack_air";
            const string atk_earth = "attack_earth";
            const string atk_fire = "attack_fire";
            const string atk_water = "attack_water";
            const string boost_air = "boost_dmg_air";
            const string boost_earth = "boost_dmg_earth";
            const string boost_fire = "boost_dmg_fire";
            const string boost_water = "boost_dmg_water";

            // We will double the damage calculation for elements other than the ones they resist
            if (monster.ResAir > 0)
            {
                value += item.Effects.Where(e => e.Code == atk_earth).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == atk_fire).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == atk_water).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == boost_earth).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == boost_fire).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == boost_water).Sum(e => e.Value) * estimatedRounds;
            }
            else if (monster.ResAir < 0)
            {
                value += item.Effects.Where(e => e.Code == atk_air).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == boost_air).Sum(e => e.Value) * estimatedRounds;
            }

            if (monster.ResEarth > 0)
            {
                value += item.Effects.Where(e => e.Code == atk_air).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == atk_fire).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == atk_water).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == boost_air).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == boost_fire).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == boost_water).Sum(e => e.Value) * estimatedRounds;
            }
            else if (monster.ResEarth < 0)
            {
                value += item.Effects.Where(e => e.Code == atk_earth).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == boost_earth).Sum(e => e.Value) * estimatedRounds;
            }

            if (monster.ResFire > 0)
            {
                value += item.Effects.Where(e => e.Code == atk_earth).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == atk_air).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == atk_water).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == boost_earth).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == boost_air).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == boost_water).Sum(e => e.Value) * estimatedRounds;
            }
            else if (monster.ResFire < 0)
            {
                value += item.Effects.Where(e => e.Code == atk_fire).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == boost_fire).Sum(e => e.Value) * estimatedRounds;
            }

            if (monster.ResWater > 0)
            {
                value += item.Effects.Where(e => e.Code == atk_earth).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == atk_fire).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == atk_air).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == boost_earth).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == boost_fire).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == boost_air).Sum(e => e.Value) * estimatedRounds;
            }
            else if (monster.ResWater < 0)
            {
                value += item.Effects.Where(e => e.Code == atk_water).Sum(e => e.Value) * estimatedRounds;
                value += item.Effects.Where(e => e.Code == boost_water).Sum(e => e.Value) * estimatedRounds;
            }

            Console.WriteLine($"{item.Code} has value {value} against monster {monster.Code}");
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
            }

            return value;
        }
    }
}
