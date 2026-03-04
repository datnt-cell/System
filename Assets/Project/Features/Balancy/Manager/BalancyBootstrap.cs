using Balancy.Models.SmartObjects;
using UnityEngine;

/// <summary>
/// Khởi tạo Balancy khi game start.
/// Tạm thời chưa theo Clean.
/// </summary>
public class BalancyBootstrap : MonoBehaviour
{
    private void Awake()
    {
        Balancy.Main.Init(new Balancy.AppConfig
        {
            ApiGameId = "48b661cc-17af-11f1-a7e8-1fec53a055ba",
            PublicKey = "YWYwM2RhYTZhNmFlOTBjODcwNjY4YW",
            Environment = Balancy.Constants.Environment.Development,
            OnReadyCallback = responseData => { Console.Log("Balancy Initialized: " + responseData.Success); }
        });
    }
}