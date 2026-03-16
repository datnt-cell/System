using System;

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
    }
}