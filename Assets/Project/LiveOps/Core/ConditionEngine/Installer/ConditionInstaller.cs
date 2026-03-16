using ConditionEngine.Application;
using ConditionEngine.Domain;

namespace ConditionEngine.Infrastructure
{
    /// <summary>
    /// Khởi tạo ConditionEngine
    /// </summary>
    public class ConditionInstaller
    {
        public ConditionInstallResult Install()
        {
            var playerProvider = new PlayerProvider(GameManager.Instance.Player.Service);
            var purchaseProvider = new PurchaseProvider(GameManager.Instance.IAPManager.Service);
            var adsProvider = new AdsProvider(GameManager.Instance.AdsManager.Service.GetState());
            var timeProvider = new TimeProvider();
            var inventoryProvider = new InventoryProvider(GameManager.Instance.Currency.Service);
            var eventProvider = new EventProvider(GameManager.Instance.GameEvents.Service);

            IConditionContext context = new DefaultConditionContext(
                playerProvider,
                purchaseProvider,
                adsProvider,
                timeProvider,
                inventoryProvider,
                eventProvider
            );

            var evaluator = new ConditionEvaluator();
            var events = new ConditionEvents();

            var service = new ConditionService(
                evaluator,
                context,
                events
            );

            return new ConditionInstallResult(service, events);
        }
    }
}

namespace ConditionEngine.Infrastructure
{
    public readonly struct ConditionInstallResult
    {
        public ConditionService Service { get; }
        public ConditionEvents Events { get; }

        public ConditionInstallResult(
            ConditionService service,
            ConditionEvents events)
        {
            Service = service;
            Events = events;
        }
    }
}