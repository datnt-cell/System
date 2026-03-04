using System;
using System.Collections.Generic;

namespace CurrencySystem.Domain
{
    /// <summary>
    /// Lưu toàn bộ số dư tiền tệ.
    /// Không phụ thuộc Unity.
    /// </summary>
    public class CurrencyState
    {
        private readonly Dictionary<CurrencyId, int> _balances = new();
        private readonly ICurrencyMetadataProvider _metadata;

        /// <summary>
        /// Event khi currency thay đổi.
        /// </summary>
        public event Action<CurrencyChangedEvent> OnCurrencyChanged;

        public CurrencyState(ICurrencyMetadataProvider metadata)
        {
            _metadata = metadata;
        }

        /// <summary>
        /// Lấy số dư hiện tại.
        /// </summary>
        public int GetBalance(CurrencyId id)
        {
            return _balances.TryGetValue(id, out var value)
                ? value
                : 0;
        }

        /// <summary>
        /// Thêm tiền (có clamp MaxStack).
        /// </summary>
        public void Add(CurrencyId id, int amount, string source = "unknown")
        {
            int oldValue = GetBalance(id);

            int maxStack = _metadata.GetMaxStack(id);

            int newValue = Math.Min(oldValue + amount, maxStack);

            _balances[id] = newValue;

            RaiseChangedEvent(
                id,
                oldValue,
                newValue,
                newValue - oldValue,
                source);
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

            RaiseChangedEvent(
                id,
                oldValue,
                newValue,
                -amount,
                source);

            return true;
        }

        /// <summary>
        /// Set giá trị trực tiếp (DÙNG CHO LOAD SAVE).
        /// Không bắn event.
        /// </summary>
        public void SetRaw(CurrencyId id, int value)
        {
            _balances[id] = value;
        }

        /// <summary>
        /// Trả toàn bộ data để save.
        /// </summary>
        public IReadOnlyDictionary<CurrencyId, int> GetAll()
            => _balances;

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
    }
}