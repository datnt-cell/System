using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using Sirenix.Utilities;
using Sirenix.OdinInspector.Editor;

[CreateAssetMenu(fileName = "CurrencyGlobalConfig", menuName = "GlobalConfigs/CurrencyGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class CurrencyGlobalConfig : GlobalConfig<CurrencyGlobalConfig>
{
    [Title("💰 CURRENCY LIST", bold: true)]
    [TableList(AlwaysExpanded = true)]
    [Searchable]
    [OnCollectionChanged(nameof(OnCurrencyListChanged))]
    [ValidateInput(nameof(ValidateCurrencyIds), "Currency Id bị trùng!")]
    public List<CurrencyConfigData> Currencies = new();

    [Space(20)]

    [Title("🎁 BUNDLE LIST", bold: true)]
    [TableList(AlwaysExpanded = true)]
    [Searchable]
    [OnCollectionChanged(nameof(OnBundleListChanged))]
    [ValidateInput(nameof(ValidateBundleIds), "Bundle Id bị trùng!")]
    public List<CurrencyBundleConfigData> Bundles = new();

    // ===== AUTO ID CURRENCY =====

    private void OnCurrencyListChanged(CollectionChangeInfo info)
    {
        if (info.ChangeType == CollectionChangeType.Add)
        {
            var newItem = info.Value as CurrencyConfigData;
            if (newItem != null && string.IsNullOrEmpty(newItem.Id))
            {
                newItem.Id = GenerateNextCurrencyId();
            }
        }
    }

    private string GenerateNextCurrencyId()
    {
        int max = Currencies
            .Where(x => !string.IsNullOrEmpty(x.Id))
            .Select(x => ExtractNumber(x.Id))
            .DefaultIfEmpty(0)
            .Max();

        return $"CUR_{(max + 1):000}";
    }

    // ===== AUTO ID BUNDLE =====

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

    // ===== VALIDATION =====

    private bool ValidateCurrencyIds(List<CurrencyConfigData> list)
        => list.Select(x => x.Id).Distinct().Count() == list.Count;

    private bool ValidateBundleIds(List<CurrencyBundleConfigData> list)
        => list.Select(x => x.Id).Distinct().Count() == list.Count;

    // ===== DROPDOWN SUPPORT =====

    public IEnumerable<string> GetAllCurrencyIds()
    {
        return Currencies.Select(x => x.Id);
    }
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

[System.Serializable]
public class CurrencyConfigData
{
    [HorizontalGroup("Row", Width = 70)]
    [PreviewField(60)]
    [HideLabel]
    public Sprite Icon;

    // ===== INFO BLOCK =====
    [VerticalGroup("Row/Info")]
    [ReadOnly]
    public string Id;

    [VerticalGroup("Row/Info")]
    public string DisplayName;

    [VerticalGroup("Row/Info")]
    [LabelWidth(40)]
    public ConfigType Type;

    // ===== STACK =====
    [HorizontalGroup("Row", Width = 110)]
    [LabelWidth(70)]
    [MinValue(0)]
    public int MaxStack = 0;
}

public enum ConfigType
{
    Currency,
    Item,
    Event
}