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
    private readonly ITimeProvider _timeProvider;

    public AdsService(
        AdsState state,
        IAdsRepository repo,
        IAdsPolicy policy,
        IAdsProvider provider,
        ITimeProvider timeProvider)
    {
        _state = state;
        _repo = repo;
        _policy = policy;
        _provider = provider;
        _timeProvider = timeProvider;

        // Load dữ liệu khi khởi tạo
        _repo.Load(_state);
    }

    public void TryShowInterstitial(int level, int season)
    {
        if (_state.IsRemoveAds.Value)
            return;

        if (!_provider.IsInterstitialAvailable())
            return;

        float now = _timeProvider.CurrentTime;

        bool canShow = _policy.CanShowInterstitial(
            level,
            season,
            now,
            _state.NextAvailableAdTime);

        if (!canShow)
            return;

        _provider.ShowInterstitial();

        _state.IncreaseInterstitial();

        float nextTime = _policy.GetNextCooldown(false, now);

        _state.SetNextAdTime(nextTime);

        _repo.Save(_state);
    }

    public void ShowRewarded(UnityAction<bool> callback)
    {
        if (!_provider.IsRewardedAvailable())
        {
            callback?.Invoke(false);
            return;
        }

        float now = _timeProvider.CurrentTime;

        _state.ShowShield();

        _provider.ShowRewarded(success =>
        {
            if (success)
            {
                _state.IncreaseReward();

                float nextTime = _policy.GetNextCooldown(true, now);
                _state.SetNextAdTime(nextTime);

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