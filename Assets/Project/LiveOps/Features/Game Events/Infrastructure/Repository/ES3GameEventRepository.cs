using GameEventModule.Domain;

namespace GameEventModule.Infrastructure.Repositories
{
    /// <summary>
    /// Repository lưu GameEventState bằng Easy Save
    /// Dữ liệu được lưu trong file riêng GameEvent.es3
    /// </summary>
    public class ES3GameEventRepository : IGameEventRepository
    {
        private const string FileName = "GameEvent.es3";
        private const string KeyPrefix = "event_";

        public GameEventState GetState(GameEventId id)
        {
            var key = KeyPrefix + id.Value;

            if (!ES3.KeyExists(key, FileName))
                return null;

            return ES3.Load<GameEventState>(key, FileName);
        }

        public void SaveState(GameEventId id, GameEventState state)
        {
            var key = KeyPrefix + id.Value;

            ES3.Save(key, state, FileName);
        }
    }
}