using System;
using ConditionEngine.Domain;

namespace ConditionEngine.Presentation
{
    /// <summary>
    /// Factory tạo Domain Condition từ config node
    /// </summary>
    public static class ConditionFactory
    {
        public static ICondition Create(ValueNode node)
        {
            switch (node.Type)
            {
                case ConditionType.PlayerLevel:
                    return new PlayerLevelCondition(node.Min, node.Max);

                case ConditionType.Stage:
                    return new StageCondition(node.Min, node.Max);

                case ConditionType.SessionCount:
                    return new SessionCountCondition(node.Min, node.Max);

                case ConditionType.TotalSpend:
                    return new TotalSpendCondition(node.Min, node.Max);

                case ConditionType.AdsWatchCount:
                    return new AdsWatchCountCondition(node.Min, node.Max);

                case ConditionType.ProductPurchased:
                    return new ProductPurchasedCondition(node.Value);

                case ConditionType.InventoryItem:
                    return new InventoryItemCondition(node.Value);

                case ConditionType.CurrencyAmount:
                    return new CurrencyAmountCondition(node.Value, node.Min);

                case ConditionType.Country:
                    return new CountryCondition(node.Value);

                case ConditionType.AppBuildVersion:
                    return new AppBuildVersionCondition(node.Min, node.Max);

                case ConditionType.NewUser:
                    return new NewUserCondition();

                case ConditionType.Quest:
                    return new QuestCondition(node.Value);

                case ConditionType.EventProgress:
                    return new EventProgressCondition(node.Value, node.Min, node.Max);

                case ConditionType.DateRange:
                    return new DateRangeCondition(node.StartDate, node.EndDate);

                case ConditionType.DayOfWeek:
                    return new DayOfWeekCondition(node.Day);

                case ConditionType.MultiDayOfWeek:
                    return new MultiDayOfWeekCondition(node.Days);

                case ConditionType.TimeRange:
                    return new TimeRangeCondition(node.StartTime, node.EndTime);

                case ConditionType.DaysSinceInstall:
                    return new DaysSinceInstallCondition(node.Min, node.Max);

                case ConditionType.PlayTime:
                    return new PlayTimeCondition(node.Min, node.Max);

                case ConditionType.PurchaseCount:
                    return new PurchaseCountCondition(node.Min, node.Max);

                case ConditionType.HasAnyPurchase:
                    return new HasAnyPurchaseCondition();

                case ConditionType.RemoveAdsPurchased:
                    return new RemoveAdsPurchasedCondition();

                case ConditionType.RewardedAdsWatched:
                    return new RewardedAdsWatchedCondition(node.Min, node.Max);

                case ConditionType.InterstitialAdsWatched:
                    return new InterstitialAdsWatchedCondition(node.Min, node.Max);

                case ConditionType.TotalAdsRevenue:
                    return new TotalAdsRevenueCondition(node.Min, node.Max);

                case ConditionType.AdsRevenueToday:
                    return new AdsRevenueTodayCondition(node.Min, node.Max);

                case ConditionType.Not:

                    if (node.Child == null)
                        throw new Exception("NotCondition requires child node");

                    return new NotCondition(node.Child.Build());

                default:
                    throw new Exception($"Unsupported ConditionType: {node.Type}");
            }
        }

    }
}