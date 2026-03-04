using Balancy.Models;
using Balancy.Models.SmartObjects;
using CurrencySystem.Domain;

namespace CurrencySystem.Infrastructure
{
    /// <summary>
    /// Triển khai metadata provider bằng Balancy DataEditor.
    /// 
    /// Đây là nơi DUY NHẤT được phép gọi:
    /// Balancy.DataEditor
    /// 
    /// Nếu sau này bỏ Balancy, chỉ cần thay class này.
    /// </summary>
    public class BalancyCurrencyMetadataProvider
        : ICurrencyMetadataProvider
    {
        public bool Exists(CurrencyId id)
        {
            // CurrencyId.Value phải trùng với UnnyId trong Balancy
            return Balancy.DataEditor
                .GetModelByUnnyId<Item>(id.Value) != null;
        }

        public int GetMaxStack(CurrencyId id)
        {
            var item = Balancy.DataEditor
                .GetModelByUnnyId<Item>(id.Value);

            // Nếu không có config thì cho phép vô hạn
            return item?.MaxStack ?? int.MaxValue;
        }

        public string GetDisplayName(CurrencyId id)
        {
            var item = Balancy.DataEditor
                .GetModelByUnnyId<Item>(id.Value);

            return item?.Name.ToString() ?? id.Value;
        }
    }
}