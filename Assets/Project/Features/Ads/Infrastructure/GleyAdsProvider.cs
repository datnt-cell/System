using UnityEngine.Events;

/// <summary>
/// Adapter cho Gley Ads.
/// Nếu sau này đổi sang LevelPlay chỉ cần đổi class này.
/// </summary>
public class GleyAdsProvider : IAdsProvider
{
    public void ShowInterstitial()
    {
        Gley.MobileAds.API.ShowInterstitial();
    }

    public void ShowRewarded(UnityAction<bool> callback)
    {
        Gley.MobileAds.API.ShowRewardedVideo(callback);
    }
}