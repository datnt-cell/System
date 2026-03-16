using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.OdinInspector.Editor;

/// <summary>
/// Global config chứa toàn bộ Game Offers
/// Dùng để config các personalised offers cho LiveOps
/// </summary>
[CreateAssetMenu(fileName = "GameOfferGlobalConfig", menuName = "GlobalConfigs/GameOfferGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/Offers/")]
public class GameOfferGlobalConfig : GlobalConfig<GameOfferGlobalConfig>
{
    // =========================
    // HEADER HELP
    // =========================

    [PropertyOrder(-10)]
    [HorizontalGroup("OfferHeader", Width = 28)]
    [Button("ℹ", ButtonSizes.Small)]
    [PropertyTooltip("@GetConfigNote()")]
    private void ShowConfigInfo()
    {
        Sirenix.Utilities.Editor.SirenixEditorGUI.MessageBox(GetConfigNote());
    }

    [PropertyOrder(-11)]
    [Title("🎁 GAME OFFERS", bold: true)]
    [HorizontalGroup("OfferHeader/Title")]
    [HideLabel]
    [ShowInInspector]
    private string HeaderSpacer => "";

    private string GetConfigNote()
    {
        return
            "GAME OFFER CONFIG\n\n" +

            "Duration\n" +
            "• Thời gian offer tồn tại sau khi kích hoạt.\n" +
            "• 00:00:00 → Infinity (không hết hạn).\n" +
            "• Ví dụ: 1 day → Offer tồn tại 24h.\n\n" +

            "Wait For Activation\n" +
            "• FALSE → Offer active ngay khi spawn.\n" +
            "• TRUE → Offer chỉ active khi có trigger (fail level, complete level...).\n\n" +

            "Purchase Limit\n" +
            "• Số lần player có thể mua offer.\n" +
            "• Ví dụ: Limit = 1 → chỉ mua 1 lần.";
    }

    // =========================
    // DATA
    // =========================

    [PropertyOrder(0)]
    [TableList]
    [Searchable]
    [OnCollectionChanged(nameof(OnOfferListChanged))]
    [ValidateInput(nameof(ValidateIds), "Offer Id bị trùng!")]
    public List<GameOfferConfigData> Offers = new();

    /// <summary>
    /// Lấy offer theo ID
    /// </summary>
    public GameOfferConfigData Get(string id)
    {
        return Offers.FirstOrDefault(o => o.Id == id);
    }

    /// <summary>
    /// Lấy tất cả offer
    /// </summary>
    public List<GameOfferConfigData> GetAll()
    {
        return Offers;
    }

    /// <summary>
    /// Khi designer thêm offer mới
    /// → auto generate ID
    /// </summary>
    private void OnOfferListChanged(CollectionChangeInfo info)
    {
        if (info.ChangeType == CollectionChangeType.Add)
        {
            var newItem = info.Value as GameOfferConfigData;

            if (newItem != null && string.IsNullOrEmpty(newItem.Id))
            {
                newItem.Id = GenerateNextId();
            }
        }
    }

    /// <summary>
    /// Generate ID dạng:
    /// OFFER_001
    /// OFFER_002
    /// </summary>
    private string GenerateNextId()
    {
        int max = Offers
            .Where(x => !string.IsNullOrEmpty(x.Id))
            .Select(x => ExtractNumber(x.Id))
            .DefaultIfEmpty(0)
            .Max();

        return $"OFFER_{(max + 1):000}";
    }

    /// <summary>
    /// Extract số từ ID
    /// OFFER_012 -> 12
    /// </summary>
    private int ExtractNumber(string id)
    {
        var digits = new string(id.Where(char.IsDigit).ToArray());

        return int.TryParse(digits, out int number)
            ? number
            : 0;
    }

    /// <summary>
    /// Validate không cho trùng ID
    /// </summary>
    private bool ValidateIds(List<GameOfferConfigData> list)
    {
        return list.Select(x => x.Id).Distinct().Count() == list.Count;
    }
}