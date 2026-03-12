using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class GameOfferConfigData
{
    // =========================
    // BASIC
    // =========================
    [BoxGroup("Info")]
    [ReadOnly]
    [LabelWidth(40)]
    public string Id;

    [BoxGroup("Info")]
    [LabelWidth(100)]
    public string DisplayName;

    // =========================
    // TIME
    // =========================

    [BoxGroup("TIME")]
    [TableColumnWidth(140)]
    [LabelText("Duration (s)")]
    [MinValue(1)]
    public int Duration = 86400;

    [BoxGroup("TIME")]
    [TableColumnWidth(140)]
    [LabelText("Wait Activation")]
    public bool WaitForActivation;


    // =========================
    // STORE
    // =========================

    [BoxGroup("STORE")]
    [TableColumnWidth(180)]
    [LabelText("Store Item")]
    [ValueDropdown(nameof(GetStoreItemIds))]
    public string StoreItemId;

    [BoxGroup("STORE")]
    [TableColumnWidth(120)]
    [LabelText("Purchase Limit")]
    [MinValue(1)]
    public int Limit = 1;


    /// <summary>
    /// Dropdown StoreItemId
    /// </summary>
    private static IEnumerable<string> GetStoreItemIds()
    {
        if (StoreItemsGlobalConfig.Instance == null)
            return new List<string>();

        return StoreItemsGlobalConfig.Instance
            .Items
            .Select(x => x.Id);
    }
}