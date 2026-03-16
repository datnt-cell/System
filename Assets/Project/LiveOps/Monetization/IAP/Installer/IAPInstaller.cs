using IAPModule.Infrastructure.Providers;
using IAPModule.Infrastructure.Repositories;
using IAPModule.Infrastructure.Analytics;
using IAPModule.Infrastructure;

using IAPModule.Application.UseCases;
using IAPModule.Application.Services;
using IAPModule.Application.Interfaces;

using IAPModule.Domain;
using IAPModule.Domain.Policies;

using Cysharp.Threading.Tasks;

public class IAPInstaller
{
    public async UniTask<IAPInstallerResult> Install(LoadingView view)
    {
        var paymentState = new PaymentState();

        var repository = new EasySaveIAPRepository();
        repository.LoadStats(paymentState);

        var provider = new GleyIAPProvider();
        await provider.InitializeAsync();

        var analytics = new FirebaseIAPAnalytics();

        IRewardPolicy rewardPolicy =
            new SimpleRewardPolicy(repository);

        IPaymentService paymentService =
            new PaymentService(paymentState, repository);

        var purchaseUseCase =
            new PurchaseUseCase(
                provider,
                repository,
                analytics,
                rewardPolicy,
                paymentService
            );

        var events = new IAPEvents();

        var service =
            new IAPService(
                view,
                purchaseUseCase,
                events
            );

        return new IAPInstallerResult(
            service,
            events
        );
    }
}