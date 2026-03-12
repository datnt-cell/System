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
        private readonly GameOfferState _state;

        public GameOfferViewModel(GameOfferState state)
        {
            _state = state;
        }

        /// <summary>
        /// Danh sách offer đang active.
        /// UI Shop sẽ đọc từ đây.
        /// </summary>
        public IReadOnlyList<GameOfferRuntimeData> ActiveOffers
        {
            get
            {
                return _state.ActiveOffers;
            }
        }

        /// <summary>
        /// Lấy runtime data của một offer.
        /// </summary>
        public GameOfferRuntimeData GetOffer(string offerId)
        {
            return _state.Get(offerId);
        }

        /// <summary>
        /// Kiểm tra offer có active không.
        /// </summary>
        public bool IsActive(string offerId)
        {
            return _state.Contains(offerId);
        }
    }
}