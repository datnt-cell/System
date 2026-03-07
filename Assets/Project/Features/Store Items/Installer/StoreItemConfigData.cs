using UnityEngine;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Gley.EasyIAP;

[Serializable]
public class StoreItemConfigData
{
    // =========================
    // HEADER
    // =========================

    [HorizontalGroup("Header", Width = 70)]
    [PreviewField(60)]
    [HideLabel]
    public Sprite Icon;

    [VerticalGroup("Header/Info")]
    [ReadOnly]
    public string Id;

    [VerticalGroup("Header/Info")]
    public string DisplayName;

    // =========================
    // PRICE
    // =========================

    [BoxGroup("PRICE")]
    public StorePriceType PriceType;

    [BoxGroup("PRICE")]
    [ShowIf(nameof(IsIap))]
    public ShopProductNames ProductId;

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
    [TableList(AlwaysExpanded = true)]
    public List<CurrencyRewardConfig> CustomRewards = new();

    // =========================
    // HELPERS
    // =========================

    private bool IsIap() => PriceType == StorePriceType.IAP;

    private bool UseExistingBundle() => RewardMode == StoreRewardMode.UseExistingBundle;
    private bool UseCustomBundle() => RewardMode == StoreRewardMode.CustomBundle;

    private IEnumerable<string> GetBundleIds()
    {
        return StoreItemsGlobalConfig.Instance.GetAllBundleIds();
    }
}

public enum StorePriceType
{
    Free,
    IAP
}

public enum StoreRewardMode
{
    UseExistingBundle,
    CustomBundle
}