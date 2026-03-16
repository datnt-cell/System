using System;
using GameSystems.Random.Interfaces;

namespace GameSystems.Random.Providers
{
    public class SeededRandomProvider : IRandomProvider
    {
        private readonly System.Random _random;

        public SeededRandomProvider(int seed)
        {
            _random = new System.Random(seed);
        }

        public int Range(int min, int max)
        {
            return _random.Next(min, max);
        }

        public float Value()
        {
            return (float)_random.NextDouble();
        }
    }
}