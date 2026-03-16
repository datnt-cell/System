using R3;
using System.Collections.Generic;

public class AdsState
{
    /// <summary>
    /// Shield đang bật (che quảng cáo)
    /// </summary>
    public ReactiveProperty<bool> IsShieldVisible { get; }
        = new(false);

    /// <summary>
    /// Người chơi đã mua remove ads
    /// </summary>
    public ReactiveProperty<bool> IsRemoveAds { get; }
        = new(false);

    /// <summary>
    /// Thời gian quảng cáo tiếp theo
    /// </summary>
    public float NextAvailableAdTime { get; private set; }

    /// <summary>
    /// Chu kỳ interstitial
    /// </summary>
    public ReactiveProperty<int> InterstitialPeriod { get; }
        = new(3);

    /// <summary>
    /// Tổng revenue user
    /// </summary>
    public ReactiveProperty<double> TotalRevenue { get; }
        = new(0);

    /// <summary>
    /// Revenue hôm nay
    /// </summary>
    public ReactiveProperty<double> RevenueToday { get; }
        = new(0);

    /// <summary>
    /// State theo từng loại quảng cáo
    /// </summary>
    public Dictionary<AdType, AdTypeState> AdTypes { get; }
        = new();

    public AdsState()
    {
        AdTypes[AdType.Interstitial] = new AdTypeState();
        AdTypes[AdType.Rewarded] = new AdTypeState();
        AdTypes[AdType.Custom] = new AdTypeState();
    }

    public void AddAdRevenue(AdType type, double revenue)
    {
        var ad = AdTypes[type];

        ad.AddImpression(revenue);

        TotalRevenue.Value += revenue;
        RevenueToday.Value += revenue;
    }

    public void ResetToday()
    {
        RevenueToday.Value = 0;

        foreach (var ad in AdTypes.Values)
            ad.ResetToday();
    }

    public void SetRemoveAds()
        => IsRemoveAds.Value = true;

    public void ShowShield()
        => IsShieldVisible.Value = true;

    public void HideShield()
        => IsShieldVisible.Value = false;

    public void SetNextAdTime(float time)
        => NextAvailableAdTime = time;
}