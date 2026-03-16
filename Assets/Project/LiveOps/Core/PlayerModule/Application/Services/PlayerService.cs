using PlayerSystem.Domain;
using System;

namespace PlayerSystem.Application
{
    /// <summary>
    /// Service xử lý business logic Player
    /// </summary>
    public class PlayerService
    {
        private readonly IPlayerRepository _repository;
        private readonly PlayerEvents _events;

        private PlayerData _data;

        public IPlayerEvents Events => _events;

        public PlayerService(
            IPlayerRepository repository,
            PlayerEvents events)
        {
            _repository = repository;
            _events = events;
        }

        // =====================
        // LOAD
        // =====================

        public void Load()
        {
            _data = _repository.Load();

            if (_data == null)
                _data = new PlayerData();

            long now = GetNow();

            if (_data.FirstLoginTime == 0)
                _data.FirstLoginTime = now;

            _data.LastLoginTime = now;
            _data.CurrentDay = now / 86400;

            AutoDetectDeviceInfo();
        }

        // =====================
        // DEVICE INFO
        // =====================

        private void AutoDetectDeviceInfo()
        {
            if (string.IsNullOrEmpty(_data.DeviceId))
                _data.DeviceId = UnityEngine.SystemInfo.deviceUniqueIdentifier;

            _data.Platform = UnityEngine.Application.platform.ToString();
            _data.EngineVersion = UnityEngine.Application.unityVersion;
            _data.SystemLanguage = UnityEngine.Application.systemLanguage.ToString();
        }

        // =====================
        // SESSION
        // =====================

        public void AddSession()
        {
            _data.SessionCount++;

            _events.Publish(PlayerEvent.Session(_data.SessionCount));

            Save();
        }

        // =====================
        // PLAYTIME
        // =====================

        public void AddPlayTime(int seconds)
        {
            if (seconds <= 0)
                return;

            _data.TotalPlayTimeSeconds += seconds;

            _events.Publish(PlayerEvent.PlayTime(_data.TotalPlayTimeSeconds));
        }

        // =====================
        // PROGRESS
        // =====================

        public void SetLevel(int level)
        {
            if (_data.Level == level)
                return;

            _data.Level = level;

            _events.Publish(PlayerEvent.Level(level));

            Save();
        }

        public void AddStage(int amount = 1)
        {
            if (amount <= 0)
                return;

            _data.Stage += amount;

            _events.Publish(PlayerEvent.Stage(_data.Stage));

            Save();
        }

        public void SetStage(int stage)
        {
            if (_data.Stage == stage)
                return;

            _data.Stage = stage;

            _events.Publish(PlayerEvent.Stage(stage));

            Save();
        }

        public void SetTutorialStep(int step)
        {
            if (_data.TutorialStep == step)
                return;

            _data.TutorialStep = step;

            _events.Publish(new PlayerEvent
            {
                Type = PlayerEventType.TutorialStepChanged,
                IntValue = step
            });

            Save();
        }

        // =====================
        // USER
        // =====================

        public void SetNewUser(bool value)
        {
            if (_data.IsNewUser == value)
                return;

            _data.IsNewUser = value;

            _events.Publish(new PlayerEvent
            {
                Type = PlayerEventType.NewUserChanged,
                IntValue = value ? 1 : 0
            });

            Save();
        }

        public void SetDontDisturb(bool value)
        {
            if (_data.DontDisturb == value)
                return;

            _data.DontDisturb = value;

            Save();
        }

        // =====================
        // TRAFFIC
        // =====================

        public void SetTrafficSource(string source)
        {
            if (_data.TrafficSource == source)
                return;

            _data.TrafficSource = source;

            _events.Publish(PlayerEvent.TrafficSource(source));

            Save();
        }

        public void SetTrafficCampaign(string campaign)
        {
            if (_data.TrafficCampaign == campaign)
                return;

            _data.TrafficCampaign = campaign;

            _events.Publish(PlayerEvent.TrafficCampaign(campaign));

            Save();
        }

        // =====================
        // PURCHASE
        // =====================

        public void SetLastPurchaseTime(long timestamp)
        {
            if (_data.LastPurchaseTime == timestamp)
                return;

            _data.LastPurchaseTime = timestamp;

            _events.Publish(PlayerEvent.Purchase(timestamp));

            Save();
        }

        // =====================
        // SAVE
        // =====================

        public void Save()
        {
            _repository.Save(_data);
        }

        // =====================
        // GETTERS
        // =====================

        public int GetLevel() => _data.Level;
        public int GetStage() => _data.Stage;
        public int GetTutorialStep() => _data.TutorialStep;
        public int GetSessionCount() => _data.SessionCount;
        public int GetPlayTime() => _data.TotalPlayTimeSeconds;
        public bool IsNewUser() => _data.IsNewUser;
        public bool DontDisturb() => _data.DontDisturb;
        public string GetCountry() => _data.Country;
        public long GetCurrentDay() => _data.CurrentDay;
        public string GetPlatform() => _data.Platform;
        public int GetAppVersion() => _data.AppVersion;
        public string GetEngineVersion() => _data.EngineVersion;
        public string GetTrafficSource() => _data.TrafficSource;
        public string GetTrafficCampaign() => _data.TrafficCampaign;
        public long GetLastPurchaseTime() => _data.LastPurchaseTime;

        // =====================
        // TIME
        // =====================

        private long GetNow()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}