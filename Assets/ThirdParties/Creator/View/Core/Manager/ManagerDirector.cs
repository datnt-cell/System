using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace Creator
{
    public class ManagerDirector : ManagerBase
    {
        private static Data CreateAndQueue(
            string sceneName,
            object data,
            Callback onShown,
            Callback onHidden,
            bool hasShield,
            SceneLoadMode loadMode)
        {
            var sceneData = new Data(
                data,
                sceneName,
                onShown,
                onHidden,
                hasShield,
                loadMode
            );

            m_DataQueue.Enqueue(sceneData);
            return sceneData;
        }

        private static void LoadAdditive(Data sceneData)
        {
            SceneLoader.Load(sceneData, LoadSceneMode.Additive);
        }

        public static void RunScene(
            string sceneName,
            object data = null,
            string log = "",
            SceneLoadMode loadMode = SceneLoadMode.BuildIn)
        {
            var sceneData = CreateAndQueue(
                sceneName,
                data,
                onShown: null,
                onHidden: null,
                hasShield: true,
                loadMode
            );

            m_MainSceneName = sceneName;
            Object.FadeOutScene();
        }

        public static void PushScene(
            string sceneName,
            object data = null,
            Callback onShown = null,
            Callback onHidden = null,
            bool hasShield = true,
            string log = "",
            SceneLoadMode loadMode = SceneLoadMode.BuildIn)
        {
            var sceneData = CreateAndQueue(
                sceneName,
                data,
                onShown,
                onHidden,
                hasShield,
                loadMode
            );

            Object.ShieldOn();

            if (m_ControllerStack.Count > 0)
                m_ControllerStack.Peek().GetCanvasGroup().blocksRaycasts = false;

            LoadAdditive(sceneData);
        }

        public static void PushSceneTracked(
            string sceneName,
            object data = null,
            Callback onShown = null,
            UnityAction onHidden = null,
            bool hasShield = true,
            string log = "",
            SceneLoadMode loadMode = SceneLoadMode.BuildIn)
        {
            float startTime = Time.realtimeSinceStartup;

            PushScene(
                sceneName,
                data,
                onShown,
                () =>
                {
                    float duration = Time.realtimeSinceStartup - startTime;

                    // TODO: Hook Firebase / Adjust event here
                    // Analytics.LogSceneDuration(sceneName, duration);

                    onHidden?.Invoke();
                },
                hasShield,
                log,
                loadMode
            );
        }

        public static void ReplaceScene(
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
            currentController.HidePopup(false);

            Callback wrappedHidden = () =>
            {
                onHidden?.Invoke();
                currentController.ShowPopup(false);
            };

            var sceneData = CreateAndQueue(
                sceneName,
                data,
                onShown,
                wrappedHidden,
                hasShield: false,
                loadMode
            );

            Object.ShieldOn();
            LoadAdditive(sceneData);
        }

        public static void PopScene()
        {
            if (m_ControllerStack.Count <= 1)
            {
                return;
            }

            ActivatePreviousController(true);
            HideController(true);

            Object.ShieldOn();
            m_ControllerStack.Peek().Hide();
        }

        public static Controller GetRunningScene()
        {
            return m_ControllerStack.First();
        }

        public static void Pause()
        {
            Time.timeScale = 0f;
        }

        public static void Resume()
        {
            Time.timeScale = 1f;
        }

        public static void PopToRootScene()
        {
            if (m_ControllerStack == null || m_ControllerStack.Count == 0)
                return;

            while (m_ControllerStack.Count > 1)
            {
                PopTopControllerImmediate();
            }

            var rootController = m_ControllerStack.Peek();
            if (rootController == null)
                return;

            if (rootController.Animation
                .TryGetComponent<CanvasGroup>(out var canvasGroup))
            {
                canvasGroup.blocksRaycasts = true;
            }
        }

        public static void PopScenesImmediate(string[] sceneNames)
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

            while (tempStack.Count > 0)
            {
                m_ControllerStack.Push(tempStack.Pop());
            }

            if (m_ControllerStack.Count > 0)
            {
                m_ControllerStack.Peek().OnReFocus();
            }
        }

        protected static void PopTopControllerImmediate()
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
    }
}
