namespace ConditionEngine.Domain
{
    /// <summary>
    /// Condition kiểm tra player đã mua product chưa
    /// </summary>
    public class ProductPurchasedCondition : ConditionBase
    {
        public string ProductId;

        public bool RequiredState;

        public ProductPurchasedCondition(string productId, bool requiredState = true)
        {
            ProductId = productId;
            RequiredState = requiredState;
        }

        public override bool Evaluate(IConditionContext context)
        {
            bool purchased = context.HasPurchased(ProductId);

            return purchased == RequiredState;
        }
    }
}