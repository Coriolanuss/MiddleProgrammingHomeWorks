using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeWork2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //new Task2_Bechmarks().SequentialKeys_ConstHash();
            //new Task2_Bechmarks().RandomKeys_ConstHash();
            //new Task2_Bechmarks().SequentialKeys_SelfHash();
            //new Task2_Bechmarks().RandomKeys_SelfHash();
            //new Task2_Bechmarks().SequentialKeys_SimpleHash();
            //new Task2_Bechmarks().RandomKeys_SimpleHash();
            //new Task2_Bechmarks().SequentialKeys_ComplexHash();
            //new Task2_Bechmarks().RandomKeys_ComplexHash();

            BenchmarkRunner.Run<Task2_Bechmarks>();
        }
    }

    [MemoryDiagnoser]
    public class Task2_Bechmarks
    {
        private const int itemsAmount = 1_000_000;
        private static Random random = new Random(42);

        [Benchmark]
        public void SequentialKeys_ConstHash() => AddItemsToContainer<ConstHashInt>(Enumerable.Range(0, itemsAmount));
        [Benchmark]
        public void RandomKeys_ConstHash() => AddItemsToContainer<ConstHashInt>(Enumerable.Range(0, itemsAmount).OrderBy(x => random.Next()));
        [Benchmark]
        public void SequentialKeys_SelfHash() => AddItemsToContainer<SelfHashInt>(Enumerable.Range(0, itemsAmount));
        [Benchmark]
        public void RandomKeys_SelfHash() => AddItemsToContainer<SelfHashInt>(Enumerable.Range(0, itemsAmount).OrderBy(x => random.Next()));
        [Benchmark]
        public void SequentialKeys_SimpleHash() => AddItemsToContainer<SimpleHashInt>(Enumerable.Range(0, itemsAmount));
        [Benchmark]
        public void RandomKeys_SimpleHash() => AddItemsToContainer<SimpleHashInt>(Enumerable.Range(0, itemsAmount).OrderBy(x => random.Next()));
        [Benchmark]
        public void SequentialKeys_ComplexHash() => AddItemsToContainer<ComplexHashInt>(Enumerable.Range(0, itemsAmount));
        [Benchmark]
        public void RandomKeys_ComplexHash() => AddItemsToContainer<ComplexHashInt>(Enumerable.Range(0, itemsAmount).OrderBy(x => random.Next()));

        private void AddItemsToContainer<TInt>(IEnumerable<int> keys)
            where TInt : CustomHashInt, new()
        {
            var container = new CustomDataStructure<TInt, object>();

            foreach (var key in keys)
            {
                container.Add(new TInt { x = key }, new object());
            }
        }
    }
}
