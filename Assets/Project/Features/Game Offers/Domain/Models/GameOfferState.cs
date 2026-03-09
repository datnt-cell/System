using System.Collections.Generic;

namespace GameOfferSystem.Domain
{
    /// <summary>
    /// Lưu toàn bộ runtime state của GameOffer.
    /// </summary>
    public class GameOfferState
    {
        private readonly Dictionary<string, GameOfferRuntimeData> _offers
            = new Dictionary<string, GameOfferRuntimeData>();

        public IReadOnlyList<GameOfferRuntimeData> ActiveOffers
        {
            get
            {
                return new List<GameOfferRuntimeData>(_offers.Values);
            }
        }

        public void Add(GameOfferRuntimeData data)
        {
            _offers[data.OfferId] = data;
        }

        public bool Contains(string offerId)
        {
            return _offers.ContainsKey(offerId);
        }

        public GameOfferRuntimeData Get(string offerId)
        {
            _offers.TryGetValue(offerId, out var data);
            return data;
        }
    }
}