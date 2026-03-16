using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using Sirenix.Utilities;
using Sirenix.OdinInspector.Editor;

namespace BattlePassModule.Infrastructure.Config
{
    [CreateAssetMenu(fileName = "BattlePassGlobalConfig", menuName = "GlobalConfigs/BattlePassGlobalConfig")]
    [GlobalConfig("Assets/Resources/GlobalConfig/BattlePass/")]
    public class BattlePassGlobalConfig : GlobalConfig<BattlePassGlobalConfig>
    {
        [Title("🎫 SEASONS", bold: true)]
        [TableList]
        [Searchable]
        [OnCollectionChanged(nameof(OnSeasonListChanged))]
        [ValidateInput(nameof(ValidateSeasonIds), "Season Id bị trùng!")]
        public List<BattlePassSeasonConfig> Seasons = new();

        // ===== AUTO ID =====

        private void OnSeasonListChanged(CollectionChangeInfo info)
        {
            if (info.ChangeType == CollectionChangeType.Add)
            {
                var newItem = info.Value as BattlePassSeasonConfig;

                if (newItem != null && string.IsNullOrEmpty(newItem.Id))
                {
                    newItem.Id = GenerateNextSeasonId();
                }
            }
        }

        private string GenerateNextSeasonId()
        {
            int max = Seasons
                .Where(x => !string.IsNullOrEmpty(x.Id))
                .Select(x => ExtractNumber(x.Id))
                .DefaultIfEmpty(0)
                .Max();

            return $"SEASON_{(max + 1):000}";
        }

        private int ExtractNumber(string id)
        {
            var digits = new string(id.Where(char.IsDigit).ToArray());
            return int.TryParse(digits, out int number) ? number : 0;
        }

        private bool ValidateSeasonIds(List<BattlePassSeasonConfig> list)
            => list.Select(x => x.Id).Distinct().Count() == list.Count;
    }
}