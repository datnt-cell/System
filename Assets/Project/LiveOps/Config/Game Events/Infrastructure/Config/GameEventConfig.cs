using System;
using System.Linq;
using System.Collections.Generic;
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

        [HorizontalGroup("Root/Config/Condition", Width = 28)]
        [Button("ℹ", ButtonSizes.Small)]
        [PropertyTooltip("@GetConditionNote()")]
        private void ShowInfo()
        {
            UnityEngine.Debug.Log(GetConditionNote());
        }

        private string GetConditionNote()
        {
            return
                "Event kích hoạt khi Condition = TRUE\n\n" +
                "Finish Type:\n" +
                "• Condition: Event tắt khi Condition FALSE\n" +
                "• Duration: Event chạy thêm một khoảng thời gian sau khi kích hoạt\n\n" +
                "Sau khi Event kết thúc và hết Cooldown,\n" +
                "nếu Condition vẫn TRUE thì Event sẽ kích hoạt lại.";
        }

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
        // ATTACHMENTS
        // =========================

        [HorizontalGroup("Root")]
        [BoxGroup("Root/Config")]
        [ListDrawerSettings(Expanded = true)]
        public List<GameEventAttachmentConfig> Attachments = new();

        // =========================
        // STATE
        // =========================

        private bool IsDurationMode => FinishType == EventFinishType.Duration;

        // =========================
        // DROPDOWN
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

            var attachments = Attachments
                .Select(a => a.Build())
                .Where(a => a != null)
                .ToList();

            return new GameEvent(
                new GameEventId(Id),
                Priority,
                condition,
                finishPolicy,
                attachments);
        }
    }
}