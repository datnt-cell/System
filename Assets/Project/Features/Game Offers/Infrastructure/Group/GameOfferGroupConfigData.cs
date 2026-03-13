using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UniLabs.Time;

/// <summary>
/// Dữ liệu config của một Game Offer Group
/// </summary>
[Serializable]
public class GameOfferGroupConfigData
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

    [BoxGroup("Info")]
    [TableColumnWidth(40)]
    [LabelText("Group Type")]
    public OfferGroupType Type;

    [BoxGroup("CONFIG")]
    [LabelText("Duration")]
    public UTimeSpan Duration = TimeSpan.FromDays(1);

    [BoxGroup("CONFIG")]
    [LabelText("Wait Activation")]
    public bool WaitForActivation;

    // =========================
    // OFFERS
    // =========================

    [BoxGroup("OFFERS")]
    [TableColumnWidth(80)]
    [LabelText("Offer Ids")]
    [ValueDropdown(nameof(GetOfferIds))]
    public List<string> OfferIds = new();

    // =========================
    // TIME
    // =========================
    private static IEnumerable<ValueDropdownItem<string>> GetOfferIds()
    {
        if (GameOfferGlobalConfig.Instance == null)
            return Enumerable.Empty<ValueDropdownItem<string>>();

        return GameOfferGlobalConfig.Instance
            .Offers
            .Select(offer =>
            {
                string label = string.IsNullOrEmpty(offer.DisplayName)
                    ? offer.Id
                    : offer.DisplayName;

                return new ValueDropdownItem<string>(label, offer.Id);
            });
    }
}