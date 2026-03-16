using CurrencySystem.Domain;

namespace CurrencySystem.Application
{
    /// <summary>
    /// Interface lưu trữ dữ liệu.
    /// Infrastructure sẽ implement.
    /// </summary>
    public interface ICurrencyRepository
    {
        void Save(CurrencyState state);
        void Load(CurrencyState state);
    }
}