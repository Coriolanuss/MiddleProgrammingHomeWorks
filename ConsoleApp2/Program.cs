using BenchmarkDotNet.Running;

namespace MiddleProgrammingHomeWorks
{
    class Program
    {
        static void Main(string[] args)
        {
            //BenchmarkRunner.Run<Task1_AllocateTotalMemory>();
            //new AllocateMemory().AllocateMemoryFillingStreams();
            new AllocateMemory().AllocateMemoryGrowingLinkedList();
        }
    }
}
