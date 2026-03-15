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

    // =========================
    // INTERSTITIAL
    // =========================

    public bool IsInterstitialAvailable()
    {
        return Gley.MobileAds.API.IsInterstitialAvailable();
    }

    public void ShowInterstitial(UnityAction<double> onClosed)
    {
        if (!IsInterstitialAvailable())
        {
            onClosed?.Invoke(0);
            return;
        }

        Gley.MobileAds.API.ShowInterstitial(() =>
        {
            // Gley không trả revenue -> return 0
            onClosed?.Invoke(0);
        });
    }

    // =========================
    // REWARDED
    // =========================

    public bool IsRewardedAvailable()
    {
        return Gley.MobileAds.API.IsRewardedVideoAvailable();
    }

    public void ShowRewarded(UnityAction<bool, double> callback)
    {
        if (!IsRewardedAvailable())
        {
            callback?.Invoke(false, 0);
            return;
        }

        Gley.MobileAds.API.ShowRewardedVideo(success =>
        {
            // Gley không có revenue callback
            double revenue = 0;

            callback?.Invoke(success, revenue);
        });
    }

    // =========================
    // SETTINGS
    // =========================

    public void SetRemoveAds(bool remove)
    {
        Gley.MobileAds.API.RemoveAds(remove);
    }
}