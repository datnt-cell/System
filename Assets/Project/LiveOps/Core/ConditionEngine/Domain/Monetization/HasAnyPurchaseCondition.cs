namespace ConditionEngine.Domain
{
    public class HasAnyPurchaseCondition : ConditionBase
    {
        public override bool Evaluate(IConditionContext context)
        {
            return context.HasAnyPurchase();
        }
    }
}