using UnityEngine;
using Game.Application.Tasks;
using Game.Domain.Tasks;
using Game.Installer;
using System;

namespace Game.Manager
{
    /// <summary>
    /// TaskManager runtime
    /// - Attach 1 instance duy nhất vào scene
    /// - Chỉ expose Service + TaskEvents
    /// </summary>
    public class TaskManager : MonoBehaviour
    {
        public TaskService Service { get; private set; }
        public TaskEvents Events { get; private set; }

        private TaskInstaller _installer;

        public void Initialize()
        {
            var result = _installer.Install();

            Service = result.Service;
            Events = result.Events;
        }
    }
}