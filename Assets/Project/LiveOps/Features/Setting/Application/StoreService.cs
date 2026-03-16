using System;
using System.Collections.Generic;
using R3;
using Cysharp.Threading.Tasks;
using UnityEngine;
using StoreSystem.Application;
using StoreSystem.Domain;

namespace StoreSystem.Presentation
{
    /// <summary>
    /// Service trung gian giữa UI và StoreItemUseCase.
    /// Không chứa business logic.
    /// </summary>
    public class StoreService : IDisposable
    {
        private readonly StoreItemUseCase _useCase;
        private readonly CompositeDisposable _disposables = new();

        public ReactiveProperty<bool> IsProcessing = new(false);

        public Subject<string> OnPurchaseStart = new();
        public Subject<(string, PurchaseProductResponseData)> OnPurchaseResult = new();
        public Subject<Exception> OnPurchaseError = new();

        public StoreService(StoreItemUseCase useCase)
        {
            _useCase = useCase;

            Bind();
        }

        private void Bind()
        {
            IsProcessing
                .DistinctUntilChanged()
                .Subscribe(active =>
                {
                    Debug.Log($"[Store] Processing: {active}");
                })
                .AddTo(_disposables);
        }

        // =========================
        // QUERY
        // =========================

        public IReadOnlyCollection<StoreItem> GetAll()
        {
            return _useCase.GetAll();
        }

        public StoreItem Get(string id)
        {
            return _useCase.Get(id);
        }

        public bool Exists(string id)
        {
            return _useCase.Get(id) != null;
        }

        // =========================
        // VALIDATION
        // =========================

        public bool CanPurchase(string id)
        {
            if (IsProcessing.Value)
                return false;

            var item = _useCase.Get(id);

            if (item == null)
                return false;

            return item.CanPurchase();
        }

        // =========================
        // PURCHASE
        // =========================

        public async UniTask<PurchaseProductResponseData> Purchase(string id)
        {
            if (IsProcessing.Value)
            {
                var error = ResponseData.GetErrorResponse<PurchaseProductResponseData>(
                    Errors.PaymentProcessing,
                    "Store purchase processing"
                );

                OnPurchaseResult.OnNext((id, error));
                return error;
            }

            IsProcessing.Value = true;

            try
            {
                OnPurchaseStart.OnNext(id);

                var result = await _useCase.Purchase(id);

                OnPurchaseResult.OnNext((id, result));

                return result;
            }
            catch (Exception e)
            {
                Debug.LogException(e);

                OnPurchaseError.OnNext(e);

                var error = ResponseData.GetErrorResponse<PurchaseProductResponseData>(
                    Errors.Unknown,
                    "Store purchase exception"
                );

                OnPurchaseResult.OnNext((id, error));

                return error;
            }
            finally
            {
                IsProcessing.Value = false;
            }
        }

        // =========================
        // CLEANUP
        // =========================

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}