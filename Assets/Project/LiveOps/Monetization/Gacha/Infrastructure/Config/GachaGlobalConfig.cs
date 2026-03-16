using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using Sirenix.Utilities;
using Sirenix.OdinInspector.Editor;

[CreateAssetMenu(
    fileName = "GachaGlobalConfig",
    menuName = "GlobalConfigs/GachaGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/Gacha/")]
public class GachaGlobalConfig : GlobalConfig<GachaGlobalConfig>
{
    [Title("🎰 GACHA POOLS", bold: true)]
    [TableList]
    [Searchable]
    [OnCollectionChanged(nameof(OnPoolListChanged))]
    [ValidateInput(nameof(ValidatePoolIds), "Pool Id bị trùng!")]
    public List<GachaPoolConfigData> Pools = new();

    // =========================
    // AUTO ID
    // =========================

    private void OnPoolListChanged(CollectionChangeInfo info)
    {
        if (info.ChangeType == CollectionChangeType.Add)
        {
            var newItem = info.Value as GachaPoolConfigData;

            if (newItem != null && string.IsNullOrEmpty(newItem.Id))
            {
                newItem.Id = GenerateNextPoolId();
            }
        }
    }

    private string GenerateNextPoolId()
    {
        int max = Pools
            .Where(x => !string.IsNullOrEmpty(x.Id))
            .Select(x => ExtractNumber(x.Id))
            .DefaultIfEmpty(0)
            .Max();

        return $"POOL_{(max + 1):000}";
    }

    private int ExtractNumber(string id)
    {
        var digits = new string(id.Where(char.IsDigit).ToArray());

        return int.TryParse(digits, out int number)
            ? number
            : 0;
    }

    private bool ValidatePoolIds(List<GachaPoolConfigData> list)
        => list.Select(x => x.Id).Distinct().Count() == list.Count;

    // =========================
    // Runtime helper
    // =========================

    public GachaPoolConfigData GetPool(string id)
    {
        return Pools.FirstOrDefault(x => x.Id == id);
    }
}