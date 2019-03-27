using BenchmarkDotNet.Running;

namespace MiddleProgrammingHomeWorks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Task1_AllocateTotalMemory>();
        }
    }
}
