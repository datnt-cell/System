using System.Collections.Generic;
using CurrencySystem.Domain;

namespace CurrencySystem.Application
{
    /// <summary>
    /// Response cho từng reward khi mở bundle
    /// </summary>
    public class CurrencyBundleRewardResponse
    {
        public string BundleId { get; set; }
        public CurrencyId CurrencyId { get; set; }
        public int Amount { get; set; }
        public int Balance { get; set; }
        public int ChangedAmount { get; set; }
        public string Source { get; set; }
        public bool Success { get; set; }
        public Errors? ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// Response tổng hợp khi mở bundle
    /// </summary>
    public class CurrencyBundleOpenResponse : ResponseData
    {
        public string BundleId { get; set; }
        public List<CurrencyBundleRewardResponse> Rewards { get; set; } = new();
    }
}