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
    [BoxGroup("Info")]
    [ReadOnly]
    [LabelWidth(40)]
    public string Id;

    [BoxGroup("Info")]
    [LabelWidth(100)]
    public string DisplayName;

    [HorizontalGroup("Info/TypeRow")]
    [BoxGroup("Info/TypeRow/Type")]
    [LabelText("Group Type")]
    public OfferGroupType Type;

    [HorizontalGroup("Info/TypeRow", Width = 28)]
    [Button("ℹ", ButtonSizes.Small)]
    [PropertyTooltip("@GetTypeNote()")]
    private void ShowTypeInfo()
    {
        Sirenix.Utilities.Editor.SirenixEditorGUI.MessageBox(GetTypeNote());
    }

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
        if (StoreItemsGlobalConfig.Instance == null)
            return Enumerable.Empty<ValueDropdownItem<string>>();

        return StoreItemsGlobalConfig.Instance
            .Items
            .Select(offer =>
            {
                string label = string.IsNullOrEmpty(offer.DisplayName)
                    ? offer.Id
                    : offer.DisplayName;

                return new ValueDropdownItem<string>(label, offer.Id);
            });
    }

    private string GetTypeNote()
    {
        return
            "OFFER GROUP TYPE\n\n" +

            "UnlimitedPurchases\n" +
            "• Player có thể mua offer nhiều lần.\n" +
            "• Không giới hạn số lần mua.\n\n" +

            "ChainDeals\n" +
            "• Offer xuất hiện theo thứ tự.\n" +
            "• Mua xong offer hiện tại → mở offer tiếp theo.\n\n" +

            "OnlyOnePurchase\n" +
            "• Player chỉ được mua 1 offer trong group.\n" +
            "• Sau khi mua 1 offer → các offer khác sẽ bị khóa.\n\n" +

            "PurchaseEachOfferOnce\n" +
            "• Mỗi offer chỉ mua được 1 lần.\n" +
            "• Player có thể mua tất cả offer trong group.";
    }
}