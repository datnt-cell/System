using R3;
using Gley.EasyIAP;
using Cysharp.Threading.Tasks;
using IAPModule.Application.UseCases;
using IAPModule.Domain.Entities;

/// <summary>
/// Presenter kết nối View và ViewModel.
/// Không chứa logic business.
/// Không chứa logic SDK.
/// </summary>
public class IAPPresenter : IIapPaymentService
{
    private readonly IAPLoading _loading;
    private readonly PurchaseUseCase _useCase;
    private readonly CompositeDisposable _disposables = new();

    public ReactiveProperty<bool> IsProcessing = new(false);
    public Subject<(ShopProductNames, PurchaseResult)> OnPurchaseResult = new();

    public IAPPresenter(IAPLoading loading, PurchaseUseCase useCase)
    {
        _loading = loading;
        _useCase = useCase;
        Bind();
    }

    /// <summary>
    /// Bind ViewModel -> View
    /// </summary>
    private void Bind()
    {
        // Shield loading
        IsProcessing
            .Subscribe(active =>
            {
                _loading.SetShield(active);
            })
            .AddTo(_disposables);

        // Purchase success
        OnPurchaseResult
            .Subscribe(r =>
            {
                var log = string.Format("{0} {1}", r.Item1.ToString(), r.Item2.IsSuccess ? "Thành Công" : "Thất Bại");
                UnityEngine.Debug.Log(log);
            })
            .AddTo(_disposables);
    }

    /// <summary>
    /// View gọi khi click buy
    /// </summary>
    public async UniTask<PurchaseResult> Purchase(ShopProductNames productId)
    {
        // bật trạng thái processing
        // UI có thể disable button
        IsProcessing.Value = true;

        // gọi UseCase xử lý nghiệp vụ purchase
        var result = await _useCase.ExecuteAsync(productId);

        // tắt trạng thái processing
        IsProcessing.Value = false;

        // nếu purchase thành công
        OnPurchaseResult.OnNext(new(productId, result));

        return result;
    }

    public bool CanPurchase(ShopProductNames productId)
    {
        return _useCase.CanBuy(productId);
    }

    /// <summary>
    /// Dispose khi view bị destroy
    /// </summary>
    public void Dispose()
    {
        _disposables.Dispose();
    }
}