using System;
using ConditionEngine.Domain;
using LBG;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ConditionEngine.Presentation
{
    [Serializable]
    public class ConditionEntry
    {
        [BoxGroup("Info")]
        [ReadOnly]
        [LabelWidth(40)]
        public string Id;

        [BoxGroup("Info")]
        [LabelWidth(40)]
        public string DisplayName;

        [BoxGroup("Condition")]
        [SerializeReference]
        [SubclassSelector]
        [HideReferenceObjectPicker]
        public ConditionNode Node;

        public ICondition Build()
        {
            if (Node == null)
                return null;

            return Node.Build();
        }
    }
}