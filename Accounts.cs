using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;

namespace Artifacts
{
    internal class Accounts
    {
        private AccountsApi _api;
        private static Configuration _config;
        private static HttpClient _httpClient;

        internal static Accounts Instance => lazy.Value;

        internal static void Config(
            Configuration config,
            HttpClient httpClient
            )
        {
            _config = config;
            _httpClient = httpClient;
        }

        private static readonly Lazy<Accounts> lazy =
            new(() =>
            {
                if (_config == null || _httpClient == null)
                {
                    throw new InvalidOperationException("Items not configured. Call Items.Config() before accessing the Instance.");
                }
                return new Accounts(_config, _httpClient);
            });
            
        private Accounts(
            Configuration config,
            HttpClient httpClient
            )
        {
            _api = new AccountsApi(httpClient, config);
        }

        internal async Task<List<AccountAchievementSchema>> GetAchievements()
        {
            var result = await Utils.ApiCallGet(async () => await _api.GetAccountAchievementsAccountsAccountAchievementsGetAsync("stevegpope", completed: true)) as DataPageAccountAchievementSchema;
            return result.Data;
        }
    }
}
