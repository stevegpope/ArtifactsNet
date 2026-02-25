using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Artifacts
{
    internal class CooldownManager
    {
        public DateTime _nextCall = DateTime.MinValue;
        public double _savedMs = 0;
        public double LastCooldown { get; private set; }

        // Testing shows that this is about right for my machine and network
        private int _millisToSubtract = 300;
        private const int MillisToSubtractPerCall = 25;
        private bool _subtracting = true;
        private bool _subtracted = false;
        private DateTime _lastSubtract = DateTime.MinValue;

        internal async Task Cooldown(double totalSeconds)
        {
            Console.WriteLine($"Cooldown seconds {totalSeconds}");
            LastCooldown = totalSeconds;
            _nextCall = DateTime.UtcNow + TimeSpan.FromSeconds(totalSeconds);
        }

        internal async Task BackOff(string errorContent)
        {
            // Go back a little
            if (_subtracted)
            {
                _subtracting = false;
                _millisToSubtract -= MillisToSubtractPerCall;
                _millisToSubtract = Math.Max(0, _millisToSubtract);
                Console.WriteLine($"millisToSubtract: {_millisToSubtract}");
            }

            Console.WriteLine($"In Cooldown, next {_nextCall} now {DateTime.UtcNow}");
            var seconds = GetCooldownSeconds(errorContent);
            if (seconds > 0)
            {
                await Cooldown(seconds);
            }
        }

        private async Task CooldownMillis(double milliseconds)
        {
            if (_subtracting || DateTime.UtcNow > _lastSubtract + TimeSpan.FromMinutes(5))
            {
                _lastSubtract = DateTime.UtcNow;
                _subtracted = true;
                _millisToSubtract += MillisToSubtractPerCall;
                Console.WriteLine($"Millis to subtract {_millisToSubtract}");
            }

            double cooldownMillis = milliseconds - _millisToSubtract;

            if (cooldownMillis <= 0)
            {
                return;
            }

            var mod = cooldownMillis % 1000;
            var diff = 1000 - mod;
            _savedMs += diff;

            const int maxWidth = 25;
            int barWidth = Math.Min(maxWidth, (int)cooldownMillis);

            for (double remaining = cooldownMillis; remaining >= 0; remaining -= 1000)
            {
                double progress = remaining / (double)cooldownMillis;
                int hashes = (int)Math.Round(barWidth * progress);

                string bar = new string('#', hashes).PadRight(barWidth, ' ');
                Console.Write($"\rCooldown [{bar}] {remaining:F0}/{cooldownMillis:F0}s, saved {TimeSpan.FromMilliseconds(_savedMs)}");

                var wait = Math.Min(Math.Ceiling(remaining), 1000);
                await Task.Delay((int)wait);
            }

            Console.WriteLine();
        }

        internal async Task WaitForNextCall()
        {
            if (_nextCall > DateTime.MinValue)
            {
                await CooldownMillis((_nextCall - DateTime.UtcNow).TotalMilliseconds);
            }
        }

        private static double GetCooldownSeconds(string input)
        {
            var match = Regex.Match(input, @"([\d.]+)\s*seconds");

            var result = match.Success
                ? double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture)
                : 0;

            return result;
        }
    }
}
