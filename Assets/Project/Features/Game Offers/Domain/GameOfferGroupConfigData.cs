using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

/// <summary>
/// Dữ liệu config của một Game Offer Group
/// </summary>
[Serializable]
public class GameOfferGroupConfigData
{
    // =========================
    // BASIC
    // =========================

    [BoxGroup("BASIC")]
    [TableColumnWidth(350, Resizable = false)]
    [ReadOnly]
    [LabelText("ID")]
    public string Id;

    [BoxGroup("BASIC")]
    [LabelWidth(100)]
    public string DisplayName;


    [BoxGroup("BASIC")]
    [TableColumnWidth(350)]
    [LabelText("Group Type")]
    public OfferGroupType Type;


    // =========================
    // TIME
    // =========================

    [BoxGroup("TIME")]
    [TableColumnWidth(160)]
    [LabelText("Duration (s)")]
    [MinValue(1)]
    public int Duration = 86400;

    [BoxGroup("TIME")]
    [TableColumnWidth(160)]
    [LabelText("Wait Activation")]
    public bool WaitForActivation;


    // =========================
    // OFFERS
    // =========================

    [BoxGroup("OFFERS")]
    [TableColumnWidth(260)]
    [LabelText("Offer Ids")]
    [ValueDropdown(nameof(GetOfferIds))]
    public List<string> OfferIds = new();

    private static IEnumerable<string> GetOfferIds()
    {
        if (GameOfferGlobalConfig.Instance == null)
            return Enumerable.Empty<string>();

        return GameOfferGlobalConfig.Instance
            .Offers
            .Select(x => x.Id);
    }
}