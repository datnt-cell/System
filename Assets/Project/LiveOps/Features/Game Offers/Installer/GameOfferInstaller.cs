using GameOfferSystem.Domain;
using GameOfferSystem.Infrastructure;

namespace GameOfferSystem.Installer
{
    /// <summary>
    /// Installer chịu trách nhiệm tạo và kết nối toàn bộ dependency
    /// cho Game Offer System.
    /// 
    /// Đây là Composition Root của hệ thống.
    /// </summary>
    public class GameOfferInstaller
    {
        public GameOfferInstallResult Install()
        {
            // =========================
            // CONFIG
            // =========================

            var offerConfig = GameOfferGlobalConfig.Instance;
            var groupConfig = GameOfferGroupGlobalConfig.Instance;

            // =========================
            // DOMAIN
            // =========================

            var offerState = new GameOfferState();
            var groupState = new GameOfferGroupState();

            // =========================
            // INFRASTRUCTURE
            // =========================

            IGameOfferRepository offerRepository =
                new EasySaveGameOfferRepository();

            IGameOfferGroupRepository groupRepository =
                new EasySaveGameOfferGroupRepository();

            // =========================
            // EVENTS
            // =========================

            var offerEvents = new GameOfferEvents();
            var groupEvents = new GameOfferGroupEvents();

            // =========================
            // APPLICATION
            // =========================

            var offerService = new GameOfferService(
                offerState,
                offerRepository,
                offerConfig,
                offerEvents
            );

            var groupService = new GameOfferGroupService(
                groupState,
                groupRepository,
                groupConfig,
                groupEvents
            );

            return new GameOfferInstallResult(
                offerService,
                groupService
            );
        }
    }
}