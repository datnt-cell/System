using Gley.EasyIAP;
using IAPModule.Application.Interfaces;
using IAPModule.Domain;

namespace IAPModule.Infrastructure.Repositories
{
    /// <summary>
    /// Repository lưu trạng thái IAP bằng Easy Save.
    /// File lưu: IAP_Save.es3
    /// </summary>
    public class EasySaveIAPRepository : IIAPRepository
    {
        const string FILE_NAME = "IAP_Save.es3";

        const string KEY_TOTAL_SPEND = "iap_total_spend";
        const string KEY_PAYMENT_COUNT = "iap_payment_count";
        const string KEY_MAX_PAYMENT = "iap_max_payment";
        const string KEY_LAST_PAYMENT = "iap_last_payment";

        /// <summary>
        /// Kiểm tra product đã nhận reward chưa
        /// </summary>
        public bool HasRewarded(ShopProductNames productId)
        {
            return ES3.Load(GetKey(productId), FILE_NAME, false);
        }

        /// <summary>
        /// Đánh dấu product đã nhận reward
        /// </summary>
        public void MarkRewarded(ShopProductNames productId)
        {
            ES3.Save(GetKey(productId), true, FILE_NAME);
        }

        /// <summary>
        /// Tạo key lưu trữ cho product
        /// </summary>
        string GetKey(ShopProductNames productId)
        {
            return $"IAP_Rewarded_{productId}";
        }

        public void SaveStats(PaymentState state)
        {
            ES3.Save(KEY_TOTAL_SPEND, state.TotalSpend.Value, FILE_NAME);
            ES3.Save(KEY_PAYMENT_COUNT, state.PaymentsCount.Value, FILE_NAME);
            ES3.Save(KEY_MAX_PAYMENT, state.MaxPayment.Value, FILE_NAME);
            ES3.Save(KEY_LAST_PAYMENT, state.LastPaymentTime.Value, FILE_NAME);
        }

        public void LoadStats(PaymentState state)
        {
            state.TotalSpend.Value =
                ES3.Load(KEY_TOTAL_SPEND, FILE_NAME, 0f);

            state.PaymentsCount.Value =
                ES3.Load(KEY_PAYMENT_COUNT, FILE_NAME, 0);

            state.MaxPayment.Value =
                ES3.Load(KEY_MAX_PAYMENT, FILE_NAME, 0f);

            state.LastPaymentTime.Value =
                ES3.Load(KEY_LAST_PAYMENT, FILE_NAME, 0L);
        }
    }
}