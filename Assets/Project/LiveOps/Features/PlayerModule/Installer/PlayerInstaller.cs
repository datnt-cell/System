using PlayerSystem.Application;
using PlayerSystem.Domain;
using PlayerSystem.Infrastructure;

public class PlayerInstaller
{
    public PlayerService PlayerService { get; private set; }

    public void Install()
    {
        IPlayerRepository repository = new EasySavePlayerRepository();

        PlayerService = new PlayerService(repository);
    }
}