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
        public static double LastCooldown
        {
            get
            {
                return _cooldownManager.LastCooldown;
            }
        }

        private static readonly CooldownManager _cooldownManager = new CooldownManager();

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
                result = await ApiCall(call);
            }
            while (result == null);

            return result;
        }

        internal static async Task<object> ApiCall(Func<Task<dynamic>> call)
        {
            try
            {
                await _cooldownManager.WaitForNextCall();

                var result = await call();

                if (TryGetProperty(result, "Data", out object data))
                {
                    if (TryGetProperty(result?.Data, "Cooldown", out object cooldown))
                    {
                        if (cooldown is CooldownSchema cooldownSchema)
                        {
                            await _cooldownManager.Cooldown(cooldownSchema.TotalSeconds);
                        }
                        else if (cooldown is int cooldownValue)
                        {
                            await _cooldownManager.Cooldown(cooldownValue);
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
                return await ApiCall(call);
            }
            catch (ApiException ex)
            {
                if (ex.ErrorCode == 499)
                {
                    // Too fast, back off
                    await _cooldownManager.BackOff(ex.ErrorContent.ToString());

                    return await ApiCall(call);
                }
                else if (ex.ErrorCode == 486)
                {
                    // An action is already in progress. Try again
                    return await ApiCall(call);
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
