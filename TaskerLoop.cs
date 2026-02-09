
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
            await ExchangeCoins();

            if (string.IsNullOrEmpty(Utils.Details.Task))
            {
                // Go to task master
                Console.WriteLine($"Moving to task master");
                await _character.MoveTo(MapContentType.TasksMaster, code: "items");

                await AcceptNewTask();
            }

            Console.WriteLine($"Current task:\n + " +
                $"task : {Utils.Details.Task}\n" +
                $"task type : {Utils.Details.TaskType}\n" +
                $"task progress : {Utils.Details.TaskProgress}/{Utils.Details.TaskTotal}");

            while (true)
            {
                // We switch task types on completion so we don't get left behind
                if (Utils.Details.TaskType == "items")
                {
                    await HandleItemsTask();
                    await _character.MoveTo(MapContentType.TasksMaster, "items");
                    await FinishTask();
                    await _character.MoveTo(MapContentType.TasksMaster, "items");
                    await AcceptNewTask();
                }
                else if (Utils.Details.TaskType == "monsters")
                {
                    await HandleMonstersTask();
                    await _character.MoveTo(MapContentType.TasksMaster, "monsters");
                    await FinishTask();
                    await _character.MoveTo(MapContentType.TasksMaster, "items");
                    await AcceptNewTask();
                }
                else
                {
                    throw new NotImplementedException($"Task type {Utils.Details.TaskType} not implemented");
                }

                await _character.MoveTo(MapContentType.Bank);
                await _character.DepositAllItems();

                await ExchangeCoins();
            }
        }

        private async Task ExchangeCoins()
        {
            Console.WriteLine("Try to exchange coins");
            // Assumes we start at the bank
            var bankItems = await Bank.Instance.GetItems();
            foreach (var item in bankItems)
            {
                if (item.Code == "tasks_coin" && item.Quantity >= 6)
                {
                    var amount = await _character.WithdrawItems("tasks_coin");
                    if (amount >= 6)
                    {
                        Console.WriteLine($"Got {amount} coins, going to exchange");
                        await _character.MoveTo(MapContentType.TasksMaster);

                        while (amount >= 6)
                        {
                            Console.WriteLine($"Exchange task coins");
                            await Utils.ApiCall(async () =>
                            {
                                var result = await _api.ActionTaskExchangeMyNameActionTaskExchangePostAsync(_character.Name);
                                Console.WriteLine($"Got rewards: {result.Data.Rewards.Gold} gold, {string.Join(',', result.Data.Rewards.Items.Select(item => item.Code))}");
                                return result;
                            });

                            amount -= 6;
                        }
                    }
                }
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
                var result = await _api.ActionCompleteTaskMyNameActionTaskCompletePostAsync(_character.Name);
                Console.WriteLine($"Finished task! {result.Data.Rewards.Gold} Gold");
                foreach(var item in result.Data.Rewards.Items)
                {
                    Console.WriteLine($"drop: {item.Quantity} {item.Code}"); 
                }

                return result;
            });
        }

        private async Task HandleItemsTask()
        {
            while (Utils.Details.TaskProgress < Utils.Details.TaskTotal)
            {
                var remaining = Utils.Details.TaskTotal - Utils.Details.TaskProgress;
                Console.WriteLine($"{remaining} {Utils.Details.Task} left for task");

                // Go to bank and deposit all items to make room
                await _character.MoveTo(MapContentType.Bank);
                await _character.DepositAllItems();

                var withdrawn = await _character.WithdrawItems(Utils.Details.Task, remaining);
                if (withdrawn > 0)
                {
                    await ExchangeItems(Utils.Details.Task, withdrawn, remaining);
                    remaining -= withdrawn;
                }

                if (remaining > 0)
                {
                    // Go gather, craft, or hunt down the item
                    Console.WriteLine($"Fetching item {Utils.Details.Task}");
                    var gathered = await _character.GatherItems(Utils.Details.Task, remaining);
                    await ExchangeItems(Utils.Details.Task, gathered, remaining);
                }
            }
        }

        private async Task ExchangeItems(string code, int quantity, int remaining)
        {
            // Go to task master to turn in items
            Console.WriteLine($"Moving to task master to turn in {quantity} items from inventory");
            await _character.MoveTo(MapContentType.TasksMaster, "items");
            await _character.TurnInItems(code, Math.Min(quantity, remaining));
        }

        private async Task HandleMonstersTask()
        {
            while (Utils.Details.TaskProgress < Utils.Details.TaskTotal)
            {
                var remaining = Utils.Details.TaskTotal - Utils.Details.TaskProgress;

                var monster = Utils.Details.Task;

                // TODO: gear up for the monster

                await _character.FightLoop(remaining, monster);
            }
        }
    }
}
