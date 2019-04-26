namespace HomeWork4
{
    public interface ITransactable<TKey, TValue> : IAlmostCrud<TKey, TValue>
    {
        Transaction<TKey, TValue> BeginTransaction();

        void Commit(Transaction<TKey, TValue> transaction);

        void RollbackTransation(Transaction<TKey, TValue> transaction);
    }
}
