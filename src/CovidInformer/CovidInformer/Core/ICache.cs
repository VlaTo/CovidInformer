namespace CovidInformer.Core
{
    public interface ICache<in TKey, TValue>
    {
        TValue Get(TKey key);

        bool TryGet(TKey key, out TValue value);

        void Put(TKey key, TValue value);

        void Invalidate(TKey key);
    }
}