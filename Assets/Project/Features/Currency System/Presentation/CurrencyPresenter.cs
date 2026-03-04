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

        public CurrencyPresenter(CurrencyService service)
        {
            _service = service;
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
    }
}