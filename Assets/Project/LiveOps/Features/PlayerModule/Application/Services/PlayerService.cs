using R3;
using PlayerSystem.Domain;
using System;

namespace PlayerSystem.Application
{
    /// <summary>
    /// Service quản lý PlayerData và expose reactive stream
    /// </summary>
    public class PlayerService
    {
        private readonly IPlayerRepository _repository;
        private PlayerData _data;

        // =====================
        // PLAYER PROGRESS
        // =====================

        public ReactiveProperty<int> Level { get; } = new();
        public ReactiveProperty<int> Stage { get; } = new();
        public ReactiveProperty<int> SessionCount { get; } = new();

        // =====================
        // PLAYER INFO
        // =====================

        public ReactiveProperty<bool> IsNewUser { get; } = new();
        public ReactiveProperty<string> Country { get; } = new();
        public ReactiveProperty<string> Segment { get; } = new();

        // =====================
        // TIME
        // =====================

        public ReactiveProperty<int> DaysSinceInstall { get; } = new();
        public ReactiveProperty<int> PlayTimeMinutes { get; } = new();

        public PlayerService(IPlayerRepository repository)
        {
            _repository = repository;
        }

        public void Load()
        {
            _data = _repository.Load();

            if (_data == null)
                _data = new PlayerData();

            if (_data.FirstInstallTimestamp == 0)
            {
                _data.FirstInstallTimestamp = GetNow();
            }

            UpdateDaysSinceInstall();

            Level.Value = _data.Level;
            Stage.Value = _data.Stage;
            SessionCount.Value = _data.SessionCount;

            IsNewUser.Value = _data.IsNewUser;
            Country.Value = _data.Country;
            Segment.Value = _data.Segment;

            DaysSinceInstall.Value = _data.DaysSinceInstall;
            PlayTimeMinutes.Value = _data.TotalPlayTimeMinutes;
        }

        public void UpdateDaysSinceInstall()
        {
            long now = GetNow();

            int days = (int)((now - _data.FirstInstallTimestamp) / 86400);

            _data.DaysSinceInstall = days;

            DaysSinceInstall.Value = days;
        }

        private long GetNow()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public void Save()
        {
            _repository.Save(_data);
        }

        // =====================
        // STAGE
        // =====================

        public void SetStage(int stage)
        {
            if (_data.Stage == stage)
                return;

            _data.Stage = stage;

            Stage.Value = stage;

            Save();
        }

        public void AddStage(int amount = 1)
        {
            if (amount <= 0)
                return;

            _data.Stage += amount;

            Stage.Value = _data.Stage;

            Save();
        }

        // =====================
        // SESSION
        // =====================

        public void AddSession()
        {
            _data.SessionCount++;

            SessionCount.Value = _data.SessionCount;

            Save();
        }

        // =====================
        // PLAYTIME
        // =====================

        public void AddPlayTime(int minutes)
        {
            if (minutes <= 0)
                return;

            _data.TotalPlayTimeMinutes += minutes;

            PlayTimeMinutes.Value = _data.TotalPlayTimeMinutes;
        }

        // =====================
        // PLAYER INFO
        // =====================

        public void SetCountry(string country)
        {
            if (_data.Country == country)
                return;

            _data.Country = country;

            Country.Value = country;

            Save();
        }

        public void SetSegment(string segment)
        {
            if (_data.Segment == segment)
                return;

            _data.Segment = segment;

            Segment.Value = segment;

            Save();
        }

        public void SetNewUser(bool value)
        {
            if (_data.IsNewUser == value)
                return;

            _data.IsNewUser = value;

            IsNewUser.Value = value;

            Save();
        }

        public void SetDaysSinceInstall(int days)
        {
            if (_data.DaysSinceInstall == days)
                return;

            _data.DaysSinceInstall = days;

            DaysSinceInstall.Value = days;

            Save();
        }
    }
}