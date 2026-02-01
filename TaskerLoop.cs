
using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Model;

namespace Artifacts
{
    internal class TaskerLoop
    {
        private Character _character;
        private MyCharactersApi _api;

        public TaskerLoop(Character character)
        {
            _character = character;
            _api = character._api;
        }

        internal async Task RunAsync()
        {
            Console.WriteLine($"Starting tasker loop");

            if (string.IsNullOrEmpty(Utils.Details.Task))
            {
                // Go to task master
                Console.WriteLine($"Moving to task master");
                await _character.Move(4, 13);

                await AcceptNewTask();
            }

            Console.WriteLine($"Current task:\n + " +
                $"task : {Utils.Details.Task}\n" +
                $"task type : {Utils.Details.TaskType}\n" +
                $"task progress : {Utils.Details.TaskProgress}/{Utils.Details.TaskTotal}");

            if (Utils.Details.TaskType == "items")
            {
                while (true)
                {
                    await HandleItemsTask(Utils.Details);
                    await _character.MoveTo(MapContentType.TasksMaster, "items");
                    await FinishTask();
                    await AcceptNewTask();
                }
            }
            else if (Utils.Details.TaskType == "monsters")
            {
                // Go to bank and deposit all items
                await _character.MoveTo(MapContentType.Bank);
                await _character.DepositAllItems();

                await HandleMonstersTask(Utils.Details);
            }
            else
            {
               throw new NotImplementedException($"Task type {Utils.Details.TaskType} not implemented");
            }
        }

        private async Task AcceptNewTask()
        {
            // Get new task
            Console.WriteLine($"Getting new task");
            await Utils.ApiCall(async () =>
            {
                var result = await _api.ActionAcceptNewTaskMyNameActionTaskNewPostAsync(_character.Name);
                Console.WriteLine($"new task: " + result.Data.Task.ToJson());
                return result;
            });
        }

        private async Task FinishTask()
        {
            await Utils.ApiCall(async () =>
            {
                return _api.ActionCompleteTaskMyNameActionTaskCompletePostAsync(_character.Name);
            });
        }

        private async Task DepositNonQuestItemsAndGetMore()
        {
            var questItems = await GetQuestItems();

            if (!Utils.Details.Inventory.All(i => questItems.Contains(i.Code) || string.IsNullOrEmpty(i.Code)))
            {
                // Go to bank and deposit all items except task items
                await _character.MoveTo(MapContentType.Bank);
                await _character.DepositGold();

                // Deposit all items except task items
                await _character.DepositExcept(questItems);

                // Get any quest items from bank
                await _character.WithdrawItems(questItems);
            }
        }

        private async Task<List<string>> GetQuestItems()
        {
            var items = new List<string>
            {
                // Add the original item
                Utils.Details.Task
            };

            // Add any craft components
            var item = await Items.Instance.GetItem(Utils.Details.Task);
            if (item.Craft != null)
            {
                foreach (var component in item.Craft.Items)
                {
                    items.Add(component.Code);
                }
            }

            return items;
        }

        private async Task HandleItemsTask(CharacterSchema details)
        {
            while (details.TaskProgress < details.TaskTotal)
            {
                await DepositNonQuestItemsAndGetMore();

                var remaining = details.TaskTotal - details.TaskProgress;

                // Do we already have the items in inventory?
                foreach (var inventory in details.Inventory)
                {
                    if (inventory.Code == details.Task && inventory.Quantity > 0)
                    {
                        await ExchangeItems(inventory.Code, inventory.Quantity, remaining);
                        continue;
                    }
                }

                // Either craft the item, or go gather it
                var item = await Items.Instance.GetItem(details.Task);
                if (item.Craft != null)
                {
                    // Craft the item
                    Console.WriteLine($"Crafting item {item.Name}");
                    var crafted = await _character.CraftItems(item, remaining);
                    await ExchangeItems(details.Task, crafted, remaining);
                }
                else
                {
                    // Go gather the item
                    Console.WriteLine($"Gathering item {item.Name}");
                    var gathered = await _character.GatherItems(item.Code, remaining);
                    await ExchangeItems(details.Task, gathered, remaining);
                }

            }
        }
        private async Task ExchangeItems(string code, int quantity, int remaining)
        {
            // Go to task master to turn in items
            Console.WriteLine($"Moving to task master to turn in items from inventory");
            await _character.MoveTo(MapContentType.TasksMaster, "items");
            await _character.TurnInItems(code, Math.Min(quantity, remaining));
        }

        private async Task HandleMonstersTask(CharacterSchema details)
        {
            throw new NotImplementedException();
        }
    }
}
