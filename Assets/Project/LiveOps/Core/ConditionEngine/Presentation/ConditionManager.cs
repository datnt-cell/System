using UnityEngine;
using ConditionEngine.Application;
using ConditionEngine.Infrastructure;

namespace ConditionEngine
{
    /// <summary>
    /// Entry point của ConditionEngine
    /// </summary>
    public class ConditionManager : MonoBehaviour
    {
        public ConditionService Service { get; private set; }
        public IConditionEvents Events => Service?.Events;

        private ConditionInstaller _installer;

        public void Initialize()
        {
            _installer = new ConditionInstaller();

            var result = _installer.Install();

            Service = result.Service;

            Service.LoadConditions();
        }
    }
}