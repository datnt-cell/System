using Gley.EasyIAP;
using IAPModule.Application.Interfaces;

namespace IAPModule.Domain.Policies
{
    /// <summary>
    /// Policy đơn giản:
    /// - Nếu đã reward rồi thì không cho reward nữa (Non-consumable)
    /// - Nếu chưa reward thì cho phép
    /// </summary>
    /// 
    public class SimpleRewardPolicy : IRewardPolicy
    {
        private readonly IIAPRepository _repository;

        public SimpleRewardPolicy(IIAPRepository repository)
        {
            _repository = repository;
        }

        public bool CanGrant(ShopProductNames productId)
        {
            // Nếu đã reward rồi thì không cho nhận nữa
            return !_repository.HasRewarded(productId);
        }

        public void MarkGranted(ShopProductNames productId)
        {
            // Đánh dấu đã nhận reward
            _repository.MarkRewarded(productId);
        }
    }
}