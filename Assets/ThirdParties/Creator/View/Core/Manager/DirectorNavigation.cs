using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace Creator
{
    public partial class ManagerDirector
    {
        /// <summary>
        /// Tạo SceneData và đưa vào hàng đợi load
        /// </summary>
        private static Data CreateSceneDataAndEnqueue(
            string sceneName,
            object data,
            Callback onShown,
            Callback onHidden,
            bool hasShield,
            SceneLoadMode loadMode,
            NavigationLayer layer = NavigationLayer.Main)
        {
            var sceneData = new Data(
                data,
                sceneName,
                onShown,
                onHidden,
                hasShield,
                loadMode,
                layer
            );

            // Đưa scene vào queue chờ load
            m_DataQueue.Enqueue(sceneData);
            return sceneData;
        }

        /// <summary>
        /// Load scene theo dạng Additive
        /// </summary>
        private static void LoadSceneAdditive(Data sceneData)
        {
            SceneLoader.Load(sceneData, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Set scene gốc (Root Scene) của app
        /// </summary>
        public static void SetRootScene(
            string sceneName,
            object data = null,
            string log = "",
            SceneLoadMode loadMode = SceneLoadMode.BuildIn)
        {
            var sceneData = CreateSceneDataAndEnqueue(
                sceneName,
                data,
                onShown: null,
                onHidden: null,
                hasShield: true,
                loadMode
            );

            // Lưu lại tên main scene
            m_MainSceneName = sceneName;

            // Fade out scene hiện tại trước khi load scene mới
            Object.FadeOutScene();
        }

        /// <summary>
        /// Push một scene mới lên stack (giống Push Popup)
        /// </summary>
        public static void PushSceneToStack(
            string sceneName,
            object data = null,
            Callback onShown = null,
            Callback onHidden = null,
            bool hasShield = true,
            string log = "",
            SceneLoadMode loadMode = SceneLoadMode.BuildIn,
            NavigationLayer layer = NavigationLayer.Main)
        {
            var sceneData = CreateSceneDataAndEnqueue(
                sceneName,
                data,
                onShown,
                onHidden,
                hasShield,
                loadMode,
                layer
            );

            // Bật shield để chặn input
            Object.ShieldOn();

            // Disable raycast của scene hiện tại
            if (m_ControllerStack.Count > 0)
                m_ControllerStack.Peek().GetCanvasGroup().blocksRaycasts = false;

            LoadSceneAdditive(sceneData);
        }

        /// <summary>
        /// Push scene và track thời gian tồn tại của scene
        /// </summary>
        public static void PushSceneWithTracking(
            string sceneName,
            object data = null,
            Callback onShown = null,
            UnityAction onHidden = null,
            bool hasShield = true,
            string log = "",
            SceneLoadMode loadMode = SceneLoadMode.BuildIn)
        {
            float startTime = Time.realtimeSinceStartup;

            PushSceneToStack(
                sceneName,
                data,
                onShown,
                () =>
                {
                    float duration = Time.realtimeSinceStartup - startTime;

                    // TODO: Hook Firebase / Adjust event tại đây
                    // Analytics.LogSceneDuration(sceneName, duration);

                    onHidden?.Invoke();
                },
                hasShield,
                log,
                loadMode
            );
        }

        /// <summary>
        /// Thay thế scene trên cùng của stack
        /// </summary>
        public static void ReplaceTopScene(
            string sceneName,
            object data = null,
            Callback onShown = null,
            Callback onHidden = null,
            string log = "",
            SceneLoadMode loadMode = SceneLoadMode.BuildIn)
        {
            if (m_ControllerStack.Count == 0)
                return;

            var currentController = m_ControllerStack.Peek();

            // Ẩn popup hiện tại (không animation)
            currentController.HidePopup(false);

            Callback wrappedHidden = () =>
            {
                onHidden?.Invoke();

                // Hiện lại popup cũ sau khi scene mới đóng
                currentController.ShowPopup(false);
            };

            var sceneData = CreateSceneDataAndEnqueue(
                sceneName,
                data,
                onShown,
                wrappedHidden,
                hasShield: false,
                loadMode
            );

            Object.ShieldOn();
            LoadSceneAdditive(sceneData);
        }
    }
}