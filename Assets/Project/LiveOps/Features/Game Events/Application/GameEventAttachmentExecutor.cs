using GameEventModule.Domain;

namespace GameEventModule.Application
{
    public class GameEventAttachmentExecutor : IGameEventAttachmentExecutor
    {
        private readonly IGameOfferService offerService;

        public GameEventAttachmentExecutor(IGameOfferService offerService)
        {
            this.offerService = offerService;
        }

        public void Execute(IGameEventAttachment attachment)
        {
            if (attachment is GameOfferAttachment offer)
            {
                if (!string.IsNullOrEmpty(offer.OfferGroupId))
                {
                    offerService.ActivateGroup(offer.OfferGroupId);
                    return;
                }

                if (!string.IsNullOrEmpty(offer.OfferId))
                {
                    offerService.ActivateOffer(offer.OfferId);
                }
            }
        }
    }
}