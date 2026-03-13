
using System;
using Newtonsoft.Json;

[Serializable]
public class PurchaseProductResponseData : ResponseData
{
    [JsonIgnore]
    private string product_id;

    [JsonIgnore]
    public bool PurchasedOnClient;

    [JsonIgnore]
    public string Receipt;

    [JsonIgnore]
    public string ProductId
    {
        get
        {
            return product_id;
        }
        set
        {
            product_id = value;
        }
    }
}