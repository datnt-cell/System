using System;
using System.Collections.Generic;

namespace CurrencySystem.Domain
{
    /// <summary>
    /// Lưu toàn bộ số dư tiền tệ.
    /// Không phụ thuộc Unity.
    /// Không còn event.
    /// </summary>
    public class CurrencyState
    {
        private readonly Dictionary<CurrencyId, int> _balances = new();
        private readonly ICurrencyMetadataProvider _metadata;

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
        public int Add(CurrencyId id, int amount)
        {
            int oldValue = GetBalance(id);
            int maxStack = _metadata.GetMaxStack(id);
            int newValue = Math.Min(oldValue + amount, maxStack);
            _balances[id] = newValue;
            return newValue - oldValue; // trả về delta
        }

        /// <summary>
        /// Trừ tiền.
        /// </summary>
        public bool Spend(CurrencyId id, int amount, out int delta)
        {
            int oldValue = GetBalance(id);
            if (oldValue < amount)
            {
                delta = 0;
                return false;
            }

            int newValue = oldValue - amount;
            _balances[id] = newValue;
            delta = -amount;
            return true;
        }

        /// <summary>
        /// Set giá trị trực tiếp (DÙNG CHO LOAD SAVE).
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
    }
}