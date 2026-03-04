namespace CurrencySystem.Domain
{
    /// <summary>
    /// Cung cấp metadata của currency từ hệ thống bên ngoài.
    /// 
    /// Lý do tồn tại:
    /// - CurrencyState không biết Balancy là gì.
    /// - Metadata (MaxStack, Name, Icon...) có thể đến từ LiveOps.
    /// - Có thể thay Balancy bằng RemoteConfig khác mà không sửa Domain.
    /// </summary>
    public interface ICurrencyMetadataProvider
    {
        /// <summary>
        /// Currency này có tồn tại trong hệ thống config không?
        /// </summary>
        bool Exists(CurrencyId id);

        /// <summary>
        /// Lấy giới hạn tối đa (vd: MaxStack từ Balancy Item).
        /// </summary>
        int GetMaxStack(CurrencyId id);

        /// <summary>
        /// Lấy tên hiển thị (phục vụ UI).
        /// </summary>
        string GetDisplayName(CurrencyId id);
    }
}