using System;

namespace ConditionEngine.Domain
{
    /// <summary>
    /// Condition kiểm tra số ngày từ khi player cài game
    /// </summary>
    public class DaysSinceInstallCondition : ICondition
    {
        private readonly int _min;
        private readonly int _max;

        public DaysSinceInstallCondition(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public bool Evaluate(IConditionContext context)
        {
            int days = context.DaysSinceInstall;

            return days >= _min && days <= _max;
        }
    }
}