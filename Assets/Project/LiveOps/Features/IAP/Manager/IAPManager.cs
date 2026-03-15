using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IAPModule
{
    /// <summary>
    /// Manager là entry point của Unity.
    /// Giữ reference Presenter.
    /// </summary>
    public class IAPManager : MonoBehaviour
    {
        [SerializeField] private LoadingView view;

        public IAPService Service { get; private set; }

        public async UniTask Initialize()
        {
            IAPInstaller installer = new IAPInstaller();

            Service = await installer.Install(view);   // ✅ await initialize IAP
        }

        private void OnDestroy()
        {
            Service?.Dispose();
        }
    }
}