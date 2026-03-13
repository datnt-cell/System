using Cysharp.Threading.Tasks;

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
        /// Thực hiện mua item.
        /// </summary>
        public async UniTask<PurchaseProductResponseData> Purchase()
        {
            // Validate payment
            var validation = _priceStrategy.ValidatePayment();
            if (!validation.Success)
                return validation;

            // Execute payment
            var payment = await _priceStrategy.ExecutePayment();
            if (!payment.Success)
                return payment;

            // Grant reward
            GrantReward();

            return payment;
        }

        public void GrantReward()
        {
            _rewardStrategy.Grant();
        }
    }
}