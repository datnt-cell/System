using IAPModule.Infrastructure.Providers;
using IAPModule.Infrastructure.Repositories;
using IAPModule.Infrastructure.Analytics;

using IAPModule.Application.UseCases;
using IAPModule.Application.Services;
using IAPModule.Application.Interfaces;

using IAPModule.Domain;
using IAPModule.Domain.Policies;

using Cysharp.Threading.Tasks;

public class IAPInstaller
{
    public async UniTask<IAPService> Install(LoadingView view)
    {
        // =========================
        // Domain
        // =========================
        var paymentState = new PaymentState();

        // =========================
        // Infrastructure
        // =========================

        var repository = new EasySaveIAPRepository();

        // load saved payment stats
        repository.LoadStats(paymentState);

        var provider = new GleyIAPProvider();
        await provider.InitializeAsync(); // wait store init

        var analytics = new FirebaseIAPAnalytics();

        IRewardPolicy rewardPolicy =
            new SimpleRewardPolicy(repository);

        // =========================
        // Application
        // =========================

        IPaymentService paymentService =
            new PaymentService(paymentState, repository);

        PurchaseUseCase purchaseUseCase =
            new PurchaseUseCase(
                provider,
                repository,
                analytics,
                rewardPolicy,
                paymentService
            );

        // =========================
        // Presentation
        // =========================

        IAPService service =
            new IAPService(view, purchaseUseCase);

        return service;
    }
}