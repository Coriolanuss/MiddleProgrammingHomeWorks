namespace HomeWork4
{
    public interface IAlmostCrud<TKey, TValue>
    {
        void Add(TKey key, TValue value);

        TValue Get(TKey key);

        bool TryGet(TKey key, out TValue value);

        // Update(TKey key, TValue value);

        void Remove(TKey key);
    }
}
