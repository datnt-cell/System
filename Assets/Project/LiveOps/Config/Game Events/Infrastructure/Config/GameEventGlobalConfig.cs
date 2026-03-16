using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using GameEventModule.Domain;
using LBG;
using Sirenix.OdinInspector.Editor;

namespace GameEventModule.Infrastructure.Config
{
    [CreateAssetMenu(fileName = "GameEventGlobalConfig", menuName = "GlobalConfigs/GameEventGlobalConfig")]
    [GlobalConfig("Assets/Resources/GlobalConfig/GameEvents")]
    public class GameEventGlobalConfig : GlobalConfig<GameEventGlobalConfig>
    {
        [Title("GAME EVENTS")]

        [TableList(ShowIndexLabels = true)]
        [Searchable]

        [OnCollectionChanged(nameof(OnEventListChanged))]
        [ValidateInput(nameof(ValidateEventIds), "Event Id bị trùng!")]

        public List<GameEventConfig> Events = new();

        // =========================
        // AUTO ID
        // =========================

        private void OnEventListChanged(CollectionChangeInfo info)
        {
            if (info.ChangeType == CollectionChangeType.Add)
            {
                var config = info.Value as GameEventConfig;

                if (config != null && string.IsNullOrEmpty(config.Id))
                {
                    config.Id = GenerateNextEventId();
                }
            }
        }

        private string GenerateNextEventId()
        {
            int max = Events
                .Where(x => !string.IsNullOrEmpty(x.Id))
                .Select(x => ExtractNumber(x.Id))
                .DefaultIfEmpty(0)
                .Max();

            return $"EVENT_{(max + 1):000}";
        }

        private int ExtractNumber(string id)
        {
            var digits = new string(id.Where(char.IsDigit).ToArray());
            return int.TryParse(digits, out int number) ? number : 0;
        }

        // =========================
        // VALIDATION
        // =========================

        private bool ValidateEventIds(List<GameEventConfig> list)
        {
            return list
                .Where(x => !string.IsNullOrEmpty(x.Id))
                .Select(x => x.Id)
                .Distinct()
                .Count() == list.Count;
        }

        // =========================
        // BUILD DOMAIN
        // =========================

        public List<GameEvent> Build()
        {
            return Events
                .Where(e => e != null)
                .Select(e => e.Build())
                .ToList();
        }
    }
}