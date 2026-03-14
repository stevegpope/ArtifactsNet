namespace Artifacts
{
    internal class GathererLoop
    {
        private Character _character;

        public GathererLoop(Character character)
        {
            _character = character;
        }

        internal async Task RunAsync()
        {
            while(true)
            {
                var items = new[] { "hardwood_plank" };

                await _character.DepositAllItems();
                var bankItems = await Bank.Instance.GetItems();
                string minItem = items.MinBy(x => bankItems.Where(b => b.Code == x).Sum(b => b.Quantity));
                await _character.GatherItems(minItem, 3, ignoreBank: true);
            }
        }
    }
}
