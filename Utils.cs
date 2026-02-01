using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;
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

        internal static async Task Cooldown(int seconds)
        {
            Console.WriteLine($"Waiting for cooldown: {seconds} seconds");
            await Task.Delay(seconds * 1000);
        }

        internal static double CalculateManhattanDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        internal static async Task ApiCall(Func<Task<dynamic>> call)
        {
            try
            {
                var result = await call();
                if (TryGetProperty(result?.Data, "Cooldown", out object cooldown))

                {
                    var cooldownschema = (CooldownSchema)cooldown;
                    await Utils.Cooldown(cooldownschema.RemainingSeconds);
                }

                if (TryGetProperty(result?.Data, "Character", out object character))
                {
                    Details = (CharacterSchema)character;
                }

                if (TryGetProperty(result?.Data, "Bank", out object bank))
                {
                    if (bank is BankSchema bankSchema) {
                        Bank = bankSchema;
                    }
                }
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
                        await Utils.Cooldown(seconds);
                    }
                    await ApiCall(call);
                    return;
                }

                Console.WriteLine($"API call failed: {ex.ErrorContent}, code  {ex.ErrorCode}");
                throw;
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
                if (closest == null)
                {
                    closest = map;
                    continue;
                }

                var currentDistance = CalculateManhattanDistance(Details.X, Details.Y, map.X, map.Y);
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    closest = map;
                }
            }

            return closest;
        }
    }
}
