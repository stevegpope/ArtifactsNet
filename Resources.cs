using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;

namespace Artifacts
{
    internal class Resources
    {
        private ResourcesApi _api;
        private static Configuration _config;
        private static HttpClient _httpClient;

        internal static Resources Instance => lazy.Value;

        internal static void Config(
            Configuration config,
            HttpClient httpClient
            )
        {
            _config = config;
            _httpClient = httpClient;
        }

        private static readonly Lazy<Resources> lazy =
            new(() =>
            {
                if (_config == null || _httpClient == null)
                {
                    throw new InvalidOperationException("Map not configured. Call Map.Config() before accessing the Instance.");
                }
                return new Resources(_config, _httpClient);
            });
            
        private Resources(
            Configuration config,
            HttpClient httpClient
            )
        {
            _api = new ResourcesApi(httpClient, config);
        }

        internal async Task<string> GetResourceDrop(string drop)
        {
            string resourceCode = null;
            await Utils.ApiCall(async () =>
            {
                var result = await _api.GetAllResourcesResourcesGetAsync(drop: drop);
                var data = result.Data;
                if (data.Any())
                {
                    resourceCode = result.Data.FirstOrDefault().Code;
                }
                return result;
            });

            return resourceCode;
        }
    }
}
