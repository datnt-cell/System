using GameOfferSystem.Domain;
using GameOfferSystem.Infrastructure;
using GameOfferSystem.Presentation;

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
            var config = GameOfferGlobalConfig.Instance;

            // =========================
            // DOMAIN
            // =========================

            // Runtime state của toàn bộ GameOffer
            var state = new GameOfferState();

            // =========================
            // INFRASTRUCTURE
            // =========================

            // Repository dùng để Save/Load dữ liệu runtime
            IGameOfferRepository repository =
                new EasySaveGameOfferRepository();

            // =========================
            // APPLICATION
            // =========================

            // Service chứa toàn bộ business logic
            var events = new GameOfferEvents(); 

            var service = new GameOfferService(
                state,
                repository,
                config,
                events
            );

            // =========================
            // PRESENTATION
            // =========================

            // ViewModel cho UI binding
            var viewModel = new GameOfferViewModel(state);

            // Presenter cho gameplay gọi
            var presenter = new GameOfferPresenter(service);

            return new GameOfferInstallResult(
                presenter,
                viewModel,
                service
            );
        }
    }
}