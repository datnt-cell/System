using R3;

namespace GameEventModule.Application
{
    public interface IGameEventEvents
    {
        Observable<GameEventEvent> Stream { get; }
    }
}