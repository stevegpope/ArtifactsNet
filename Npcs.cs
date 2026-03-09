using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;

namespace Artifacts
{
    internal class Npcs
    {
        private NPCsApi _api;
        private static Configuration _config;
        private static HttpClient _httpClient;
        private static Dictionary<string, NPCSchema> _cache;

        internal static Npcs Instance => lazy.Value;

        internal static void Config(
            Configuration config,
            HttpClient httpClient
            )
        {
            _config = config;
            _httpClient = httpClient;
        }

        private static readonly Lazy<Npcs> lazy =
            new(() =>
            {
                if (_config == null || _httpClient == null)
                {
                    throw new InvalidOperationException("Events not configured. Call Events.Config() before accessing the Instance.");
                }
                return new Npcs(_config, _httpClient);
            });
            
        private Npcs(
            Configuration config,
            HttpClient httpClient
            )
        {
            _api = new NPCsApi(httpClient, config);
        }

        internal async Task CacheNpcs()
        {
            if (_cache == null)
            {
                Console.WriteLine("Caching NPCs");
                _cache = new Dictionary<string, NPCSchema>();
                var pageNum = 1;
                while (true)
                {
                    var page = await _api.GetAllNpcsNpcsDetailsGetAsync(page: pageNum);
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

        internal static Dictionary<string, NPCSchema> GetAllNpcs()
        {
            return _cache;
        }

        internal List<SimpleNPCItem> GetNpcItems(string npcCode)
        {
            return _cache[npcCode].Items;
        }
    }
}
