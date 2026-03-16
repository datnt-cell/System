using Cysharp.Threading.Tasks;
using UnityEngine;
using IAPModule.Infrastructure;

namespace IAPModule
{
    public class IAPManager : MonoBehaviour
    {
        [SerializeField] private LoadingView view;

        public IAPService Service { get; private set; }
        public IAPEvents Events { get; private set; }

        public async UniTask Initialize()
        {
            IAPInstaller installer = new IAPInstaller();

            var result = await installer.Install(view);

            Service = result.Service;
            Events = result.Events;
        }

        private void OnDestroy()
        {
            Service?.Dispose();
        }
    }
}