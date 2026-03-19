using System;

namespace CurrencySystem.Application
{
    /// <summary>
    /// Response chi tiết cho mỗi thao tác currency.
    /// </summary>
    public class CurrencyResponse : ResponseData
    {
        /// <summary>
        /// Số dư hiện tại.
        /// </summary>
        public int Balance { get; set; }

        /// <summary>
        /// Lượng thay đổi (dương nếu Add, âm nếu Spend)
        /// </summary>
        public int ChangedAmount { get; set; }

        /// <summary>
        /// Nguồn thao tác.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Factory tạo response thành công.
        /// </summary>
        public static CurrencyResponse CreateSuccess(int balance, int changedAmount, string source = "")
        {
            return new CurrencyResponse
            {
                Success = true,
                Balance = balance,
                ChangedAmount = changedAmount,
                Source = source
            };
        }

        /// <summary>
        /// Factory tạo response lỗi.
        /// </summary>
        public static CurrencyResponse CreateError(Errors code, string message = "")
        {
            return GetErrorResponse<CurrencyResponse>(code, message);
        }
    }
}