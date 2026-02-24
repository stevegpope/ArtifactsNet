
using ArtifactsMmoClient.Model;

namespace Artifacts
{
    internal class CraftSpecificItemLoop
    {
        private readonly Character _character;
        private readonly string _itemCode;
        private readonly int _quantity;

        public CraftSpecificItemLoop(Character character, string item, int quantity)
        {
            _character = character;
            _itemCode = item;
            _quantity = quantity;
        }

        internal async Task RunAsync()
        {
            Console.WriteLine($"Starting CraftSpecificItemLoop to make {_quantity} {_itemCode}");

            var item = Items.GetItem(_itemCode);
            var crafted = await _character.CraftItems(item, _quantity);
        }
    }
}
