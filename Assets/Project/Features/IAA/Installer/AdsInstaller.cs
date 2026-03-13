using Cysharp.Threading.Tasks;

/// <summary>
/// Installer chịu trách nhiệm khởi tạo và liên kết các dependency.
/// </summary>
public class AdsInstaller
{
    public async UniTask<AdsPresenter> Install(AdsLoading view)
    {
        // =========================
        // Domain
        // =========================
        AdsState state = new AdsState();

        // =========================
        // Infrastructure
        // =========================
        IAdsRepository repository = new EasySaveAdsRepository();
        IAdsPolicy policy = new DefaultAdsPolicy(5, 30f, 15, 30, 0);
        IAdsProvider provider = new GleyAdsProvider();
        UnityTimeProvider timeProvider = new UnityTimeProvider();

        // Initialize Ads SDK
        await provider.InitializeAsync();

        // =========================
        // Application
        // =========================
        AdsService service =
            new AdsService(state, repository, policy, provider, timeProvider);

        // =========================
        // Presentation
        // =========================
        AdsViewModel viewModel = new AdsViewModel(state);

        AdsPresenter presenter =
            new AdsPresenter(view, viewModel, service);

        return presenter;
    }
}