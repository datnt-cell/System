using R3;

namespace StoreSystem.Domain
{
    public interface IStoreEvents
    {
        Observable<StoreEvent> Stream { get; }
    }
}