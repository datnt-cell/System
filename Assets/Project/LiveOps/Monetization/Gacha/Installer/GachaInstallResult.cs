using GachaSystem.Application.Services;
using GachaSystem.Infrastructure.Events;

namespace GachaSystem.Installer
{
    public class GachaInstallResult
    {
        public GachaService Service;
        public GachaEvents Events;
    }
}