
using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Model;

namespace Artifacts
{
    internal class FighterLoop
    {
        private Character _character;
        private Random _random = Random.Shared;

        public FighterLoop(Character character)
        {
            _character = character;
        }

        internal async Task RunAsync()
        {
            Console.WriteLine($"Starting fighter loop");

            while (true)
            {
                // Choose a monster lower level than us
                var level = Utils.Details.Level - 1;
                var candidates = Monsters.Instance.GetMonsters(maxLevel: level);

                if (candidates == null || candidates.Data.Count == 0)
                {
                    throw new ArgumentException($"No candidates available below level {Utils.Details.Level}");
                }

                // Pick a random one
                var monster = candidates.Data.ElementAt(_random.Next(candidates.Data.Count));

                Console.WriteLine($"Fighter chose monster {monster}");

                // TODO: more fights than this after testing
                await _character.FightLoop(2, "cow");
            }
        }
    }
}
