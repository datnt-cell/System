using System;
using ConditionEngine.Domain;
using LBG;
using Sirenix.OdinInspector;
using UniLabs.Time;
using UnityEngine;
using System.Collections.Generic;

namespace ConditionEngine.Presentation
{
    [Serializable]
    [SubclassPath("Condition", "Value Condition")]
    public class ValueNode : ConditionNode
    {
        private const string GROUP = "$Summary";

        [FoldoutGroup("$Summary")]
        [HorizontalGroup("$Summary/Cond")]
        [LabelText("Condition")]
        [ValueDropdown(nameof(GetConditionTypes))]
        public ConditionType Type;

        [HorizontalGroup("$Summary/Cond", Width = 25)]
        [Button("ℹ", ButtonSizes.Small)]
        [PropertyTooltip("@GetConditionNote()")]
        private void ShowInfo()
        {
            Debug.Log(GetConditionNote());
        }

        #region NUMBER CONDITIONS

        [FoldoutGroup(GROUP)]
        [BoxGroup(GROUP + "/📏 Khoảng giá trị")]
        [ShowIf("@IsNumber() || IsEventProgress()")]
        [VerticalGroup(GROUP + "/📏 Khoảng giá trị/Values")]
        [LabelText("Giá trị nhỏ nhất")]
        public int Min;

        [ShowIf("@IsNumber() || IsEventProgress()")]
        [VerticalGroup(GROUP + "/📏 Khoảng giá trị/Values")]
        [LabelText("Giá trị lớn nhất")]
        public int Max;

        #endregion

        #region STRING CONDITIONS

        [FoldoutGroup(GROUP)]
        [BoxGroup(GROUP + "/🔤 Giá trị")]
        [ShowIf("@IsString() || IsEventProgress()")]
        [LabelText("Giá trị")]
        public string Value;

        #endregion

        #region EVENT PROGRESS

        [FoldoutGroup(GROUP)]
        [BoxGroup(GROUP + "/🎯 Event Progress")]
        [ShowIf(nameof(IsEventProgress))]
        [VerticalGroup(GROUP + "/🎯 Event Progress/Values")]
        [LabelText("Event Key")]
        public string EventKey;

        [ShowIf(nameof(IsEventProgress))]
        [VerticalGroup(GROUP + "/🎯 Event Progress/Values")]
        [LabelText("Min")]
        public int EventMin;

        [ShowIf(nameof(IsEventProgress))]
        [VerticalGroup(GROUP + "/🎯 Event Progress/Values")]
        [LabelText("Max")]
        public int EventMax;

        #endregion

        #region DATE RANGE

        [FoldoutGroup(GROUP)]
        [BoxGroup(GROUP + "/📅 Khoảng ngày")]
        [ShowIf(nameof(IsDateRange))]
        [VerticalGroup(GROUP + "/📅 Khoảng ngày/Values")]
        [LabelText("Ngày bắt đầu")]
        public UDateTime StartDate;

        [ShowIf(nameof(IsDateRange))]
        [VerticalGroup(GROUP + "/📅 Khoảng ngày/Values")]
        [LabelText("Ngày kết thúc")]
        public UDateTime EndDate;

        #endregion

        #region TIME RANGE

        [FoldoutGroup(GROUP)]
        [BoxGroup(GROUP + "/⏰ Khoảng thời gian")]
        [ShowIf(nameof(IsTimeRange))]
        [VerticalGroup(GROUP + "/⏰ Khoảng thời gian/Values")]
        [LabelText("Bắt đầu")]
        public UTimeSpan StartTime;

        [ShowIf(nameof(IsTimeRange))]
        [VerticalGroup(GROUP + "/⏰ Khoảng thời gian/Values")]
        [LabelText("Kết thúc")]
        public UTimeSpan EndTime;

        #endregion

        #region DAY CONDITIONS

        [FoldoutGroup(GROUP)]
        [BoxGroup(GROUP + "/📆 Ngày trong tuần")]
        [ShowIf(nameof(IsSingleDay))]
        [LabelText("Ngày")]
        public DayOfWeek Day;

        [FoldoutGroup(GROUP)]
        [BoxGroup(GROUP + "/📆 Nhiều ngày trong tuần")]
        [ShowIf(nameof(IsMultiDay))]
        [EnumToggleButtons, HideLabel]
        public WeekDayFlags Days;

        #endregion

        #region NOT CONDITION

        [FoldoutGroup(GROUP)]
        [BoxGroup(GROUP + "/❗ Phủ định")]
        [ShowIf(nameof(IsNot))]
        [SerializeReference]
        [SubclassSelector]
        [LabelText("Condition")]
        public ConditionNode Child;

        #endregion

        // ======================
        // BUILD CONDITION
        // ======================

        public override ICondition Build()
        {
            return ConditionFactory.Create(this);
        }

        #region SUMMARY

        protected override string GetSummary()
        {
            switch (Type)
            {
                case ConditionType.PlayerLevel:
                    return $"👤 Level người chơi [{Min} - {Max}]";

                case ConditionType.Stage:
                    return $"🎮 Stage [{Min} - {Max}]";

                case ConditionType.SessionCount:
                    return $"📱 Số lần mở game [{Min} - {Max}]";

                case ConditionType.TotalSpend:
                    return $"💰 Tổng tiền đã nạp [{Min} - {Max}]";

                case ConditionType.AdsWatchCount:
                    return $"📺 Số quảng cáo đã xem [{Min} - {Max}]";

                case ConditionType.ProductPurchased:
                    return $"🛒 Đã mua sản phẩm = {Value}";

                case ConditionType.InventoryItem:
                    return $"🎒 Có item trong kho = {Value}";

                case ConditionType.CurrencyAmount:
                    return $"💎 Số lượng tiền [{Min} - {Max}]";

                case ConditionType.Country:
                    return $"🌍 Quốc gia = {Value}";

                case ConditionType.AppBuildVersion:
                    return $"📦 Phiên bản build [{Min} - {Max}]";

                case ConditionType.NewUser:
                    return $"🆕 Người chơi mới";

                case ConditionType.Quest:
                    return $"📜 Nhiệm vụ = {Value}";

                case ConditionType.DateRange:
                    return $"📅 Khoảng ngày [{StartDate} → {EndDate}]";

                case ConditionType.TimeRange:
                    return $"⏰ Khoảng giờ [{StartTime?.TimeSpan} → {EndTime?.TimeSpan}]";

                case ConditionType.DayOfWeek:
                    return $"📆 Ngày trong tuần = {Day}";

                case ConditionType.MultiDayOfWeek:
                    return $"🗓 Các ngày trong tuần = {Days}";

                case ConditionType.Not:
                    return $"🚫 Không ({Child?.Summary})";

                case ConditionType.EventProgress:
                    return $"🎯 Tiến độ sự kiện {EventKey} [{EventMin} - {EventMax}]";

                case ConditionType.DaysSinceInstall:
                    return $"📆 Số ngày từ khi cài game [{Min} - {Max}]";

                case ConditionType.PlayTime:
                    return $"⏱ Thời gian chơi [{Min} - {Max}]";
                case ConditionType.PurchaseCount:
                    return $"💰 Số lần mua [{Min} - {Max}]";

                case ConditionType.HasAnyPurchase:
                    return "🛒 Đã từng mua IAP";

                case ConditionType.RemoveAdsPurchased:
                    return "🚫 Đã mua Remove Ads";

                case ConditionType.RewardedAdsWatched:
                    return $"📺 Rewarded Ads đã xem [{Min} - {Max}]";

                case ConditionType.InterstitialAdsWatched:
                    return $"📺 Interstitial Ads đã xem [{Min} - {Max}]";

                case ConditionType.TotalAdsRevenue:
                    return $"💵 Tổng doanh thu Ads [{Min} - {Max}]";

                case ConditionType.AdsRevenueToday:
                    return $"📅 Doanh thu Ads hôm nay [{Min} - {Max}]";
                default:
                    return Type.ToString();
            }
        }

        #endregion

        #region TYPE CHECK

        // ======================
        // NUMBER CONDITIONS
        // ======================

        bool IsNumber() =>
            Type == ConditionType.PlayerLevel ||
            Type == ConditionType.Stage ||
            Type == ConditionType.SessionCount ||
            Type == ConditionType.TotalSpend ||
            Type == ConditionType.PurchaseCount ||
            Type == ConditionType.AdsWatchCount ||
            Type == ConditionType.RewardedAdsWatched ||
            Type == ConditionType.InterstitialAdsWatched ||
            Type == ConditionType.TotalAdsRevenue ||
            Type == ConditionType.AdsRevenueToday ||
            Type == ConditionType.CurrencyAmount ||
            Type == ConditionType.AppBuildVersion ||
            Type == ConditionType.DaysSinceInstall ||
            Type == ConditionType.PlayTime;


        // ======================
        // STRING CONDITIONS
        // ======================

        bool IsString() =>
            Type == ConditionType.ProductPurchased ||
            Type == ConditionType.InventoryItem ||
            Type == ConditionType.Country ||
            Type == ConditionType.Quest;


        // ======================
        // NUMBER + STRING (SPECIAL)
        // ======================

        bool IsEventProgress() =>
            Type == ConditionType.EventProgress;


        // ======================
        // DATE/TIME CONDITIONS
        // ======================

        bool IsDateRange() =>
            Type == ConditionType.DateRange;

        bool IsTimeRange() =>
            Type == ConditionType.TimeRange;


        // ======================
        // DAY CONDITIONS
        // ======================

        bool IsSingleDay() =>
            Type == ConditionType.DayOfWeek;

        bool IsMultiDay() =>
            Type == ConditionType.MultiDayOfWeek;


        // ======================
        // LOGIC CONDITIONS
        // ======================

        bool IsNot() =>
            Type == ConditionType.Not;

        bool IsBool() =>
            Type == ConditionType.HasAnyPurchase ||
            Type == ConditionType.RemoveAdsPurchased;

        #endregion

        // ======================
        // DROPDOWN
        // ======================

        private static IEnumerable<ValueDropdownItem<ConditionType>> GetConditionTypes()
        {
            var list = new List<ValueDropdownItem<ConditionType>>();

            // ======================
            // 👤 NGƯỜI CHƠI
            // ======================

            list.Add(new ValueDropdownItem<ConditionType>("👤 Người chơi/Level", ConditionType.PlayerLevel));
            list.Add(new ValueDropdownItem<ConditionType>("👤 Người chơi/Stage", ConditionType.Stage));
            list.Add(new ValueDropdownItem<ConditionType>("👤 Người chơi/Số lần mở game", ConditionType.SessionCount));

            // ======================
            // 💰 KIẾM TIỀN (IAP)
            // ======================

            list.Add(new ValueDropdownItem<ConditionType>("💰 Kiếm tiền/Tổng tiền đã nạp", ConditionType.TotalSpend));
            list.Add(new ValueDropdownItem<ConditionType>("💰 Kiếm tiền/Số lần mua", ConditionType.PurchaseCount));
            list.Add(new ValueDropdownItem<ConditionType>("💰 Kiếm tiền/Đã từng mua", ConditionType.HasAnyPurchase));
            list.Add(new ValueDropdownItem<ConditionType>("💰 Kiếm tiền/Đã mua sản phẩm", ConditionType.ProductPurchased));
            list.Add(new ValueDropdownItem<ConditionType>("💰 Kiếm tiền/Đã mua Remove Ads", ConditionType.RemoveAdsPurchased));

            // ======================
            // 📺 QUẢNG CÁO
            // ======================

            list.Add(new ValueDropdownItem<ConditionType>("📺 Ads/Số quảng cáo đã xem", ConditionType.AdsWatchCount));
            list.Add(new ValueDropdownItem<ConditionType>("📺 Ads/Rewarded đã xem", ConditionType.RewardedAdsWatched));
            list.Add(new ValueDropdownItem<ConditionType>("📺 Ads/Interstitial đã xem", ConditionType.InterstitialAdsWatched));
            list.Add(new ValueDropdownItem<ConditionType>("📺 Ads/Tổng doanh thu", ConditionType.TotalAdsRevenue));
            list.Add(new ValueDropdownItem<ConditionType>("📺 Ads/Doanh thu hôm nay", ConditionType.AdsRevenueToday));

            // ======================
            // 🎒 KHO ĐỒ
            // ======================

            list.Add(new ValueDropdownItem<ConditionType>("🎒 Kho đồ/Có item", ConditionType.InventoryItem));
            list.Add(new ValueDropdownItem<ConditionType>("🎒 Kho đồ/Số lượng tiền", ConditionType.CurrencyAmount));

            // ======================
            // 🌍 THÔNG TIN NGƯỜI CHƠI
            // ======================

            list.Add(new ValueDropdownItem<ConditionType>("🌍 Thông tin người chơi/Quốc gia", ConditionType.Country));
            list.Add(new ValueDropdownItem<ConditionType>("🌍 Thông tin người chơi/Phiên bản build", ConditionType.AppBuildVersion));

            // ======================
            // 🎮 TRẠNG THÁI GAME
            // ======================

            list.Add(new ValueDropdownItem<ConditionType>("🎮 Trạng thái game/Người chơi mới", ConditionType.NewUser));
            list.Add(new ValueDropdownItem<ConditionType>("🎮 Trạng thái game/Nhiệm vụ", ConditionType.Quest));
            list.Add(new ValueDropdownItem<ConditionType>("🎮 Trạng thái game/Tiến độ sự kiện", ConditionType.EventProgress));

            // ======================
            // ⏱ THỜI GIAN
            // ======================

            list.Add(new ValueDropdownItem<ConditionType>("⏱ Thời gian/Khoảng ngày", ConditionType.DateRange));
            list.Add(new ValueDropdownItem<ConditionType>("⏱ Thời gian/Khoảng giờ", ConditionType.TimeRange));
            list.Add(new ValueDropdownItem<ConditionType>("⏱ Thời gian/Ngày trong tuần", ConditionType.DayOfWeek));
            list.Add(new ValueDropdownItem<ConditionType>("⏱ Thời gian/Nhiều ngày trong tuần", ConditionType.MultiDayOfWeek));
            list.Add(new ValueDropdownItem<ConditionType>("⏱ Thời gian/Số ngày từ khi cài game", ConditionType.DaysSinceInstall));
            list.Add(new ValueDropdownItem<ConditionType>("⏱ Thời gian/Tổng thời gian chơi", ConditionType.PlayTime));

            // ======================
            // 🔀 LOGIC
            // ======================

            list.Add(new ValueDropdownItem<ConditionType>("🔀 Logic/Phủ định (Not)", ConditionType.Not));

            return list;
        }

        // ======================
        // NOTE
        // ======================

        private string GetConditionNote()
        {
            switch (Type)
            {
                case ConditionType.PlayerLevel:
                    return "👤 Player Level\n\n" +
                           "Kiểm tra level hiện tại của player.\n\n" +
                           "Cần nhập:\n" +
                           "• Min: Level tối thiểu\n" +
                           "• Max: Level tối đa\n\n" +
                           "Ví dụ:\n" +
                           "Min = 10, Max = 20 → Player phải có level từ 10 đến 20.";

                case ConditionType.Stage:
                    return "🎮 Stage\n\n" +
                           "Kiểm tra stage hiện tại của player.\n\n" +
                           "Cần nhập:\n" +
                           "• Min: Stage tối thiểu\n" +
                           "• Max: Stage tối đa\n\n" +
                           "Ví dụ:\n" +
                           "Min = 50, Max = 100 → Player đang ở stage 50 → 100.";

                case ConditionType.SessionCount:
                    return "📱 Session Count\n\n" +
                           "Kiểm tra số lần player đã mở game.\n\n" +
                           "Cần nhập:\n" +
                           "• Min: Số session tối thiểu\n" +
                           "• Max: Số session tối đa\n\n" +
                           "Ví dụ:\n" +
                           "Min = 5 → Player phải mở game ít nhất 5 lần.";

                case ConditionType.TotalSpend:
                    return "💰 Total Spend\n\n" +
                           "Kiểm tra tổng số tiền player đã nạp.\n\n" +
                           "Cần nhập:\n" +
                           "• Min: Số tiền tối thiểu\n" +
                           "• Max: Số tiền tối đa\n\n" +
                           "Ví dụ:\n" +
                           "Min = 10 → Player đã spend ít nhất 10$.";

                case ConditionType.AdsWatchCount:
                    return "📺 Ads Watched\n\n" +
                           "Kiểm tra số quảng cáo player đã xem.\n\n" +
                           "Cần nhập:\n" +
                           "• Min: Số ads tối thiểu\n" +
                           "• Max: Số ads tối đa\n\n" +
                           "Ví dụ:\n" +
                           "Min = 20 → Player đã xem ≥ 20 quảng cáo.";

                case ConditionType.ProductPurchased:
                    return "🛒 Product Purchased\n\n" +
                           "Kiểm tra player đã mua một sản phẩm IAP.\n\n" +
                           "Cần nhập:\n" +
                           "• Value: ProductId của IAP\n\n" +
                           "Ví dụ:\n" +
                           "Value = remove_ads → Player đã mua gói remove_ads.";

                case ConditionType.InventoryItem:
                    return "🎒 Inventory Item\n\n" +
                           "Kiểm tra player có item trong inventory.\n\n" +
                           "Cần nhập:\n" +
                           "• Value: Id của item\n\n" +
                           "Ví dụ:\n" +
                           "Value = golden_key.";

                case ConditionType.CurrencyAmount:
                    return "💎 Currency Amount\n\n" +
                           "Kiểm tra số lượng currency player đang có.\n\n" +
                           "Cần nhập:\n" +
                           "• Min: Số lượng tối thiểu\n" +
                           "• Max: Số lượng tối đa\n\n" +
                           "Ví dụ:\n" +
                           "Min = 1000 → Player có ít nhất 1000 coins.";

                case ConditionType.Country:
                    return "🌍 Country\n\n" +
                           "Kiểm tra quốc gia của player.\n\n" +
                           "Cần nhập:\n" +
                           "• Value: Mã quốc gia (ISO code)\n\n" +
                           "Ví dụ:\n" +
                           "Value = US.";

                case ConditionType.AppBuildVersion:
                    return "📦 App Build Version\n\n" +
                           "Kiểm tra build version của app.\n\n" +
                           "Cần nhập:\n" +
                           "• Min: Build tối thiểu\n" +
                           "• Max: Build tối đa\n\n" +
                           "Ví dụ:\n" +
                           "Min = 120.";

                case ConditionType.NewUser:
                    return "🆕 New User\n\n" +
                           "Kiểm tra player có phải user mới.\n\n" +
                           "Thường dùng cho:\n" +
                           "• Tutorial\n" +
                           "• Beginner offer\n" +
                           "• Onboarding event.";

                case ConditionType.Quest:
                    return "📜 Quest Completed\n\n" +
                           "Kiểm tra player đã hoàn thành quest.\n\n" +
                           "Cần nhập:\n" +
                           "• Value: Id của quest\n\n" +
                           "Ví dụ:\n" +
                           "Value = tutorial_complete.";

                case ConditionType.DateRange:
                    return "📅 Date Range\n\n" +
                           "Condition chỉ active trong khoảng ngày.\n\n" +
                           "Cần nhập:\n" +
                           "• StartDate: Ngày bắt đầu\n" +
                           "• EndDate: Ngày kết thúc\n\n" +
                           "Ví dụ:\n" +
                           "25/10 → 05/11.";

                case ConditionType.TimeRange:
                    return "⏰ Time Range\n\n" +
                           "Condition active trong khoảng giờ mỗi ngày.\n\n" +
                           "Cần nhập:\n" +
                           "• StartTime\n" +
                           "• EndTime\n\n" +
                           "Ví dụ:\n" +
                           "18:00 → 22:00 (giờ vàng).";

                case ConditionType.DayOfWeek:
                    return "📆 Day Of Week\n\n" +
                           "Condition chỉ active vào một ngày trong tuần.\n\n" +
                           "Cần chọn:\n" +
                           "• Day\n\n" +
                           "Ví dụ:\n" +
                           "Sunday.";

                case ConditionType.MultiDayOfWeek:
                    return "📆 Multiple Days\n\n" +
                           "Condition active vào nhiều ngày trong tuần.\n\n" +
                           "Cần chọn:\n" +
                           "• Các ngày trong tuần\n\n" +
                           "Ví dụ:\n" +
                           "Friday + Saturday + Sunday.";

                case ConditionType.Not:
                    return "🔀 NOT Condition\n\n" +
                           "Đảo ngược kết quả của condition con.\n\n" +
                           "Ví dụ:\n" +
                           "NOT (Country = US) → Player KHÔNG ở US.";

                case ConditionType.EventProgress:
                    return "🎯 Event Progress\n\n" +
                           "Kiểm tra tiến độ player trong một event.\n\n" +
                           "Cần nhập:\n" +
                           "• Value: EventId\n" +
                           "• Min: Progress tối thiểu\n" +
                           "• Max: Progress tối đa\n\n" +
                           "Ví dụ:\n" +
                           "Event = halloween\n" +
                           "Min = 5.";

                case ConditionType.DaysSinceInstall:
                    return "📆 Days Since Install\n\n" +
                           "Số ngày kể từ khi player cài game.\n\n" +
                           "Cần nhập:\n" +
                           "• Min\n" +
                           "• Max\n\n" +
                           "Ví dụ:\n" +
                           "Min = 3 → Player đã cài game ≥ 3 ngày.";

                case ConditionType.PlayTime:
                    return "⏱ Play Time\n\n" +
                           "Tổng thời gian player đã chơi game.\n\n" +
                           "Cần nhập:\n" +
                           "• Min\n" +
                           "• Max\n\n" +
                           "Ví dụ:\n" +
                           "Min = 3600 → Player chơi ≥ 1 giờ.";

                case ConditionType.PurchaseCount:
                    return "💰 Purchase Count\n\n" +
                           "Kiểm tra tổng số lần player đã mua IAP.\n\n" +
                           "Cần nhập:\n" +
                           "• Min: Số lần mua tối thiểu\n" +
                           "• Max: Số lần mua tối đa\n\n" +
                           "Ví dụ:\n" +
                           "Min = 1 → Player đã mua ít nhất 1 lần.";

                case ConditionType.HasAnyPurchase:
                    return "🛒 Has Any Purchase\n\n" +
                           "Kiểm tra player đã từng mua IAP hay chưa.\n\n" +
                           "Không cần nhập Min / Max.\n\n" +
                           "Ví dụ:\n" +
                           "Condition TRUE nếu player đã từng mua ít nhất 1 sản phẩm.";

                case ConditionType.RemoveAdsPurchased:
                    return "🚫 Remove Ads Purchased\n\n" +
                           "Kiểm tra player đã mua gói Remove Ads hay chưa.\n\n" +
                           "Không cần nhập Min / Max.\n\n" +
                           "Ví dụ:\n" +
                           "Condition TRUE nếu player đã mua Remove Ads.";

                case ConditionType.RewardedAdsWatched:
                    return "📺 Rewarded Ads Watched\n\n" +
                           "Số lượng quảng cáo Rewarded player đã xem.\n\n" +
                           "Cần nhập:\n" +
                           "• Min\n" +
                           "• Max\n\n" +
                           "Ví dụ:\n" +
                           "Min = 5 → Player đã xem ít nhất 5 Rewarded Ads.";

                case ConditionType.InterstitialAdsWatched:
                    return "📺 Interstitial Ads Watched\n\n" +
                           "Số lượng quảng cáo Interstitial đã hiển thị cho player.\n\n" +
                           "Cần nhập:\n" +
                           "• Min\n" +
                           "• Max\n\n" +
                           "Ví dụ:\n" +
                           "Min = 10 → Player đã thấy ít nhất 10 Interstitial Ads.";

                case ConditionType.TotalAdsRevenue:
                    return "💵 Total Ads Revenue\n\n" +
                           "Tổng doanh thu quảng cáo từ player.\n\n" +
                           "Cần nhập:\n" +
                           "• Min\n" +
                           "• Max\n\n" +
                           "Ví dụ:\n" +
                           "Min = 0.5 → Player đã tạo ≥ $0.5 doanh thu ads.";

                case ConditionType.AdsRevenueToday:
                    return "📅 Ads Revenue Today\n\n" +
                           "Doanh thu quảng cáo player tạo ra trong hôm nay.\n\n" +
                           "Cần nhập:\n" +
                           "• Min\n" +
                           "• Max\n\n" +
                           "Ví dụ:\n" +
                           "Min = 0.1 → Player đã tạo ≥ $0.1 doanh thu ads hôm nay.";

                default: return "";
            }
        }
    }
}