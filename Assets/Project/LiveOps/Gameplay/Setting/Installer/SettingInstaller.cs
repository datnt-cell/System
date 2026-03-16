/// <summary>
/// Installer chịu trách nhiệm khởi tạo dependency
/// </summary>
public class SettingInstaller
{
    public SettingInstallerResult Install()
    {
        ISettingRepository repository = new EasySaveSettingRepository();
        IHapticService haptic = new MobileHapticService();

        SettingState state = new SettingState(
            repository.GetSound(),
            repository.GetVibration(),
            repository.GetMusic()
        );

        SettingEvents events = new SettingEvents();

        SettingService service =
            new SettingService(
                state,
                repository,
                haptic,
                events);

        return new SettingInstallerResult(
            service,
            events);
    }
}

public class SettingInstallerResult
{
    public SettingService Service { get; }
    public SettingEvents Events { get; }

    public SettingInstallerResult(
        SettingService service,
        SettingEvents events)
    {
        Service = service;
        Events = events;
    }
}