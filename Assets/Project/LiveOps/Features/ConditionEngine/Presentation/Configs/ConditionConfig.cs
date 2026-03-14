using UnityEngine;
using ConditionEngine.Domain;
using Sirenix.OdinInspector;
using LBG;

namespace ConditionEngine.Presentation
{
    [CreateAssetMenu(menuName = "Game/Condition Config")]
    public class ConditionConfig : ScriptableObject
    {
        [SerializeReference]
        [SubclassSelector]
        [HideReferenceObjectPicker]
        public ConditionNode Root;

        public ICondition Build()
        {
            if (Root == null)
                return null;

            return Root.Build();
        }
    }
}