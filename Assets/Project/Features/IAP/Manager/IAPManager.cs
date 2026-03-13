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
        [SerializeField] private IAPLoading view;

        public IAPPresenter Presenter { get; private set; }

        public async UniTask Initialize()
        {
            IAPInstaller installer = new IAPInstaller();

            Presenter = await installer.Install(view);   // ✅ await initialize IAP
        }

        private void OnDestroy()
        {
            Presenter?.Dispose();
        }
    }
}