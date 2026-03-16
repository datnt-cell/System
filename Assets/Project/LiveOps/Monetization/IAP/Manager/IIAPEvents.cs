using R3;

namespace IAPModule.Domain
{
    public interface IIAPEvents
    {
        Observable<IAPEvent> Stream { get; }
    }
}