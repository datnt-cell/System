/// <summary>
/// Installer chịu trách nhiệm khởi tạo và liên kết các dependency.
/// </summary>
public class AdsInstaller
{
    public AdsPresenter Install(AdsView view)
    {
        // Domain
        AdsState state = new AdsState();

        // Infrastructure
        IAdsRepository repository = new EasySaveAdsRepository();
        IAdsPolicy policy = new DefaultAdsPolicy(5, 30f);
        IAdsProvider provider = new GleyAdsProvider();

        // Application
        AdsService service = new AdsService(state, repository, policy, provider);

        // Presentation
        AdsViewModel viewModel = new AdsViewModel(state);
        AdsPresenter presenter = new AdsPresenter(view, viewModel, service);

        return presenter;
    }
}