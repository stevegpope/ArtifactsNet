using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;

namespace Artifacts
{
    internal class Items
    {
        private ItemsApi _api;
        private static Configuration _config;
        private static HttpClient _httpClient;

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

        internal async Task<ItemSchema> GetItem(string code)
        {
            var item = await _api.GetItemItemsCodeGetAsync(code);
            if (item != null)
            {
                return item.Data;
            }
            else
            {
                throw new Exception($"Item with code {code} not found.");
            }
        }
    }
}
