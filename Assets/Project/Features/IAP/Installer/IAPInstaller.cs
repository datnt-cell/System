using IAPModule.Infrastructure.Providers;
using IAPModule.Infrastructure.Repositories;
using IAPModule.Infrastructure.Analytics;
using IAPModule.Application.UseCases;
using IAPModule.Domain.Policies;

public class IAPInstaller
{
    public IAPPresenter Install(IAPLoading view)
    {
        // =========================
        // Infrastructure
        // =========================
        var repository = new EasySaveIAPRepository();
        var provider = new GleyIAPProvider();
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