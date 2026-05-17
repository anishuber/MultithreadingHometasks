using Scenarios;

namespace Demo
{
    /// <summary>
    /// Program entry point for the demo application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main entry point. Executes demo tasks in order.
        /// </summary>
        public static void Main()
        {
            ThreadingScenario.RunTask1();
            string path = ThreadingScenario.RunTask2();
            ThreadingScenario.RunTask3(path);
        }
    }
}
