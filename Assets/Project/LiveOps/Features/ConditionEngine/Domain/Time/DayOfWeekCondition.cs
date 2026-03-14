using System;

namespace ConditionEngine.Domain
{
    /// <summary>
    /// Condition kiểm tra ngày trong tuần
    /// </summary>
    public class DayOfWeekCondition : ConditionBase
    {
        private readonly DayOfWeek _day;

        public DayOfWeekCondition(DayOfWeek day)
        {
            _day = day;
        }

        public override bool Evaluate(IConditionContext context)
        {
            return context.UtcNow.DayOfWeek == _day;
        }
    }
}