using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.OdinInspector.Editor;

[CreateAssetMenu(fileName = "GameOfferGroupGlobalConfig", menuName = "GlobalConfigs/GameOfferGroupGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/Offers/")]
public class GameOfferGroupGlobalConfig : GlobalConfig<GameOfferGroupGlobalConfig>
{
    [Title("🎁 GAME OFFERS GROUP", bold: true)]

    [TableList(AlwaysExpanded = true)]
    [Searchable]
    [OnCollectionChanged(nameof(OnOfferListChanged))]
    [ValidateInput(nameof(ValidateIds), "Offer Id bị trùng!")]
    public List<GameOfferGroupConfigData> Groups = new();

     /// <summary>
    /// Lấy offer theo ID
    /// </summary>
    public GameOfferGroupConfigData Get(string id)
    {
        return Groups.FirstOrDefault(o => o.Id == id);
    }

    /// <summary>
    /// Lấy tất cả offer
    /// </summary>
    public List<GameOfferGroupConfigData> GetAll()
    {
        return Groups;
    }

    /// <summary>
    /// Khi designer thêm offer mới
    /// → auto generate ID
    /// </summary>
    private void OnOfferListChanged(CollectionChangeInfo info)
    {
        if (info.ChangeType == CollectionChangeType.Add)
        {
            var newItem = info.Value as GameOfferGroupConfigData;

            if (newItem != null && string.IsNullOrEmpty(newItem.Id))
            {
                newItem.Id = GenerateNextId();
            }
        }
    }

    /// <summary>
    /// Generate ID dạng:
    /// GROUP_OFFER_001
    /// GROUP_OFFER_002
    /// </summary>
    private string GenerateNextId()
    {
        int max = Groups
            .Where(x => !string.IsNullOrEmpty(x.Id))
            .Select(x => ExtractNumber(x.Id))
            .DefaultIfEmpty(0)
            .Max();

        return $"GROUP_{(max + 1):000}";
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
    private bool ValidateIds(List<GameOfferGroupConfigData> list)
    {
        return list.Select(x => x.Id).Distinct().Count() == list.Count;
    }
}