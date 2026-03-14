using ArtifactsMmoClient.Client;
using ArtifactsMmoClient.Model;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Artifacts
{
    internal static class Utils
    {
        public static Dictionary<string,CharacterSchema> Details = new Dictionary<string,CharacterSchema>();
        private static Dictionary<string, CooldownManager> _cooldowns = new Dictionary<string, CooldownManager>();

        public static BankSchema Bank { get; private set; }
        public static double LastCooldown(string name)
        {
            return _cooldowns[name].LastCooldown;
        }

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

        internal static double CalculateManhattanDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        internal static async Task<object> ApiCallGet(Func<Task<dynamic>> call)
        {
            object result = null;

            do
            { 
                result = await ApiCall(null, call);
            }
            while (result == null);

            return result;
        }

        internal static async Task<object> ApiCall(string name, Func<Task<dynamic>> call)
        {
            CooldownManager cooldownManager = null;
            if (name != null)
            {
                if (!_cooldowns.TryGetValue(name, out cooldownManager))
                {
                    cooldownManager = new CooldownManager();
                    _cooldowns[name] = cooldownManager;
                }
                cooldownManager = _cooldowns[name];
            }

            try
            {
                if (cooldownManager != null) await cooldownManager.WaitForNextCall();

                var result = await call();

                if (TryGetProperty(result, "Data", out object data))
                {
                    if (TryGetProperty(result?.Data, "Cooldown", out object cooldown))
                    {
                        if (cooldown is CooldownSchema cooldownSchema)
                        {
                            if (cooldownManager != null) await cooldownManager.Cooldown(cooldownSchema.TotalSeconds);
                        }
                        else if (cooldown is int cooldownValue)
                        {
                            if (cooldownManager != null) await cooldownManager.Cooldown(cooldownValue);
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
                await Task.Delay(10 * 1000);
                return await ApiCall(name, call);
            }
            catch (ApiException ex)
            {
                if (ex.ErrorCode == 499)
                {
                    // Too fast, back off
                    if (cooldownManager != null) await cooldownManager.BackOff(ex.ErrorContent.ToString());

                    return await ApiCall(name, call);
                }
                else if (ex.ErrorCode == 486)
                {
                    // An action is already in progress. Try again
                    return await ApiCall(name, call);
                }
                else if (ex.ErrorCode == 502)
                { 
                    Console.WriteLine($"{ex.ErrorContent}, usually this worked");
                    return null;
                }

                Console.WriteLine($"API call failed: {ex.ErrorContent}, code  {ex.ErrorCode}");
                throw;
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
    }
}
