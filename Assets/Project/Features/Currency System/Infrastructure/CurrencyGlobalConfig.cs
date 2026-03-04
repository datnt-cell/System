using UnityEngine;
using System.Collections.Generic;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
using System.Linq;
using Sirenix.OdinInspector.Editor;

[CreateAssetMenu(fileName = "CurrencyGlobalConfig", menuName = "GlobalConfigs/CurrencyGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class CurrencyGlobalConfig : GlobalConfig<CurrencyGlobalConfig>
{
    [Title("CURRENCY LIST")]
    [TableList(AlwaysExpanded = true)]
    [Searchable]
    [OnCollectionChanged(nameof(OnCurrencyListChanged))]
    public List<CurrencyConfigData> Currencies = new();

    private void OnCurrencyListChanged(CollectionChangeInfo info)
    {
        if (info.ChangeType == CollectionChangeType.Add)
        {
            var newItem = info.Value as CurrencyConfigData;
            if (newItem != null && string.IsNullOrEmpty(newItem.Id))
            {
                newItem.Id = GenerateNextId();
            }
        }
    }

    private string GenerateNextId()
    {
        int maxNumber = 0;

        foreach (var currency in Currencies)
        {
            if (string.IsNullOrEmpty(currency.Id))
                continue;

            // Lấy phần số phía sau
            var digits = new string(currency.Id.Where(char.IsDigit).ToArray());

            if (int.TryParse(digits, out int number))
            {
                if (number > maxNumber)
                    maxNumber = number;
            }
        }

        return $"CURRENCY_{maxNumber + 1}";
    }
}

[System.Serializable]
public class CurrencyConfigData
{
    [HorizontalGroup("Row", Width = 90)]
    [PreviewField(60)]
    [HideLabel]
    public Sprite Icon;

    [VerticalGroup("Row/Info")]
    [Required]
    public string Id;

    [VerticalGroup("Row/Info")]
    public string DisplayName;

    [HorizontalGroup("Row", Width = 120)]
    [MinValue(0)]
    public int MaxStack = int.MaxValue;
}