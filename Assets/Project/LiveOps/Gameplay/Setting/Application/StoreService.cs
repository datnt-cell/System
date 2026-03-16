using System;
using System.Collections.Generic;
using R3;
using Cysharp.Threading.Tasks;
using UnityEngine;
using StoreSystem.Application;
using StoreSystem.Domain;
using StoreSystem.Infrastructure;

namespace StoreSystem.Presentation
{
    /// <summary>
    /// Service trung gian giữa UI và StoreItemUseCase.
    /// Không chứa business logic.
    /// </summary>
    public class StoreService : IDisposable
    {
        private readonly StoreItemUseCase _useCase;
        private readonly StoreEvents _events;
        private readonly CompositeDisposable _disposables = new();

        public ReactiveProperty<bool> IsProcessing = new(false);

        public StoreService(
            StoreItemUseCase useCase,
            StoreEvents events)
        {
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

                _events.Publish(StoreEvent.PurchaseResult(id, error));
                return error;
            }

            IsProcessing.Value = true;

            try
            {
                _events.Publish(StoreEvent.PurchaseStart(id));

                var result = await _useCase.Purchase(id);

                _events.Publish(StoreEvent.PurchaseResult(id, result));

                return result;
            }
            catch (Exception e)
            {
                Debug.LogException(e);

                _events.Publish(StoreEvent.PurchaseError(id, e));

                var error = ResponseData.GetErrorResponse<PurchaseProductResponseData>(
                    Errors.Unknown,
                    "Store purchase exception"
                );

                _events.Publish(StoreEvent.PurchaseResult(id, error));

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