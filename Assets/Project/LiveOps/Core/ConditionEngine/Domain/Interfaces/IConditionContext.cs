using System;

namespace ConditionEngine.Domain
{
    /// <summary>
    /// Context chứa toàn bộ dữ liệu runtime của player
    /// Condition sẽ đọc dữ liệu từ đây
    /// 
    /// Đây là contract trung tâm của ConditionEngine
    /// mọi Provider sẽ được adapter vào đây
    /// </summary>
    public interface IConditionContext
    {
        // =====================
        // PLAYER PROGRESS
        // =====================

        int PlayerLevel { get; }

        int Stage { get; }

        int SessionCount { get; }

        bool IsNewUser { get; }

        // =====================
        // MONETIZATION
        // =====================

        /// <summary>
        /// Tổng tiền player đã spend
        /// </summary>
        float TotalSpend { get; }

        /// <summary>
        /// Tổng số purchase
        /// </summary>
        int PurchaseCount { get; }

        /// <summary>
        /// Player đã từng mua IAP chưa
        /// </summary>
        bool HasAnyPurchase();

        /// <summary>
        /// Player đã mua product cụ thể
        /// </summary>
        bool HasPurchased(string productId);

        // =====================
        // ADS
        // =====================

        /// <summary>
        /// Tổng ads đã xem
        /// </summary>
        int AdsWatchCount { get; }

        /// <summary>
        /// Rewarded ads đã xem
        /// </summary>
        int RewardedAdsWatched { get; }

        /// <summary>
        /// Interstitial ads đã hiển thị
        /// </summary>
        int InterstitialAdsWatched { get; }

        /// <summary>
        /// Revenue rewarded ads
        /// </summary>
        double RewardedAdsRevenue { get; }

        /// <summary>
        /// Revenue interstitial ads
        /// </summary>
        double InterstitialAdsRevenue { get; }

        /// <summary>
        /// Tổng revenue từ ads
        /// </summary>
        double TotalAdsRevenue { get; }

        /// <summary>
        /// Revenue ads hôm nay
        /// </summary>
        double AdsRevenueToday { get; }

        /// <summary>
        /// Player đã mua remove ads
        /// </summary>
        bool IsRemoveAdsPurchased { get; }

        // =====================
        // PLAYER INFO
        // =====================

        string Country { get; }

        int AppBuildVersion { get; }

        // =====================
        // TIME
        // =====================

        DateTime UtcNow { get; }

        /// <summary>
        /// Số ngày từ lúc cài game
        /// </summary>
        int DaysSinceInstall { get; }

        /// <summary>
        /// Tổng thời gian chơi (phút)
        /// </summary>
        int TotalPlayTimeMinutes { get; }

        // =====================
        // INVENTORY
        // =====================

        /// <summary>
        /// Player có item không
        /// </summary>
        bool HasItem(string itemId);

        /// <summary>
        /// Lấy số lượng currency
        /// </summary>
        int GetCurrency(string currencyId);

        // =====================
        // EVENT
        // =====================

        /// <summary>
        /// Progress của event
        /// </summary>
        int GetEventProgress(string eventId);
    }
}