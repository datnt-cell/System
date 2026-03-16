using Cysharp.Threading.Tasks;
using UnityEngine.Events;

public interface IAdsProvider
{
    UniTask InitializeAsync();

    bool IsInitialized();

    // =========================
    // INTERSTITIAL
    // =========================

    bool IsInterstitialAvailable();

    /// <summary>
    /// Show interstitial và trả revenue khi ad đóng
    /// </summary>
    void ShowInterstitial(UnityAction<double> onClosed);

    // =========================
    // REWARDED
    // =========================

    bool IsRewardedAvailable();

    /// <summary>
    /// success = có nhận reward
    /// revenue = ad revenue
    /// </summary>
    void ShowRewarded(UnityAction<bool, double> callback);

    // =========================
    // SETTINGS
    // =========================

    void SetRemoveAds(bool remove);
}