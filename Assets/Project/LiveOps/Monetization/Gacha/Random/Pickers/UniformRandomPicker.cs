using System.Collections.Generic;
using GameSystems.Random.Interfaces;

namespace GameSystems.Random.Pickers
{
    public static class UniformRandomPicker
    {
        public static int PickIndex<T>(
            IList<T> items,
            IRandomProvider random)
        {
            return random.Range(0, items.Count);
        }
    }
}