using Hometask3.ThreadingClassLibrary;
using Helpers;

namespace Demo
{
    /// <summary>
    /// Demo task 3: compares reading a file with different thread counts and prints timings and results.
    /// </summary>
    public static class Task3
    {
        /// <summary>
        /// Executes task 3: runs file-read scenarios (1, 2, and 10 threads) and prints timing and content.
        /// </summary>
        /// <param name="path">Path to the file to read in the benchmarks.</param>
        public static void Run(string path)
        {
            Console.WriteLine("Running task 3");
            Console.WriteLine("\nOne thread read running: ");
            string res1 = Performance.MeasurePerformance<string>(() => ThreadingClass.ReadFileOneThread(path));
            Console.WriteLine(res1);

            Console.WriteLine("\nTwo thread read running: ");
            string res2 = Performance.MeasurePerformance<string>(() => ThreadingClass.ReadFileTwoThreads(path));
            Console.WriteLine(res2);

            Console.WriteLine("\nThree thread read running: ");
            string res3 = Performance.MeasurePerformance<string>(() => ThreadingClass.ReadFileTenThreads(path));
            Console.WriteLine(res3);
        }
    }
}
