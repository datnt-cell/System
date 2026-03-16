using Sirenix.OdinInspector;

public enum ConditionType
{
    // PLAYER
    [LabelText("👤 Người chơi/Level")]
    PlayerLevel,

    [LabelText("🎮 Người chơi/Stage")]
    Stage,

    [LabelText("📱 Người chơi/Số lần mở game")]
    SessionCount,

    // MONETIZATION
    [LabelText("💰 Kiếm tiền/Tổng tiền đã nạp")]
    TotalSpend,

    [LabelText("💰 Kiếm tiền/Số lần mua")]
    PurchaseCount,

    [LabelText("🛒 Kiếm tiền/Đã từng mua")]
    HasAnyPurchase,

    [LabelText("🛒 Kiếm tiền/Đã mua sản phẩm")]
    ProductPurchased,

    [LabelText("🚫 Ads/Đã mua remove ads")]
    RemoveAdsPurchased,

    // ADS
    [LabelText("📺 Ads/Số quảng cáo đã xem")]
    AdsWatchCount,

    [LabelText("📺 Ads/Rewarded đã xem")]
    RewardedAdsWatched,

    [LabelText("📺 Ads/Interstitial đã xem")]
    InterstitialAdsWatched,

    [LabelText("💵 Ads/Tổng doanh thu quảng cáo")]
    TotalAdsRevenue,

    [LabelText("📅 Ads/Doanh thu quảng cáo hôm nay")]
    AdsRevenueToday,

    // INVENTORY
    [LabelText("🎒 Kho đồ/Có item")]
    InventoryItem,

    [LabelText("💎 Kho đồ/Số lượng tiền")]
    CurrencyAmount,

    // PLAYER INFO
    [LabelText("🌍 Thông tin người chơi/Quốc gia")]
    Country,

    [LabelText("📦 Thông tin người chơi/Phiên bản build")]
    AppBuildVersion,

    // GAME STATE
    [LabelText("🆕 Trạng thái game/Người chơi mới")]
    NewUser,

    [LabelText("📜 Trạng thái game/Nhiệm vụ")]
    Quest,

    [LabelText("🎯 Trạng thái game/Tiến độ sự kiện")]
    EventProgress,

    // TIME
    [LabelText("📅 Thời gian/Khoảng ngày")]
    DateRange,

    [LabelText("⏰ Thời gian/Khoảng giờ")]
    TimeRange,

    [LabelText("📆 Thời gian/Ngày trong tuần")]
    DayOfWeek,

    [LabelText("🗓 Thời gian/Nhiều ngày trong tuần")]
    MultiDayOfWeek,

    [LabelText("📆 Thời gian/Số ngày từ khi cài game")]
    DaysSinceInstall,

    [LabelText("⏱ Thời gian/Tổng thời gian chơi")]
    PlayTime,

    // LOGIC
    [LabelText("🚫 Logic/Phủ định (Not)")]
    Not,
}