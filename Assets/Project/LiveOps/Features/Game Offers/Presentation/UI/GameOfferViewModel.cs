using System.Collections.Generic;
using GameOfferSystem.Domain;

namespace GameOfferSystem.Presentation
{
    /// <summary>
    /// ViewModel cung cấp dữ liệu cho UI.
    /// Không chứa logic gameplay.
    /// </summary>
    public class GameOfferViewModel
    {
        private readonly GameOfferState _offerState;
        private readonly GameOfferGroupState _groupState;

        public GameOfferViewModel(
            GameOfferState offerState,
            GameOfferGroupState groupState)
        {
            _offerState = offerState;
            _groupState = groupState;
        }

        // =========================
        // OFFER
        // =========================

        /// <summary>
        /// Danh sách offer đang active.
        /// UI Shop / Popup đọc từ đây.
        /// </summary>
        public IReadOnlyList<GameOfferRuntimeData> ActiveOffers
        {
            get
            {
                return _offerState.ActiveOffers;
            }
        }

        /// <summary>
        /// Lấy runtime data của một offer.
        /// </summary>
        public GameOfferRuntimeData GetOffer(string offerId)
        {
            return _offerState.Get(offerId);
        }

        /// <summary>
        /// Kiểm tra offer có active không.
        /// </summary>
        public bool IsOfferActive(string offerId)
        {
            return _offerState.Contains(offerId);
        }

        // =========================
        // GROUP
        // =========================

        /// <summary>
        /// Danh sách group đang active.
        /// </summary>
        public IReadOnlyCollection<GameOfferGroupRuntimeData> ActiveGroups
        {
            get
            {
                return _groupState.Values;
            }
        }

        /// <summary>
        /// Lấy runtime data của một group.
        /// </summary>
        public GameOfferGroupRuntimeData GetGroup(string groupId)
        {
            return _groupState.Get(groupId);
        }

        /// <summary>
        /// Kiểm tra group có active không.
        /// </summary>
        public bool IsGroupActive(string groupId)
        {
            return _groupState.ContainsKey(groupId);
        }
    }
}