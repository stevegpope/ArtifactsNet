
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;
using StackExchange.Redis;
using System.Diagnostics;

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
            const string boss = "king_slime";

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
            var team = topGuys.Take(3);

            async Task gearUp(Character character)
            {
                await character.DepositAllItems();
                await character.GetFood();

                var timer = Stopwatch.StartNew();
                while (team.Any(t => !HasEnoughFood(t)))
                {
                    if (HasEnoughFood(character))
                    {
                        if (timer.Elapsed > TimeSpan.FromSeconds(120))
                        {
                            var recipe = await character.GatherFishAndCook(10);
                            await character.DepositItem(recipe, 10);
                        }
                        else 
                        {
                            await Task.Delay(1000);
                        }
                    }
                    else
                    {
                        await character.ChefRun(CountFood(character));
                    }
                }

                await character.GearUpMonster(boss);
            }

            var gearUpTasks = new List<Task>
            {
                gearUp(topGuys.ElementAt(0)),
                gearUp(topGuys.ElementAt(1)),
                gearUp(topGuys.ElementAt(2)),
            };

            static async Task makePotions(Character character)
            {
                var monster = Monsters.GetMonster(boss);
                while (true)
                {
                    Console.WriteLine($"Support role loop {character.Name}");
                    await character.DepositAllItems();

                    var slot = Random.Shared.Next(2) == 0 ? ItemSlot.Utility1 : ItemSlot.Utility2;
                    ItemSchema weapon = ChooseWeapon();
                    ItemSchema potion = character.ChoosePotion(monster, slot, weapon);
                    //await character.CraftItems(potion, 25);
                    await character.GatherFishAndCook(25);
                }
            }

            var supportTasks = new List<Task>
            {
                makePotions(topGuys.ElementAt(3)),
                makePotions(topGuys.ElementAt(4)),
            };

            await Task.WhenAll(gearUpTasks);
            await Task.WhenAll(BossFightLoop(team, boss), Task.WhenAll(supportTasks));
        }

        private static bool HasEnoughFood(Character character)
        {
            return CountFood(character) >= 50;
        }

        private static int CountFood(Character character)
        {
            var food = 0;
            foreach(var item in Utils.Details[character.Name].Inventory)
            {
                if (string.IsNullOrEmpty(item.Code)) continue;

                var itemDef = Items.GetItem(item.Code);
                if (itemDef.Subtype == "food")
                {
                    food += item.Quantity;
                }
            }

            return food;
        }

        private static ItemSchema ChooseWeapon()
        {
            var weapons = Items.GetAllItems().Values.Where(x => x.Type == "weapon");
            return weapons.ElementAt(Random.Shared.Next(weapons.Count()));
        }

        internal static async Task BossFightLoop(IEnumerable<Character> team, string boss)
        {
            Console.WriteLine("Boss Fight!");

            var leader = team.ElementAt(2); // the one who triggers fight

            int losses = 0;

            while (true)
            {
                //
                // PHASE 1: REST IF NEEDED
                //
                await Task.WhenAll(team.Select(async c =>
                {
                    var stats = Utils.Details[c.Name];

                    if (stats.Hp < stats.MaxHp * 0.75)
                    {
                        if (await c.Rest())
                        {
                            Console.WriteLine($"{c.Name} rested");
                        }
                    }
                }));

                //
                // PHASE 2: MOVE TO BOSS
                //
                await Task.WhenAll(team.Select(c =>
                    c.MoveTo(MapContentType.Monster, code: boss)));

                //
                // PHASE 3: FIGHT
                //
                try
                {
                    var participants = team.Take(2).Select(x => x.Name).ToList();

                    var result = await leader.Fight(participants);

                    if (result.Data.Fight.Result == FightResult.Win)
                    {
                        losses = 0;
                    }
                    else
                    {
                        losses++;

                        Console.WriteLine($"loss {losses}/3");

                        if (losses >= 3)
                        {
                            Console.WriteLine($"Giving up on {boss}");
                            return;
                        }
                    }
                }
                catch (ApiException ex)
                {
                    if (ex.ErrorCode == 497)
                    {
                        Console.WriteLine("Inventory full, depositing");

                        await Task.WhenAll(team.Select(c => c.DepositAllItems()));
                        continue;
                    }

                    Console.WriteLine($"Fight error {ex.ErrorContent}");
                    return;
                }
            }
        }
    }
}
