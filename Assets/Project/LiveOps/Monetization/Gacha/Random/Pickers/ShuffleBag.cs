using System.Collections.Generic;
using GameSystems.Random.Interfaces;

namespace GameSystems.Random.Pickers
{
    public class ShuffleBag<T>
    {
        private readonly List<T> _items = new();
        private readonly IRandomProvider _random;

        private int _cursor;

        public ShuffleBag(IEnumerable<T> items, IRandomProvider random)
        {
            _items.AddRange(items);
            _random = random;

            Shuffle();
        }

        public T Next()
        {
            if (_cursor >= _items.Count)
            {
                Shuffle();
                _cursor = 0;
            }

            return _items[_cursor++];
        }

        private void Shuffle()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                int rand = _random.Range(i, _items.Count);

                (_items[i], _items[rand]) = (_items[rand], _items[i]);
            }
        }
    }
}