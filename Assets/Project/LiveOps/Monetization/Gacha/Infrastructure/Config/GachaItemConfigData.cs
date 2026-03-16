using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using CurrencySystem.Domain;
using GachaSystem.Domain.Models;

[System.Serializable]
[InlineProperty]
public class GachaItemConfigData
{
    // =========================
    // ROW 1 : REWARD
    // =========================

    [VerticalGroup("RowReward")]
    [LabelText("Type")]
    public GachaRewardType RewardType;

    // CurrencyId

    [VerticalGroup("RowReward")]
    [ShowIf(nameof(IsCurrency))]
    [LabelText("Currency")]
    [ValueDropdown(nameof(GetCurrencyIds))]
    public string CurrencyId;

    // Amount

    [VerticalGroup("RowReward")]
    [ShowIf(nameof(IsCurrency))]
    [LabelText("Amount")]
    [MinValue(1)]
    public int Amount = 1;

    // BundleId

    [VerticalGroup("RowReward")]
    [ShowIf(nameof(IsBundle))]
    [LabelText("Bundle")]
    [ValueDropdown(nameof(GetBundleIds))]
    public string BundleId;

    // =========================
    // ROW 2 : WEIGHT
    // =========================

    [HorizontalGroup("RowWeight", Width = 140)]
    [LabelText("Weight")]
    [MinValue(1)]
    public int Weight = 10;

    // =========================
    // CONDITIONS
    // =========================

    private bool IsCurrency()
    {
        return RewardType == GachaRewardType.Currency;
    }

    private bool IsBundle()
    {
        return RewardType == GachaRewardType.CurrencyBundle;
    }

    // =========================
    // DROPDOWNS
    // =========================

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

    private static IEnumerable<ValueDropdownItem<string>> GetBundleIds()
    {
        if (CurrencyBundleGlobalConfig.Instance == null)
            return Enumerable.Empty<ValueDropdownItem<string>>();

        return CurrencyBundleGlobalConfig.Instance.Bundles
            .Select(b =>
            {
                string label = string.IsNullOrEmpty(b.DisplayName)
                    ? b.Id
                    : b.DisplayName;

                return new ValueDropdownItem<string>(label, b.Id);
            });
    }
}