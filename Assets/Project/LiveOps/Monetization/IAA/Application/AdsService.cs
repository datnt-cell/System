using R3;
using UnityEngine.Events;
using System;

namespace AdsSystem.Application
{
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

        public ReactiveProperty<bool> IsProcessing = new(false);

        public AdsState GetAdsState() => _state;

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

        public void BindView(LoadingView view)
        {
            _view = view;
            IsProcessing
                .DistinctUntilChanged()
                .Subscribe(active => _view.SetShield(active))
                .AddTo(_disposables);
        }

        // =========================
        // INTERSTITIAL
        // =========================
        public AdsResponse TryShowInterstitial(int level = 0, int season = 0)
        {
            if (_state.IsRemoveAds.Value)
                return PublishFail(AdType.Interstitial, "Remove Ads active");

            if (!_provider.IsInterstitialAvailable())
                return PublishFail(AdType.Interstitial, "Interstitial not available");

            float now = _timeProvider.CurrentTime;
            if (!_policy.CanShowInterstitial(level, season, now, _state.NextAvailableAdTime))
                return PublishFail(AdType.Interstitial, "Cooldown not finished");

            _provider.ShowInterstitial(_ => { });

            var response = new AdsResponse
            {
                Success = true,
                Type = AdType.Interstitial
            };

            _events.Publish(AdsEvent.Interstitial());

            return response;
        }

        // =========================
        // REWARDED
        // =========================
        public void ShowRewarded(UnityAction<AdsResponse> callback)
        {
            if (!_provider.IsRewardedAvailable())
            {
                var failResponse = PublishFail(AdType.Rewarded, "Rewarded not available");
                callback?.Invoke(failResponse);
                return;
            }

            _state.ShowShield();
            IsProcessing.Value = true;

            _provider.ShowRewarded((success, _) =>
            {
                var response = new AdsResponse
                {
                    Success = success,
                    Type = AdType.Rewarded
                };

                _state.HideShield();
                IsProcessing.Value = false;

                _events.Publish(AdsEvent.Rewarded(success));
                callback?.Invoke(response);
            });
        }

        // =========================
        // REMOVE ADS
        // =========================
        public AdsResponse RemoveAds()
        {
            _state.SetRemoveAds();
            _provider.SetRemoveAds(true);
            _repo.Save(_state);

            _events.Publish(AdsEvent.RemoveAds());

            return new AdsResponse
            {
                Success = true,
                Type = AdType.Custom
            };
        }

        // =========================
        // HELPER
        // =========================
        private AdsResponse PublishFail(AdType adType, string message)
        {
            var response = new AdsResponse
            {
                Success = false,
                Type = adType,
                ErrorMessage = message
            };

            _events.Publish(AdsEvent.Fail(adType, message));
            return response;
        }

        public void Dispose() => _disposables.Dispose();
    }
}