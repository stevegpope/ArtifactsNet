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

        internal async Task<NPCItem> FindNpcItem(string code)
        {
            var page = await Utils.ApiCallGet(async () =>
            {
                return await _api.GetAllNpcsItemsNpcsItemsGetAsync(code);
            }) as DataPageNPCItem;

            if (page == null || page.Data == null || page.Data.Count == 0)
            {
                return null;
            }

            return page.Data.First();
        }

        internal async Task<List<NPCItem>> GetNpcItems(string code)
        {
            var items = await Utils.ApiCallGet(async () =>
            {
                return await _api.GetNpcItemsNpcsItemsCodeGetAsync(code);
            });

            if (items is DataPageNPCItem itemPage)
            {
                return itemPage.Data;
            }

            return Enumerable.Empty<NPCItem>().ToList();
        }
    }
}
