using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using Sirenix.Utilities;
using Sirenix.OdinInspector.Editor;

[CreateAssetMenu(fileName = "CurrencyBundleGlobalConfig", menuName = "GlobalConfigs/CurrencyBundleGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class CurrencyBundleGlobalConfig : GlobalConfig<CurrencyBundleGlobalConfig>
{
    [Title("🎁 BUNDLE LIST", bold: true)]
    [TableList(AlwaysExpanded = true)]
    [Searchable]
    [OnCollectionChanged(nameof(OnBundleListChanged))]
    [ValidateInput(nameof(ValidateBundleIds), "Bundle Id bị trùng!")]
    public List<CurrencyBundleConfigData> Bundles = new();

    // ===== AUTO ID =====

    private void OnBundleListChanged(CollectionChangeInfo info)
    {
        if (info.ChangeType == CollectionChangeType.Add)
        {
            var newItem = info.Value as CurrencyBundleConfigData;
            if (newItem != null && string.IsNullOrEmpty(newItem.Id))
            {
                newItem.Id = GenerateNextBundleId();
            }
        }
    }

    private string GenerateNextBundleId()
    {
        int max = Bundles
            .Where(x => !string.IsNullOrEmpty(x.Id))
            .Select(x => ExtractNumber(x.Id))
            .DefaultIfEmpty(0)
            .Max();

        return $"BUNDLE_{(max + 1):000}";
    }

    private int ExtractNumber(string id)
    {
        var digits = new string(id.Where(char.IsDigit).ToArray());
        return int.TryParse(digits, out int number) ? number : 0;
    }

    private bool ValidateBundleIds(List<CurrencyBundleConfigData> list)
        => list.Select(x => x.Id).Distinct().Count() == list.Count;
}

[System.Serializable]
public class CurrencyBundleConfigData
{
    [HorizontalGroup("Row")]
    [ReadOnly]
    public string Id;

    [TableList(AlwaysExpanded = true)]
    public List<CurrencyRewardConfig> Rewards = new();
}

[System.Serializable]
public class CurrencyRewardConfig
{
    [ValueDropdown(nameof(GetCurrencyIds))]
    public string CurrencyId;

    [MinValue(1)]
    public int Amount;

    private IEnumerable<string> GetCurrencyIds()
    {
        return CurrencyGlobalConfig.Instance.GetAllCurrencyIds();
    }
}
