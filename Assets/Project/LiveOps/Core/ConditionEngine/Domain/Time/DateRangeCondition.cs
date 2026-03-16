using System;

namespace ConditionEngine.Domain
{
    /// <summary>
    /// Condition kiểm tra thời gian hiện tại có nằm trong khoảng cho phép
    /// Ví dụ dùng cho:
    /// Event
    /// Offer
    /// LiveOps
    /// </summary>
    public class DateRangeCondition : ConditionBase
    {
        public DateTime Start;

        public DateTime End;

        public DateRangeCondition(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public override bool Evaluate(IConditionContext context)
        {
            var now = context.UtcNow;

            return now >= Start && now <= End;
        }
    }
}