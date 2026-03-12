namespace StoreSystem.Domain
{
    /// <summary>
    /// Aggregate Root đại diện một vật phẩm bán trong Shop hoặc Offer.
    /// </summary>
    public class StoreItem
    {
        public string Id { get; }

        private readonly IPriceStrategy _priceStrategy;
        private readonly IRewardStrategy _rewardStrategy;

        public StoreItem(
            string id,
            IPriceStrategy priceStrategy,
            IRewardStrategy rewardStrategy)
        {
            Id = id;
            _priceStrategy = priceStrategy;
            _rewardStrategy = rewardStrategy;
        }

        /// <summary>
        /// Thử mua item.
        /// </summary>
        public bool TryPurchase()
        {
            if (!_priceStrategy.CanPay())
                return false;

            _priceStrategy.Pay();
            _rewardStrategy.Grant();

            return true;
        }
    }
}