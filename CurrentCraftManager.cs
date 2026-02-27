namespace Artifacts
{
    internal class CurrentCraftManager
    {
        private string _filename;

        internal class CraftInfo
        {
            public int Quantity { get; set; }
            public string Code { get; set; }
        }

        internal CurrentCraftManager(string name)
        {
            _filename = $"{name}.txt";
        }

        internal void StartCraft(string code, int craftAmount)
        {
            File.WriteAllText(_filename, $"{craftAmount} {code}");
        }

        internal void FinishCraft()
        {
            File.Delete(_filename);
        }

        internal CraftInfo GetCurrentCraft()
        {
            if (!File.Exists(_filename))
            {
                return null;
            }

            var current = File.ReadAllText(_filename);

            return new CraftInfo
            {
                Quantity = int.Parse(current[0..current.IndexOf(' ')]),
                Code = current.Substring(current.IndexOf(' ') + 1)
            };
        }
    }
}
