using System.Collections.Generic;
using GameOfferSystem.Domain;

namespace GameOfferSystem.Infrastructure
{
    /// <summary>
    /// Repository lưu GameOffer runtime data bằng Easy Save 3
    /// </summary>
    public class EasySaveGameOfferRepository : IGameOfferRepository
    {
        private const string KEY = "GAME_OFFER_RUNTIME_DATA";
        private const string FILE = "GameOfferRuntime.es3";

        /// <summary>
        /// Lưu runtime data của các Offer
        /// </summary>
        public void Save(List<GameOfferRuntimeData> offers)
        {
            ES3.Save(KEY, offers, FILE);
        }

        /// <summary>
        /// Load runtime data từ file
        /// </summary>
        public List<GameOfferRuntimeData> Load()
        {
            if (!ES3.KeyExists(KEY, FILE))
                return new List<GameOfferRuntimeData>();

            return ES3.Load<List<GameOfferRuntimeData>>(KEY, FILE);
        }
    }
}