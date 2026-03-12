using System.Collections.Generic;
using GameOfferSystem.Domain;

namespace GameOfferSystem.Infrastructure
{
    /// <summary>
    /// Interface định nghĩa cách lưu / load GameOffer runtime data.
    /// Infrastructure layer sẽ implement interface này.
    /// </summary>
    public interface IGameOfferRepository
    {
        /// <summary>
        /// Lưu toàn bộ runtime data của offers.
        /// </summary>
        void Save(List<GameOfferRuntimeData> offers);

        /// <summary>
        /// Load runtime data của offers từ storage.
        /// </summary>
        List<GameOfferRuntimeData> Load();
    }
}