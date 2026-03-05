using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.OdinInspector.Editor;
using Gley.EasyIAP;

[CreateAssetMenu(fileName = "StoreItemsGlobalConfig", menuName = "GlobalConfigs/StoreItemsGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class StoreItemsGlobalConfig : GlobalConfig<StoreItemsGlobalConfig>
{
    [Title("STORE ITEM LIST")]
    [TableList(AlwaysExpanded = true, ShowIndexLabels = true)]
    [Searchable]
    [OnCollectionChanged(nameof(OnStoreItemListChanged))]
    [ValidateInput(nameof(ValidateStoreItemIds), "Store Item Id bị trùng!")]
    public List<StoreItemConfigData> Items = new();

    // =========================
    // AUTO ID
    // =========================

    private void OnStoreItemListChanged(CollectionChangeInfo info)
    {
        if (info.ChangeType == CollectionChangeType.Add)
        {
            var newItem = info.Value as StoreItemConfigData;
            if (newItem != null && string.IsNullOrEmpty(newItem.Id))
            {
                newItem.Id = GenerateNextStoreItemId();
            }
        }
    }

    private string GenerateNextStoreItemId()
    {
        int max = Items
            .Where(x => !string.IsNullOrEmpty(x.Id))
            .Select(x => ExtractNumber(x.Id))
            .DefaultIfEmpty(0)
            .Max();

        return $"STORE_{(max + 1):000}";
    }

    private int ExtractNumber(string id)
    {
        var digits = new string(id.Where(char.IsDigit).ToArray());
        return int.TryParse(digits, out int number) ? number : 0;
    }

    // =========================
    // VALIDATION
    // =========================

    private bool ValidateStoreItemIds(List<StoreItemConfigData> list)
        => list.Select(x => x.Id).Distinct().Count() == list.Count;

    // =========================
    // DROPDOWN SUPPORT
    // =========================

    public IEnumerable<string> GetAllBundleIds()
    {
        return CurrencyGlobalConfig.Instance.Bundles.Select(x => x.Id);
    }
}