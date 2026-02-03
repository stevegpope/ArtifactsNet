
namespace Artifacts
{
    internal class CharacterLoop
    {
        private string[] _roles;
        private Character _character;

        public CharacterLoop(Character character, string[] roles)
        {
            _roles = roles;
            _character = character;
        }

        internal async Task RunAsync()
        {
            Console.WriteLine($"Running roles {string.Join(", ", _roles)} for {_character.Name}");

            while (true)
            {
                if (_roles.Contains("tasker", StringComparer.OrdinalIgnoreCase))
                {
                    var loop = new TaskerLoop(_character);
                    await loop.RunAsync();
                    return;
                }
                else if (_roles.Contains("fighter"))
                {
                    var loop = new FighterLoop(_character);
                    await loop.RunAsync();
                    return;
                }
                else if (_roles.Contains("crafter"))
                {
                    var loop = new CrafterLoop(_character);
                    await loop.RunAsync();
                    return;
                }
                else if (_roles.Contains("gatherer"))
                {
                    var loop = new GatherLoop(_character);
                    await loop.RunAsync();
                    return;
                }

                throw new NotImplementedException("Other roles are not implemented yet.");
            }
        }
    }
}