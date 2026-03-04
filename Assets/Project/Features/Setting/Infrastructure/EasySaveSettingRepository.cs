using ES3Internal;

/// <summary>
/// Repository lưu trữ Setting sử dụng Easy Save 3 (ES3)
/// Chỉ chịu trách nhiệm đọc/ghi dữ liệu.
/// Không chứa business logic.
/// </summary>
public class EasySaveSettingRepository : ISettingRepository
{
    // Tên file lưu trữ (có thể tách riêng file để dễ quản lý)
    private const string FileName = "settings.es3";

    // Keys
    private const string KeySound = "sound";
    private const string KeyVibration = "vibration";
    private const string KeyMusic = "music";

    // Default values
    private const bool DefaultSound = true;
    private const bool DefaultVibration = true;
    private const bool DefaultMusic = false;

    #region GET

    public bool GetSound()
    {
        return ES3.KeyExists(KeySound, FileName)
            ? ES3.Load<bool>(KeySound, FileName)
            : DefaultSound;
    }

    public bool GetVibration()
    {
        return ES3.KeyExists(KeyVibration, FileName)
            ? ES3.Load<bool>(KeyVibration, FileName)
            : DefaultVibration;
    }

    public bool GetMusic()
    {
        return ES3.KeyExists(KeyMusic, FileName)
            ? ES3.Load<bool>(KeyMusic, FileName)
            : DefaultMusic;
    }

    #endregion

    #region SAVE

    public void SaveSound(bool value)
    {
        ES3.Save(KeySound, value, FileName);
    }

    public void SaveVibration(bool value)
    {
        ES3.Save(KeyVibration, value, FileName);
    }

    public void SaveMusic(bool value)
    {
        ES3.Save(KeyMusic, value, FileName);
    }

    #endregion
}