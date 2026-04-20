using Hometask3.ThreadingClassLibrary;
using Helpers;

namespace Demo
{
    public class Task3
    {
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
