namespace CovidInformer.Core
{
    internal sealed class CachedValue<TValue>
        where TValue : class
    {
        private TValue item;

        public CachedValue()
        {
            item = null;
        }

        public bool TryGetValue(out TValue value)
        {
            if (null == item)
            {
                value = null;
                return false;
            }

            value = item;

            return true;
        }

        public void Set(TValue value)
        {
            item = value;
        }

        public void Invalidate()
        {
            item = null;
        }
    }
}