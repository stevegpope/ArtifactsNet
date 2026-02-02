using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;

namespace Artifacts
{
    internal class Bank
    {
        private MyAccountApi _api;
        private static Configuration _config;
        private static HttpClient _httpClient;

        internal static Bank Instance => lazy.Value;

        internal static void Config(
            Configuration config,
            HttpClient httpClient
            )
        {
            _config = config;
            _httpClient = httpClient;
        }

        private static readonly Lazy<Bank> lazy =
            new(() =>
            {
                if (_config == null || _httpClient == null)
                {
                    throw new InvalidOperationException("Items not configured. Call Items.Config() before accessing the Instance.");
                }
                return new Bank(_config, _httpClient);
            });
            
        private Bank(
            Configuration config,
            HttpClient httpClient
            )
        {
            _api = new MyAccountApi(httpClient, config);
        }

        internal async Task<List<SimpleItemSchema>> GetItems()
        {
            var result = await _api.GetBankItemsMyBankItemsGetAsync();
            return result.Data;
        }
    }
}
