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

        public GameOfferService Service { get; }

        public GameOfferInstallResult(
            GameOfferPresenter presenter,
            GameOfferViewModel viewModel,
            GameOfferService service)
        {
            Presenter = presenter;
            ViewModel = viewModel;
            Service = service;
        }
    }
}