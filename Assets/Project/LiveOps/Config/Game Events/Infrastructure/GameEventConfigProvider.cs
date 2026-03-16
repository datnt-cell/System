using System.Collections.Generic;
using GameEventModule.Application;
using GameEventModule.Domain;
using GameEventModule.Infrastructure.Config;

namespace GameEventModule.Infrastructure
{
    public class GameEventConfigProvider : IGameEventConfigProvider
    {
        public List<GameEvent> GetEvents()
        {
            var config = GameEventGlobalConfig.Instance;

            var result = new List<GameEvent>();

            foreach (var entry in config.Events)
            {
                if (entry == null)
                    continue;

                var gameEvent = entry.Build();

                if (gameEvent != null)
                    result.Add(gameEvent);
            }

            return result;
        }
    }
}