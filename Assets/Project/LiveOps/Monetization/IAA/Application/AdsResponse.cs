namespace AdsSystem.Application
{
    /// <summary>
    /// Response trả về khi thao tác Ads
    /// </summary>
    public class AdsResponse
    {
        public bool Success { get; set; }
        public AdType Type { get; set; }
        public string ErrorMessage { get; set; }
    }
}