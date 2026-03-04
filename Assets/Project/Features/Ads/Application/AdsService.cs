using UnityEngine.Events;

/// <summary>
/// Xử lý toàn bộ logic quảng cáo.
/// Không biết View là gì.
/// </summary>
public class AdsService
{
    private readonly AdsState _state;
    private readonly IAdsRepository _repo;
    private readonly IAdsPolicy _policy;
    private readonly IAdsProvider _provider;

    public AdsService(
        AdsState state,
        IAdsRepository repo,
        IAdsPolicy policy,
        IAdsProvider provider)
    {
        _state = state;
        _repo = repo;
        _policy = policy;
        _provider = provider;

        // Load dữ liệu khi khởi tạo
        _repo.Load(_state);
    }

    public void TryShowInterstitial(int level)
    {
        if (_state.IsRemoveAds.Value)
            return;

        if (!_policy.CanShowInterstitial(level, _state.InterstitialCount.Value))
            return;

        _provider.ShowInterstitial();

        _state.IncreaseInterstitial();
        _repo.Save(_state);
    }

    public void ShowRewarded(UnityAction<bool> callback)
    {
        _state.ShowShield();

        _provider.ShowRewarded(success =>
        {
            if (success)
            {
                _state.IncreaseReward();
                _repo.Save(_state);
            }

            _state.HideShield();
            callback?.Invoke(success);
        });
    }

    public void RemoveAds()
    {
        _state.SetRemoveAds();
        _repo.Save(_state);
    }
}