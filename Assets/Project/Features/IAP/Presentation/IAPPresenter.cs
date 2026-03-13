using R3;
using Gley.EasyIAP;
using Cysharp.Threading.Tasks;
using IAPModule.Application.UseCases;
using UnityEngine;

/// <summary>
/// Presenter kết nối View và ViewModel.
/// Không chứa logic business.
/// Không chứa logic SDK.
/// </summary>
public class IAPPresenter : IIapPaymentService
{
    private readonly LoadingView _loading;
    private readonly PurchaseUseCase _useCase;
    private readonly CompositeDisposable _disposables = new();

    public ReactiveProperty<bool> IsProcessing = new(false);

    public Subject<(ShopProductNames, PurchaseProductResponseData)> OnPurchaseResult = new();

    public IAPPresenter(LoadingView loading, PurchaseUseCase useCase)
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
        IsProcessing
            .DistinctUntilChanged()
            .Subscribe(active =>
            {
                _loading.SetShield(active);
            })
            .AddTo(_disposables);

        OnPurchaseResult
            .Subscribe(r =>
            {
                var log = $"{r.Item1} {(r.Item2.Success ? "Thành Công" : "Thất Bại")}";
                Debug.Log(log);
            })
            .AddTo(_disposables);
    }


    /// <summary>
    /// View gọi khi click buy
    /// </summary>
    public async UniTask<PurchaseProductResponseData> Purchase(ShopProductNames productId)
    {
        // bật trạng thái processing
        IsProcessing.Value = true;

        // validate trước khi mua
        var validation = _useCase.ValidatePurchase(productId);

        if (!validation.Success)
        {
            IsProcessing.Value = false;
            return validation;
        }

        // gọi UseCase xử lý nghiệp vụ purchase
        var result = await _useCase.ExecuteAsync(productId);

        // tắt trạng thái processing
        IsProcessing.Value = false;

        // push result cho UI
        OnPurchaseResult.OnNext(new(productId, result));

        return result;
    }

    public PurchaseProductResponseData ValidatePurchase(ShopProductNames productId)
    {
        return _useCase.ValidatePurchase(productId);
    }

    /// <summary>
    /// Dispose khi view bị destroy
    /// </summary>
    public void Dispose()
    {
        _disposables.Dispose();
    }
}