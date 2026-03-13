using GameOfferSystem.Domain;

namespace GameOfferSystem.Infrastructure
{
    public interface IGameOfferGroupEvents
    {
        void OnGroupActivated(GameOfferGroupRuntimeData data);

        void OnGroupOfferPurchased(string groupId, string offerId);

        void OnGroupPurchaseFailed(string groupId, string offerId, string reason);

        void OnGroupCompleted(string groupId);
    }
}