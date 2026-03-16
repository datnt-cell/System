using BattlePassModule.Domain;

namespace BattlePassModule.Application
{
    public interface IBattlePassRepository
    {
        BattlePassProgress LoadProgress(string seasonId);

        void SaveProgress(BattlePassProgress progress);
    }
}