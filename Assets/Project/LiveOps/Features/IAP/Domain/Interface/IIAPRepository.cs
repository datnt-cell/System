using Gley.EasyIAP;
using IAPModule.Domain;

namespace IAPModule.Application.Interfaces
{
    /// <summary>
    /// Repository lưu trạng thái IAP
    /// - Product reward
    /// - Payment statistics
    /// </summary>
    public interface IIAPRepository
    {
        // =========================
        // PRODUCT REWARD
        // =========================

        bool HasRewarded(ShopProductNames productId);

        void MarkRewarded(ShopProductNames productId);

        // =========================
        // PAYMENT STATS
        // =========================

        void SaveStats(PaymentState state);

        void LoadStats(PaymentState state);
    }
}