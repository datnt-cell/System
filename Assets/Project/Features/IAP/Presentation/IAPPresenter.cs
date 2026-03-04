using R3;
using IAPModule.Presentation.ViewModel;
using Gley.EasyIAP;
using Cysharp.Threading.Tasks;

/// <summary>
/// Presenter kết nối View và ViewModel.
/// Không chứa logic business.
/// Không chứa logic SDK.
/// </summary>
public class IAPPresenter
{
    private readonly IAPLoading _view;
    private readonly IAPViewModel _vm;

    private readonly CompositeDisposable _disposables = new();

    public IAPPresenter(
        IAPLoading view,
        IAPViewModel vm)
    {
        _view = view;
        _vm = vm;

        Bind();
    }

    /// <summary>
    /// Bind ViewModel -> View
    /// </summary>
    private void Bind()
    {
        // Shield loading
        _vm.IsProcessing
            .Subscribe(active =>
            {
                _view.SetShield(active);
            })
            .AddTo(_disposables);

        // Purchase success
        _vm.OnPurchaseSuccess
            .Subscribe(productId =>
            {
                UnityEngine.Debug.Log("Mua thành công: " + productId);
                _view.ShowSuccess(productId);
            })
            .AddTo(_disposables);
    }

    /// <summary>
    /// View gọi khi click buy
    /// </summary>
    public void OnClickBuy(ShopProductNames productId)
    {
        _vm.BuyAsync(productId).Forget();
    }

    /// <summary>
    /// Dispose khi view bị destroy
    /// </summary>
    public void Dispose()
    {
        _disposables.Dispose();
    }
}