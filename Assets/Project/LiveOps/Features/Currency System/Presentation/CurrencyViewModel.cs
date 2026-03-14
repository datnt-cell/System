using System.Collections.Generic;
using CurrencySystem.Domain;
using R3;

namespace CurrencySystem.Presentation
{
    /// <summary>
    /// ViewModel chuyển Domain event thành Reactive stream.
    /// Hỗ trợ unlimited currency.
    /// </summary>
    public class CurrencyViewModel
    {
        private readonly CurrencyState _state;

        /// <summary>
        /// Lưu ReactiveProperty cho từng currency.
        /// </summary>
        private readonly Dictionary<string, ReactiveProperty<int>> _balances
            = new();

        /// <summary>
        /// Stream bắn ra khi có thay đổi currency.
        /// UI có thể subscribe để animate delta.
        /// </summary>
        private readonly Subject<CurrencyChangedEvent> _currencyChanged
            = new();

        public Observable<CurrencyChangedEvent> OnCurrencyChanged
            => _currencyChanged;

        public CurrencyViewModel(CurrencyState state)
        {
            _state = state;

            // Subscribe event từ Domain
            _state.OnCurrencyChanged += HandleCurrencyChanged;
        }

        /// <summary>
        /// Lấy reactive property theo currency id.
        /// Nếu chưa tồn tại sẽ tự tạo.
        /// </summary>
        public ReadOnlyReactiveProperty<int> GetBalance(string id)
        {
            if (!_balances.ContainsKey(id))
            {
                var currencyId = new CurrencyId(id);

                var rp = new ReactiveProperty<int>(
                    _state.GetBalance(currencyId));

                _balances[id] = rp;
            }

            return _balances[id].ToReadOnlyReactiveProperty();
        }

        /// <summary>
        /// Khi Domain thay đổi, cập nhật reactive và bắn event ra ngoài.
        /// </summary>
        private void HandleCurrencyChanged(CurrencyChangedEvent e)
        {
            string id = e.Id.Value;

            if (_balances.TryGetValue(id, out var rp))
            {
                rp.Value = e.NewValue;
            }

            _currencyChanged.OnNext(e);
        }
    }
}