namespace GameEventModule.Domain
{
    /// <summary>
    /// Repository lưu trạng thái runtime của Event
    /// </summary>
    public interface IGameEventRepository
    {
        GameEventState GetState(GameEventId id);

        void SaveState(GameEventId id, GameEventState state);
    }
}