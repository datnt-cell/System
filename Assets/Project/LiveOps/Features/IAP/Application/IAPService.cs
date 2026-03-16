using R3;
using Gley.EasyIAP;
using Cysharp.Threading.Tasks;
using IAPModule.Application.UseCases;
using UnityEngine;
using IAPModule.Application.Interfaces;
using System;

public class IAPService : IIapPaymentService, IDisposable
{
    private readonly LoadingView _loading;
    private readonly PurchaseUseCase _useCase;

    private readonly CompositeDisposable _disposables = new();

    public ReactiveProperty<bool> IsProcessing = new(false);

    /// <summary>
    /// Event trả kết quả purchase cho UI
    /// </summary>
    public Subject<(ShopProductNames, PurchaseProductResponseData)> OnPurchaseResult = new();

    /// <summary>
    /// Event khi purchase bắt đầu
    /// </summary>
    public Subject<ShopProductNames> OnPurchaseStart = new();

    /// <summary>
    /// Event lỗi hệ thống
    /// </summary>
    public Subject<Exception> OnPurchaseError = new();

    public IPaymentService GetPaymentService() => _useCase.GetPaymentService();

    public IAPService(LoadingView loading, PurchaseUseCase useCase)
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

        OnPurchaseError
            .Subscribe(e =>
            {
                Debug.LogError($"IAP ERROR: {e}");
            })
            .AddTo(_disposables);
    }

    /// <summary>
    /// View gọi khi click buy
    /// </summary>
    public async UniTask<PurchaseProductResponseData> Purchase(ShopProductNames productId)
    {
        // tránh double click
        if (IsProcessing.Value)
        {
            var error = ResponseData.GetErrorResponse<PurchaseProductResponseData>(
                Errors.PaymentProcessing,
                "Purchase is processing"
            );

            OnPurchaseResult.OnNext((productId, error));
            return error;
        }

        IsProcessing.Value = true;

        try
        {
            OnPurchaseStart.OnNext(productId);

            // validate trước khi mua
            var validation = _useCase.ValidatePurchase(productId);

            if (!validation.Success)
            {
                var error = ResponseData.GetErrorResponse<PurchaseProductResponseData>(validation);
                OnPurchaseResult.OnNext((productId, error));
                return error;
            }

            // gọi UseCase xử lý nghiệp vụ purchase
            var result = await _useCase.ExecuteAsync(productId);

            OnPurchaseResult.OnNext((productId, result));
            return result;
        }
        catch (Exception e)
        {
            Debug.LogException(e);

            OnPurchaseError.OnNext(e);

            var error = ResponseData.GetErrorResponse<PurchaseProductResponseData>(
                Errors.Unknown,
                "IAP exception"
            );

            OnPurchaseResult.OnNext((productId, error));
            return error;
        }
        finally
        {
            // đảm bảo luôn tắt loading
            IsProcessing.Value = false;
        }
    }

    public PurchaseProductResponseData ValidatePurchase(ShopProductNames productId)
    {
        return _useCase.ValidatePurchase(productId);
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}