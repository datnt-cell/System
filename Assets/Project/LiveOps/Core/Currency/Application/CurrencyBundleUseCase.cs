using System.Collections.Generic;
using CurrencySystem.Domain;

namespace CurrencySystem.Application
{
    public class CurrencyBundleUseCase
    {
        private readonly CurrencyService _currencyService;
        private readonly ICurrencyBundleProvider _bundleProvider;
        private readonly CurrencyBundleEvents _bundleEvents;

        public CurrencyBundleUseCase(
            CurrencyService currencyService,
            ICurrencyBundleProvider bundleProvider,
            CurrencyBundleEvents bundleEvents)
        {
            _currencyService = currencyService;
            _bundleProvider = bundleProvider;
            _bundleEvents = bundleEvents;
        }

        /// <summary>
        /// Kiểm tra bundle có thể mở không
        /// </summary>
        public bool CanOpenBundle(string bundleId)
        {
            var bundle = _bundleProvider.GetBundle(bundleId);
            return bundle != null && bundle.Rewards.Count > 0;
        }

        /// <summary>
        /// Mở bundle và trả về response tổng hợp
        /// </summary>
        public CurrencyBundleOpenResponse OpenBundle(string bundleId, string source)
        {
            var bundle = _bundleProvider.GetBundle(bundleId);
            if (bundle == null)
            {
                return CurrencyBundleOpenResponse.GetErrorResponse<CurrencyBundleOpenResponse>(
                    Errors.Unknown,
                    $"Bundle not found: {bundleId}"
                );
            }

            var response = new CurrencyBundleOpenResponse
            {
                BundleId = bundleId,
                Success = true
            };

            foreach (var reward in bundle.Rewards)
            {
                // Add currency
                var currencyResponse = _currencyService.AddCurrency(reward.Id, reward.Amount, source);

                // Tạo response reward chi tiết
                var rewardResponse = new CurrencyBundleRewardResponse
                {
                    BundleId = bundleId,
                    CurrencyId = reward.Id,
                    Amount = reward.Amount,
                    Balance = currencyResponse.Balance,
                    ChangedAmount = currencyResponse.ChangedAmount,
                    Source = source,
                    Success = currencyResponse.Success,
                    ErrorCode = currencyResponse.Error?.Code,
                    ErrorMessage = currencyResponse.Error?.Message
                };

                response.Rewards.Add(rewardResponse);

                // Publish bundle event
                var evt = currencyResponse.Success
                    ? CurrencyBundleEvent.Opened(
                        bundleId,
                        reward.Id,
                        reward.Amount,
                        rewardResponse.Balance,
                        rewardResponse.ChangedAmount,
                        source
                    )
                    : CurrencyBundleEvent.Fail(
                        bundleId,
                        reward.Id,
                        reward.Amount,
                        source,
                        rewardResponse.ErrorCode ?? Errors.Unknown,
                        rewardResponse.ErrorMessage ?? "Unknown error"
                    );

                _bundleEvents.Publish(evt);
            }

            return response;
        }
    }
}