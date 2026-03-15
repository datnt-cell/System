using R3;

/// <summary>
/// ViewModel chỉ expose dữ liệu reactive cho View.
/// </summary>
public class AdsViewModel
{
    public ReadOnlyReactiveProperty<bool> IsShieldVisible { get; }
    public ReadOnlyReactiveProperty<bool> IsRemoveAds { get; }

    // =========================
    // INTERSTITIAL
    // =========================

    public ReadOnlyReactiveProperty<int> InterstitialCount { get; }
    public ReadOnlyReactiveProperty<double> InterstitialRevenue { get; }

    // =========================
    // REWARDED
    // =========================

    public ReadOnlyReactiveProperty<int> RewardCount { get; }
    public ReadOnlyReactiveProperty<double> RewardRevenue { get; }

    // =========================
    // TOTAL
    // =========================

    public ReadOnlyReactiveProperty<double> TotalRevenue { get; }
    public ReadOnlyReactiveProperty<double> RevenueToday { get; }

    public AdsViewModel(AdsState state)
    {
        IsShieldVisible = state.IsShieldVisible.ToReadOnlyReactiveProperty();
        IsRemoveAds = state.IsRemoveAds.ToReadOnlyReactiveProperty();

        var inter = state.AdTypes[AdType.Interstitial];
        var reward = state.AdTypes[AdType.Rewarded];

        InterstitialCount = inter.Count.ToReadOnlyReactiveProperty();
        InterstitialRevenue = inter.Revenue.ToReadOnlyReactiveProperty();

        RewardCount = reward.Count.ToReadOnlyReactiveProperty();
        RewardRevenue = reward.Revenue.ToReadOnlyReactiveProperty();

        TotalRevenue = state.TotalRevenue.ToReadOnlyReactiveProperty();
        RevenueToday = state.RevenueToday.ToReadOnlyReactiveProperty();
    }
}