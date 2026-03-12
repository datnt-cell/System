using System.Collections.Generic;
using GameOfferSystem.Domain;

namespace GameOfferSystem.Infrastructure
{
    /// <summary>
    /// Repository lưu GameOffer runtime data bằng Easy Save 3
    /// </summary>
    public class EasySaveGameOfferRepository : IGameOfferRepository
    {
        private const string SaveKey = "GAME_OFFER_RUNTIME_DATA";

        /// <summary>
        /// Lưu runtime data
        /// </summary>
        public void Save(List<GameOfferRuntimeData> offers)
        {
            ES3.Save(SaveKey, offers);
        }

        /// <summary>
        /// Load runtime data
        /// </summary>
        public List<GameOfferRuntimeData> Load()
        {
            if (!ES3.KeyExists(SaveKey))
                return new List<GameOfferRuntimeData>();

            return ES3.Load<List<GameOfferRuntimeData>>(SaveKey);
        }
    }
}