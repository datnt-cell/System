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

    // ===== AUTO ID =====

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

    private int ExtractNumber(string id)
    {
        var digits = new string(id.Where(char.IsDigit).ToArray());
        return int.TryParse(digits, out int number) ? number : 0;
    }

    private bool ValidateCurrencyIds(List<CurrencyConfigData> list)
        => list.Select(x => x.Id).Distinct().Count() == list.Count;

    public IEnumerable<string> GetAllCurrencyIds()
    {
        return Currencies.Select(x => x.Id);
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