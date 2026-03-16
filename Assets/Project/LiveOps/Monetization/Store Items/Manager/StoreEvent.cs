namespace StoreSystem.Domain
{
    public enum StoreEventType
    {
        PurchaseStart,
        PurchaseResult,
        PurchaseError
    }

    public class StoreEvent
    {
        public StoreEventType Type;
        public string ItemId;
        public PurchaseProductResponseData Result;
        public System.Exception Exception;

        public static StoreEvent PurchaseStart(string id)
        {
            return new StoreEvent
            {
                Type = StoreEventType.PurchaseStart,
                ItemId = id
            };
        }

        public static StoreEvent PurchaseResult(string id, PurchaseProductResponseData result)
        {
            return new StoreEvent
            {
                Type = StoreEventType.PurchaseResult,
                ItemId = id,
                Result = result
            };
        }

        public static StoreEvent PurchaseError(string id, System.Exception e)
        {
            return new StoreEvent
            {
                Type = StoreEventType.PurchaseError,
                ItemId = id,
                Exception = e
            };
        }
    }
}