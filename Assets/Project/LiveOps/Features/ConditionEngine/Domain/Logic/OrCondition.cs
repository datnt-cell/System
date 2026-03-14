using System.Collections.Generic;

namespace ConditionEngine.Domain
{
    /// <summary>
    /// OR logic
    /// Chỉ cần 1 condition pass
    /// </summary>
    public class OrCondition : ConditionBase
    {
        public List<ICondition> Conditions = new();

        public override bool Evaluate(IConditionContext context)
        {
            foreach (var condition in Conditions)
            {
                if (condition.Evaluate(context))
                    return true;
            }

            return false;
        }
    }
}