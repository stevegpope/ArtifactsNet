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

        internal static Monsters Instance => lazy.Value;

        internal static void Config(
            Configuration config,
            HttpClient httpClient
            )
        {
            _config = config;
            _httpClient = httpClient;
        }

        internal DataPageMonsterSchema GetMonsters(int maxLevel, string dropCode = null)
        {
            var monsters = _api.GetAllMonstersMonstersGet(maxLevel: maxLevel, drop: dropCode);
            return monsters;
        }

        internal async Task<MonsterSchema> GetMonster(string monsterCode)
        {
            try
            {
                var response = await _api.GetMonsterMonstersCodeGetAsync(monsterCode);
                return response.Data;
            }
            catch(ApiException ex)
            {
                Console.WriteLine(ex.ErrorContent);
                return null;
            }
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
