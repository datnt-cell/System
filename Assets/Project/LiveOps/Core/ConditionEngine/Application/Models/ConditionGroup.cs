using System.Collections.Generic;
using ConditionEngine.Domain;

namespace ConditionEngine.Application
{
    /// <summary>
    /// Group chứa nhiều condition
    /// </summary>
    public class ConditionGroup
    {
        /// <summary>
        /// Root condition tree
        /// </summary>
        public ICondition RootCondition;

        public ConditionGroup(ICondition root)
        {
            RootCondition = root;
        }
    }
}