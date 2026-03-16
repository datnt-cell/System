using GameSystems.Random.Providers;
using GachaSystem.Application.Services;
using GachaSystem.Infrastructure.Events;

namespace GachaSystem.Installer
{
    public class GachaInstaller
    {
        public GachaInstallResult Install()
        {
            // =========================
            // RANDOM
            // =========================

            var random = new UnityRandomProvider();

            // =========================
            // EVENTS
            // =========================

            var events = new GachaEvents();

            // =========================
            // SERVICE
            // =========================

            var service = new GachaService(
                random,
                events
            );

            // =========================
            // RESULT
            // =========================

            return new GachaInstallResult
            {
                Service = service,
                Events = events
            };
        }
    }
}