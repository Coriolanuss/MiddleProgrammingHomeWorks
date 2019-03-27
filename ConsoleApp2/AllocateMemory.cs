using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.IO;

namespace MiddleProgrammingHomeWorks
{
    [LegacyJitX86Job, LegacyJitX64Job, RyuJitX64Job]
    [MemoryDiagnoser]
    public class AllocateMemory
    {
        // Allocates series 2GB streams - maximum index in any single dimension is 2,147,483,591 for byte arrays.
        [Benchmark]
        public void AllocateMemoryFillingStreams()
        {
            List<(MemoryStream, long)> lists = new List<(MemoryStream, long)>();

            try
            {
                while (true)
                {
                    lists.Add(FillMemoryStream(new MemoryStream()));
                }
            }
            catch (OutOfMemoryException)
            {
                // Couldn't create another LinkedList object, haven't reached that
            }
        }

        private (MemoryStream, long) FillMemoryStream(MemoryStream stream)
        {
            try
            {
                while (true)
                {
                    stream.WriteByte(1);
                }
            }
            catch (OutOfMemoryException)
            {
            }

            return (stream, stream.Length);
        }

        // Have seen ~28GB allocated.
        [Benchmark]
        public void AllocateMemoryGrowingLinkedList()
        {
            var memoryBefore = GC.GetTotalMemory(false);
            LinkedList<byte[]> byteLinkedList = new LinkedList<byte[]>();

            try
            {
                while (true)
                {
                    byteLinkedList.AddLast(new byte[1024 * 1024]); // 1MB
                }
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine($"Total allocated memory: {(GC.GetTotalMemory(false) - memoryBefore) / (1024.0 * 1024.0)} MB.");
            }
        }
    }
}
