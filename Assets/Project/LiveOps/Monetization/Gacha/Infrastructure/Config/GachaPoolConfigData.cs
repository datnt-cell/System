using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using GachaSystem.Domain.Models;

[System.Serializable]
public class GachaPoolConfigData
{
    // =========================
    // INFO
    // =========================

    [BoxGroup("POOL")]
    [ReadOnly]
    [LabelWidth(40)]
    public string Id;

    [BoxGroup("POOL")]
    [LabelWidth(100)]
    public string DisplayName;

    // =========================
    // ITEMS
    // =========================

    [BoxGroup("ITEMS")]
    [TableList]
    [ValidateInput(nameof(ValidateItems), "Reward bị trùng!")]
    public List<GachaItemConfigData> Items = new();

    // =========================
    // VALIDATE
    // =========================

    private bool ValidateItems(List<GachaItemConfigData> list)
    {
        return list
            .Select(GetRewardKey)
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct()
            .Count() == list.Count;
    }

    private string GetRewardKey(GachaItemConfigData item)
    {
        if (item.RewardType == GachaRewardType.Currency)
            return $"C_{item.CurrencyId}";

        if (item.RewardType == GachaRewardType.CurrencyBundle)
            return $"B_{item.BundleId}";

        return null;
    }

    // =========================
    // WEIGHT
    // =========================

    [ShowInInspector]
    [ReadOnly]
    [BoxGroup("POOL")]
    public int TotalWeight => Items.Sum(x => x.Weight);

    // =========================
    // DROP RATE PREVIEW
    // =========================

    [TableList]
    [ShowInInspector]
    [ReadOnly]
    [BoxGroup("PREVIEW")]
    private List<GachaDropPreview> DropRates
    {
        get
        {
            int total = TotalWeight;

            if (total == 0)
                return new List<GachaDropPreview>();

            return Items.Select(x =>
            {
                float percent = (float)x.Weight / total * 100f;

                return new GachaDropPreview
                {
                    Reward = GetRewardKey(x),
                    Weight = x.Weight,
                    Percent = percent
                };

            }).ToList();
        }
    }
}

[System.Serializable]
public class GachaDropPreview
{
    [LabelWidth(100)]
    public string Reward;

    [LabelWidth(60)]
    public int Weight;

    [LabelWidth(60)]
    [SuffixLabel("%")]
    public float Percent;
}