namespace GameEventModule.Domain
{
    /// <summary>
    /// Attachment kích hoạt Game Offer khi Event bắt đầu
    /// </summary>
    public class GameOfferAttachment : IGameEventAttachment
    {
        /// <summary>
        /// Id của Offer Group
        /// </summary>
        public string OfferGroupId { get; }

        /// <summary>
        /// Id của Offer đơn lẻ
        /// </summary>
        public string OfferId { get; }

        public GameOfferAttachment(string offerGroupId, string offerId)
        {
            OfferGroupId = offerGroupId;
            OfferId = offerId;
        }

        /// <summary>
        /// Thực thi attachment
        /// </summary>
        public void Execute()
        {
            // Domain không gọi service trực tiếp
            // Logic thực tế sẽ được xử lý ở Application layer
        }

        public bool HasOfferGroup()
        {
            return !string.IsNullOrEmpty(OfferGroupId);
        }

        public bool HasOffer()
        {
            return !string.IsNullOrEmpty(OfferId);
        }
    }
}