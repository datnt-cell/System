using System;

namespace ConditionEngine.Domain
{
    /// <summary>
    /// Condition kiểm tra giờ trong ngày
    /// Ví dụ: 18:00 -> 20:00
    /// </summary>
    public class TimeRangeCondition : ConditionBase
    {
        private readonly TimeSpan _start;

        private readonly TimeSpan _end;

        public TimeRangeCondition(TimeSpan start, TimeSpan end)
        {
            _start = start;
            _end = end;
        }

        public override bool Evaluate(IConditionContext context)
        {
            var now = context.UtcNow.TimeOfDay;

            return now >= _start && now <= _end;
        }
    }
}