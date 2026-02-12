using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;

namespace Artifacts
{
    internal class Events
    {
        private EventsApi _api;
        private static Configuration _config;
        private static HttpClient _httpClient;

        internal static Events Instance => lazy.Value;

        internal static void Config(
            Configuration config,
            HttpClient httpClient
            )
        {
            _config = config;
            _httpClient = httpClient;
        }

        private static readonly Lazy<Events> lazy =
            new(() =>
            {
                if (_config == null || _httpClient == null)
                {
                    throw new InvalidOperationException("Events not configured. Call Events.Config() before accessing the Instance.");
                }
                return new Events(_config, _httpClient);
            });
            
        private Events(
            Configuration config,
            HttpClient httpClient
            )
        {
            _api = new EventsApi(httpClient, config);
        }

        internal async Task<List<ActiveEventSchema>> GetActiveEvents()
        {
            var events = await Utils.ApiCall(async () =>
            {
                return await _api.GetAllActiveEventsEventsActiveGetAsync();
            });

            if (events is DataPageActiveEventSchema eventPage)
            {
                return eventPage.Data;
            }

            return Enumerable.Empty<ActiveEventSchema>().ToList();
        }
    }
}
