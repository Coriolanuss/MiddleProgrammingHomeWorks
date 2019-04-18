using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork3
{
    // DISCLAIMER: Sorry for all the mess below, no bright ideas at 4AM :)
    public class Program
    {
        static void Main(string[] args)
        {
            //List<RoomInfo> firstCollection = RoomInfo.ReadFromCsv(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Barcelona1.csv")).ToList();
            //List<RoomInfo> secondCollection = RoomInfo.ReadFromCsv(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\barcelona2.csv")).ToList();

            // Check if collections are sorted.
            //Console.WriteLine($"Is first sorted: {IsCollectionSorted(firstCollection)}");
            //Console.WriteLine($"Is second sorted: {IsCollectionSorted(secondCollection)}");

            // Check if the custom UNION ALL works.
            //var customUnionAll = HandMadeUnionAll(firstCollection.GetEnumerator(), secondCollection.GetEnumerator()).ToList();
            //var builtInUnionAll = firstCollection.Concat(secondCollection).OrderBy(x => x.id).ToList();
            //Console.WriteLine($"Are UNION ALL collections equal: {Enumerable.SequenceEqual(customUnionAll, builtInUnionAll)}");

            // Check if the custom UNION works.
            //var customUnion = HandMadeUnion(firstCollection.GetEnumerator(), secondCollection.GetEnumerator()).ToList();
            //var builtInUnion = firstCollection.Union(secondCollection, new RoomInfoEqualityComparer()).OrderBy(x => x.id).ToList();
            //Console.WriteLine($"Are UNION collections equal: {Enumerable.SequenceEqual(customUnion, builtInUnion)}");

            BenchmarkRunner.Run<Task3_Bechmarks>();
        }

        private static bool IsCollectionSorted(IEnumerable<RoomInfo> collection) =>
            Enumerable.SequenceEqual(collection, collection.OrderBy(x => x.id));

        public static IEnumerable<T> HandMadeUnionAll<T>(IEnumerator<T> first, IEnumerator<T> second)
            where T : IComparable<T>
        {
            if (!first.MoveNext())
            {
                // enumerate the second to the end, because the first has already ended.
                while (second.MoveNext())
                {
                    yield return second.Current;
                }
                yield break;
            }

            if (!second.MoveNext())
            {
                // enumerate the first to the end, because the second has already ended.
                while (first.MoveNext())
                {
                    yield return first.Current;
                }
                yield break;
            }

            while (true)
            {
                if (first.Current.CompareTo(second.Current) <= 0) // if the first item is less than or equal to the second item
                {
                    yield return first.Current;

                    if (!first.MoveNext())
                    {
                        // enumerate the second to the end, because the first has already ended.
                        yield return second.Current;
                        while (second.MoveNext())
                        {
                            yield return second.Current;
                        }
                        break;
                    }
                }
                else // if the first item is greater than the second item
                {
                    yield return second.Current;

                    if (!second.MoveNext())
                    {
                        // enumerate the first to the end, because the second has already ended.
                        yield return first.Current;
                        while (first.MoveNext())
                        {
                            yield return first.Current;
                        }
                        yield break;
                    }
                }
            }
        }

        public static IEnumerable<T> HandMadeUnion<T>(IEnumerator<T> first, IEnumerator<T> second)
            where T : IComparable<T>
        {
            if (!first.MoveNext())
            {
                // enumerate the second to the end, because the first has already ended.
                while (second.MoveNext())
                {
                    yield return second.Current;
                }
                yield break;
            }

            if (!second.MoveNext())
            {
                // enumerate the first to the end, because the second has already ended.
                while (first.MoveNext())
                {
                    yield return first.Current;
                }
                yield break;
            }

            while (true)
            {
                if (first.Current.CompareTo(second.Current) == 0) // if the first item is equal to the second item
                {
                    yield return first.Current;

                    // skip the duplicate
                    if (!second.MoveNext())
                    {
                        // enumerate the first to the end, because the second has already ended.
                        while (first.MoveNext())
                        {
                            yield return first.Current;
                        }
                        yield break;
                    }

                    if (!first.MoveNext())
                    {
                        // enumerate the second to the end, because the first has already ended.
                        while (second.MoveNext())
                        {
                            yield return second.Current;
                        }
                        break;
                    }
                }
                else if (first.Current.CompareTo(second.Current) < 0) // if the first item is less than the second item
                {
                    yield return first.Current;

                    if (!first.MoveNext())
                    {
                        // enumerate the second to the end, because the first has already ended.
                        yield return second.Current;
                        while (second.MoveNext())
                        {
                            yield return second.Current;
                        }
                        break;
                    }
                }
                else // if the first item is greater than the second item
                {
                    yield return second.Current;

                    if (!second.MoveNext())
                    {
                        // enumerate the first to the end, because the second has already ended.
                        yield return first.Current;
                        while (first.MoveNext())
                        {
                            yield return first.Current;
                        }
                        yield break;
                    }
                }
            }
        }
    }

    public class Task3_Bechmarks
    {
        List<RoomInfo> firstCollection = RoomInfo.ReadFromCsv(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Barcelona1.csv")).ToList();
        List<RoomInfo> secondCollection = RoomInfo.ReadFromCsv(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\barcelona2.csv")).ToList();

        [Benchmark]
        public void SelectUnionAll() =>
            Program.HandMadeUnionAll(firstCollection.GetEnumerator(), secondCollection.GetEnumerator()).
                Select(x => new { x.name, x.latitude, x.longitude }).ToList();

        [Benchmark]
        public void SelectUnion() =>
            Program.HandMadeUnion(firstCollection.GetEnumerator(), secondCollection.GetEnumerator()).
                Select(x => new { x.name, x.latitude, x.longitude }).ToList();
    }
}
