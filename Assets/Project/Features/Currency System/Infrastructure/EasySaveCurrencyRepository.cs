using System.Collections.Generic;
using CurrencySystem.Application;
using CurrencySystem.Domain;

namespace CurrencySystem.Infrastructure
{
    /// <summary>
    /// Lưu dữ liệu bằng Easy Save.
    /// </summary>
    public class EasySaveCurrencyRepository : ICurrencyRepository
    {
        private const string KEY = "currency_data";

        public void Save(CurrencyState state)
        {
            Dictionary<string, int> saveData = new();

            foreach (var pair in state.GetAll())
            {
                saveData[pair.Key.Value] = pair.Value;
            }

            ES3.Save(KEY, saveData);
        }

        public void Load(CurrencyState state)
        {
            if (!ES3.KeyExists(KEY))
                return;

            Dictionary<string, int> data =
                ES3.Load<Dictionary<string, int>>(KEY);

            foreach (var pair in data)
            {
                state.Add(new CurrencyId(pair.Key), pair.Value);
            }
        }
    }
}