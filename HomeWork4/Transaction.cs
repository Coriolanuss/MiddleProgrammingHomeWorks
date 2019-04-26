using System;
using System.Collections.Generic;

namespace HomeWork4
{
    public class Transaction<TKey, TValue> : IAlmostCrud<TKey, TValue>, IDisposable
    {
        private readonly ITransactable<TKey, TValue> owner;
        private readonly IDictionary<TKey, TValue> positiveDelta = new Dictionary<TKey, TValue>();
        private readonly IDictionary<TKey, TValue> negativeDelta = new Dictionary<TKey, TValue>();
        private bool finished = false;

        internal Transaction(ITransactable<TKey, TValue> owner)
        {
            this.owner = owner ?? throw new ArgumentNullException(nameof(owner));
        }

        public void Add(TKey key, TValue value)
        {
            ThrowIfFinished();

            if (negativeDelta.ContainsKey(key))
            {
                negativeDelta.Remove(key);
            }
            else if (owner.TryGet(key, out _) || positiveDelta.ContainsKey(key))
            {
                throw new ArgumentException("An item with the same key has already been added.");
            }
            else
            {
                positiveDelta.Add(key, value);
            }
        }

        public TValue Get(TKey key)
        {
            ThrowIfFinished();

            if (negativeDelta.TryGetValue(key, out _))
            {
                throw new KeyNotFoundException("The given key was not present in the dictionary.");
            }
            else if (owner.TryGet(key, out TValue ownerValue))
            {
                return ownerValue;
            }
            else if (positiveDelta.TryGetValue(key, out TValue deltaValue))
            {
                return deltaValue;
            }
            else
            {
                throw new KeyNotFoundException("The given key was not present in the dictionary.");
            }
        }

        public bool TryGet(TKey key, out TValue value)
        {
            ThrowIfFinished();

            if (negativeDelta.TryGetValue(key, out _))
            {
                value = default;
                return false;
            }
            else if (owner.TryGet(key, out TValue ownerValue))
            {
                value = ownerValue;
                return true;
            }
            else if (positiveDelta.TryGetValue(key, out TValue deltaValue))
            {
                value = deltaValue;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public void Remove(TKey key)
        {
            ThrowIfFinished();

            if (owner.TryGet(key, out TValue value))
            {
                negativeDelta.Add(key, value);
            }
            else if (positiveDelta.TryGetValue(key, out _))
            {
                positiveDelta.Remove(key);
            }
            else
            {
                // Do nothing, just like Dictionary<TKey, TValue>.
            }
        }

        public int ChangesCount => positiveDelta.Count + negativeDelta.Count;

        internal void Execute()
        {
            ThrowIfFinished();

            foreach (var itemToRemove in negativeDelta)
            {
                owner.Remove(itemToRemove.Key);
            }

            foreach (var itemToAdd in positiveDelta)
            {
                owner.Add(itemToAdd.Key, itemToAdd.Value);
            }

            finished = true;
        }

        internal void RollBack()
        {
            ThrowIfFinished();

            negativeDelta.Clear();
            positiveDelta.Clear();
            finished = true;
        }

        private void ThrowIfFinished()
        {
            if (finished)
            {
                throw new InvalidOperationException("The transaction has already finished.");
            }
        }

        public void Dispose() => Execute();
    }
}
