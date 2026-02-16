using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Artifacts
{
    internal static class Utils
    {
        public static CharacterSchema Details { get; set; }
        public static BankSchema Bank { get; private set; }
        public static int LastCooldown { get; private set; }
        public static Character Character { get; set; }

        private static DateTime LastBossCheck = DateTime.MinValue;

        internal static string ToJson<T>(
            this T obj
            )
        {
            return System.Text.Json.JsonSerializer.Serialize(
                obj,
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });
        }

        static async Task Cooldown(int totalSeconds)
        {
            LastCooldown = totalSeconds;
            const int maxWidth = 25;
            int barWidth = Math.Min(maxWidth, totalSeconds);

            for (int remaining = totalSeconds; remaining >= 0; remaining--)
            {
                double progress = remaining / (double)totalSeconds;
                int hashes = (int)Math.Round(barWidth * progress);

                string bar = new string('#', hashes).PadRight(barWidth, ' ');
                Console.Write($"\rCooldown [{bar}] {remaining}/{totalSeconds}s ");

                await Task.Delay(1000);
            }

            Console.WriteLine();

            await BossCheck();
        }

        private static async Task BossCheck()
        {
            if (LastBossCheck > DateTime.UtcNow)
            {
                return;
            }

            LastBossCheck = DateTime.UtcNow + TimeSpan.FromMinutes(1);
            await Character.MeetForBoss();
        }

        internal static double CalculateManhattanDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        internal static async Task<object> ApiCall(Func<Task<dynamic>> call)
        {
            try
            {
                var result = await call();
                if (TryGetProperty(result, "Data", out object data))
                {
                    if (TryGetProperty(result?.Data, "Cooldown", out object cooldown))
                    {
                        if (cooldown is CooldownSchema cooldownSchema)
                        {
                            await Cooldown(cooldownSchema.TotalSeconds);
                        }
                        else if (cooldown is int cooldownValue)
                        {
                            await Cooldown(cooldownValue);
                        }
                        else
                        {
                            Console.WriteLine($"Cooldown: {cooldown} type {cooldown.GetType()}");
                        }
                    }

                    if (TryGetProperty(result?.Data, "Character", out object character))
                    {
                        Details = (CharacterSchema)character;
                    }

                    if (TryGetProperty(result?.Data, "Characters", out object characters))
                    {
                        var list = (List<CharacterSchema>)characters;
                        Details = list.First(x => x.Name == Details.Name);
                    }

                    if (TryGetProperty(result?.Data, "Bank", out object bank))
                    {
                        if (bank is BankSchema bankSchema)
                        {
                            Bank = bankSchema;
                        }
                    }
                }

                return result;
            }
            catch (HttpRequestException ex)
            {
                // Network error, wait a minute and try again
                Console.WriteLine($"Network outage: {ex.Message}");
                await Task.Delay(60 * 1000);
                return await ApiCall(call);
            }
            catch (ApiException ex)
            {
                if (ex.ErrorCode == 499)
                {
                    Console.WriteLine($"In Cooldown");
                    var content = ex.ErrorContent;
                    var seconds = GetCooldownSeconds(content.ToString());
                    if (seconds > 0)
                    {
                        await Cooldown(seconds);
                    }
                    return await ApiCall(call);
                }
                else if (ex.ErrorCode == 486)
                {
                    // An action is already in progress. Try again
                    return await ApiCall(call);
                }
                else if (ex.ErrorCode == 502)
                { 
                    Console.WriteLine($"{ex.ErrorContent}, trying again");
                    await Task.Delay(5000); 
                    return await ApiCall(call);
                }

                Console.WriteLine($"API call failed: {ex.ErrorContent}, code  {ex.ErrorCode}");
                throw;
            }
        }

        internal static int GetSkillLevel(string skill)
        {
            switch (skill)
            {
                case "weaponcrafting":
                    return Utils.Details.WeaponcraftingLevel;
                case "gearcrafting":
                    return Utils.Details.GearcraftingLevel;
                case "jewelrycrafting":
                    return Utils.Details.JewelrycraftingLevel;
                case "cooking":
                    return Utils.Details.CookingLevel;
                case "alchemy":
                    return Utils.Details.AlchemyLevel;
                case "woodcutting":
                    return Utils.Details.WoodcuttingLevel;
                case "mining":
                    return Utils.Details.MiningLevel;
                case "fishing":
                    return Utils.Details.FishingLevel;
                default:
                    throw new Exception($"Unexpected skill {skill}");
            }
        }

        internal static CraftSkill GetSkillCraft(string skill)
        {
            switch (skill)
            {
                case "weaponcrafting":
                    return CraftSkill.Weaponcrafting;
                case "gearcrafting":
                    return CraftSkill.Gearcrafting;
                case "jewelrycrafting":
                    return CraftSkill.Jewelrycrafting;
                case "cooking":
                    return CraftSkill.Cooking;
                case "alchemy":
                    return CraftSkill.Alchemy;
                case "woodcutting":
                    return CraftSkill.Woodcutting;
                case "mining":
                    return CraftSkill.Mining;
                default:
                    throw new Exception($"Unexpected skill {skill}");
            }
        }

        private static bool TryGetProperty(dynamic obj, string name, out object value)
        {
            value = null;
            if (obj == null) return false;

            var prop = obj.GetType().GetProperty(name);
            if (prop == null) return false;

            value = prop.GetValue(obj);
            return value != null;
        }

        private static int GetCooldownSeconds(string input)
        {
            var match = Regex.Match(input, @"([\d.]+)\s*seconds");

            var result = match.Success
                ? double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture)
                : 0;

            return (int)Math.Ceiling(result);
        }

        internal static MapSchema GetClosest(List<MapSchema> data)
        {
            if (data == null || data.Count == 0)
            {
                throw new ArgumentException("No map data");
            }

            MapSchema closest = null;
            double minDistance = double.MaxValue;

            foreach (var map in data)
            {
                var currentDistance = CalculateManhattanDistance(Details.X, Details.Y, map.X, map.Y);
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    closest = map;
                }
            }

            return closest;
        }

        internal static ItemSlot GetSlot(string slotType)
        {
            return JsonConvert.DeserializeObject<ItemSlot>($"\"{slotType}\"");
        }
    }
}
