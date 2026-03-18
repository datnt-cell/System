using CurrencySystem.Application;

namespace StoreSystem.Domain
{
    /// <summary>
    /// Reward dạng bundle (tái sử dụng CurrencyBundleUseCase).
    /// </summary>
    public class BundleRewardStrategy : IRewardStrategy
    {
        private readonly CurrencyBundleUseCase _bundleUseCase;
        private readonly string _bundleId;

        public BundleRewardStrategy(
            CurrencyBundleUseCase bundleUseCase,
            string bundleId)
        {
            _bundleUseCase = bundleUseCase;
            _bundleId = bundleId;
        }

        public void Grant()
        {
            _bundleUseCase.OpenBundle(_bundleId, _bundleId);
        }
    }
}