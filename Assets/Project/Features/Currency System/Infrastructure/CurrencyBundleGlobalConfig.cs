using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using Sirenix.Utilities;
using Sirenix.OdinInspector.Editor;
using CurrencySystem.Domain;

[CreateAssetMenu(fileName = "CurrencyBundleGlobalConfig", menuName = "GlobalConfigs/CurrencyBundleGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/Items/")]
public class CurrencyBundleGlobalConfig : GlobalConfig<CurrencyBundleGlobalConfig>
{
    [Title("🎁 BUNDLE LIST", bold: true)]
    [TableList()]
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
    // =========================
    // INFO
    // =========================
    [BoxGroup("BUNDLE")]
    [ReadOnly]
    [LabelWidth(40)]
    public string Id;

    [BoxGroup("BUNDLE")]
    [LabelWidth(100)]
    public string DisplayName;

    // =========================
    // REWARDS
    // =========================

    [BoxGroup("REWARDS")]
    [TableList(AlwaysExpanded = false)]
    [LabelText("Currencies")]
    [ValidateInput(nameof(ValidateRewards), "Currency bị trùng!")]
    public List<CurrencyRewardConfig> Rewards = new();

    private bool ValidateRewards(List<CurrencyRewardConfig> list)
    {
        return list
            .Where(x => !string.IsNullOrEmpty(x.CurrencyId))
            .Select(x => x.CurrencyId)
            .Distinct()
            .Count() == list.Count;
    }
}

[System.Serializable]
public class CurrencyRewardConfig
{
    [HorizontalGroup("Row")]
    [LabelWidth(70)]
    [ValueDropdown(nameof(GetCurrencyIds))]
    public string CurrencyId;

    [HorizontalGroup("Row", Width = 120)]
    [LabelWidth(50)]
    [MinValue(1)]
    public int Amount = 1;

    private static IEnumerable<ValueDropdownItem<string>> GetCurrencyIds()
    {
        if (CurrencyGlobalConfig.Instance == null)
            return Enumerable.Empty<ValueDropdownItem<string>>();

        return CurrencyGlobalConfig.Instance.Currencies
            .Select(c =>
            {
                string label = string.IsNullOrEmpty(c.DisplayName)
                    ? c.Id
                    : c.DisplayName;

                return new ValueDropdownItem<string>(label, c.Id);
            });
    }

    public CurrencyId GetCurrencyId()
    {
        return new CurrencyId(CurrencyId);
    }
}