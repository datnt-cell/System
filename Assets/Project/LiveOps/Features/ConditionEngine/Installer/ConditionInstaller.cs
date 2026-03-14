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
            var playerProvider = new PlayerProvider(GameManager.Instance.Player.PlayerService);
            var purchaseProvider = new PurchaseProvider();
            var adsProvider = new AdsProvider();
            var timeProvider = new TimeProvider();
            var inventoryProvider = new InventoryProvider();
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