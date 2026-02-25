using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Artifacts
{
    internal static class Utils
    {
        public static Dictionary<string,CharacterSchema> Details = new Dictionary<string,CharacterSchema>();
        public static BankSchema Bank { get; private set; }
        public static DateTime _nextCall = DateTime.MinValue;
        public static double _savedMs = 0;
        public static int LastCooldown { get; private set; }

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

        internal static async Task Cooldown(int totalSeconds)
        {
            Console.WriteLine($"Cooldown seconds {totalSeconds}");
            LastCooldown = totalSeconds;
            _nextCall = DateTime.UtcNow + TimeSpan.FromSeconds(totalSeconds);
        }

        private static int millisToSubtract = 0;
        private const int millisToSubtractPerCall = 25;
        private static bool subtracting = true;
        private static bool subtracted = false;

        private static async Task CooldownMillis(double milliseconds)
        {
            if (subtracting)
            {
                subtracted = true;
                millisToSubtract += millisToSubtractPerCall;
                Console.WriteLine($"Millis to subtract {millisToSubtract}");
            }

            double cooldownMillis = milliseconds - millisToSubtract;

            if (cooldownMillis <= 0)
            {
                return;
            }

            var mod = cooldownMillis % 1000;
            var diff = 1000 - mod;
            _savedMs += diff;

            const int maxWidth = 25;
            int barWidth = Math.Min(maxWidth, (int)cooldownMillis);

            for (double remaining = cooldownMillis; remaining >= 0; remaining-=1000)
            {
                double progress = remaining / (double)cooldownMillis;
                int hashes = (int)Math.Round(barWidth * progress);

                string bar = new string('#', hashes).PadRight(barWidth, ' ');
                Console.Write($"\rCooldown [{bar}] {remaining:F0}/{cooldownMillis:F0}s, saved {_savedMs}");

                var wait = Math.Min(Math.Ceiling(remaining), 1000);
                await Task.Delay((int)wait);
            }

            Console.WriteLine();
        }

        internal static double CalculateManhattanDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        internal static async Task<object> ApiCall(Func<Task<dynamic>> call)
        {
            try
            {
                if (_nextCall > DateTime.MinValue)
                {
                    await CooldownMillis((_nextCall - DateTime.UtcNow).TotalMilliseconds);
                }

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
                        var characterSchema = (CharacterSchema)character;
                        Details[characterSchema.Name] = characterSchema;
                    }

                    if (TryGetProperty(result?.Data, "Characters", out object characters))
                    {
                        var list = (List<CharacterSchema>)characters;
                        foreach(var characterSchema in list)
                        {
                            Details[characterSchema.Name] = characterSchema;
                        }
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
                    // Go back a little
                    if (subtracting && subtracted)
                    {
                        subtracting = false;
                        millisToSubtract -= 5;
                        Console.WriteLine($"Cooldown hit, reducing millisToSubtract to {millisToSubtract}");
                    }

                    Console.WriteLine($"In Cooldown, next {_nextCall} now {DateTime.UtcNow}");
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

        private static async Task CurrentCooldown()
        {
            throw new NotImplementedException();
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
    }
}
