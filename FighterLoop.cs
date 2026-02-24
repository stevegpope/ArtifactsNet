
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
            await _character.TrainFighting(10);
        }
    }
}
