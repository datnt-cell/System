using Cysharp.Threading.Tasks;

/// <summary>
/// Installer chịu trách nhiệm khởi tạo và liên kết các dependency.
/// </summary>
public class AdsInstaller
{
    public async UniTask<AdsService> Install(LoadingView view)
    {
        AdsState state = new AdsState();

        IAdsRepository repository = new EasySaveAdsRepository();
        IAdsPolicy policy = new DefaultAdsPolicy(5, 30f, 15, 30, 0);
        IAdsProvider provider = new GleyAdsProvider();
        UnityTimeProvider timeProvider = new UnityTimeProvider();

        await provider.InitializeAsync();

        AdsService service =
            new AdsService(state, repository, policy, provider, timeProvider);

        service.BindView(view);

        return service;
    }
}