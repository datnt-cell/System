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

        public void OpenBundle(string bundleId, string source)
        {
            var bundle = _bundleProvider.GetBundle(bundleId);

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