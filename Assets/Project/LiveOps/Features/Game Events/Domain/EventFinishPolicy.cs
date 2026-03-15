using System;

namespace GameEventModule.Domain
{
    /// <summary>
    /// Chính sách kết thúc Event
    /// </summary>
    public class EventFinishPolicy
    {
        /// <summary>
        /// Cách kết thúc event
        /// </summary>
        public EventFinishType FinishType { get; }

        /// <summary>
        /// Thời gian tồn tại nếu FinishType = Duration
        /// </summary>
        public TimeSpan Duration { get; }

        /// <summary>
        /// Thời gian cooldown trước khi event có thể kích hoạt lại
        /// </summary>
        public TimeSpan Cooldown { get; }

        public EventFinishPolicy(
            EventFinishType finishType,
            TimeSpan duration,
            TimeSpan cooldown)
        {
            FinishType = finishType;
            Duration = duration;
            Cooldown = cooldown;
        }
    }
}