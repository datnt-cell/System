using UnityEngine;
using PlayerSystem.Application;
using System;

namespace PlayerSystem.Presentation
{
    /// <summary>
    /// Entry point của PlayerModule
    /// Chịu trách nhiệm khởi tạo và điều phối Player System
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        private PlayerInstaller _installer;

        public PlayerService Service { get; private set; }

        private float _playTimer;
        private bool _initialized;

        // =====================
        // INITIALIZE
        // =====================

        public void Initialize()
        {
            if (_initialized)
            {
                Debug.LogWarning("[PlayerManager] Already initialized");
                return;
            }

            _installer = new PlayerInstaller();
            _installer.Install();

            Service = _installer.PlayerService;

            if (Service == null)
            {
                Debug.LogError("[PlayerManager] Service missing");
                return;
            }

            SetupPlayerSession();

            _initialized = true;
        }

        // =====================
        // SESSION SETUP
        // =====================

        private void SetupPlayerSession()
        {
            // tăng session
            Service.AddSession();

            // xác định new user
            bool isNewUser = Service.SessionCount.Value <= 1;
            Service.SetNewUser(isNewUser);
        }

        // =====================
        // UPDATE
        // =====================

        private void Update()
        {
            if (!_initialized)
                return;

            _playTimer += Time.deltaTime;

            if (_playTimer >= 1f)
            {
                _playTimer = 0f;

                Service.AddPlayTime(1);
            }
        }

        // =====================
        // APP LIFECYCLE
        // =====================

        private void OnApplicationPause(bool pause)
        {
            if (!_initialized)
                return;

            if (pause)
                Service.Save();
        }

        private void OnApplicationQuit()
        {
            if (!_initialized)
                return;

            Service.Save();
        }

        // =====================
        // GAMEPLAY API
        // =====================

        public void CompleteStage(int amount = 1)
        {
            if (!_initialized)
                return;

            Service.AddStage(amount);
        }

        public void SetLevel(int level)
        {
            if (!_initialized)
                return;

            Service.SetLevel(level);
        }

        public void SetTutorialStep(int step)
        {
            if (!_initialized)
                return;

            Service.SetTutorialStep(step);
        }

        public void OnPurchaseSuccess()
        {
            if (!_initialized)
                return;

            long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            Service.SetLastPurchaseTime(now);
        }

        public void SetTrafficCampaign(string campaign)
        {
            if (!_initialized)
                return;

            Service.SetTrafficCampaign(campaign);
        }

        public void SetTrafficSource(string source)
        {
            if (!_initialized)
                return;

            Service.SetTrafficSource(source);
        }

        // =====================
        // READ API
        // =====================

        public int GetLevel()
        {
            return Service?.Level.Value ?? 0;
        }

        public int GetStage()
        {
            return Service?.Stage.Value ?? 0;
        }

        public int GetSessionCount()
        {
            return Service?.SessionCount.Value ?? 0;
        }

        public int GetPlayTime()
        {
            return Service?.PlayTimeSeconds.Value ?? 0;
        }

        public bool IsNewUser()
        {
            return Service?.IsNewUser.Value ?? false;
        }

        public string GetCountry()
        {
            return Service?.Country.Value ?? "";
        }
    }
}