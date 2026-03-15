using System;
using System.Linq;
using ConditionEngine.Presentation;
using GameEventModule.Domain;
using Sirenix.OdinInspector;
using UniLabs.Time;

namespace GameEventModule.Infrastructure.Config
{
    [Serializable]
    [InlineProperty]
    public class GameEventConfig
    {
        // =========================
        // INFO
        // =========================

        [HorizontalGroup("Root", 0.35f)]
        [BoxGroup("Root/Info")]
        [ReadOnly]
        public string Id;

        [HorizontalGroup("Root")]
        [BoxGroup("Root/Info")]
        public string DisplayName;

        [HorizontalGroup("Root")]
        [BoxGroup("Root/Info")]
        public int Priority;

        // =========================
        // CONFIG
        // =========================

        [HorizontalGroup("Root", 0.65f)]
        [BoxGroup("Root/Config")]

        [HorizontalGroup("Root/Config/Condition")]
        [ValueDropdown(nameof(GetConditionDropdown))]
        [LabelText("Condition")]
        public string ConditionId;

        [HorizontalGroup("Root/Config/Condition", width: 24)]
        [Button(SdfIconType.InfoCircle)]
        private void ToggleHelp()
        {
            showHelp = !showHelp;
        }

        [BoxGroup("Root/Config")]
        [ShowIf(nameof(showHelp))]
        [InfoBox(
            "Event kích hoạt khi Condition = TRUE\n\n" +
            "Finish Type:\n" +
            "• Condition: Event tắt khi Condition FALSE\n" +
            "• Duration: Event chạy thêm một khoảng thời gian sau khi kích hoạt\n\n" +
            "Sau khi Event kết thúc và hết Cooldown,\n" +
            "nếu Condition vẫn TRUE thì Event sẽ kích hoạt lại.",
            InfoMessageType.None)]
        private bool _;

        [HorizontalGroup("Root")]
        [BoxGroup("Root/Config")]
        public EventFinishType FinishType;

        [HorizontalGroup("Root")]
        [BoxGroup("Root/Config")]
        [ShowIf(nameof(IsDurationMode))]
        public UTimeSpan Duration;

        [HorizontalGroup("Root")]
        [BoxGroup("Root/Config")]
        public UTimeSpan Cooldown;

        // =========================
        // OFFER ATTACHMENT
        // =========================

        [HorizontalGroup("Root")]
        [BoxGroup("Root/Config")]
        [LabelText("Offer Group")]
        [ValueDropdown(nameof(GetOfferGroupDropdown))]
        [ShowIf(nameof(UseOfferGroup))]
        public string OfferGroupId;

        [HorizontalGroup("Root")]
        [BoxGroup("Root/Config")]
        [LabelText("Offer")]
        [ValueDropdown(nameof(GetOfferDropdown))]
        [ShowIf(nameof(UseSingleOffer))]
        public string OfferId;

        // =========================
        // STATE
        // =========================

        [NonSerialized]
        private bool showHelp;

        private bool IsDurationMode => FinishType == EventFinishType.Duration;

        private bool UseOfferGroup => string.IsNullOrEmpty(OfferId);

        private bool UseSingleOffer => string.IsNullOrEmpty(OfferGroupId);

        // =========================
        // DROPDOWNS
        // =========================

        private static ValueDropdownList<string> GetConditionDropdown()
        {
            var list = new ValueDropdownList<string>();

            foreach (var node in ConditionGlobalConfig.Instance.Nodes)
            {
                if (node == null) continue;
                list.Add($"{node.Id} | {node.DisplayName}", node.Id);
            }

            return list;
        }

        private static ValueDropdownList<string> GetOfferGroupDropdown()
        {
            var list = new ValueDropdownList<string>();

            foreach (var group in GameOfferGroupGlobalConfig.Instance.Groups)
            {
                if (group == null) continue;
                list.Add($"{group.Id} | {group.DisplayName}", group.Id);
            }

            return list;
        }

        private static ValueDropdownList<string> GetOfferDropdown()
        {
            var list = new ValueDropdownList<string>();

            foreach (var offer in GameOfferGlobalConfig.Instance.Offers)
            {
                if (offer == null) continue;
                list.Add($"{offer.Id} | {offer.DisplayName}", offer.Id);
            }

            return list;
        }

        // =========================
        // BUILD DOMAIN
        // =========================

        public GameEvent Build()
        {
            var condition = ConditionGlobalConfig.Instance
                .Nodes
                .FirstOrDefault(x => x.Id == ConditionId)
                ?.Build();

            var finishPolicy = new EventFinishPolicy(
                FinishType,
                Duration,
                Cooldown);

            var attachment = new GameOfferAttachment(
                OfferGroupId,
                OfferId);

            return new GameEvent(
                new GameEventId(Id),
                Priority,
                condition,
                finishPolicy,
                attachment);
        }
    }
}