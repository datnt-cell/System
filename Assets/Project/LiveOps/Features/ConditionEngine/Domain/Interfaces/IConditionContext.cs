using System;

namespace ConditionEngine.Domain
{
    /// <summary>
    /// Context chứa toàn bộ dữ liệu runtime của player
    /// Condition sẽ đọc dữ liệu từ đây
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

        float TotalSpend { get; }

        /// <summary>
        /// Tổng số ads đã xem
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

        bool HasPurchased(string productId);

        // =====================
        // PLAYER INFO
        // =====================

        string Country { get; }

        int AppBuildVersion { get; }

        /// <summary>
        /// Segment của player (whale, spender, non_spender...)
        /// </summary>
        string PlayerSegment { get; }

        // =====================
        // TIME
        // =====================

        DateTime UtcNow { get; }

        /// <summary>
        /// Số ngày kể từ khi player cài game
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
        /// Kiểm tra player có item không
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
        /// Lấy progress của event
        /// </summary>
        int GetEventProgress(string eventId);
    }
}