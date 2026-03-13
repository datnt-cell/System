namespace GameOfferSystem.Domain
{
    /// <summary>
    /// Các lỗi có thể xảy ra khi player mua Offer
    /// </summary>
    public enum OfferPurchaseError
    {
        /// <summary>
        /// Không có lỗi
        /// </summary>
        None = 0,

        /// <summary>
        /// Không tìm thấy offer
        /// </summary>
        OfferNotFound,

        /// <summary>
        /// Offer chưa được kích hoạt
        /// </summary>
        OfferNotActive,

        /// <summary>
        /// Player không được phép mua
        /// (ví dụ: đã mua đủ số lần)
        /// </summary>
        PurchaseNotAllowed,

        /// <summary>
        /// Offer đã hết hạn
        /// </summary>
        OfferExpired,

        /// <summary>
        /// Đã đạt giới hạn mua
        /// </summary>
        PurchaseLimitReached,

        /// <summary>
        /// Không đủ currency
        /// </summary>
        NotEnoughCurrency,

        /// <summary>
        /// Lỗi hệ thống
        /// </summary>
        UnknownError
    }
}