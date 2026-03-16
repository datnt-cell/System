using System;
using System.Collections.Generic;
using GameSystems.Random.Interfaces;

namespace GameSystems.Random.Pickers
{
    public static class WeightedRandomPicker
    {
        public static int PickIndex<T>(
            IReadOnlyList<T> items,
            Func<T, int> weightSelector,
            IRandomProvider random)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            if (items.Count == 0)
                throw new Exception("WeightedRandomPicker: items empty");

            int totalWeight = 0;

            for (int i = 0; i < items.Count; i++)
            {
                int weight = weightSelector(items[i]);

                if (weight > 0)
                    totalWeight += weight;
            }

            if (totalWeight <= 0)
                throw new Exception("WeightedRandomPicker: total weight = 0");

            int rand = random.Range(0, totalWeight);

            for (int i = 0; i < items.Count; i++)
            {
                int weight = weightSelector(items[i]);

                if (weight <= 0)
                    continue;

                if (rand < weight)
                    return i;

                rand -= weight;
            }

            return items.Count - 1;
        }
    }
}