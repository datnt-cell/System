using GameOfferSystem.Domain;

namespace GameOfferSystem.Presentation
{
    /// <summary>
    /// Presenter là lớp trung gian giữa Gameplay/UI và Service.
    /// Gameplay chỉ nên gọi Presenter thay vì gọi Service trực tiếp.
    /// </summary>
    public class GameOfferPresenter
    {
        private readonly GameOfferService _offerService;
        private readonly GameOfferGroupService _groupService;

        public GameOfferPresenter(
            GameOfferService offerService,
            GameOfferGroupService groupService)
        {
            _offerService = offerService;
            _groupService = groupService;
        }

        // =========================
        // OFFER
        // =========================

        /// <summary>
        /// Kích hoạt một offer
        /// </summary>
        public void ActivateOffer(string offerId)
        {
            _offerService.ActivateOffer(offerId);
        }

        /// <summary>
        /// Đánh dấu player đã nhìn thấy offer
        /// </summary>
        public void MarkSeen(string offerId)
        {
            _offerService.MarkSeen(offerId);
        }

        /// <summary>
        /// Kiểm tra player có thể mua offer không
        /// </summary>
        public OfferPurchaseError CanPurchase(string offerId)
        {
            return _offerService.CanPurchase(offerId);
        }

        /// <summary>
        /// Thực hiện mua offer
        /// </summary>
        public PurchaseOfferResponse PurchaseOffer(string offerId)
        {
            return _offerService.Purchase(offerId);
        }

        // =========================
        // GROUP
        // =========================

        /// <summary>
        /// Kích hoạt group
        /// </summary>
        public void ActivateGroup(string groupId)
        {
            _groupService.ActivateGroup(groupId);
        }

        /// <summary>
        /// Player nhìn thấy group
        /// </summary>
        public void MarkGroupSeen(string groupId)
        {
            _groupService.MarkSeen(groupId);
        }

        /// <summary>
        /// Lấy offer hiện tại trong group
        /// </summary>
        public string GetAvailableOfferInGroup(string groupId)
        {
            return _groupService.GetAvailableOffer(groupId);
        }

        /// <summary>
        /// Kiểm tra có thể mua offer trong group
        /// </summary>
        public OfferPurchaseError CanPurchaseGroupOffer(string groupId, string offerId)
        {
            return _groupService.CanPurchase(groupId, offerId);
        }

        /// <summary>
        /// Mua offer trong group
        /// </summary>
        public PurchaseOfferGroupResponse PurchaseGroupOffer(string groupId, string offerId)
        {
            return _groupService.Purchase(groupId, offerId);
        }
    }
}