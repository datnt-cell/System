using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEventModule.Domain
{
    public class GameEventState
    {
        public bool IsActive { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime CooldownEndTime { get; set; }

        public DateTime NextCheckTime { get; set; }

        public Dictionary<string, int> Progress { get; set; } = new();

        // =========================
        // STATE API
        // =========================

        public void Start(DateTime now, DateTime endTime)
        {
            IsActive = true;
            StartTime = now;
            EndTime = endTime;
        }

        public void Stop(DateTime now, TimeSpan cooldown)
        {
            IsActive = false;
            CooldownEndTime = now + cooldown;
        }

        public bool IsInCooldown(DateTime now)
        {
            return CooldownEndTime > now;
        }

        // =========================
        // PROGRESS API
        // =========================

        public int GetProgress(string key)
        {
            if (Progress.TryGetValue(key, out var value))
                return value;

            return 0;
        }

        public void SetProgress(string key, int value)
        {
            Progress[key] = value;
        }

        public void AddProgress(string key, int amount)
        {
            if (!Progress.ContainsKey(key))
                Progress[key] = 0;

            Progress[key] += amount;
        }

        public int GetTotalProgress()
        {
            if (Progress.Count == 0)
                return 0;

            return Progress.Values.Sum();
        }
    }
}