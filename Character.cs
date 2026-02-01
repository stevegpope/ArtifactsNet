using ArtifactsMmoClient.Api;
using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Artifacts
{
    internal class Character
    {
        internal MyCharactersApi _api;
        internal string Name { get; }

        internal Character(
            Configuration config,
            HttpClient httpClient,
            string name
            )
        {
            _api = new MyCharactersApi(httpClient, config);

            Name = name;
        }

        internal async Task Init()
        {
            var response = await _api.GetMyCharactersMyCharactersGetAsync();
            var character = response.Data.First(c => c.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));
            Utils.Details = character;
        }

        internal async Task Move(int x, int y)
        {
            Console.WriteLine($"Move to {x},{y}");
            await Utils.ApiCall(async () =>
            {
                try
                {
                    var response = await _api.ActionMoveMyNameActionMovePostAsync(Name, new DestinationSchema(x, y));
                    return response;
                }
                catch (ApiException ex)
                {
                    if (ex.ErrorCode == 490)
                    {
                        Console.WriteLine($"Character {Name} is already at location");
                        return null;
                    }

                    throw;
                }
            });
        }

        internal async Task MoveTo(MapContentType locationType, string code = null)
        {
            Console.WriteLine($"Moving {Name} to {locationType}, code {code}");
            var response = await Map.Instance.GetMapLayer(locationType, code);
            if (response.Data != null && response.Data.Any())
            {
                MapSchema location = Utils.GetClosest(response.Data);
                await Move(location.X, location.Y);
            }
            else
            {
                throw new Exception($"No locations found for type {locationType}");
            }
        }

        internal async Task TurnInItems(string code, int quantity)
        {
            Console.WriteLine($"Turning in {quantity} of {code} for character {Name}");
            await Utils.ApiCall(async () =>
                {
                    var item = new SimpleItemSchema(code, quantity);
                    return await _api.ActionTaskTradeMyNameActionTaskTradePostAsync(Name, item);
                });
        }

        internal async Task<int> CraftItems(ItemSchema item, int remaining)
        {
            Console.WriteLine($"Crafting {remaining} of {item.Code} for character {Name}");
            var minSkillLevel = item.Craft.Level;
            var skill = item.Craft.Skill;

            var ownedItems = new Dictionary<string, int>();
            var gatherQuantities = new Dictionary<string, int>();

            foreach (var component in item.Craft.Items)
            {
                var ownedItem = Utils.Details.Inventory.FirstOrDefault(i => i.Code.Equals(component.Code, StringComparison.OrdinalIgnoreCase));
                var ownedQuantity = ownedItem != null ? ownedItem.Quantity : 0;
                ownedItems[component.Code] = ownedQuantity;
                gatherQuantities[component.Code] = 0;
            }

            // We'll make a few at a time
            // TODO: smarter calculation to maximize inventory usage
            var batchSize = 50;
            var craftQuantity = Math.Min(remaining, batchSize);
            for (var index = 0; index < batchSize; index++)
            {
                foreach (var component in item.Craft.Items)
                {
                    var ownedQuantity = ownedItems[component.Code];
                    if (ownedQuantity >= component.Quantity)
                    {
                        // We have enough of this component to craft this one
                        ownedItems[component.Code] -= component.Quantity;
                    }
                    else
                    {
                        // Need to gather more
                        gatherQuantities[component.Code] += component.Quantity - ownedQuantity;
                        ownedItems[component.Code] = 0;
                    }
                }
            }

            // Get the items required for crafting
            foreach (var component in item.Craft.Items)
            {
                var gatherQuantity = gatherQuantities[component.Code];
                if (gatherQuantity > 0)
                {
                    await GatherItems(component.Code, gatherQuantity);
                }
            }

            // Go to the crafting location
            await MoveClosest(MapContentType.Workshop, item.Craft.Skill.Value.ToString());

            // Craft the items one at a time until we can't make any more. We may be using inventory items from before, or we may run out of gathered items.
            var itemsCrafted = 0;
            for (int index = 0; index < craftQuantity; index++)
            {
                try
                {
                    await Utils.ApiCall(async () =>
                    {
                        Console.WriteLine($"Crafting 1 {item.Code} for character {Name}");
                        var response = await _api.ActionCraftingMyNameActionCraftingPostAsync(Name, new CraftingSchema(item.Code, 1));
                        return response;
                    });
                }
                catch (ApiException ex)
                {
                    if (ex.ErrorCode == 497)
                    {
                        Console.WriteLine($"Inventory full for character {Name}, crafted {itemsCrafted} of {item.Code}");
                    }
                    else if (ex.ErrorCode == 478)
                    {
                        Console.WriteLine("Ran out of components creating items");
                    }

                    break;
                }
                itemsCrafted++;
            }

            Console.WriteLine($"Crafted {itemsCrafted} of {item.Code} for character {Name}");
            return itemsCrafted;
        }

        private async Task MoveClosest(MapContentType contentType, string code)
        {
            Console.WriteLine($"Moving to closest {contentType} with code {code} for character {Name}");
            var response = await Map.Instance.GetMapLayer(contentType, code);
            if (response.Data != null && response.Data.Any())
            {
                var locations = response.Data;

                // Find the closest location
                var location = locations.OrderBy(loc =>  Utils.CalculateManhattanDistance(loc.X, loc.Y, Utils.Details.X, Utils.Details.Y)).First();
                Console.WriteLine($"Closest location is at ({location.X}, {location.Y})");
                await Move(location.X, location.Y);
            }
            else
            {
                throw new Exception($"No locations found for type {contentType} and code {code}");
            }
        }

        internal async Task<int> GatherItems(string code, int remaining)
        {
            Console.WriteLine($"Gathering {remaining} of {code} for character {Name}");

            var resourceCode = await Resources.Instance.GetResourceDrop(code);
            if (resourceCode == null)
            {
                throw new Exception($"No resource found with drop {code}");
            }

            await MoveClosest(MapContentType.Resource, resourceCode);

            // TODO: gear up

            var gathered = 0;
            for (var index = 0; index < remaining; index++)
            {
                Console.WriteLine($"Gather 1 {code}");
                try
                {
                    await Utils.ApiCall(async () =>
                    {
                        var response = await _api.ActionGatheringMyNameActionGatheringPostAsync(Name);
                        gathered++;
                        return response;
                    });
                }
                catch (ApiException ex)
                {
                    if (ex.ErrorCode == 497)
                    {
                        Console.WriteLine($"Inventory full for character {Name}, gathered {gathered} of {code}");
                        break;
                    }
                    throw;
                }
            }

            return gathered;
        }

        internal async Task DepositAllItems()
        {
            Console.WriteLine($"Depositing all items for character {Name}");
            await DepositGold();

            await Utils.ApiCall(async () =>
            {
                var items = new List<SimpleItemSchema>();
                foreach (var item in Utils.Details.Inventory)
                {
                    if (!string.IsNullOrEmpty(item.Code))
                    {
                        items.Add(new SimpleItemSchema(item.Code, item.Quantity));
                    }
                }
                var response = await _api.ActionDepositBankItemMyNameActionBankDepositItemPostAsync(Name, items);
                return response;
            });
        }

        internal async Task DepositExcept(List<string> excludeCodes)
        {
            Console.WriteLine($"Depositing all items except {string.Join(", ", excludeCodes)} for character {Name}");
            await DepositGold();

            var items = new List<SimpleItemSchema>();
            foreach (var item in Utils.Details.Inventory)
            {
                if (!string.IsNullOrEmpty(item.Code) && !excludeCodes.Contains(item.Code, StringComparer.OrdinalIgnoreCase))
                {
                    items.Add(new SimpleItemSchema(item.Code, item.Quantity));
                }
            }

            if (items.Count == 0)
            {
                Console.WriteLine($"No items to deposit for character {Name}");
                return;
            }

            await Utils.ApiCall(async () =>
            {
                var response = await _api.ActionDepositBankItemMyNameActionBankDepositItemPostAsync(Name, items);
                return response;
            });
        }

        internal async Task DepositGold()
        {
            if (Utils.Details.Gold <= 0)
            {
                return;
            }

            await Utils.ApiCall(async () =>
            {
                Console.WriteLine($"Depositing {Utils.Details.Gold} gold for character {Name}");
                var response = await _api.ActionDepositBankGoldMyNameActionBankDepositGoldPostAsync(Name, new DepositWithdrawGoldSchema(quantity: Utils.Details.Gold));
                return response;
            });
        }

        internal async Task WithdrawItems(List<string> questItems)
        {
        }
    }
}
