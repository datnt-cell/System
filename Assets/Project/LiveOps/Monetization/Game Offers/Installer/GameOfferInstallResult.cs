using GameOfferSystem.Infrastructure;

namespace GameOfferSystem.Installer
{
    public class GameOfferInstallResult
    {
        public GameOfferService OfferService { get; }
        public GameOfferGroupService GroupService { get; }

        public GameOfferEvents OfferEvents { get; }
        public GameOfferGroupEvents GroupEvents { get; }

        public GameOfferInstallResult(
            GameOfferService offerService,
            GameOfferGroupService groupService,
            GameOfferEvents offerEvents,
            GameOfferGroupEvents groupEvents)
        {
            OfferService = offerService;
            GroupService = groupService;

            OfferEvents = offerEvents;
            GroupEvents = groupEvents;
        }
    }
}