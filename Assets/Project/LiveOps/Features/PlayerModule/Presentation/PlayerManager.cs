using UnityEngine;
using PlayerSystem.Application;
using PlayerSystem.Domain;

namespace PlayerSystem.Presentation
{
    /// <summary>
    /// Entry point của PlayerModule
    /// Chịu trách nhiệm khởi tạo và điều phối Player System
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        private PlayerInstaller _installer;

        public PlayerService PlayerService { get; private set; }

        private float _playTimer;

        private bool _initialized;

        /// <summary>
        /// Khởi tạo PlayerModule
        /// </summary>
        public void Initialize()
        {
            if (_initialized)
            {
                Debug.LogWarning("[PlayerManager] Already initialized");
                return;
            }

            _installer = new PlayerInstaller();

            if (_installer == null)
            {
                Debug.LogError("[PlayerManager] Installer creation failed");
                return;
            }

            _installer.Install();

            PlayerService = _installer.PlayerService;

            if (PlayerService == null)
            {
                Debug.LogError("[PlayerManager] Player dependencies missing");
                return;
            }

            AutoSetupPlayerInfo();

            _initialized = true;
        }

        private void AutoSetupPlayerInfo()
        {
            // =========================
            // SET NEW USER
            // =========================

            if (PlayerService.SessionCount.Value == 1)
            {
                PlayerService.SetNewUser(true);
            }
            else
            {
                PlayerService.SetNewUser(false);
            }

            // =========================
            // SET COUNTRY
            // =========================

            if (string.IsNullOrEmpty(PlayerService.Country.Value))
            {
                // string country = DetectCountry();
                // PlayerService.SetCountry(country);
            }

            PlayerService.AddSession();

            PlayerService.UpdateDaysSinceInstall();
        }


        private void Update()
        {
            if (!_initialized)
                return;


            _playTimer += Time.deltaTime;

            if (_playTimer >= 60f)
            {
                _playTimer = 0f;
                PlayerService.AddPlayTime(1);
            }
        }

        private void OnApplicationPause(bool pause)
        {
            if (!_initialized)
                return;

            if (pause && PlayerService != null)
            {
                PlayerService.Save();
            }
        }

        private void OnApplicationQuit()
        {
            if (!_initialized)
                return;

            PlayerService?.Save();
        }

        /// <summary>
        /// Gameplay gọi khi player hoàn thành stage
        /// </summary>
        public void CompleteStage(int stage)
        {
            if (!_initialized)
                return;

            PlayerService?.AddStage(stage);
        }

        /// <summary>
        /// Lấy level hiện tại
        /// </summary>
        public int GetLevel()
        {
            if (PlayerService == null)
                return 0;

            return PlayerService.Level.Value;
        }

        /// <summary>
        /// Lấy stage hiện tại
        /// </summary>
        public int GetStage()
        {
            if (PlayerService == null)
                return 0;

            return PlayerService.Stage.Value;
        }

        /// <summary>
        /// Lấy playtime
        /// </summary>
        public int GetPlayTime()
        {
            if (PlayerService == null)
                return 0;

            return PlayerService.PlayTimeMinutes.Value;
        }
    }
}