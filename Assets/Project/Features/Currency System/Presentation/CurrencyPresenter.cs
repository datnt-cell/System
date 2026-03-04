using CurrencySystem.Application;
using CurrencySystem.Domain;

namespace CurrencySystem.Presentation
{
    /// <summary>
    /// Presenter kết nối UI và Service.
    /// Không chứa logic business.
    /// </summary>
    public class CurrencyPresenter
    {
        private readonly CurrencyService _service;
        private readonly CurrencyBundleUseCase _bundleUseCase;

        public CurrencyPresenter(
            CurrencyService service,
            CurrencyBundleUseCase bundleUseCase)
        {
            _service = service;
            _bundleUseCase = bundleUseCase;
        }


        /// <summary>
        /// Thêm tiền với source rõ ràng.
        /// </summary>
        public void Add(string currencyId, int amount, string source)
        {
            _service.Add(new CurrencyId(currencyId), amount, source);
        }

        /// <summary>
        /// Trừ tiền.
        /// </summary>
        public bool Spend(string currencyId, int amount, string source)
        {
            return _service.Spend(
                new CurrencyId(currencyId),
                amount,
                source);
        }

        /// <summary>
        /// Lấy số dư hiện tại (dùng cho debug hoặc logic ngoài UI).
        /// </summary>
        public int GetBalance(string currencyId)
        {
            return _service.GetBalance(new CurrencyId(currencyId));
        }

        public void OpenBundle(string bundleId, string source)
        {
            _bundleUseCase.OpenBundle(bundleId, source);
        }
    }
}