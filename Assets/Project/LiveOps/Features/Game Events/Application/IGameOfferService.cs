namespace GameEventModule.Application
{
    public interface IGameOfferService
    {
        void ActivateOffer(string offerId);

        void ActivateGroup(string groupId);
    }
}