using UnityEngine;
using GameSystems.Random.Interfaces;

namespace GameSystems.Random.Providers
{
    public class UnityRandomProvider : IRandomProvider
    {
        public int Range(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public float Value()
        {
            return UnityEngine.Random.value;
        }
    }
}