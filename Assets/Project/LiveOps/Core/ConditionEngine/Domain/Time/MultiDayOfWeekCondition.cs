using System;

namespace ConditionEngine.Domain
{
    /// <summary>
    /// Condition kiểm tra nhiều ngày trong tuần
    /// Ví dụ: Weekend, Mon-Wed-Fri
    /// </summary>
    public class MultiDayOfWeekCondition : ICondition
    {
        private readonly WeekDayFlags _days;

        public MultiDayOfWeekCondition(WeekDayFlags days)
        {
            _days = days;
        }

        public bool Evaluate(IConditionContext context)
        {
            var now = context.UtcNow.DayOfWeek;

            var flag = ConvertToFlag(now);

            return (_days & flag) != 0;
        }

        private static WeekDayFlags ConvertToFlag(DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Monday => WeekDayFlags.Monday,
                DayOfWeek.Tuesday => WeekDayFlags.Tuesday,
                DayOfWeek.Wednesday => WeekDayFlags.Wednesday,
                DayOfWeek.Thursday => WeekDayFlags.Thursday,
                DayOfWeek.Friday => WeekDayFlags.Friday,
                DayOfWeek.Saturday => WeekDayFlags.Saturday,
                DayOfWeek.Sunday => WeekDayFlags.Sunday,
                _ => WeekDayFlags.None
            };
        }
    }
}