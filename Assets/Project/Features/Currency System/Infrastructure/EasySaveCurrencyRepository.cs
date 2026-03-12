using System.Collections.Generic;
using CurrencySystem.Application;
using CurrencySystem.Domain;

namespace CurrencySystem.Infrastructure
{
    /// <summary>
    /// Lưu dữ liệu Currency bằng Easy Save.
    /// File lưu: Currency_Save.es3
    /// </summary>
    public class EasySaveCurrencyRepository
        : ICurrencyRepository
    {
        private const string FILE_NAME = "Currency_Save.es3";
        private const string KEY = "currency_data";

        public void Save(CurrencyState state)
        {
            Dictionary<string, int> saveData = new();

            foreach (var pair in state.GetAll())
            {
                saveData[pair.Key.Value] = pair.Value;
            }

            ES3.Save(KEY, saveData, FILE_NAME);
        }

        public void Load(CurrencyState state)
        {
            if (!ES3.KeyExists(KEY, FILE_NAME))
                return;

            Dictionary<string, int> data =
                ES3.Load<Dictionary<string, int>>(KEY, FILE_NAME);

            foreach (var pair in data)
            {
                // DÙNG SetRaw để không trigger event
                state.SetRaw(
                    new CurrencyId(pair.Key),
                    pair.Value);
            }
        }
    }
}