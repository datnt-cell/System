namespace CurrencySystem.Domain
{
    /// <summary>
    /// Loại item/currency.
    /// </summary>
    public enum ItemType
    {
        Currency,   // tiền tệ chính
        Event,      // sự kiện đặc biệt
        Item        // vật phẩm/booster/weapon...
    }

    /// <summary>
    /// Value Object đại diện cho một loại tiền tệ.
    /// Dùng string để có thể mở rộng qua LiveOps / RemoteConfig.
    /// Ví dụ: "coin", "gem", "event_token"
    /// </summary>
    public readonly struct CurrencyId
    {
        public string Value { get; }

        public CurrencyId(string value)
        {
            Value = value;
        }

        public override string ToString() => Value;
    }
}

