using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeWork2
{
    // Not thread-safe
    public class CustomDataStructure<TKey, TValue>
        where TKey : CustomHashInt
    {
        private (TKey key, TValue value)[] array = new (TKey, TValue)[1000];
        private Dictionary<TKey, TValue> dictionary;
        bool useDictionary = false;

        public void Add(TKey key, TValue value)
        {
            if (!useDictionary)
            {
                // Dictionary hasn't been utilized yet.
                if (IsSmallGap(key.x))
                {
                    // Gap is insignificant, continue using array.
                    AddToArray(key, value);
                }
                else
                {
                    // Move to using dictionary from now on.
                    useDictionary = true;
                    InitializeDictionary();
                    dictionary.Add(key, value);
                }
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        private bool IsSmallGap(int newKey) => (array.Length * 2) > newKey;

        private void AddToArray(TKey key, TValue value)
        {
            if (array.Length <= key.x)
            {
                Array.Resize(ref array, array.Length * 2);
            }

            array[key.x] = (key, value);
        }

        private void InitializeDictionary()
        {
            var nonemptyItems = array.Where(x => x.key != null).ToArray();

            dictionary = new Dictionary<TKey, TValue>(nonemptyItems.Length);

            for (int i = 0; i < nonemptyItems.Length; i++)
            {
                dictionary.Add(nonemptyItems[i].key, nonemptyItems[i].value);
            }
        }
    }
}
