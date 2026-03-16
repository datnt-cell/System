using R3;

namespace ConditionEngine.Application
{
    public interface IConditionEvents
    {
        Observable<ConditionEvent> Stream { get; }
    }
}