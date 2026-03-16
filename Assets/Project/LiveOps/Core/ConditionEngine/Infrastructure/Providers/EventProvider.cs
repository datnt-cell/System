using GameEventModule.Application;

namespace ConditionEngine.Infrastructure
{
    /// <summary>
    /// Provider lấy progress của event từ GameEventService
    /// </summary>
    public class EventProvider
    {
        private readonly GameEventService _eventService;

        public EventProvider(GameEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Lấy progress của event
        /// </summary>
        public int GetProgress(string eventId)
        {
            return _eventService.GetTotalProgress(eventId);
        }
    }
}