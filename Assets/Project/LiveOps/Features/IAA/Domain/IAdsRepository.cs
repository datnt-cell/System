/// <summary>
/// Interface lưu trữ dữ liệu quảng cáo.
/// </summary>
public interface IAdsRepository
{
    void Save(AdsState state);
    void Load(AdsState state);
}