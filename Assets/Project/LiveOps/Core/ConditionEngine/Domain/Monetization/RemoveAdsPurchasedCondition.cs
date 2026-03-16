namespace ConditionEngine.Domain
{
    public class RemoveAdsPurchasedCondition : ConditionBase
    {
        public override bool Evaluate(IConditionContext context)
        {
            return context.IsRemoveAdsPurchased;
        }
    }
}