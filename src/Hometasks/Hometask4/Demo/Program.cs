using Scenarios;

namespace Demo
{
    internal static class Program
    {
        static async Task Main()
        {
            TaskScenario.RunTask1();
            var filePath = TaskScenario.RunTask2();
            await TaskScenario.RunTask3(filePath);
        }
    }
}
