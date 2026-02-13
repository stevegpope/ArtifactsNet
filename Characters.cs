using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace Artifacts
{
    internal class Characters
    {
        internal readonly CharactersApi _charactersApi;
        private static Lazy<Characters>? _instance;

        internal static Characters Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("Characters class is not initialized. Call Init() first.");
                }
                return _instance.Value;
            }
        }

        private Characters(
            HttpClient httpClient,
            Configuration configuration
            )
        {
            _charactersApi = new CharactersApi(httpClient, configuration);
        }

        internal static void Config(
            HttpClient httpClient,
            Configuration configuration
            )
        {
            _instance = new Lazy<Characters>(() => new Characters(httpClient, configuration));
        }

        internal async Task<CharacterSchema> GetDetailsAsync(string name)
        {
            var result = await Utils.ApiCall(async () => await _charactersApi.GetCharacterCharactersNameGetAsync(name)) as CharacterResponseSchema;
            return result.Data;
        }

        internal async Task<List<ActiveCharacterSchema>> GetCharacters()
        {
            var result = await Utils.ApiCall(async () => await _charactersApi.GetActiveCharactersCharactersActiveGetAsync()) as DataPageActiveCharacterSchema;
            return result.Data;
        }
    }
}
