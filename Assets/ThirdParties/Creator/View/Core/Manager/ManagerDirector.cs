using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace Creator
{
    public class ManagerDirector : ManagerBase
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

        /// <summary>
        /// Pop scene trên cùng khỏi stack
        /// </summary>
        public static void PopTopScene()
        {
            if (m_ControllerStack.Count <= 1)
                return;

            ActivatePreviousController(true);
            HideController(true);

            Object.ShieldOn();

            // Hide scene trên cùng
            m_ControllerStack.Peek().Hide();
        }

        /// <summary>
        /// Đóng UI hệ thống (System Layer)
        /// </summary>
        public static void PopSystem()
        {
            if (m_ControllerStack.Count == 0)
                return;

            var top = m_ControllerStack.Peek();

            if (top.Data.layer != NavigationLayer.System)
                return;

            ActivatePreviousController(true);
            HideController(true);

            Object.ShieldOn();
            top.Hide();
        }

        /// <summary>
        /// Đóng popup (Modal) trên cùng
        /// </summary>
        public static void PopModal()
        {
            if (m_ControllerStack.Count == 0)
                return;

            var top = m_ControllerStack.Peek();

            if (top.Data.layer != NavigationLayer.Modal)
                return;

            ActivatePreviousController(true);
            HideController(true);

            Object.ShieldOn();
            top.Hide();
        }

        /// <summary>
        /// Đóng toàn bộ Modal + System và quay về Main Scene gần nhất
        /// </summary>
        public static void PopToMain()
        {
            if (m_ControllerStack == null || m_ControllerStack.Count == 0)
                return;

            while (m_ControllerStack.Count > 0)
            {
                var controller = m_ControllerStack.Peek();

                if (controller.Data.layer == NavigationLayer.Main)
                    break;

                PopTopControllerImmediately();
            }

            if (m_ControllerStack.Count == 0)
                return;

            var mainController = m_ControllerStack.Peek();

            if (mainController.Animation
                .TryGetComponent<CanvasGroup>(out var canvasGroup))
            {
                canvasGroup.blocksRaycasts = true;
            }

            mainController.OnReFocus();
        }

        /// <summary>
        /// Pop tất cả scene về Root
        /// </summary>
        public static void PopToRoot()
        {
            if (m_ControllerStack == null || m_ControllerStack.Count == 0)
                return;

            while (m_ControllerStack.Count > 1)
            {
                PopTopControllerImmediately();
            }

            var rootController = m_ControllerStack.Peek();
            if (rootController == null)
                return;

            // Enable lại raycast cho root
            if (rootController.Animation
                .TryGetComponent<CanvasGroup>(out var canvasGroup))
            {
                canvasGroup.blocksRaycasts = true;
            }
            
            rootController.OnReFocus();
        }

        /// <summary>
        /// Pop ngay lập tức các scene theo danh sách tên truyền vào
        /// </summary>
        public static void PopScenesImmediately(string[] sceneNames)
        {
            if (m_ControllerStack == null || m_ControllerStack.Count == 0)
                return;

            if (sceneNames == null || sceneNames.Length == 0)
                return;

            var tempStack = new Stack<Controller>();

            while (m_ControllerStack.Count > 0)
            {
                var controller = m_ControllerStack.Pop();
                if (controller == null)
                    continue;

                var sceneName = controller.SceneName();

                if (sceneNames.Contains(sceneName))
                {
                    controller.Data.onHidden?.Invoke();
                    RemovePendingDataForScene(sceneName);
                    SceneLoader.Unload(controller.Data);
                }
                else
                {
                    tempStack.Push(controller);
                }
            }

            // Restore lại stack
            while (tempStack.Count > 0)
            {
                m_ControllerStack.Push(tempStack.Pop());
            }

            // Focus lại scene top
            if (m_ControllerStack.Count > 0)
            {
                m_ControllerStack.Peek().OnReFocus();
            }
        }

        /// <summary>
        /// Pop controller trên cùng ngay lập tức (không animation)
        /// </summary>
        protected static void PopTopControllerImmediately()
        {
            if (m_ControllerStack.Count == 0)
                return;

            var controller = m_ControllerStack.Pop();

            controller.Data.onHidden?.Invoke();

            RemovePendingDataForScene(controller.SceneName());

            SceneLoader.Unload(controller.Data);

            if (m_ControllerStack.Count > 0)
            {
                m_ControllerStack.Peek().OnReFocus();
            }
        }

        public static Controller GetRunningScene()
        {
            return m_ControllerStack.Peek();
        }

        public static void Pause()
        {
            Time.timeScale = 0f;
        }

        public static void Resume()
        {
            Time.timeScale = 1f;
        }

        static void RemovePendingDataForScene(string sceneName)
        {
            if (m_DataQueue.Count == 0)
                return;

            var temp = new Queue<Data>();

            while (m_DataQueue.Count > 0)
            {
                var data = m_DataQueue.Dequeue();

                if (data.sceneName == sceneName)
                    continue;

                temp.Enqueue(data);
            }

            m_DataQueue = temp;
        }

        public static Stack<Controller> GetSceneStack()
        {
            return new Stack<Controller>(m_ControllerStack);
        }

        protected static void ActivatePreviousController(bool active)
        {
            ActivatePreviousController(m_ControllerStack.Peek(), active);
        }

        protected static void HideController(bool active)
        {
            HideController(m_ControllerStack.Peek(), active);
        }

        protected static void ActivatePreviousController(Controller controller, bool active)
        {
            Stack<Controller> temp = new Stack<Controller>();

            while (m_ControllerStack.Count > 0)
            {
                var top = m_ControllerStack.Pop();
                temp.Push(top);

                if (top == controller && m_ControllerStack.Count > 0)
                {
                    var previousController = m_ControllerStack.Peek();
                    previousController.gameObject.SetActive(active);
                    break;
                }
            }

            while (temp.Count > 0)
            {
                m_ControllerStack.Push(temp.Pop());
            }
        }

        protected static void HideController(Controller controller, bool active)
        {
            Stack<Controller> temp = new Stack<Controller>();

            while (m_ControllerStack.Count > 0)
            {
                var top = m_ControllerStack.Pop();
                temp.Push(top);

                if (top == controller && m_ControllerStack.Count > 0)
                {
                    var previousController = m_ControllerStack.Peek();
                    if (previousController.Animation.TryGetComponent<CanvasGroup>(out var canvasGroup))
                    {
                        canvasGroup.blocksRaycasts = active;
                    }
                    break;
                }
            }

            while (temp.Count > 0)
            {
                m_ControllerStack.Push(temp.Pop());
            }
        }

        protected static void Unload()
        {
            if (m_ControllerStack.Count > 0)
            {
                Unload(m_ControllerStack.Pop());
            }
        }

        protected static void Unload(Controller controller)
        {
            if (controller != null && controller.Data != null && controller.Data.scene != null)
            {
                SceneLoader.Unload(controller.Data);
            }
        }

        protected static Controller GetController(Scene scene)
        {
            var roots = scene.GetRootGameObjects();
            for (int i = 0; i < roots.Length; i++)
            {
                if (roots[i].TryGetComponent<Controller>(out var component))
                {
                    return component;
                }
            }
            return null;
        }

        /// <summary>
        /// Lấy controller trên cùng theo layer
        /// </summary>
        private static Controller FindTopController(NavigationLayer layer)
        {
            foreach (var controller in m_ControllerStack)
            {
                if (controller.Data.layer == layer)
                    return controller;
            }

            return null;
        }
    }
}
