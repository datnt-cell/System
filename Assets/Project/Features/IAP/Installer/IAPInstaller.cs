using IAPModule.Infrastructure.Providers;
using IAPModule.Infrastructure.Repositories;
using IAPModule.Infrastructure.Analytics;
using IAPModule.Application.UseCases;
using IAPModule.Domain.Policies;
using Cysharp.Threading.Tasks;

public class IAPInstaller
{
    public async UniTask<IAPPresenter> Install(IAPLoading view)
    {
        // =========================
        // Infrastructure
        // =========================
        var repository = new EasySaveIAPRepository();

        var provider = new GleyIAPProvider();
        await provider.InitializeAsync();   // ✅ đợi IAP initialize

        var analytics = new FirebaseIAPAnalytics();

        IRewardPolicy rewardPolicy = new SimpleRewardPolicy(repository);

        // =========================
        // Application
        // =========================
        PurchaseUseCase purchaseUseCase =
            new PurchaseUseCase(provider, repository, analytics, rewardPolicy);

        // =========================
        // Presentation
        // =========================
        IAPPresenter presenter =
            new IAPPresenter(view, purchaseUseCase);

        return presenter;
    }
}