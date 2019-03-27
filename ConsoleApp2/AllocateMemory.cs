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
        // Allocates series 2GB streams - max CLR object size (?).
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

        // Have seen 28GB allocated in debugger disgnostics window when everything crashed.
        [Benchmark]
        public void AllocateMemoryGrowingLinkedList()
        {
            LinkedList<byte[]> byteLinkedList = new LinkedList<byte[]>();

            try
            {
                while (true)
                {
                    byteLinkedList.AddLast(new byte[1024 * 100]);
                }
            }
            catch (OutOfMemoryException)
            {
            }
        }
    }
}
