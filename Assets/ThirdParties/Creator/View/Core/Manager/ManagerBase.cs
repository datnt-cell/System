using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Creator
{
    public enum SceneLoadMode
    {
        BuildIn,
        Addressable
    }

    public class ManagerBase
    {
        public class Data
        {
            // ===================== PAYLOAD =====================
            public object data;

            // ===================== CALLBACK =====================
            public ManagerBase.Callback onShown;
            public ManagerBase.Callback onHidden;

            // ===================== SCENE INFO =====================
            public string sceneName;
            public Scene scene; // luôn được gán trong OnSceneLoaded
            public SceneLoadMode loadMode;

            // 🔥 CHỈ dùng khi load bằng Addressables
            public AsyncOperationHandle<SceneInstance>? addressableHandle;

            // ===================== UI / FLOW =====================
            public bool hasShield;

            // ===================== STATE =====================
            public bool IsAddressable => loadMode == SceneLoadMode.Addressable;
            public bool IsLoaded =>
             IsAddressable
                 ? addressableHandle.HasValue && addressableHandle.Value.IsValid()
                 : scene.IsValid();

            // ===================== CTOR =====================
            public Data(
                object data,
                string sceneName,
                ManagerBase.Callback onShown = null,
                ManagerBase.Callback onHidden = null,
                bool hasShield = true,
                SceneLoadMode loadMode = SceneLoadMode.BuildIn)
            {
                this.data = data;
                this.sceneName = sceneName;
                this.loadMode = loadMode;
                this.onShown = onShown;
                this.onHidden = onHidden;
                this.hasShield = hasShield;

                scene = default;
                addressableHandle = null;
            }
        }

        public delegate void Callback();

        protected static Stack<Controller> m_ControllerStack = new Stack<Controller>();

        protected static Queue<Data> m_DataQueue = new Queue<Data>();

        protected static float m_TimeScene;

        protected static string m_SceneNow;

        public static int stackCount
        {
            get
            {
                return m_ControllerStack.Count;
            }
        }

        public static Controller MainController
        {
            get
            {
                return m_MainController;
            }
        }

        public static float SceneAnimationDuration
        {
            get;
            set;
        }

        public static float ButtonAnimationDuration
        {
            get;
            set;
        }

        public static ManagerObject Object
        {
            get;
            protected set;
        }

        protected static string m_MainSceneName;

        protected static Controller m_MainController;
    }
}
