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
        private static Dictionary<string, ResourceSchema> _cache = null;

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

        internal async Task CacheItems()
        {
            if (_cache == null)
            {
                Console.WriteLine("Caching resources");
                _cache = new Dictionary<string, ResourceSchema>();
                var pageNum = 1;
                while (true)
                {
                    var page = await _api.GetAllResourcesResourcesGetAsync();
                    foreach (var item in page.Data)
                    {
                        _cache[item.Code] = item;
                    }

                    pageNum++;
                    if (pageNum >= page.Pages)
                    {
                        break;
                    }
                }
            }
        }

        internal static ResourceSchema GetResource(string resourceCode)
        {
            if (_cache.TryGetValue(resourceCode, out var resource))
            {
                return resource; 
            }
            return null;
        }

        internal async Task<ResourceSchema> GetResourceDrop(string drop)
        {
            return _cache.Values.FirstOrDefault(resource => resource.Drops.Any(d => d.Code == drop));
        }

        internal async Task<string> GetResourceSkill(ItemSchema item)
        {
            foreach(var cacheItem in _cache.Values)
            {
                if (item.Code ==  cacheItem.Code)
                {
                    return cacheItem.Skill.ToString().ToLower();
                }
                else if (cacheItem.Drops.Any(drop => drop.Code == item.Code))
                {
                    return cacheItem.Skill.ToString().ToLower();
                }
            }

            return null;
        }

        internal Dictionary<string, ResourceSchema> GetAllResources()
        {
            return _cache;
        }
    }
}
