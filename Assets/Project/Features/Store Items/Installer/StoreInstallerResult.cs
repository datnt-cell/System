using StoreSystem.Application;

namespace StoreSystem.Installer
{
    public class StoreInstallerResult
    {
        public StoreItemUseCase UseCase;

        public StoreInstallerResult(StoreItemUseCase useCase)
        {
            UseCase = useCase;
        }
    }
}