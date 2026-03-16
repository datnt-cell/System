using R3;

namespace GachaSystem.Application.Interfaces
{
    public interface IGachaEvents
    {
        Observable<GachaEvent> Stream { get; }

        void Publish(GachaEvent evt);
    }
}