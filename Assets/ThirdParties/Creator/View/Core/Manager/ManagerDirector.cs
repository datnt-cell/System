using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace Creator
{
    public partial class ManagerDirector : ManagerBase
    {
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
    }
}
