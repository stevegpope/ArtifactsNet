using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;

namespace Artifacts
{
    internal class Monsters
    {
        private MonstersApi _api;
        private static Configuration _config;
        private static HttpClient _httpClient;
        private static Dictionary<string, MonsterSchema> _cache = null;

        internal static Monsters Instance => lazy.Value;

        internal static void Config(
            Configuration config,
            HttpClient httpClient
            )
        {
            _config = config;
            _httpClient = httpClient;
        }

        internal async Task CacheMonsters()
        {
            if (_cache == null)
            {
                Console.WriteLine("Caching monsters");
                _cache = new Dictionary<string, MonsterSchema>();
                var pageNum = 1;
                while (true)
                {
                    var page = await _api.GetAllMonstersMonstersGetAsync(page: pageNum);
                    foreach (var item in page.Data)
                    {
                        _cache[item.Code] = item;
                    }

                    pageNum++;
                    if (pageNum > page.Pages)
                    {
                        break;
                    }
                }
            }
        }

        internal DataPageMonsterSchema GetMonsters(int maxLevel, string dropCode = null)
        {
            var monsters = _api.GetAllMonstersMonstersGet(maxLevel: maxLevel, drop: dropCode);
            return monsters;
        }

        internal async Task<MonsterSchema> GetMonster(string monsterCode)
        {
            if (_cache.TryGetValue(monsterCode, out var monster))
            {
                return monster; 
            }

            throw new Exception($"Unknown monster: {monsterCode}");
        }

        private static readonly Lazy<Monsters> lazy =
            new(() =>
            {
                if (_config == null || _httpClient == null)
                {
                    throw new InvalidOperationException("Monsters not configured. Call Monsters.Config() before accessing the Instance.");
                }
                return new Monsters(_config, _httpClient);
            });
            
        private Monsters(
            Configuration config,
            HttpClient httpClient
            )
        {
            _api = new MonstersApi(httpClient, config);
        }
    }
}
