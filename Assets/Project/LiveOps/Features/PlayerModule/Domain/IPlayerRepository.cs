namespace PlayerSystem.Domain
{
    /// <summary>
    /// Repository quản lý việc lưu và tải PlayerData
    /// Tầng Domain chỉ định nghĩa interface
    /// Implementation sẽ nằm ở Infrastructure
    /// </summary>
    public interface IPlayerRepository
    {
        /// <summary>
        /// Load dữ liệu player từ save system
        /// </summary>
        PlayerData Load();

        /// <summary>
        /// Lưu dữ liệu player
        /// </summary>
        void Save(PlayerData data);
    }
}