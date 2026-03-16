using R3;
using Cysharp.Threading.Tasks;
using UnityEngine;
using IAPModule.Application.UseCases;
using IAPModule.Application.Interfaces;
using IAPModule.Domain;
using IAPModule.Infrastructure;
using Gley.EasyIAP;
using System;

public class IAPService : IIapPaymentService, IDisposable
{
    private readonly LoadingView _loading;
    private readonly PurchaseUseCase _useCase;
    private readonly IAPEvents _events;

    private readonly CompositeDisposable _disposables = new();

    public ReactiveProperty<bool> IsProcessing = new(false);

    public IPaymentService GetPaymentService()
        => _useCase.GetPaymentService();

    public IAPService(
        LoadingView loading,
        PurchaseUseCase useCase,
        IAPEvents events)
    {
        _loading = loading;
        _useCase = useCase;
        _events = events;

        Bind();
    }

    private void Bind()
    {
        IsProcessing
            .DistinctUntilChanged()
            .Subscribe(active =>
            {
                _loading.SetShield(active);
            })
            .AddTo(_disposables);
    }

    public async UniTask<PurchaseProductResponseData> Purchase(
        ShopProductNames productId)
    {
        if (IsProcessing.Value)
        {
            var error =
                ResponseData.GetErrorResponse<PurchaseProductResponseData>(
                    Errors.PaymentProcessing,
                    "Purchase is processing"
                );

            _events.Publish(IAPEvent.PurchaseResult(productId, error));
            return error;
        }

        IsProcessing.Value = true;

        try
        {
            _events.Publish(IAPEvent.PurchaseStart(productId));

            var validation = _useCase.ValidatePurchase(productId);

            if (!validation.Success)
            {
                var error =
                    ResponseData.GetErrorResponse<PurchaseProductResponseData>(validation);

                _events.Publish(IAPEvent.PurchaseResult(productId, error));
                return error;
            }

            var result = await _useCase.ExecuteAsync(productId);

            _events.Publish(IAPEvent.PurchaseResult(productId, result));

            return result;
        }
        catch (Exception e)
        {
            Debug.LogException(e);

            _events.Publish(IAPEvent.PurchaseError(productId, e));

            var error =
                ResponseData.GetErrorResponse<PurchaseProductResponseData>(
                    Errors.Unknown,
                    "IAP exception"
                );

            _events.Publish(IAPEvent.PurchaseResult(productId, error));

            return error;
        }
        finally
        {
            IsProcessing.Value = false;
        }
    }

    public PurchaseProductResponseData ValidatePurchase(
        ShopProductNames productId)
    {
        return _useCase.ValidatePurchase(productId);
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}