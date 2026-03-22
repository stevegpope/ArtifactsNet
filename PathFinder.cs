using ArtifactsMmoClient.Model;

namespace Artifacts
{
    internal class PathFinder
    {
        private readonly CharacterSchema character;
        private List<MapSchema> maps;
        private DateTime LastUpdate = DateTime.MinValue;
        private List<AccountAchievementSchema> achievements = null;

        internal PathFinder(CharacterSchema characterSchema)
        {
            this.character = characterSchema;
        }

        internal async Task<List<MapSchema>> GetPath(MapContentType contentType, string code = null)
        {
            code = code?.ToLower();
            maps = await GetMaps();
            var destinations = maps.Where(m => m?.Interactions?.Content?.Type == contentType && (code == null || m?.Interactions?.Content?.Code == code)).ToList();
            if (!destinations.Any())
            {
                return new List<MapSchema>();
            }

            return FindShortestPath(destinations);
        }

        private async Task<List<MapSchema>> GetMaps()
        {
            // Cache the map for 10 minutes. Events will be affected
            if (LastUpdate < DateTime.UtcNow - TimeSpan.FromMinutes(10))
            {
                LastUpdate = DateTime.UtcNow;
                maps = await Map.Instance.GetAllMaps();
            }

            return maps;
        }

        private List<MapSchema> FindShortestPath(List<MapSchema> destinations)
        {
            var shortestDistance = double.MaxValue;
            IEnumerable<MapSchema> shortestPath = null;

            foreach (var map in destinations)
            {
                IEnumerable<MapSchema> path = FindPath(map);
                if (path == null || !path.Any())
                {
                    continue;
                }

                if (shortestPath == null)
                {
                    shortestPath = path;
                    shortestDistance = CalculatePathLength(path);
                }
                else
                {
                    double distance = CalculatePathLength(path);
                    if (distance < shortestDistance)
                    {
                        shortestPath = path;
                        shortestDistance = distance;
                    }
                }
            }

            if (shortestPath != null)
            {
                // Added these depth-first, so they need to be reversed for traversal
                return shortestPath.Reverse().ToList();
            }

            return null;
        }

        private double CalculatePathLength(IEnumerable<MapSchema> path)
        {
            // Start at the character
            MapSchema previous = maps.First(m => m.MapId == character.MapId);

            // It does not matter if the path crosses layers, as we only need the X,Y distance for movement
            double distance = 0.0;

            foreach (var map in path)
            {
                distance += Utils.CalculateManhattanDistance(previous.X, previous.Y, map.X, map.Y);
                previous = map;
            }

            return distance;
        }

        private IEnumerable<MapSchema> FindPath(MapSchema destination)
        {
            var path = new List<MapSchema>();
            var searched = new List<MapSchema>();
            var start = maps.First(m => m.MapId == character.MapId);
            if (FindPathWithTransitions(start, destination, searched, ref path))
            {
                return path;
            }

            return null;
        }

        private bool FindPathWithTransitions(MapSchema position, MapSchema destination, List<MapSchema> searched, ref List<MapSchema> path)
        {
            // Path will result in any transition pit-stops followed by the destination. In the common case,
            // there will only be a single entry for the destination.

            if (position.MapId == destination.MapId)
            {
                path.Add(destination);
                return true;
            }

            // Have we been here?
            if (searched.Any(m => m.MapId == position.MapId)) return false;

            searched.Add(position);

            // Is the character allowed here?
            if (position.Access.Type == MapAccessType.Blocked) return false;
            if (position.Access.Type == MapAccessType.Teleportation) return false;
            if (position.Access.Type == MapAccessType.Conditional)
            {
                if (!MeetsConditions(position.Access.Conditions)) return false;
            }

            // go left, right, up, down, and transition
            var left = maps.FirstOrDefault(m => m.X == position.X - 1 && m.Y == position.Y && m.Layer == position.Layer);
            if (left != null && FindPathWithTransitions(left, destination, searched, ref path))
            {
                return true;
            }

            var right = maps.FirstOrDefault(m => m.X == position.X + 1 && m.Y == position.Y && m.Layer == position.Layer);
            if (right != null && FindPathWithTransitions(right, destination, searched, ref path))
            {
                return true;
            }

            var up = maps.FirstOrDefault(m => m.X == position.X && m.Y == position.Y - 1 && m.Layer == position.Layer);
            if (up != null && FindPathWithTransitions(up, destination, searched, ref path))
            {
                return true;
            }

            var down = maps.FirstOrDefault(m => m.X == position.X && m.Y == position.Y + 1 && m.Layer == position.Layer);
            if (down != null && FindPathWithTransitions(down, destination, searched, ref path))
            {
                return true;
            }

            if (position?.Interactions?.Transition != null)
            {
                var transition = maps.First(m => m.MapId == position.Interactions.Transition.MapId);
                if (transition != null && FindPathWithTransitions(transition, destination, searched, ref path))
                {
                    path.Add(position);
                    return true;
                }
            }

            return false;
        }

        private bool MeetsConditions(List<ConditionSchema> conditions)
        {
            foreach (var condition in conditions)
            {
                if (condition.Operator == ConditionOperator.HasItem ||
                    condition.Operator == ConditionOperator.Cost)
                {
                    if (condition.Code == "gold" && character.Gold < condition.Value) return false;
                    if (character.Inventory.Any(i => i.Code == condition.Code && i.Quantity > condition.Value)) continue;
                    if (!HasEquipped(condition.Code)) return false;
                }
                else if (condition.Operator == ConditionOperator.AchievementUnlocked)
                {
                    if (achievements == null)
                        achievements = Accounts.Instance.GetAchievements().Result;

                    if (!achievements.Any(a => a.Code == condition.Code)) return false;
                }
            }

            return true;
        }

        private bool HasEquipped(string code)
        {
            if (code == character.AmuletSlot) return true;
            if (code == character.Artifact1Slot) return true;
            if (code == character.Artifact2Slot) return true;
            if (code == character.Artifact3Slot) return true;
            if (code == character.BagSlot) return true;
            if (code == character.BodyArmorSlot) return true;
            if (code == character.BootsSlot) return true;
            if (code == character.HelmetSlot) return true;
            if (code == character.LegArmorSlot) return true;
            if (code == character.Ring1Slot) return true;
            if (code == character.Ring2Slot) return true;
            if (code == character.ShieldSlot) return true;
            if (code == character.Utility1Slot) return true;
            if (code == character.Utility2Slot) return true;
            if (code == character.WeaponSlot) return true;

            return false;
        }
    }
}
