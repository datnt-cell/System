/// <summary>
/// Installer chịu trách nhiệm khởi tạo dependency
/// </summary>
public class SettingInstaller
{
    public SettingPresenter Install()
    {
        // Infrastructure
        ISettingRepository repository = new EasySaveSettingRepository();
        IHapticService haptic = new MobileHapticService();

        // Load dữ liệu ban đầu
        SettingState state = new SettingState(
            repository.GetSound(),
            repository.GetVibration(),
            repository.GetMusic()
        );

        // Application
        SettingService service = new SettingService(state, repository, haptic);

        // Presentation
        SettingViewModel viewModel = new SettingViewModel(state);
        SettingPresenter presenter = new SettingPresenter(viewModel, service);

        return presenter;
    }
}