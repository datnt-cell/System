using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using Sirenix.Utilities;
using Sirenix.OdinInspector.Editor;

[CreateAssetMenu(fileName = "CurrencyGlobalConfig", menuName = "GlobalConfigs/CurrencyGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/Items/")]
public class CurrencyGlobalConfig : GlobalConfig<CurrencyGlobalConfig>
{
    [Title("💰 CURRENCY LIST", bold: true)]
    [TableList(AlwaysExpanded = false)]
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
    // =========================
    // INFO
    // =========================

    [HorizontalGroup("Content")]
    [BoxGroup("Content/INFO")]
    [HorizontalGroup("Content/INFO/Split", Width = 70)]
    [PreviewField(60)]
    [HideLabel]
    public Sprite Icon;

    [HorizontalGroup("Content")]
    [BoxGroup("Content/INFO")]
    [VerticalGroup("Content/INFO/Split/Fields")]
    [ReadOnly]
    public string Id;

    [HorizontalGroup("Content")]
    [BoxGroup("Content/INFO")]
    [VerticalGroup("Content/INFO/Split/Fields")]
    public string DisplayName;

    [HorizontalGroup("Content")]
    [BoxGroup("Content/INFO")]
    [VerticalGroup("Content/INFO/Split/Fields")]
    public ConfigType Type;


    // =========================
    // STACK
    // =========================

    [HorizontalGroup("Content", Width = 300)]
    [BoxGroup("Content/STACK")]
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