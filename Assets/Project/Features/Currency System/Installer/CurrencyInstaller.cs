using CurrencySystem.Application;
using CurrencySystem.Domain;
using CurrencySystem.Infrastructure;
using CurrencySystem.Presentation;

namespace CurrencySystem.Installer
{
    /// <summary>
    /// Composition Root.
    /// Nơi khởi tạo và liên kết dependency.
    /// </summary>
    public class CurrencyInstaller
    {
        public CurrencyPresenter Install()
        {
            CurrencyState state = new();
            ICurrencyRepository repository =
                new EasySaveCurrencyRepository();

            CurrencyService service =
                new CurrencyService(state, repository);

            return new CurrencyPresenter(service);
        }
    }
}