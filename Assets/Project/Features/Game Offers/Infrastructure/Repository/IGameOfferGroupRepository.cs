using System.Collections.Generic;
using GameOfferSystem.Domain;

namespace GameOfferSystem.Infrastructure
{
    /// <summary>
    /// Interface định nghĩa cách lưu / load GameOfferGroup runtime data.
    /// Infrastructure layer sẽ implement interface này.
    /// </summary>
    public interface IGameOfferGroupRepository
    {
        /// <summary>
        /// Lưu toàn bộ runtime data của offer groups.
        /// </summary>
        void Save(List<GameOfferGroupRuntimeData> groups);

        /// <summary>
        /// Load runtime data của offer groups từ storage.
        /// </summary>
        List<GameOfferGroupRuntimeData> Load();
    }
}