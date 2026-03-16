using R3;
using UnityEngine.Events;
using System;

public class AdsService : IDisposable
{
    private readonly AdsState _state;
    private readonly IAdsRepository _repo;
    private readonly IAdsPolicy _policy;
    private readonly IAdsProvider _provider;
    private readonly ITimeProvider _timeProvider;
    private readonly AdsEvents _events;

    private readonly CompositeDisposable _disposables = new();

    private LoadingView _view;
    public AdsViewModel vm;

    public AdsState GetState() => _state;

    public AdsService(
        AdsState state,
        IAdsRepository repo,
        IAdsPolicy policy,
        IAdsProvider provider,
        ITimeProvider timeProvider,
        AdsEvents events)
    {
        _state = state;
        _repo = repo;
        _policy = policy;
        _provider = provider;
        _timeProvider = timeProvider;
        _events = events;

        _repo.Load(_state);

        _provider.SetRemoveAds(_state.IsRemoveAds.Value);
    }

    // =========================
    // VIEW
    // =========================

    public void BindView(LoadingView view)
    {
        _view = view;

        vm = new AdsViewModel(_state);

        vm.IsShieldVisible
            .DistinctUntilChanged()
            .Subscribe(active =>
            {
                _view.SetShield(active);
            })
            .AddTo(_disposables);
    }

    // =========================
    // INTERSTITIAL
    // =========================

    public bool CanShowInterstitial(int level = 0, int season = 0)
    {
        if (_state.IsRemoveAds.Value)
            return false;

        if (!_provider.IsInterstitialAvailable())
            return false;

        float now = _timeProvider.CurrentTime;

        return _policy.CanShowInterstitial(
            level,
            season,
            now,
            _state.NextAvailableAdTime);
    }

    public void TryShowInterstitial(int level = 0, int season = 0)
    {
        if (!CanShowInterstitial(level, season))
            return;

        float now = _timeProvider.CurrentTime;

        _provider.ShowInterstitial(revenue =>
        {
            _state.AddAdRevenue(AdType.Interstitial, revenue);

            float nextTime = _policy.GetNextCooldown(false, now);
            _state.SetNextAdTime(nextTime);

            _repo.Save(_state);

            _events.Publish(AdsEvent.Interstitial(revenue));
        });
    }

    // =========================
    // REWARDED
    // =========================

    public void ShowRewarded(UnityAction<bool> callback)
    {
        if (!_provider.IsRewardedAvailable())
        {
            callback?.Invoke(false);
            return;
        }

        float now = _timeProvider.CurrentTime;

        _events.Publish(AdsEvent.RewardStart());

        _state.ShowShield();

        _provider.ShowRewarded((success, revenue) =>
        {
            if (success)
            {
                _state.AddAdRevenue(AdType.Rewarded, revenue);

                float nextTime = _policy.GetNextCooldown(true, now);
                _state.SetNextAdTime(nextTime);

                _repo.Save(_state);
            }

            _state.HideShield();

            _events.Publish(AdsEvent.RewardResult(success, revenue));

            callback?.Invoke(success);
        });
    }

    // =========================
    // REMOVE ADS
    // =========================

    public void RemoveAds()
    {
        _state.SetRemoveAds();

        _provider.SetRemoveAds(true);

        _repo.Save(_state);

        _events.Publish(AdsEvent.RemoveAds());
    }

    // =========================

    public void Dispose()
    {
        _disposables.Dispose();
    }
}