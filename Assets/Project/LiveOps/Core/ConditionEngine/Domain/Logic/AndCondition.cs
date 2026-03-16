using System.Collections.Generic;

namespace ConditionEngine.Domain
{
    /// <summary>
    /// AND logic
    /// Tất cả condition con phải pass
    /// </summary>
    public class AndCondition : ConditionBase
    {
        public List<ICondition> Conditions = new();

        public override bool Evaluate(IConditionContext context)
        {
            foreach (var condition in Conditions)
            {
                if (!condition.Evaluate(context))
                    return false;
            }

            return true;
        }
    }
}