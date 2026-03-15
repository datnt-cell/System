using ConditionEngine.Application;
using ConditionEngine.Domain;

namespace ConditionEngine.Infrastructure
{
    /// <summary>
    /// Khởi tạo ConditionEngine
    /// </summary>
    public class ConditionInstaller
    {
        public ConditionService Install()
        {
            // providers lấy dữ liệu runtime
            var playerProvider = new PlayerProvider(GameManager.Instance.Player.Service);
            var purchaseProvider = new PurchaseProvider(GameManager.Instance.IAPManager.Service);
            var adsProvider = new AdsProvider(GameManager.Instance.AdsManager.Service.GetState());
            var timeProvider = new TimeProvider();
            var inventoryProvider = new InventoryProvider(GameManager.Instance.Currency.Service);
            var eventProvider = new EventProvider();

            // context tổng hợp dữ liệu cho condition
            IConditionContext context = new DefaultConditionContext(
                playerProvider,
                purchaseProvider,
                adsProvider,
                timeProvider,
                inventoryProvider,
                eventProvider
            );

            var evaluator = new ConditionEvaluator();

            // tạo service
            return new ConditionService(evaluator, context);
        }
    }
}