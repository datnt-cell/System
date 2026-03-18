using UnityEngine;
using Cysharp.Threading.Tasks;
using AdsSystem.Application;
// =========================
// Installer
// =========================
public class AdsInstaller
{
    public async UniTask<AdsInstallerResult> InstallAsync(LoadingView view)
    {
        // STATE
        AdsState state = new AdsState();

        // INFRASTRUCTURE
        IAdsRepository repository = new EasySaveAdsRepository();
        IAdsPolicy policy = new DefaultAdsPolicy(5, 30f, 15, 30, 0);
        IAdsProvider provider = new GleyAdsProvider();
        UnityTimeProvider timeProvider = new UnityTimeProvider();

        await provider.InitializeAsync();

        // EVENTS
        AdsEvents events = new AdsEvents();

        // SERVICE
        AdsService service = new AdsService(
            state,
            repository,
            policy,
            provider,
            timeProvider,
            events
        );

        // BIND VIEW
        service.BindView(view);

        return new AdsInstallerResult(service, events);
    }
}

// =========================
// Installer result
// =========================
public class AdsInstallerResult
{
    public AdsService Service { get; }
    public IAdsEvents Events { get; }

    public AdsInstallerResult(AdsService service, IAdsEvents events)
    {
        Service = service;
        Events = events;
    }
}