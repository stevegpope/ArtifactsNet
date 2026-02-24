
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;

namespace Artifacts
{
    internal class CharacterLoop
    {
        private string _role;
        private readonly IEnumerable<string> _arguments;
        private Character _character;

        public CharacterLoop(Character character, string role, IEnumerable<string> arguments)
        {
            _role = role;
            _arguments = arguments;
            _character = character;
        }

        internal async Task RunAsync()
        {
            Console.WriteLine($"Running roles {string.Join(", ", _role)} for {_character.Name}");
            if (_arguments != null && _arguments.Any())
            {
                Console.WriteLine($"Arguments {string.Join(',', _arguments)}");
            }

            // Start at the bank with no inventory
            await _character.MoveTo(MapContentType.Bank);
            await _character.DepositAllItems();

            while (true)
            {
                await CheckForGold();

                if (_role == "fighter")
                {
                    var loop = new FighterLoop(_character);
                    await loop.RunAsync();
                    return;
                }
                else if (_role == "crafter")
                {
                    var loop = new CrafterLoop(_character);
                    await loop.RunAsync();
                    return;
                }
                else if (_role == "boss")
                {
                    var loop = new BossLoop(_character);
                    await loop.RunAsync();
                    return;
                }
                else if (_role == "craft_specific")
                {
                    if (_arguments.Count() != 2)
                    {
                        Console.WriteLine("Usage: craft_specific <item> <quantity>");
                        return;
                    }

                    var itemCode = _arguments.First();
                    var quantity = int.Parse(_arguments.Last());

                    var loop = new CraftSpecificItemLoop(_character, itemCode, quantity);
                    await loop.RunAsync();
                    return;
                }

                throw new NotImplementedException("Other roles are not implemented yet.");
            }
        }

        private async Task CheckForGold()
        {
            var bankItems = await Bank.Instance.GetItems();
            foreach (var bankItem in bankItems)
            {
                if (bankItem.Quantity > 0)
                {
                    var item = Items.Instance.GetItem(bankItem.Code);
                    if (item.Type == "consumable" && item.Subtype == "bag")
                    {
                        var amount = await _character.WithdrawItems(item.Code, bankItem.Quantity);
                        if (amount > 0)
                        {
                            Console.WriteLine("GOLD SWEET GOLD!!");
                            await _character.Consume(item.Code, amount);
                        }
                    }
                }
            }
        }
    }
}