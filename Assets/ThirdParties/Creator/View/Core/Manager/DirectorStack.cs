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
    }
}