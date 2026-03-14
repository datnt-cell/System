using System;

namespace ConditionEngine.Domain
{
    /// <summary>
    /// Condition kiểm tra số session player đã chơi
    /// </summary>
    public class SessionCountCondition : ConditionBase
    {
        private readonly int _min;
        private readonly int _max;

        public SessionCountCondition(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public override bool Evaluate(IConditionContext context)
        {
            int session = context.SessionCount;

            if (session < _min)
                return false;

            if (_max > 0 && session > _max)
                return false;

            return true;
        }
    }
}