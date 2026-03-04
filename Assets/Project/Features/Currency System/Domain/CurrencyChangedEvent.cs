namespace CurrencySystem.Domain
{
    /// <summary>
    /// Thông tin chi tiết về một lần thay đổi currency.
    /// Dùng cho:
    /// - UI update
    /// - Analytics
    /// - Transaction log
    /// - Anti-cheat
    /// </summary>
    public readonly struct CurrencyChangedEvent
    {
        /// <summary>
        /// Loại currency bị thay đổi
        /// </summary>
        public CurrencyId Id { get; }

        /// <summary>
        /// Giá trị trước khi thay đổi
        /// </summary>
        public int OldValue { get; }

        /// <summary>
        /// Giá trị sau khi thay đổi
        /// </summary>
        public int NewValue { get; }

        /// <summary>
        /// Lượng thay đổi (có thể âm hoặc dương)
        /// </summary>
        public int Delta { get; }

        /// <summary>
        /// Nguồn thay đổi (vd: "quest_reward", "iap", "upgrade_cost")
        /// </summary>
        public string Source { get; }

        public CurrencyChangedEvent(
            CurrencyId id,
            int oldValue,
            int newValue,
            int delta,
            string source)
        {
            Id = id;
            OldValue = oldValue;
            NewValue = newValue;
            Delta = delta;
            Source = source;
        }
    }
}