/// <summary>
/// Các loại Offer Group
/// </summary>
public enum OfferGroupType
{
    /// <summary>
    /// Không giới hạn số lần mua
    /// </summary>
    UnlimitedPurchases,

    /// <summary>
    /// Chuỗi offer mua tuần tự
    /// </summary>
    ChainDeals,

    /// <summary>
    /// Chỉ được mua 1 offer trong group
    /// </summary>
    OnlyOnePurchase,

    /// <summary>
    /// Mỗi offer mua được 1 lần
    /// </summary>
    PurchaseEachOfferOnce
}