using R3;

/// <summary>
/// Entity đại diện cho trạng thái quảng cáo.
/// Là SOURCE OF TRUTH.
/// Không phụ thuộc Unity.
/// </summary>
public class AdsState
{
    public ReactiveProperty<bool> IsShieldVisible { get; }
        = new(false);

    /// <summary>
    /// Số lần đã hiển thị interstitial
    /// </summary>
    public ReactiveProperty<int> InterstitialCount { get; }
        = new(0);

    /// <summary>
    /// Số lần đã xem rewarded
    /// </summary>
    public ReactiveProperty<int> RewardCount { get; }
        = new(0);

    /// <summary>
    /// Người chơi đã mua remove ads chưa
    /// </summary>
    public ReactiveProperty<bool> IsRemoveAds { get; }
        = new(false);

    public void IncreaseInterstitial()
        => InterstitialCount.Value++;

    public void IncreaseReward()
        => RewardCount.Value++;

    public void SetRemoveAds()
        => IsRemoveAds.Value = true;

    /// <summary>
    /// Restore dữ liệu từ storage
    /// </summary>
    public void Restore(int inter, int reward, bool remove)
    {
        InterstitialCount.Value = inter;
        RewardCount.Value = reward;
        IsRemoveAds.Value = remove;
    }

    public void ShowShield() => IsShieldVisible.Value = true;
    
    public void HideShield() => IsShieldVisible.Value = false;

    public void SetNextAdTime(float time) => NextAvailableAdTime = time;

    public float NextAvailableAdTime { get; private set; }
}