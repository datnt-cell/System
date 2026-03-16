using BattlePassModule.Application;

namespace BattlePassModule
{
    public class BattlePassInstallResult
    {
        public BattlePassService Service;
        public BattlePassEvents Events;

        public BattlePassInstallResult(
            BattlePassService service,
            BattlePassEvents events)
        {
            Service = service;
            Events = events;
        }
    }
}