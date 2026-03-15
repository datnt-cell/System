namespace GameOfferSystem.Installer
{
    /// <summary>
    /// DTO chứa toàn bộ dependency đã được build
    /// bởi GameOfferInstaller.
    /// </summary>
    public readonly struct GameOfferInstallResult
    {
        public GameOfferService OfferService { get; }

        public GameOfferGroupService GroupService { get; }

        public GameOfferInstallResult(
            GameOfferService offerService,
            GameOfferGroupService groupService)
        {
            OfferService = offerService;
            GroupService = groupService;
        }
    }
}