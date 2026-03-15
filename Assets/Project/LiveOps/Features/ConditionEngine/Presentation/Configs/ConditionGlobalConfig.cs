using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using ConditionEngine.Domain;
using LBG;
using Sirenix.OdinInspector.Editor;

namespace ConditionEngine.Presentation
{
    [CreateAssetMenu(fileName = "ConditionGlobalConfig", menuName = "GlobalConfigs/ConditionGlobalConfig")]
    [GlobalConfig("Assets/Resources/GlobalConfig/Conditions")]
    public class ConditionGlobalConfig : GlobalConfig<ConditionGlobalConfig>
    {
        [Title("CONDITION NODES")]

        [SerializeReference]
        [SubclassSelector]
        [HideReferenceObjectPicker]

        [TableList(ShowIndexLabels = true)]
        [Searchable]

        [OnCollectionChanged(nameof(OnNodeListChanged))]
        [ValidateInput(nameof(ValidateNodeIds), "Condition Id bị trùng!")]

        public List<ConditionEntry> Nodes = new();

        // =========================
        // AUTO ID
        // =========================

        private void OnNodeListChanged(CollectionChangeInfo info)
        {
            if (info.ChangeType == CollectionChangeType.Add)
            {
                var node = info.Value as ConditionEntry;

                if (node != null && string.IsNullOrEmpty(node.Id))
                {
                    node.Id = GenerateNextConditionId();
                }
            }
        }

        private string GenerateNextConditionId()
        {
            int max = Nodes
                .Where(x => !string.IsNullOrEmpty(x.Id))
                .Select(x => ExtractNumber(x.Id))
                .DefaultIfEmpty(0)
                .Max();

            return $"COND_{(max + 1):000}";
        }

        private int ExtractNumber(string id)
        {
            var digits = new string(id.Where(char.IsDigit).ToArray());
            return int.TryParse(digits, out int number) ? number : 0;
        }

        // =========================
        // VALIDATION
        // =========================

        private bool ValidateNodeIds(List<ConditionEntry> list)
        {
            return list
                .Where(x => !string.IsNullOrEmpty(x.Id))
                .Select(x => x.Id)
                .Distinct()
                .Count() == list.Count;
        }
    }
}