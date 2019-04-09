using BenchmarkDotNet.Running;

namespace MiddleProgrammingHomeWorks
{
    class Program
    {
        static void Main(string[] args)
        {
            //new AllocateMemory().AllocateMemoryFillingStreams();
            new AllocateMemory().AllocateMemoryGrowingLinkedList();
        }
    }
}
