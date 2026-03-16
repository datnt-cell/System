using System;
using System.Collections.Generic;

namespace GachaSystem.Domain.Models
{
    [Serializable]
    public class GachaPool
    {
        public string Id { get; }

        public IReadOnlyList<GachaItem> Items { get; }

        public GachaPool(string id, List<GachaItem> items)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Items = items ?? throw new ArgumentNullException(nameof(items));
        }
    }
}