using System.Collections.Generic;

namespace HomeWork4
{
    public class TransactionalHashTable<TKey, TValue> : ITransactable<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> hashTable = new Dictionary<TKey, TValue>();

        public TransactionalHashTable(params (TKey key, TValue value)[] initialCollection)
        {
            foreach (var (key, value) in initialCollection)
            {
                hashTable.Add(key, value);
            }
        }

        public void Add(TKey key, TValue value) => hashTable.Add(key, value);

        public TValue Get(TKey key) => hashTable[key];

        public bool TryGet(TKey key, out TValue value)
        {
            bool result = hashTable.TryGetValue(key, out TValue outValue);
            value = result ? outValue : default;
            return result;
        }

        public void Remove(TKey key) => hashTable.Remove(key);

        public int Count => hashTable.Count;

        public Transaction<TKey, TValue> BeginTransaction()
        {
            var newTransaction = new Transaction<TKey, TValue>(this);
            return newTransaction;
        }

        public void Commit(Transaction<TKey, TValue> transaction) => transaction.Execute();

        public void RollbackTransation(Transaction<TKey, TValue> transaction) => transaction.RollBack();
    }
}
