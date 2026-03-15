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
}