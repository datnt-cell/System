using System;
using Newtonsoft.Json;

[Serializable]
public class PurchaseProductResponseData : ResponseData
{
    [JsonProperty("productId")]
    public string ProductId;

    [JsonIgnore]
    public bool PurchasedOnClient;

    [JsonProperty("price")]
    public float Price;

    [JsonProperty("currency")]
    public string Currency;

    [JsonProperty("localizedPrice")]
    public string LocalizedPrice;

    [JsonIgnore]
    public string Receipt;

    [JsonIgnore]
    public int RewardValue;

    // =========================
    // FACTORY (OPTIONAL)
    // =========================

    public static PurchaseProductResponseData CreateSuccess(string productId)
    {
        var res = GetSuccessResponse<PurchaseProductResponseData>();
        res.ProductId = productId;
        return res;
    }

    public static PurchaseProductResponseData CreateError(Errors code, string message = "")
    {
        return GetErrorResponse<PurchaseProductResponseData>(code, message);
    }

    public static PurchaseProductResponseData CreateError(ResponseData data)
    {
        return GetErrorResponse<PurchaseProductResponseData>(data);
    }
}