using Cysharp.Threading.Tasks;
using UnityEngine.Events;

public interface IAdsProvider
{
    UniTask InitializeAsync();

    bool IsInitialized();

    void ShowInterstitial();

    void ShowRewarded(UnityAction<bool> callback);

    bool IsInterstitialAvailable();

    bool IsRewardedAvailable();
}