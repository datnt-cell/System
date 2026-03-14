using ConditionEngine.Domain;

namespace ConditionEngine.Application
{
    /// <summary>
    /// Evaluate condition tree
    /// </summary>
    public class ConditionEvaluator
    {
        /// <summary>
        /// Kiểm tra condition
        /// </summary>
        public bool Evaluate(ICondition condition, IConditionContext context)
        {
            if (condition == null)
                return true;

            return condition.Evaluate(context);
        }

        /// <summary>
        /// Kiểm tra condition group
        /// </summary>
        public bool EvaluateGroup(ConditionGroup group, IConditionContext context)
        {
            if (group == null)
                return true;

            return Evaluate(group.RootCondition, context);
        }
    }
}