using GameSystems.Random.Interfaces;

namespace GameSystems.Random.Pickers
{
    public static class ProbabilityRandom
    {
        public static bool Roll(
            float probability,
            IRandomProvider random)
        {
            return random.Value() <= probability;
        }
    }
}