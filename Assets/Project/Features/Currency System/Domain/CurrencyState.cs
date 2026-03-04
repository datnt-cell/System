using System.Collections.Generic;

namespace CurrencySystem.Domain
{
    /// <summary>
    /// Lưu toàn bộ số dư tiền tệ của player.
    /// Đây là SOURCE OF TRUTH.
    /// Không phụ thuộc Unity.
    /// </summary>
    public class CurrencyState
    {
        private readonly Dictionary<CurrencyId, int> _balances = new();

        /// <summary>
        /// Lấy số dư hiện tại.
        /// Nếu chưa tồn tại thì trả về 0.
        /// </summary>
        public int GetBalance(CurrencyId id)
        {
            return _balances.TryGetValue(id, out var value) ? value : 0;
        }

        /// <summary>
        /// Thêm tiền.
        /// </summary>
        public void Add(CurrencyId id, int amount)
        {
            if (!_balances.ContainsKey(id))
                _balances[id] = 0;

            _balances[id] += amount;
        }

        /// <summary>
        /// Trừ tiền.
        /// Trả về false nếu không đủ tiền.
        /// </summary>
        public bool Spend(CurrencyId id, int amount)
        {
            if (GetBalance(id) < amount)
                return false;

            _balances[id] -= amount;
            return true;
        }

        /// <summary>
        /// Trả về toàn bộ dữ liệu (dùng cho save/load).
        /// </summary>
        public IReadOnlyDictionary<CurrencyId, int> GetAll()
            => _balances;
    }
}