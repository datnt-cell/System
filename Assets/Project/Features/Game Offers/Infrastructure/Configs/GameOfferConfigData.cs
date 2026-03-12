using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Serialization;
using UniLabs.Time;

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
    [LabelText("Duration")]
    public UTimeSpan Duration = TimeSpan.FromDays(1);

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
    private static IEnumerable<ValueDropdownItem<string>> GetStoreItemIds()
    {
        if (StoreItemsGlobalConfig.Instance == null)
            return Enumerable.Empty<ValueDropdownItem<string>>();

        return StoreItemsGlobalConfig.Instance.Items
            .Select(item =>
            {
                string label = string.IsNullOrEmpty(item.DisplayName)
                    ? item.Id
                    : item.DisplayName;

                return new ValueDropdownItem<string>(label, item.Id);
            });
    }
}