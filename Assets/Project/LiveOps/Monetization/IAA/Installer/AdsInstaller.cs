using Cysharp.Threading.Tasks;

public class AdsInstaller
{
    public async UniTask<AdsInstallerResult> Install(LoadingView view)
    {
        AdsState state = new AdsState();

        IAdsRepository repository = new EasySaveAdsRepository();

        IAdsPolicy policy =
            new DefaultAdsPolicy(5, 30f, 15, 30, 0);

        IAdsProvider provider = new GleyAdsProvider();

        UnityTimeProvider timeProvider =
            new UnityTimeProvider();

        await provider.InitializeAsync();

        AdsEvents events = new AdsEvents();

        AdsService service =
            new AdsService(
                state,
                repository,
                policy,
                provider,
                timeProvider,
                events);

        service.BindView(view);

        return new AdsInstallerResult(service, events);
    }
}

public class AdsInstallerResult
{
    public AdsService Service { get; }
    public AdsEvents Events { get; }

    public AdsInstallerResult(
        AdsService service,
        AdsEvents events)
    {
        Service = service;
        Events = events;
    }
}