using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;

namespace MiddleProgrammingHomeWorks
{
    [LegacyJitX86Job, LegacyJitX64Job, RyuJitX64Job]
    [MemoryDiagnoser]
    public class Task1_AllocateTotalMemory
    {
        [Benchmark]
        public void AllocateMemoryFillingStreams()
        {
            List<(LinkedList<byte>, long)> lists = new List<(LinkedList<byte>, long)>();

            try
            {
                while (true)
                {
                    lists.Add(FillMemoryStream(new LinkedList<byte>()));
                }
            }
            catch (OutOfMemoryException)
            {
                // Couldn't create another LinkedList
            }
        }

        private (LinkedList<byte>, long) FillMemoryStream(LinkedList<byte> list)
        {
            try
            {
                while (true)
                {
                    list.AddLast(1);
                }
            }
            catch (OutOfMemoryException)
            {
            }

            return (list, list.Count);
        }

        [Benchmark]
        public void AllocateMemoryGrowingLinkedList()
        {
            LinkedList<byte> byteLinkedList = new LinkedList<byte>();

            try
            {
                while (true)
                {
                    byteLinkedList.AddLast(1);
                }
            }
            catch (OutOfMemoryException)
            {
            }
        }
    }
}
