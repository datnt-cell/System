using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class GleyAdsProvider : IAdsProvider
{
    private bool _initialized;

    public bool IsInitialized()
    {
        return _initialized;
    }

    public async UniTask InitializeAsync()
    {
        if (_initialized)
            return;

        var tcs = new UniTaskCompletionSource();

        Gley.MobileAds.API.Initialize(() =>
        {
            Debug.Log("Ads Callback");
            tcs.TrySetResult();
        });

        var result = await UniTask.WhenAny(
            tcs.Task,
            UniTask.Delay(1000)
        );

        if (result == 0)
            Debug.Log("Ads initialized by callback");
        else
            Debug.LogWarning("Ads init timeout -> continue");

        _initialized = true;
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