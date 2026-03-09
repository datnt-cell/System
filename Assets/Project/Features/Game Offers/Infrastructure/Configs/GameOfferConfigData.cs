using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Dữ liệu config của một Game Offer
/// </summary>
[Serializable]
public class GameOfferConfigData
{
    [TableColumnWidth(140, Resizable = false)]
    [ReadOnly]
    [LabelText("ID")]
    public string Id;

    [TableColumnWidth(110)]
    [LabelText("Duration (s)")]
    [MinValue(1)]
    public int Duration = 86400;

    [TableColumnWidth(160)]
    [LabelText("Store Item")]
    [ValueDropdown(nameof(GetStoreItemIds))]
    public string StoreItemId;

    [TableColumnWidth(110)]
    [LabelText("Purchase Limit")]
    [MinValue(1)]
    public int Limit = 1;

    [TableColumnWidth(110)]
    [LabelText("Wait Activation")]
    public bool WaitForActivation;

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