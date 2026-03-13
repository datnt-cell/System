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
    [BoxGroup("CONFIG")]
    [LabelText("Duration")]
    public UTimeSpan Duration = TimeSpan.FromDays(1);

    [BoxGroup("CONFIG")]
    [LabelText("Wait Activation")]
    public bool WaitForActivation;

    [BoxGroup("CONFIG")]
    [LabelText("Purchase Limit")]
    [MinValue(1)]
    public int Limit = 1;
    // =========================
    // STORE
    // =========================

    [BoxGroup("STORE")]
    [TableColumnWidth(80)]
    [LabelText("Store Item")]
    [ValueDropdown(nameof(GetStoreItemIds))]
    public string StoreItemId;

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