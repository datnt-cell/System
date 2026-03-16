using Sirenix.OdinInspector;

public class ConditionContextDocumentation
{
    [Title("Condition Context Fields")]
    [InfoBox(
@"----------------------------------------------------------------

• PlayerLevel (read-only)
  Level hiện tại của người chơi.

• Stage (read-only)
  Stage hoặc level gameplay hiện tại.

• SessionCount (read-only)
  Tổng số session của player.
  Session tăng khi:
  - Player mở lại game
  - Hoặc sau khoảng thời gian idle dài.

• IsNewUser (read-only)
  Xác định player có phải người chơi mới hay không.

----------------------------------------------------------------
PURCHASE
----------------------------------------------------------------

• TotalSpend (read-only)
  Tổng số tiền người chơi đã chi trong game (USD).

• PurchaseCount (read-only)
  Tổng số lần purchase.

• HasAnyPurchase (read-only)
  True nếu player đã từng purchase ít nhất 1 lần.

----------------------------------------------------------------
ADS
----------------------------------------------------------------

• RewardedAdsWatched (read-only)
  Số lần người chơi xem Rewarded Ads.

• InterstitialAdsWatched (read-only)
  Số lần hiển thị Interstitial Ads.

• AdsWatchCount (read-only)
  Tổng số Ads đã xem.

• TotalAdsRevenue (read-only)
  Tổng doanh thu Ads từ player này.

• AdsRevenueToday (read-only)
  Doanh thu Ads của player trong ngày hôm nay.

----------------------------------------------------------------
PLAYER INFO
----------------------------------------------------------------

• Country (read-only)
  Quốc gia của player.

• PlayerSegment (read-only)
  Phân khúc player (ví dụ: whale, payer, f2p...).

• AppBuildVersion (read-only)
  Build version của game.

----------------------------------------------------------------
TIME
----------------------------------------------------------------

• UtcNow (read-only)
  Thời gian UTC hiện tại của game.

• DaysSinceInstall (read-only)
  Số ngày kể từ khi player cài game.

• TotalPlayTimeMinutes (read-only)
  Tổng thời gian chơi game (phút).

----------------------------------------------------------------
INVENTORY
----------------------------------------------------------------

• Gold (read-only)
  Số lượng Gold hiện tại.

• Gem (read-only)
  Số lượng Gem hiện tại.

----------------------------------------------------------------
")]
    public string Info;
}