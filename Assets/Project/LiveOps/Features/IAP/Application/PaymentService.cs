using IAPModule.Domain;
using IAPModule.Application.Interfaces;
using Gley.EasyIAP;

namespace IAPModule.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PaymentState _state;
        private readonly IIAPRepository _repository;

        public PaymentService(
            PaymentState state,
            IIAPRepository repository)
        {
            _state = state;
            _repository = repository;

            // Load data khi start
            _repository.LoadStats(_state);
        }

        public void RegisterPurchase(float price, string currency)
        {
            _state.AddPurchase(price, currency);

            // Save ngay sau khi update
            _repository.SaveStats(_state);
        }

        public float GetTotalSpend() => _state.TotalSpend.Value;

        public int GetPurchaseCount() => _state.PaymentsCount.Value;

        public bool HasPurchased(string productId)
        {
            var id = API.ConvertNameToShopProduct(productId);

            return _repository.HasRewarded(id);
        }

        public bool HasAnyPurchase()
        {
            return _state.PaymentsCount.Value > 0;
        }
    }
}