using PlayerSystem.Application;
using PlayerSystem.Domain;
using PlayerSystem.Infrastructure;

public class PlayerInstaller
{
    public PlayerInstallerResult Install()
    {
        IPlayerRepository repository =
            new EasySavePlayerRepository();

        PlayerEvents events = new PlayerEvents();

        PlayerService service =
            new PlayerService(repository, events);

        service.Load();

        return new PlayerInstallerResult(
            service,
            events);
    }
}

public class PlayerInstallerResult
{
    public PlayerService Service { get; }
    public PlayerEvents Events { get; }

    public PlayerInstallerResult(
        PlayerService service,
        PlayerEvents events)
    {
        Service = service;
        Events = events;
    }
}