using CurrencySystem.Domain;

public interface ICurrencyBundleProvider
{
    CurrencyBundle GetBundle(string id);
}