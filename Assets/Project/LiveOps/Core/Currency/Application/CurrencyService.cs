using CurrencySystem.Domain;

namespace CurrencySystem.Application
{
    public class CurrencyService
    {
        private readonly CurrencyState _state;
        private readonly ICurrencyRepository _repository;
        private readonly CurrencyEvents _events;

        public CurrencyService(
            CurrencyState state,
            ICurrencyRepository repository,
            CurrencyEvents events)
        {
            _state = state;
            _repository = repository;
            _events = events;

            _repository.Load(_state);
        }

        /// <summary>
        /// Thêm currency và trả về response chi tiết.
        /// </summary>
        public CurrencyResponse AddCurrency(CurrencyId id, int amount, string source = "")
        {
            if (amount <= 0)
            {
                var failEvent = CurrencyEvent.Fail(id, amount, source, Errors.Unknown, "Amount must be greater than 0");
                _events.Publish(failEvent);

                return CurrencyResponse.CreateError(Errors.Unknown, "Amount must be greater than 0");
            }

            int delta = _state.Add(id, amount);
            int balance = _state.GetBalance(id);

            // Publish event
            var evt = CurrencyEvent.Add(id, amount, balance, delta, source);
            _events.Publish(evt);

            // Save state
            _repository.Save(_state);

            return CurrencyResponse.CreateSuccess(balance, delta, source);
        }

        /// <summary>
        /// Trừ currency và trả về response chi tiết.
        /// </summary>
        public CurrencyResponse SpendCurrency(CurrencyId id, int amount, string source = "")
        {
            if (amount <= 0)
            {
                var failEvent = CurrencyEvent.Fail(id, amount, source, Errors.Unknown, "Amount must be greater than 0");
                _events.Publish(failEvent);

                return CurrencyResponse.CreateError(Errors.Unknown, "Amount must be greater than 0");
            }

            bool result = _state.Spend(id, amount, out int delta);
            int balance = _state.GetBalance(id);

            if (!result)
            {
                var failEvent = CurrencyEvent.Fail(id, amount, source, Errors.NotEnoughResources, $"Not enough {id} to spend {amount}");
                _events.Publish(failEvent);

                return CurrencyResponse.CreateError(Errors.NotEnoughResources, $"Not enough {id} to spend {amount}");
            }

            // Publish event
            var evt = CurrencyEvent.Spend(id, amount, balance, delta, source);
            _events.Publish(evt);

            // Save state
            _repository.Save(_state);

            return CurrencyResponse.CreateSuccess(balance, delta, source);
        }

        public int GetBalance(CurrencyId id) => _state.GetBalance(id);

        public bool HasEnough(CurrencyId id, int amount) => _state.GetBalance(id) >= amount;
    }
}