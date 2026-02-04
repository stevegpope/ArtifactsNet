
using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Model;
using System.Net.Security;

namespace Artifacts
{
    internal class GatherLoop
    {
        private Character _character;
        private Random _random = Random.Shared;

        public GatherLoop(Character character)
        {
            _character = character;
        }

        internal async Task RunAsync()
        {
            Console.WriteLine($"Starting gather loop");

            while (true)
            {
                // Choose a craft skill
                var skills = new[]
                {
                    "alchemy",
                    "woodcutting",
                    "mining",
                };

                var skill = skills.ElementAt(_random.Next(skills.Length));
                Console.WriteLine($"Gather chose skill {skill}");

                // Get the highest level thing we can gather, and bank it
                await _character.TrainGathering(Utils.GetSkillLevel(skill), skill);
            }
        }
    }
}
