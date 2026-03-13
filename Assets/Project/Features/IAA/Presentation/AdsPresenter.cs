using R3;
using UnityEngine.Events;
using System;

/// <summary>
/// Presenter kết nối View và Service.
/// Không chứa logic SDK.
/// </summary>
public class AdsPresenter : IDisposable
{
    private readonly AdsLoading _view;
    private readonly AdsViewModel _vm;
    private readonly AdsService _service;

    private readonly CompositeDisposable _disposables = new();

    public AdsPresenter(
        AdsLoading view,
        AdsViewModel vm,
        AdsService service)
    {
        _view = view;
        _vm = vm;
        _service = service;

        Bind();
    }

    private void Bind()
    {
        // Bind ViewModel -> View
        _vm.IsShieldVisible
            .Subscribe(active =>
            {
                _view.SetShieldActive(active);
            })
            .AddTo(_disposables);
    }

    public bool CanShowInterstitial()
    {
        return _service.CanShowInterstitial();
    }

    public void OnClickInterstitial()
    {
        _service.TryShowInterstitial();
    }

    public void OnClickRewarded(UnityAction<bool> callback)
    {
        _service.ShowRewarded(callback);
    }

    public void OnClickRemoveAds()
    {
        _service.RemoveAds();
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}