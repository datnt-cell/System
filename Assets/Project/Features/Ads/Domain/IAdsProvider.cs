using UnityEngine.Events;

/// <summary>
/// Adapter layer giữa hệ thống và SDK quảng cáo.
/// </summary>
public interface IAdsProvider
{
    void ShowInterstitial();
    void ShowRewarded(UnityAction<bool> callback);
}