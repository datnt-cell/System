using System.Collections.Generic;
using GameEventModule.Domain;

namespace GameEventModule.Application
{
    /// <summary>
    /// Provider cung cấp danh sách GameEvent
    /// </summary>
    public interface IGameEventConfigProvider
    {
        List<GameEvent> GetEvents();
    }
}