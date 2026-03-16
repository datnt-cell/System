using System;
using ConditionEngine.Domain;

namespace ConditionEngine.Infrastructure
{
    /// <summary>
    /// Implementation của IConditionContext
    /// Cung cấp dữ liệu runtime cho ConditionEngine
    /// </summary>
    public class DefaultConditionContext : IConditionContext
    {
        private readonly PlayerProvider _player;
        private readonly PurchaseProvider _purchase;
        private readonly AdsProvider _ads;
        private readonly TimeProvider _time;
        private readonly InventoryProvider _inventory;
        private readonly EventProvider _event;

        public DefaultConditionContext(
            PlayerProvider player,
            PurchaseProvider purchase,
            AdsProvider ads,
            TimeProvider time,
            InventoryProvider inventory,
            EventProvider eventProvider)
        {
            _player = player;
            _purchase = purchase;
            _ads = ads;
            _time = time;
            _inventory = inventory;
            _event = eventProvider;
        }

        // =====================
        // PLAYER PROGRESS
        // =====================

        public int PlayerLevel => _player.Level;

        public int Stage => _player.Stage;

        public int SessionCount => _player.SessionCount;

        public bool IsNewUser => _player.IsNewUser;

        // =====================
        // MONETIZATION
        // =====================

        public float TotalSpend => _purchase.TotalSpend;

        public int PurchaseCount => _purchase.PurchaseCount;

        public bool HasPurchased(string productId) => _purchase.HasPurchased(productId);

        public bool HasAnyPurchase() => _purchase.HasAnyPurchase();

        // =====================
        // ADS
        // =====================

        public int RewardedAdsWatched => _ads.GetRewardedAdsWatched();

        public int InterstitialAdsWatched => _ads.GetInterstitialAdsWatched();

        public int AdsWatchCount => RewardedAdsWatched + InterstitialAdsWatched;

        public double RewardedAdsRevenue => _ads.GetRewardedRevenue();

        public double InterstitialAdsRevenue => _ads.GetInterstitialRevenue();

        public double TotalAdsRevenue => _ads.GetTotalRevenue();

        public double AdsRevenueToday => _ads.GetRevenueToday();

        public bool IsRemoveAdsPurchased => _ads.IsRemoveAdsPurchased();

        // =====================
        // PLAYER INFO
        // =====================

        public string Country => _player.Country;

        public int AppBuildVersion => _player.AppVersion;

        // =====================
        // TIME
        // =====================

        public DateTime UtcNow => _time.Now;

        public int DaysSinceInstall => _player.DaysSinceInstall;

        public int TotalPlayTimeMinutes =>
            _player.TotalPlayTimeSeconds / 60;

        // =====================
        // INVENTORY
        // =====================

        public bool HasItem(string itemId)
        {
            return _inventory.HasItem(itemId);
        }

        public int GetCurrency(string currencyId)
        {
            return _inventory.GetCurrency(currencyId);
        }

        // =====================
        // EVENT
        // =====================

        public int GetEventProgress(string eventId)
        {
            return _event.GetProgress(eventId);
        }
    }
}