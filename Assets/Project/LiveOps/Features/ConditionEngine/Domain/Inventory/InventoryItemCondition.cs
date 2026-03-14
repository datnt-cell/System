namespace ConditionEngine.Domain
{
    /// <summary>
    /// Condition kiểm tra player có item trong inventory
    /// </summary>
    public class InventoryItemCondition : ConditionBase
    {
        private readonly string _itemId;

        public InventoryItemCondition(string itemId)
        {
            _itemId = itemId;
        }

        public override bool Evaluate(IConditionContext context)
        {
            return context.HasItem(_itemId);
        }
    }
}