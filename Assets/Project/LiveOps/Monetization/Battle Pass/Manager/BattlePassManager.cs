using UnityEngine;
using BattlePassModule.Application;

namespace BattlePassModule
{
    public class BattlePassManager : MonoBehaviour
    {
        public BattlePassService Service { get; private set; }

        public BattlePassEvents Events { get; private set; }

        public void Initialize()
        {
            BattlePassInstaller installer = new BattlePassInstaller();

            var result = installer.Install();

            Service = result.Service;
            Events = result.Events;
        }

        private void OnDestroy()
        {
            Service?.Dispose();
        }
    }
}