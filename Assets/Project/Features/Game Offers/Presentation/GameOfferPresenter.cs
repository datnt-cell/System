namespace GameOfferSystem.Presentation
{
    /// <summary>
    /// Presenter là lớp trung gian giữa Gameplay/UI và Service.
    /// Gameplay chỉ nên gọi Presenter thay vì gọi Service trực tiếp.
    /// </summary>
    public class GameOfferPresenter
    {
        private readonly GameOfferService _service;

        public GameOfferPresenter(GameOfferService service)
        {
            _service = service;
        }

        /// <summary>
        /// Kích hoạt một offer.
        /// </summary>
        public void ActivateOffer(string offerId)
        {
            _service.ActivateOffer(offerId);
        }

        /// <summary>
        /// Đánh dấu player đã nhìn thấy offer.
        /// </summary>
        public void MarkSeen(string offerId)
        {
            _service.MarkSeen(offerId);
        }

        /// <summary>
        /// Kiểm tra player có thể mua offer không.
        /// </summary>
        public bool CanPurchase(string offerId)
        {
            return _service.CanPurchase(offerId);
        }

        /// <summary>
        /// Thực hiện mua offer.
        /// </summary>
        public bool Purchase(string offerId)
        {
            return _service.Purchase(offerId);
        }
    }
}