using System;
using System.Collections.Generic;

namespace CurrencySystem.Domain
{
    /// <summary>
    /// Lưu toàn bộ số dư tiền tệ.
    /// Là SOURCE OF TRUTH.
    /// </summary>
    public class CurrencyState
    {
        private readonly Dictionary<CurrencyId, int> _balances = new();

        /// <summary>
        /// Event bắn ra khi có thay đổi currency.
        /// Có đầy đủ old/new/delta/source.
        /// </summary>
        public event Action<CurrencyChangedEvent> OnCurrencyChanged;

        public int GetBalance(CurrencyId id)
        {
            return _balances.TryGetValue(id, out var value)
                ? value
                : 0;
        }

        /// <summary>
        /// Thêm tiền.
        /// </summary>
        public void Add(CurrencyId id, int amount, string source = "unknown")
        {
            int oldValue = GetBalance(id);
            int newValue = oldValue + amount;

            _balances[id] = newValue;

            RaiseChangedEvent(id, oldValue, newValue, amount, source);
        }

        /// <summary>
        /// Trừ tiền.
        /// </summary>
        public bool Spend(CurrencyId id, int amount, string source = "unknown")
        {
            int oldValue = GetBalance(id);

            if (oldValue < amount)
                return false;

            int newValue = oldValue - amount;

            _balances[id] = newValue;

            RaiseChangedEvent(id, oldValue, newValue, -amount, source);

            return true;
        }

        private void RaiseChangedEvent(
            CurrencyId id,
            int oldValue,
            int newValue,
            int delta,
            string source)
        {
            OnCurrencyChanged?.Invoke(
                new CurrencyChangedEvent(
                    id,
                    oldValue,
                    newValue,
                    delta,
                    source));
        }

        public IReadOnlyDictionary<CurrencyId, int> GetAll()
            => _balances;
    }
}