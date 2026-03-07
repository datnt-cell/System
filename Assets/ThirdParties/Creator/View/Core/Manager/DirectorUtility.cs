using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace Creator
{
    public partial class ManagerDirector
    {
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