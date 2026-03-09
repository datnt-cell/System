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
    [TableColumnWidth(140, Resizable = false)]
    [ReadOnly]
    [LabelText("ID")]
    public string Id;

    [TableColumnWidth(150)]
    [LabelText("Group Type")]
    public OfferGroupType Type;

    [TableColumnWidth(110)]
    [LabelText("Duration (s)")]
    [MinValue(1)]
    public int Duration = 86400;

    [TableColumnWidth(120)]
    [LabelText("Wait Activation")]
    public bool WaitForActivation;

    [TableColumnWidth(260)]
    [LabelText("Offer Ids")]
    [ValueDropdown(nameof(GetOfferIds))]
    public List<string> OfferIds = new();

    /// <summary>
    /// Dropdown GameOfferId
    /// </summary>
    private static IEnumerable<string> GetOfferIds()
    {
        if (GameOfferGlobalConfig.Instance == null)
            return Enumerable.Empty<string>();

        return GameOfferGlobalConfig.Instance
            .Offers
            .Select(x => x.Id);
    }
}