using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Artifacts
{
    internal class Map
    {
        private MapsApi _api;
        private static Configuration _config;
        private static HttpClient _httpClient;
        private List<MapSchema> maps;
        private DateTime LastUpdate = DateTime.MinValue;

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

        internal async Task<List<MapSchema>> GetAllMaps()
        {
            // Cache the map for 5 minutes
            if (maps == null || LastUpdate < DateTime.UtcNow - TimeSpan.FromMinutes(5))
            {
                LastUpdate = DateTime.UtcNow;
                var mapSchema = await Utils.ApiCallGet(async () => await _api.GetAllMapsMapsGetAsync(size: 10000));
                maps = (mapSchema as StaticDataPageMapSchema).Data;
            }

            return maps;
        }
           

        internal async Task<List<MapSchema>> GetMapLayer(MapContentType contentType, string code = null, MapLayer layer = MapLayer.Overworld)
        {
            var maps = await GetAllMaps();
            var content = maps.Where(m => m?.Interactions?.Content?.Type == contentType);
            if (code != null)
            {
                content = content.Where(m => m?.Interactions?.Content?.Code == code);
            }

            return content.Where(m => m.Layer == layer).ToList();
        }

        internal async Task<MapSchema> GetMapPosition(int x, int y, MapLayer layer = MapLayer.Overworld)
        {
            var maps = await GetAllMaps();
            return maps.FirstOrDefault(m => m.X == x && m.Y == y && m.Layer == layer);
        }
    }
}
