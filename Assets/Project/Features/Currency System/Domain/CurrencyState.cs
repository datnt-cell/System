using System;
using System.Collections.Generic;

namespace CurrencySystem.Domain
{
    /// <summary>
    /// Lưu toàn bộ số dư tiền tệ.
    /// Đây là SOURCE OF TRUTH.
    /// 
    /// Không phụ thuộc Unity.
    /// Không phụ thuộc Balancy.
    /// Chỉ phụ thuộc abstraction ICurrencyMetadataProvider.
    /// </summary>
    public class CurrencyState
    {
        private readonly Dictionary<CurrencyId, int> _balances = new();

        /// <summary>
        /// Provider cung cấp metadata (MaxStack, Name...)
        /// Được inject từ bên ngoài (Infrastructure).
        /// </summary>
        private readonly ICurrencyMetadataProvider _metadata;

        /// <summary>
        /// Event bắn ra khi có thay đổi currency.
        /// Dùng cho:
        /// - UI update
        /// - Analytics
        /// - Save system
        /// - Anti-cheat
        /// </summary>
        public event Action<CurrencyChangedEvent> OnCurrencyChanged;

        public CurrencyState(ICurrencyMetadataProvider metadata)
        {
            _metadata = metadata;
        }

        /// <summary>
        /// Lấy số dư hiện tại.
        /// Nếu chưa tồn tại thì mặc định = 0.
        /// </summary>
        public int GetBalance(CurrencyId id)
        {
            return _balances.TryGetValue(id, out var value)
                ? value
                : 0;
        }

        /// <summary>
        /// Thêm tiền.
        /// Có clamp theo MaxStack từ metadata provider.
        /// </summary>
        public void Add(CurrencyId id, int amount, string source = "unknown")
        {
            int oldValue = GetBalance(id);

            // Lấy giới hạn tối đa từ config (Balancy)
            int maxStack = _metadata.GetMaxStack(id);

            // Clamp để tránh overflow hoặc vượt giới hạn
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
        /// Nếu không đủ tiền thì trả về false.
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
        /// Phát event thay đổi.
        /// </summary>
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

        /// <summary>
        /// Trả về toàn bộ số dư (read-only).
        /// Dùng cho save hoặc debug.
        /// </summary>
        public IReadOnlyDictionary<CurrencyId, int> GetAll()
            => _balances;
    }
}