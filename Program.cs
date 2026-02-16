using ArtifactsMmoClient.Client;

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
                Console.WriteLine("Usage: Artifacts <character-name> <roles> <optional-args>");
                return;
            }
            Console.WriteLine($"Args init {args.Length} {string.Join(",", args)}");

            var name = args[0];
            var role = args[1];
            string[] arguments = null;
            if (args.Length > 2)
            {
                arguments = args.Skip(4).ToArray();
                Console.WriteLine($"Starting {name} {role} {string.Join(",", arguments)}");
            }
            else
            {
                Console.WriteLine($"Starting {name} {role}");
            }

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
            await Items.Instance.CacheItems();

            Monsters.Config(configuration, httpClient);
            await Monsters.Instance.CacheMonsters();

            Events.Config(configuration, httpClient);
            Npcs.Config(configuration, httpClient);
            Resources.Config(configuration, httpClient);
            Bank.Config(configuration, httpClient);
            Characters.Config(httpClient, configuration);

            var character = new Character(
                configuration,
                httpClient,
                name);

            // Load character details to verify it exists
            await character.Init();
            Utils.Character = character;

            var loop = new CharacterLoop(character, role, arguments);
            await loop.RunAsync();
        }
    }
}
