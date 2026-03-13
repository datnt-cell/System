using UnityEngine;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Gley.EasyIAP;
using System.Linq;

[Serializable]
public class StoreItemConfigData
{
    [BoxGroup("Info")]
    [ReadOnly]
    [LabelWidth(40)]
    public string Id;

    [BoxGroup("Info")]
    [LabelWidth(100)]
    public string DisplayName;

    // =========================
    // PRICE
    // =========================

    [BoxGroup("PRICE")]
    public StorePriceType PriceType;

    [BoxGroup("PRICE")]
    [ShowIf(nameof(IsIap))]
    public ShopProductNames ProductId;

    [BoxGroup("PRICE")]
    [ShowIf(nameof(IsCurrency))]
    [ValidateInput(nameof(ValidateCurrencyPrices), "Currency bị trùng!")]
    public List<CurrencyRewardConfig> CurrencyPrices;

    // =========================
    // REWARD MODE
    // =========================

    [BoxGroup("REWARD")]
    public StoreRewardMode RewardMode;

    // ===== Option 1: Dùng bundle có sẵn =====

    [BoxGroup("REWARD")]
    [ShowIf(nameof(UseExistingBundle))]
    [ValueDropdown(nameof(GetBundleIds))]
    public string RewardBundleId;

    // ===== Option 2: Tạo bundle riêng =====

    [BoxGroup("REWARD")]
    [ShowIf(nameof(UseCustomBundle))]
    [TableList(AlwaysExpanded = false)]
    [LabelText("Currencies")]
    [ValidateInput(nameof(ValidateRewards), "Currency bị trùng!")]
    public List<CurrencyRewardConfig> CustomRewards = new();

    private bool ValidateRewards(List<CurrencyRewardConfig> list)
    {
        return list
            .Where(x => !string.IsNullOrEmpty(x.CurrencyId))
            .Select(x => x.CurrencyId)
            .Distinct()
            .Count() == list.Count;
    }

    private bool ValidateCurrencyPrices(List<CurrencyRewardConfig> list)
    {
        return list
            .Where(x => !string.IsNullOrEmpty(x.CurrencyId))
            .Select(x => x.CurrencyId)
            .Distinct()
            .Count() == list.Count;
    }
    // =========================
    // HELPERS
    // =========================

    private bool IsIap() => PriceType == StorePriceType.IAP;
    private bool UseExistingBundle() => RewardMode == StoreRewardMode.UseExistingBundle;
    private bool UseCustomBundle() => RewardMode == StoreRewardMode.CustomBundle;
    private bool IsCurrency() => PriceType == StorePriceType.Currency;

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

public enum StorePriceType
{
    Free,
    IAP,
    Currency
}

public enum StoreRewardMode
{
    UseExistingBundle,
    CustomBundle
}