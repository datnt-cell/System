using StoreSystem.Application;
using StoreSystem.Infrastructure;

namespace StoreSystem.Installer
{
    public class StoreInstallerResult
    {
        public StoreItemUseCase UseCase { get; }
        public StoreEvents Events { get; }

        public StoreInstallerResult(
            StoreItemUseCase useCase,
            StoreEvents events)
        {
            UseCase = useCase;
            Events = events;
        }
    }
}