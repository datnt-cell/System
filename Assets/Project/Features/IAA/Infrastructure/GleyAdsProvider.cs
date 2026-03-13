using Cysharp.Threading.Tasks;
using UnityEngine.Events;

public class GleyAdsProvider : IAdsProvider
{
    private bool _initialized;

    public bool IsInitialized()
    {
        return _initialized;
    }

    public UniTask InitializeAsync()
    {
        var tcs = new UniTaskCompletionSource();

        Gley.MobileAds.API.Initialize(() =>
        {
            _initialized = true;
            tcs.TrySetResult();
        });

        return tcs.Task;
    }

    public void ShowInterstitial()
    {
        Gley.MobileAds.API.ShowInterstitial();
    }

    public void ShowRewarded(UnityAction<bool> callback)
    {
        Gley.MobileAds.API.ShowRewardedVideo(callback);
    }

    public bool IsRewardedAvailable()
    {
        return Gley.MobileAds.API.IsRewardedVideoAvailable();
    }

    public bool IsInterstitialAvailable()
    {
        return Gley.MobileAds.API.IsInterstitialAvailable();
    }
}