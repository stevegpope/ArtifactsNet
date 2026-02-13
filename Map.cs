using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;
using System.Threading.Tasks;

namespace Artifacts
{
    internal class Map
    {
        private MapsApi _api;
        private static Configuration _config;
        private static HttpClient _httpClient;

        internal static Map Instance => lazy.Value;

        internal static void Config(
            Configuration config,
            HttpClient httpClient
            )
        {
            _config = config;
            _httpClient = httpClient;
        }

        private static readonly Lazy<Map> lazy =
            new(() =>
            {
                if (_config == null || _httpClient == null)
                {
                    throw new InvalidOperationException("Map not configured. Call Map.Config() before accessing the Instance.");
                }
                return new Map(_config, _httpClient);
            });
            
        private Map(
            Configuration config,
            HttpClient httpClient
            )
        {
            _api = new MapsApi(httpClient, config);
        }

        internal async Task<DataPageMapSchema> GetMapLayer(MapContentType contentType, string code = null, MapLayer layer = MapLayer.Overworld)
        {
            var mapSchema = await Utils.ApiCall(async () => await _api.GetLayerMapsMapsLayerGetAsync(layer, contentType, code));
            return mapSchema as DataPageMapSchema;
        }

        internal async Task<MapSchema> GetMapPosition(int x, int y)
        {
            var mapResponseSchema = await Utils.ApiCall(async () => await _api.GetMapByPositionMapsLayerXYGetAsync(MapLayer.Overworld, x, y));
            return (mapResponseSchema as MapResponseSchema).Data;
        }
    }
}
