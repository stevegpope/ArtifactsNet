
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;
using StackExchange.Redis;

namespace Artifacts
{
    internal class BossLoop
    {
        private Character _character;

        public BossLoop(Character character)
        {
            _character = character;
        }

        internal async Task RunAsync()
        {
            Console.WriteLine($"Starting boss loop");

            var characters = new[]
            {
                new Character(_character._api, "baz1"),
                new Character(_character._api, "baz2"),
                new Character(_character._api, "baz3"),
                new Character(_character._api, "baz4"),
                new Character(_character._api, "baz5"),
            };

            foreach (var character in characters)
            {
                await character.Init();
            }

            // Top 3 fight, others support
            var topGuys = characters.OrderByDescending(x => Utils.Details[x.Name].Level);

            const string boss = "king_slime";
            var monster = await Monsters.GetMonster(boss);

            static async Task gearUp(Character character)
            {
                await character.MoveTo(MapContentType.Bank);
                await character.DepositAllItems();
                await character.GearUpMonster(boss);
                Console.WriteLine($"{character.Name} gear up 1 complete");
                await character.GearUpMonster(boss);
                Console.WriteLine($"{character.Name} gear up 2 complete");
                await character.GearUpMonster(boss);
                Console.WriteLine($"{character.Name} gear up 3 complete");
                await character.GearUpMonster(boss);
                Console.WriteLine($"{character.Name} gear up 4 complete");
                await character.GearUpMonster(boss);
                Console.WriteLine($"{character.Name} gear up 5 complete");
                Console.WriteLine($"{character.Name} READY FOR {boss}");
            }

            var gearUpTasks = new List<Task>
            {
                gearUp(topGuys.ElementAt(0)),
                gearUp(topGuys.ElementAt(1)),
                gearUp(topGuys.ElementAt(2)),
            };

            var supportTasks = new List<Task>();
            var potion = Items.GetItem("minor_health_potion");
            foreach (var guy in topGuys.Skip(3))
            {
                supportTasks.Add(Task.Run(async () => {
                    while (true)
                    {
                        Console.WriteLine($"Support role loop {guy.Name}");
                        await guy.MoveTo(MapContentType.Bank);
                        await guy.DepositAllItems();

                        var slot = Random.Shared.Next(2) == 1 ? ItemSlot.Utility1 : ItemSlot.Utility2;
                        ItemSchema weapon = ChooseWeapon();
                        ItemSchema potion = guy.ChoosePotion(monster, slot, weapon);
                        await guy.CraftItems(potion, 25);
                    }
                }));
            }

            await Task.WhenAll(gearUpTasks);
            await FightLoop(topGuys.Take(3), boss);
        }

        private static ItemSchema ChooseWeapon()
        {
            var weapons = Items.GetAllItems().Values.Where(x => x.Type == "weapon");
            return weapons.ElementAt(Random.Shared.Next(weapons.Count()));
        }

        internal static async Task FightLoop(IEnumerable<Character> enumerable, string boss)
        {
            Console.WriteLine("Boss Fight!");

            var lostLastFight = 0;
            var losses = 0;

            while (true)
            {
                // If we are healthy enough fight right away
                var tasks = new List<Task>();
                foreach (var character in enumerable)
                {
                    var task = Task.Run(async () =>
                    {
                        var needsRest = false;
                        if (lostLastFight == 0 && Utils.Details[character.Name].Hp < Utils.Details[character.Name].MaxXp * .75)
                        {
                            needsRest = true;
                        }
                        else if (Utils.Details[character.Name].Hp < lostLastFight)
                        {
                            needsRest |= true;
                        }

                        if (needsRest)
                        {
                            if (await character.Rest())
                            {
                                Console.WriteLine("We cooked food, gear up again");
                                await character.GearUpMonster(boss);
                            }
                        }

                        await character.MoveTo(MapContentType.Monster, code: boss);
                    });

                    tasks.Add(task);
                }

                await Task.WhenAll(tasks);

                try
                {
                    var participants = new List<string>(enumerable.Take(2).Select(x => x.Name));
                    var result = await enumerable.ElementAt(2).Fight(participants);

                    if (result.Data.Fight.Result == FightResult.Win)
                    {
                        // Reset losses, we can beat him!
                        losses = 0;
                    }
                    else if (result.Data.Fight.Result == FightResult.Loss)
                    {
                        const int Limit = 3;
                        losses++;
                        Console.WriteLine($"loss {losses}/{Limit}");
                        if (losses >= Limit)
                        {
                            Console.WriteLine($"We Lost! Giving up on monster {boss}");
                            return;
                        }
                    }
                }
                catch (ApiException ex)
                {
                    Console.WriteLine($"Fight error: {ex.ErrorContent}");
                    if (ex.ErrorCode == 497)
                    {
                        Console.WriteLine("Inventory full, be right back");
                        tasks.Clear();
                        foreach (var character in enumerable)
                        {
                            var task = Task.Run(async () =>
                            {
                                await character.MoveTo(MapContentType.Bank);
                                await character.DepositAllItems();
                            });

                            tasks.Add(task);
                        }

                        await Task.WhenAll(tasks);
                        continue;
                    }

                    return;
                }
            }
        }
    }
}
