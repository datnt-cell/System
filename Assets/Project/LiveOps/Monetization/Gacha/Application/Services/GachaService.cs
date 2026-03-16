using System;
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

        public GachaService(
            IRandomProvider random,
            IGachaEvents events)
        {
            _random = random ?? throw new ArgumentNullException(nameof(random));
            _events = events ?? throw new ArgumentNullException(nameof(events));
        }

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
    }
}