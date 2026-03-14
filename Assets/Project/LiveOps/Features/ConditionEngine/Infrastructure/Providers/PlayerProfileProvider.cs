using PlayerSystem.Application;
using ConditionEngine.Domain;

namespace ConditionEngine.Infrastructure
{
    /// <summary>
    /// Adapter lấy dữ liệu từ PlayerService
    /// </summary>
    public class PlayerProvider : IPlayerProvider
    {
        private readonly PlayerService _playerService;

        public PlayerProvider(PlayerService playerService)
        {
            _playerService = playerService;
        }

        public int Level => _playerService.Level.Value;

        public int Stage => _playerService.Stage.Value;

        public int SessionCount => _playerService.SessionCount.Value;

        public bool IsNewUser => _playerService.IsNewUser.Value;

        public string Country => _playerService.Country.Value;

        public string Segment => _playerService.Segment.Value;

        public int DaysSinceInstall => _playerService.DaysSinceInstall.Value;

        public int TotalPlayTimeMinutes => _playerService.PlayTimeMinutes.Value;
    }
}