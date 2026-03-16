using System;
using System.Collections.Generic;
using System.Linq;
using GachaSystem.Domain.Models;
using GameSystems.Random.Interfaces;
using GameSystems.Random.Pickers;
using GachaSystem.Application.Interfaces;

namespace GachaSystem.Application.Services
{
    public class GachaService
    {
        private readonly IRandomProvider _random;
        private readonly IGachaEvents _events;

        private readonly Dictionary<string, GachaPool> _pools = new();

        public GachaService(
            IRandomProvider random,
            IGachaEvents events)
        {
            _random = random ?? throw new ArgumentNullException(nameof(random));
            _events = events ?? throw new ArgumentNullException(nameof(events));
        }

        // =========================
        // POOL MANAGEMENT
        // =========================

        public void RegisterPool(GachaPool pool)
        {
            if (pool == null)
                throw new ArgumentNullException(nameof(pool));

            if (string.IsNullOrEmpty(pool.Id))
                throw new Exception("Pool Id cannot be empty");

            if (pool.Items == null || pool.Items.Count == 0)
                throw new Exception($"Pool '{pool.Id}' has no items");

            _pools[pool.Id] = pool;

            // EVENT
            _events.Publish(GachaEvent.PoolRegistered(pool.Id));
        }

        public GachaPool GetPool(string poolId)
        {
            if (!_pools.TryGetValue(poolId, out var pool))
                throw new Exception($"GachaPool '{poolId}' not found");

            return pool;
        }

        public IReadOnlyCollection<GachaPool> GetAllPools()
        {
            return _pools.Values;
        }

        // =========================
        // ROLL BY POOL ID
        // =========================

        public GachaResult Roll(string poolId)
        {
            var pool = GetPool(poolId);
            return Roll(pool);
        }

        // =========================
        // ROLL CORE
        // =========================

        public GachaResult Roll(GachaPool pool)
        {
            if (pool == null)
                throw new ArgumentNullException(nameof(pool));

            var items = pool.Items;

            if (items == null || items.Count == 0)
                throw new Exception($"GachaPool '{pool.Id}' has no items");

            int totalWeight = 0;

            for (int i = 0; i < items.Count; i++)
            {
                int w = items[i].Weight;
                if (w > 0)
                    totalWeight += w;
            }

            if (totalWeight <= 0)
                throw new Exception($"GachaPool '{pool.Id}' total weight = 0");

            try
            {
                _events.Publish(GachaEvent.Start(pool.Id));

                int index = WeightedRandomPicker.PickIndex(
                    items,
                    x => x.Weight,
                    _random
                );

                var item = items[index];

                var result = new GachaResult(
                    pool.Id,
                    item,
                    index
                );

                _events.Publish(GachaEvent.ResultEvent(result));

                return result;
            }
            catch (Exception e)
            {
                _events.Publish(GachaEvent.Error(pool.Id, e));
                throw;
            }
        }

        // =========================
        // MULTI ROLL
        // =========================

        public List<GachaResult> RollMultiple(string poolId, int count)
        {
            if (count <= 0)
                throw new ArgumentException("Roll count must be > 0");

            var pool = GetPool(poolId);

            var results = new List<GachaResult>(count);

            try
            {
                _events.Publish(GachaEvent.Start(poolId));

                for (int i = 0; i < count; i++)
                {
                    var result = Roll(pool);
                    results.Add(result);
                }

                _events.Publish(GachaEvent.MultiResult(poolId, results));

                return results;
            }
            catch (Exception e)
            {
                _events.Publish(GachaEvent.Error(poolId, e));
                throw;
            }
        }

        // =========================
        // DEBUG / INFO
        // =========================

        public bool HasPool(string poolId)
        {
            return _pools.ContainsKey(poolId);
        }

        public int GetPoolItemCount(string poolId)
        {
            return GetPool(poolId).Items.Count;
        }
    }
}