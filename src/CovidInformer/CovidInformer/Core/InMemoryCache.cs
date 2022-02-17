using System;
using System.Collections.Generic;

namespace CovidInformer.Core
{
    internal sealed class InMemoryCache<TKey, TValue> : ICache<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> cache;

        public InMemoryCache()
        {
            cache = new Dictionary<TKey, TValue>();
        }

        public InMemoryCache(IEnumerable<KeyValuePair<TKey, TValue>> items)
            : this()
        {
            foreach (var kvp in items)
            {
                cache.Add(kvp.Key, kvp.Value);
            }
        }

        public TValue Get(TKey key)
        {
            if (false == cache.TryGetValue(key, out var value))
            {
                throw new ArgumentOutOfRangeException(nameof(key));
            }

            return value;
        }

        public bool TryGet(TKey key, out TValue value)
        {
            return cache.TryGetValue(key, out value);
        }

        public void Put(TKey key, TValue value)
        {
            cache[key] = value;
        }

        public void Invalidate(TKey key)
        {
            cache.Remove(key);
        }
    }
}