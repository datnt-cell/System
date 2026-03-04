using CurrencySystem.Domain;
using R3;

namespace CurrencySystem.Presentation
{
    /// <summary>
    /// ViewModel dùng để bind UI.
    /// </summary>
    public class CurrencyViewModel
    {
        private readonly CurrencyState _state;

        public ReadOnlyReactiveProperty<int> Coin { get; }
        public ReadOnlyReactiveProperty<int> Gem { get; }

        public CurrencyViewModel(CurrencyState state)
        {
            _state = state;

            Coin = Observable
                .EveryValueChanged(_state,
                    _ => _state.GetBalance(new CurrencyId("coin")))
                .ToReadOnlyReactiveProperty();

            Gem = Observable
                .EveryValueChanged(_state,
                    _ => _state.GetBalance(new CurrencyId("gem")))
                .ToReadOnlyReactiveProperty();
        }
    }
}