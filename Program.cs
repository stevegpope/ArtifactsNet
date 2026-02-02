using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Artifacts
{
    class Program
    {
        static async Task Main(
            string[] args
            )
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: Artifacts <character-name> <roles>");
                return;
            }

            var name = args[0];
            var roles = args[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            Console.WriteLine($"Starting {name} roles {roles.ToJson()}");

            string tokenValue = File.ReadAllText("token.txt").Trim();

            using var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://api.artifactsmmo.com")
            };

            var configuration = new Configuration()
            {
                AccessToken = tokenValue,
                UserAgent = "ArtifactsMmoClient/1.0.0/ArtifactsBot",
                BasePath = "https://api.artifactsmmo.com"
            };
            
            Map.Config(configuration, httpClient);
            Items.Config(configuration, httpClient);
            Resources.Config(configuration, httpClient);
            Monsters.Config(configuration, httpClient);
            Bank.Config(configuration, httpClient);
            Characters.Config(httpClient, configuration);

            var character = new Character(
                configuration,
                httpClient,
                name);

            // Load character details to verify it exists
            await character.Init();

            // Start at the bank with no inventory
            await character.MoveTo(MapContentType.Bank);
            await character.DepositAllItems();

            var loop = new CharacterLoop(character, roles);
            await loop.RunAsync();
        }
    }
}
