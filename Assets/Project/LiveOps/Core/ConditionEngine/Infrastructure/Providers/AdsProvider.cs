
namespace ConditionEngine.Infrastructure
{
    /// <summary>
    /// Provider cung cấp dữ liệu Ads cho ConditionEngine
    /// </summary>
    public class AdsProvider
    {
        private readonly AdsState _ads;

        public AdsProvider(AdsState ads)
        {
            _ads = ads;
        }

        public int GetRewardedAdsWatched()
        {
            return _ads.AdTypes[AdType.Rewarded].Count.Value;
        }

        public int GetInterstitialAdsWatched()
        {
            return _ads.AdTypes[AdType.Interstitial].Count.Value;
        }

        public double GetRewardedRevenue()
        {
            return _ads.AdTypes[AdType.Rewarded].Revenue.Value;
        }

        public double GetInterstitialRevenue()
        {
            return _ads.AdTypes[AdType.Interstitial].Revenue.Value;
        }

        public double GetTotalRevenue()
        {
            return _ads.TotalRevenue.Value;
        }

        public double GetRevenueToday()
        {
            return _ads.RevenueToday.Value;
        }

        public bool IsRemoveAdsPurchased()
        {
            return _ads.IsRemoveAds.Value;
        }
    }
}