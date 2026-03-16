using CurrencySystem.Domain;

namespace CurrencySystem.Application
{
    public class CurrencyService
    {
        private readonly CurrencyState _state;
        private readonly ICurrencyRepository _repository;
        private readonly CurrencyEvents _events;

        public ICurrencyEvents Events => _events;

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

        public void Add(CurrencyId id, int amount, string source = "")
        {
            if (amount <= 0)
                return;

            _state.Add(id, amount, source);

            _events.Publish(
                CurrencyEvent.Add(id, amount, source)
            );

            _repository.Save(_state);
        }

        public bool Spend(CurrencyId id, int amount, string source = "")
        {
            if (amount <= 0)
                return false;

            bool result = _state.Spend(id, amount, source);

            if (!result)
                return false;

            _events.Publish(
                CurrencyEvent.Spend(id, amount, source)
            );

            _repository.Save(_state);

            return true;
        }

        public int GetBalance(CurrencyId id)
            => _state.GetBalance(id);

        public bool HasEnough(CurrencyId id, int amount)
            => _state.GetBalance(id) >= amount;
    }
}