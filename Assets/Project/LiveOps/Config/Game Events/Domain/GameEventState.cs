using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEventModule.Domain
{
    /// <summary>
    /// Trạng thái runtime của Game Event
    /// </summary>
    public class GameEventState
    {
        /// <summary>
        /// Event đang active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Thời điểm event bắt đầu
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Thời điểm event kết thúc
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Thời điểm cooldown kết thúc
        /// </summary>
        public DateTime CooldownEndTime { get; set; }

        /// <summary>
        /// Progress của event
        /// key = progressId
        /// </summary>
        public Dictionary<string, int> Progress { get; set; } = new();

        // =========================
        // PROGRESS API
        // =========================

        /// <summary>
        /// Lấy progress theo key
        /// </summary>
        public int GetProgress(string key)
        {
            if (Progress.TryGetValue(key, out var value))
                return value;

            return 0;
        }

        /// <summary>
        /// Set progress theo key
        /// </summary>
        public void SetProgress(string key, int value)
        {
            Progress[key] = value;
        }

        /// <summary>
        /// Cộng progress theo key
        /// </summary>
        public void AddProgress(string key, int amount)
        {
            if (!Progress.ContainsKey(key))
                Progress[key] = 0;

            Progress[key] += amount;
        }

        // =========================
        // TOTAL PROGRESS
        // =========================

        /// <summary>
        /// Tổng progress của event
        /// </summary>
        public int GetTotalProgress()
        {
            if (Progress.Count == 0)
                return 0;

            return Progress.Values.Sum();
        }
    }
}