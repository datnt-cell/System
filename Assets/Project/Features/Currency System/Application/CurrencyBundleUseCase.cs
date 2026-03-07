using CurrencySystem.Domain;

namespace CurrencySystem.Application
{
    public class CurrencyBundleUseCase
    {
        private readonly CurrencyService _currencyService;
        private readonly ICurrencyBundleProvider _bundleProvider;

        public CurrencyBundleUseCase(
            CurrencyService currencyService,
            ICurrencyBundleProvider bundleProvider)
        {
            _currencyService = currencyService;
            _bundleProvider = bundleProvider;
        }

        /// <summary>
        /// Mở bundle theo ID.
        /// </summary>
        public void OpenBundle(string bundleId, string source)
        {
            var bundle = _bundleProvider.GetBundle(bundleId);

            if (bundle == null)
                throw new System.Exception($"Bundle not found: {bundleId}");

            foreach (var reward in bundle.Rewards)
            {
                _currencyService.Add(
                    reward.Id,
                    reward.Amount,
                    source);
            }
        }
    }
}