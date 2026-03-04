using R3;

/// <summary>
/// ViewModel chỉ expose dữ liệu reactive cho View.
/// </summary>
public class AdsViewModel
{
    public ReadOnlyReactiveProperty<bool> IsShieldVisible { get; }
    public ReadOnlyReactiveProperty<int> InterstitialCount { get; }
    public ReadOnlyReactiveProperty<int> RewardCount { get; }
    public ReadOnlyReactiveProperty<bool> IsRemoveAds { get; }

    public AdsViewModel(AdsState state)
    {
        InterstitialCount = state.InterstitialCount.ToReadOnlyReactiveProperty();
        RewardCount = state.RewardCount.ToReadOnlyReactiveProperty();
        IsRemoveAds = state.IsRemoveAds.ToReadOnlyReactiveProperty();
        IsShieldVisible = state.IsShieldVisible.ToReadOnlyReactiveProperty();
    }
}