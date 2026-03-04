using CurrencySystem.Application;
using CurrencySystem.Domain;

namespace CurrencySystem.Presentation
{
    /// <summary>
    /// Presenter kết nối UI và Service.
    /// </summary>
    public class CurrencyPresenter
    {
        private readonly CurrencyService _service;

        public CurrencyPresenter(CurrencyService service)
        {
            _service = service;
        }

        /// <summary>
        /// Ví dụ thêm coin.
        /// </summary>
        public void AddCoin(int amount)
        {
            _service.Add(new CurrencyId("coin"), amount);
        }

        /// <summary>
        /// Ví dụ thêm gem.
        /// </summary>
        public void AddGem(int amount)
        {
            _service.Add(new CurrencyId("gem"), amount);
        }
    }
}