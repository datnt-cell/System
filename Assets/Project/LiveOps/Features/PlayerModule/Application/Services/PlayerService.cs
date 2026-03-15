using R3;
using PlayerSystem.Domain;
using System;
using UnityEngine;

namespace PlayerSystem.Application
{
    public class PlayerService
    {
        private readonly IPlayerRepository _repository;
        private PlayerData _data;

        // =====================
        // PROGRESS
        // =====================

        public ReactiveProperty<int> Level { get; } = new();
        public ReactiveProperty<int> Stage { get; } = new();
        public ReactiveProperty<int> TutorialStep { get; } = new();

        // =====================
        // SESSION
        // =====================

        public ReactiveProperty<int> SessionCount { get; } = new();
        public ReactiveProperty<int> PlayTimeSeconds { get; } = new();

        // =====================
        // USER
        // =====================

        public ReactiveProperty<bool> IsNewUser { get; } = new();
        public ReactiveProperty<bool> DontDisturb { get; } = new();

        // =====================
        // REGION
        // =====================

        public ReactiveProperty<string> Country { get; } = new();
        public ReactiveProperty<string> SystemLanguage { get; } = new();

        // =====================
        // TRAFFIC
        // =====================

        public ReactiveProperty<string> TrafficSource { get; } = new();
        public ReactiveProperty<string> TrafficCampaign { get; } = new();

        // =====================
        // IDENTITY
        // =====================

        public ReactiveProperty<string> ProfileId { get; } = new();
        public ReactiveProperty<string> DeviceId { get; } = new();
        public ReactiveProperty<string> CustomId { get; } = new();

        // =====================
        // DEVICE
        // =====================

        public ReactiveProperty<string> Platform { get; } = new();
        public ReactiveProperty<int> AppVersion { get; } = new();
        public ReactiveProperty<string> EngineVersion { get; } = new();

        // =====================
        // TIME
        // =====================

        public ReactiveProperty<long> FirstLoginTime { get; } = new();
        public ReactiveProperty<long> LastLoginTime { get; } = new();
        public ReactiveProperty<long> CurrentDay { get; } = new();
        public ReactiveProperty<long> LastPurchaseTime { get; } = new();

        public PlayerService(IPlayerRepository repository)
        {
            _repository = repository;
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

            // auto detect device info
            AutoDetectDeviceInfo();

            SyncReactive();
        }

        private void SyncReactive()
        {
            Level.Value = _data.Level;
            Stage.Value = _data.Stage;
            TutorialStep.Value = _data.TutorialStep;

            SessionCount.Value = _data.SessionCount;
            PlayTimeSeconds.Value = _data.TotalPlayTimeSeconds;

            IsNewUser.Value = _data.IsNewUser;
            DontDisturb.Value = _data.DontDisturb;

            Country.Value = _data.Country;
            SystemLanguage.Value = _data.SystemLanguage;

            TrafficSource.Value = _data.TrafficSource;
            TrafficCampaign.Value = _data.TrafficCampaign;

            ProfileId.Value = _data.ProfileId;
            DeviceId.Value = _data.DeviceId;
            CustomId.Value = _data.CustomId;

            Platform.Value = _data.Platform;
            AppVersion.Value = _data.AppVersion;
            EngineVersion.Value = _data.EngineVersion;

            FirstLoginTime.Value = _data.FirstLoginTime;
            LastLoginTime.Value = _data.LastLoginTime;
            CurrentDay.Value = _data.CurrentDay;
            LastPurchaseTime.Value = _data.LastPurchaseTime;
        }

        // =====================
        // DEVICE AUTO DETECT
        // =====================

        private void AutoDetectDeviceInfo()
        {
            if (string.IsNullOrEmpty(_data.DeviceId))
                _data.DeviceId = SystemInfo.deviceUniqueIdentifier;

            _data.Platform = UnityEngine.Application.platform.ToString();
            _data.AppVersion = HelperLiveOps.GetBuildNumber();
            _data.EngineVersion = UnityEngine.Application.unityVersion;
            _data.SystemLanguage = UnityEngine.Application.systemLanguage.ToString();
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

        public void AddPlayTime(int seconds)
        {
            if (seconds <= 0)
                return;

            _data.TotalPlayTimeSeconds += seconds;

            PlayTimeSeconds.Value = _data.TotalPlayTimeSeconds;
        }

        // =====================
        // PROGRESS
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

        public void SetLevel(int level)
        {
            if (_data.Level == level)
                return;

            _data.Level = level;

            Level.Value = level;

            Save();
        }

        public void SetTutorialStep(int step)
        {
            if (_data.TutorialStep == step)
                return;

            _data.TutorialStep = step;

            TutorialStep.Value = step;

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

            IsNewUser.Value = value;

            Save();
        }

        public void SetDontDisturb(bool value)
        {
            if (_data.DontDisturb == value)
                return;

            _data.DontDisturb = value;

            DontDisturb.Value = value;

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

            TrafficSource.Value = source;

            Save();
        }

        public void SetTrafficCampaign(string campaign)
        {
            if (_data.TrafficCampaign == campaign)
                return;

            _data.TrafficCampaign = campaign;

            TrafficCampaign.Value = campaign;

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

            LastPurchaseTime.Value = timestamp;

            Save();
        }

        // =====================
        // SAVE
        // =====================

        public void Save()
        {
            _repository.Save(_data);
        }

        private long GetNow()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}