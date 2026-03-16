using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using CurrencySystem.Domain;
using BattlePassModule.Domain;

[System.Serializable]
[InlineProperty]
public class BattlePassRewardConfig
{
    // =========================
    // TYPE
    // =========================

    [LabelWidth(80)]
    public BattlePassRewardType RewardType;

    // =========================
    // CURRENCY
    // =========================

    [ShowIf(nameof(IsCurrency))]
    [LabelText("Currency")]
    [ValueDropdown(nameof(GetCurrencyIds))]
    public string CurrencyId;

    [ShowIf(nameof(IsCurrency))]
    [LabelText("Amount")]
    [MinValue(1)]
    public int Amount = 1;

    // =========================
    // BUNDLE
    // =========================

    [ShowIf(nameof(IsBundle))]
    [LabelText("Bundle")]
    [ValueDropdown(nameof(GetBundleIds))]
    public string BundleId;

    // =========================
    // CONDITIONS
    // =========================

    private bool IsCurrency()
    {
        return RewardType == BattlePassRewardType.Currency;
    }

    private bool IsBundle()
    {
        return RewardType == BattlePassRewardType.CurrencyBundle;
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