using System;
using Gley.EasyIAP;

namespace IAPModule.Domain
{
    public enum IAPEventType
    {
        PurchaseStart,
        PurchaseResult,
        PurchaseError
    }

    public class IAPEvent
    {
        public IAPEventType Type;
        public ShopProductNames ProductId;
        public PurchaseProductResponseData Result;
        public Exception Exception;

        public static IAPEvent PurchaseStart(ShopProductNames id)
        {
            return new IAPEvent
            {
                Type = IAPEventType.PurchaseStart,
                ProductId = id
            };
        }

        public static IAPEvent PurchaseResult(
            ShopProductNames id,
            PurchaseProductResponseData result)
        {
            return new IAPEvent
            {
                Type = IAPEventType.PurchaseResult,
                ProductId = id,
                Result = result
            };
        }

        public static IAPEvent PurchaseError(
            ShopProductNames id,
            Exception e)
        {
            return new IAPEvent
            {
                Type = IAPEventType.PurchaseError,
                ProductId = id,
                Exception = e
            };
        }
    }
}