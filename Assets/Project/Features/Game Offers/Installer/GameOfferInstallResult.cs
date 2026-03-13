using GameOfferSystem.Presentation;

namespace GameOfferSystem.Installer
{
    /// <summary>
    /// DTO chứa toàn bộ dependency đã được build
    /// bởi GameOfferInstaller.
    /// </summary>
    public readonly struct GameOfferInstallResult
    {
        public GameOfferPresenter Presenter { get; }

        public GameOfferViewModel ViewModel { get; }

        public GameOfferService OfferService { get; }

        public GameOfferGroupService GroupService { get; }

        public GameOfferInstallResult(
            GameOfferPresenter presenter,
            GameOfferViewModel viewModel,
            GameOfferService offerService,
            GameOfferGroupService groupService)
        {
            Presenter = presenter;
            ViewModel = viewModel;
            OfferService = offerService;
            GroupService = groupService;
        }
    }
}