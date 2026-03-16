using CurrencySystem.Domain;

namespace CurrencySystem.Application
{
    /// <summary>
    /// Service xử lý logic tiền tệ.
    /// Không phụ thuộc Unity.
    /// </summary>
    public class CurrencyService
    {
        private readonly CurrencyState _state;
        private readonly ICurrencyRepository _repository;

        public CurrencyService(
            CurrencyState state,
            ICurrencyRepository repository)
        {
            _state = state;
            _repository = repository;

            // Load state khi khởi tạo
            _repository.Load(_state);
        }

        public void Add(CurrencyId id, int amount, string source = "")
        {
            if (amount <= 0)
                return;

            _state.Add(id, amount, source);
            _repository.Save(_state);
        }

        public bool Spend(CurrencyId id, int amount, string source = "")
        {
            if (amount <= 0)
                return false;

            bool result = _state.Spend(id, amount, source);

            if (result)
                _repository.Save(_state);

            return result;
        }

        public int GetBalance(CurrencyId id)
            => _state.GetBalance(id);

        public bool HasEnough(CurrencyId id, int amount)
            => _state.GetBalance(id) >= amount;
    }
}