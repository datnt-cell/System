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
        public PlayerService Service { get; private set; }
        public IPlayerEvents Events { get; private set; }

        private float _playTimer;
        private bool _initialized;

        // =====================
        // INITIALIZE
        // =====================

        public void Initialize()
        {
            if (_initialized)
                return;

            var installer = new PlayerInstaller();
            var result = installer.Install();

            Service = result.Service;
            Events = result.Events;

            SetupPlayerSession();

            _initialized = true;
        }

        // =====================
        // SESSION SETUP
        // =====================

        private void SetupPlayerSession()
        {
            Service.AddSession();

            bool isNewUser = Service.GetSessionCount() <= 1;

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
    }
}