using System;
using System.Collections.Generic;
using ConditionEngine.Domain;
using LBG;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ConditionEngine.Presentation
{
    public enum LogicType
    {
        [LabelText("🔗 Tất cả điều kiện")]
        And,

        [LabelText("🔀 Một trong các điều kiện")]
        Or
    }

    [Serializable]
    public class LogicNode : ConditionNode
    {
        private const string GROUP = "$Summary";

        // =====================
        // LOGIC TYPE
        // =====================

        [FoldoutGroup(GROUP)]
        [HorizontalGroup(GROUP + "/Logic")]
        [LabelText("🧠 Kiểu Logic")]
        [LabelWidth(110)]
        [EnumToggleButtons]
        public LogicType LogicType;

        // INFO ICON
        [HorizontalGroup(GROUP + "/Logic", Width = 28)]
        [Button("ℹ", ButtonSizes.Small)]
        [PropertyTooltip("@GetLogicNote()")]
        private void ShowInfo()
        {
            Debug.Log(GetLogicNote());
        }

        // =====================
        // CHILDREN
        // =====================

        [FoldoutGroup(GROUP)]
        [LabelText("🧩 Danh sách điều kiện")]
        [ListDrawerSettings(
            DraggableItems = true,
            Expanded = false,
            ShowIndexLabels = false,
            HideAddButton = false)]
        [SerializeReference]
        [SubclassSelector]
        public List<ConditionNode> Children = new();

        // =====================
        // SUMMARY
        // =====================

        protected override string GetSummary()
        {
            if (Children == null || Children.Count == 0)
            {
                return LogicType == LogicType.And
                    ? "🔗 TẤT CẢ (chưa có điều kiện)"
                    : "🔀 MỘT TRONG (chưa có điều kiện)";
            }

            List<string> parts = new();

            foreach (var child in Children)
            {
                if (child == null)
                    continue;

                parts.Add(child.Summary);
            }

            string logicText = LogicType == LogicType.And
                ? "🔗 TẤT CẢ"
                : "🔀 MỘT TRONG";

            return $"{logicText} ({string.Join(", ", parts)})";
        }

        // =====================
        // BUILD
        // =====================

        public override ICondition Build()
        {
            if (LogicType == LogicType.And)
            {
                var node = new AndCondition();

                foreach (var child in Children)
                    node.Conditions.Add(child.Build());

                return node;
            }

            var orNode = new OrCondition();

            foreach (var child in Children)
                orNode.Conditions.Add(child.Build());

            return orNode;
        }

        // =====================
        // NOTE
        // =====================

        private string GetLogicNote()
        {
            switch (LogicType)
            {
                case LogicType.And:
                    return
                        "🔗 – Tất cả điều kiện phải đúng\n\n" +
                        "Condition chỉ TRUE khi toàn bộ điều kiện con đều đúng.\n\n" +
                        "Ví dụ:\n" +
                        "Stage ≥ 10\n" +
                        "AND Country = US\n\n" +
                        "→ Player phải đạt stage ≥ 10 và ở US.";

                case LogicType.Or:
                    return
                        "🔀 – Chỉ cần một điều kiện đúng\n\n" +
                        "Condition TRUE khi có ít nhất 1 điều kiện con đúng.\n\n" +
                        "Ví dụ:\n" +
                        "Stage ≥ 20\n" +
                        "OR Ads ≥ 5\n\n" +
                        "→ Player chỉ cần thỏa 1 trong 2.";

                default:
                    return "";
            }
        }
    }
}