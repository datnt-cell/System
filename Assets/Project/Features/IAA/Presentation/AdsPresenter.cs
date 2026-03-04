using R3;
using UnityEngine.Events;

/// <summary>
/// Presenter kết nối View và Service.
/// Không chứa logic SDK.
/// </summary>
public class AdsPresenter
{
    private readonly AdsView _view;
    private readonly AdsViewModel _vm;
    private readonly AdsService _service;

    public AdsPresenter(
        AdsView view,
        AdsViewModel vm,
        AdsService service)
    {
        _view = view;
        _vm = vm;
        _service = service;

        // Bind ViewModel -> View
        _vm.IsShieldVisible
            .Subscribe(active =>
            {
                _view.SetShieldActive(active);
            });
    }

    public void OnClickInterstitial()
    {
        _service.TryShowInterstitial(1, 0);
    }

    public void OnClickRewarded(UnityAction<bool> callback)
    {
        _service.ShowRewarded(callback);
    }

    public void OnClickRemoveAds()
    {
        _service.RemoveAds();
    }
}