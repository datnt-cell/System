using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine;

namespace Creator
{
    public static class SceneLoader
    {
        public static void Load(ManagerBase.Data data, LoadSceneMode loadMode)
        {
            if (data.loadMode == SceneLoadMode.BuildIn)
            {
                SceneManager.LoadScene(data.sceneName, loadMode);
                return;
            }

            LoadAddressable(data, loadMode);
        }

        static void LoadAddressable(ManagerBase.Data data, LoadSceneMode loadMode)
        {
            var handle = Addressables.LoadSceneAsync(data.sceneName, loadMode, activateOnLoad: false);

            data.addressableHandle = handle;

       //     GameManager.Instance.SetActiveLoadPopup(true);

            GameManager.Instance.StartCoroutine(Activate(handle, data));
        }

        static IEnumerator Activate(AsyncOperationHandle<SceneInstance> handle, ManagerBase.Data data)
        {
            yield return null;

            yield return handle;

            yield return handle.Result.ActivateAsync();

            data.scene = handle.Result.Scene;

           // GameManager.Instance.SetActiveLoadPopup(false);
        }

        public static void Unload(ManagerBase.Data data)
        {
            switch (data.loadMode)
            {
                case SceneLoadMode.BuildIn:
                    if (data.scene.IsValid())
                        SceneManager.UnloadSceneAsync(data.scene);
                    break;

                case SceneLoadMode.Addressable:
                    if (data.addressableHandle.HasValue && data.addressableHandle.Value.IsValid())
                    {
                        Addressables.UnloadSceneAsync(data.addressableHandle.Value);
                        data.addressableHandle = null;
                    }
                    break;
            }
        }
    }
}