using System;

namespace GameEventModule.Domain
{
    /// <summary>
    /// Chính sách kết thúc Event
    /// </summary>
    public class EventFinishPolicy
    {
        public EventFinishType FinishType { get; }

        public TimeSpan Duration { get; }

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

        public bool IsDuration()
        {
            return FinishType == EventFinishType.Duration;
        }

        public bool IsCondition()
        {
            return FinishType == EventFinishType.Condition;
        }

        public bool IsInfiniteDuration()
        {
            return FinishType == EventFinishType.Duration && Duration <= TimeSpan.Zero;
        }
    }
}