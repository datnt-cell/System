/// <summary>
/// Abstraction lưu trữ setting
/// </summary>
public interface ISettingRepository
{
    bool GetSound();
    bool GetVibration();
    bool GetMusic();

    void SaveSound(bool value);
    void SaveVibration(bool value);
    void SaveMusic(bool value);
}