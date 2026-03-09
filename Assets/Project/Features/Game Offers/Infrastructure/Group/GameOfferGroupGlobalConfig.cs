using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

/// <summary>
/// Global config chứa toàn bộ Offer Groups trong game.
/// Designer chỉnh sửa trong Inspector.
/// </summary>
[CreateAssetMenu(
    fileName = "GameOfferGroupGlobalConfig",
    menuName = "GlobalConfigs/GameOfferGroupGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class GameOfferGroupGlobalConfig : GlobalConfig<GameOfferGroupGlobalConfig>
{
    [Title("GAME OFFER GROUP CONFIG")]

    [Searchable]

    [TableList(
        AlwaysExpanded = true,
        ShowIndexLabels = true,
        DrawScrollView = true)]

    public List<GameOfferGroupConfigData> Groups = new();

    private Dictionary<string, GameOfferGroupConfigData> lookup;

    /// <summary>
    /// Lấy config theo Id
    /// </summary>
    public GameOfferGroupConfigData Get(string id)
    {
        if (lookup == null)
            BuildLookup();

        lookup.TryGetValue(id, out var result);

        return result;
    }

    /// <summary>
    /// Lấy toàn bộ groups
    /// </summary>
    public IReadOnlyList<GameOfferGroupConfigData> GetAll()
    {
        return Groups;
    }

    /// <summary>
    /// Build dictionary để lookup nhanh O(1)
    /// </summary>
    private void BuildLookup()
    {
        lookup = Groups
            .Where(x => !string.IsNullOrEmpty(x.Id))
            .ToDictionary(x => x.Id, x => x);
    }
}