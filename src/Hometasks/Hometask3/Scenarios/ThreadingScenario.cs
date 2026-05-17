using Common.Diagnostics;
using Common.IO;
using Samples.CarLibrary;
using Samples.Storages;

namespace Scenarios
{
    public static class ThreadingScenario
    {
        public static void RunTask1()
        {
            Console.WriteLine("Running task 1\n");
            string directoryPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                @"..\..\..\..\..\..\..",
                "artifacts");

            PathValidator.ValidateDirectoryPath(directoryPath);

            var carObjects = InMemoryCarStorage.Cars.ToList();

            var resultFiles = ThreadingClass.SerializeObjectsParallel<Car>(carObjects, directoryPath);

            foreach (var file in resultFiles)
            {
                Console.WriteLine($"\nContents of file {file}:");
                PrintFile(Path.Combine(directoryPath, file));
            }
        }

        public static string RunTask2()
        {
            Console.WriteLine("\nRunning task 2\n");

            string directoryPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                @"..\..\..\..\..\..\..",
                "artifacts");

            PathValidator.ValidateDirectoryPath(directoryPath);

            string resultFileName = "resultFile.xml";
            var files = Directory.EnumerateFiles(directoryPath, "*.xml", SearchOption.TopDirectoryOnly)
                .Where(path => !string.Equals(
                    Path.GetFileName(path),
                    resultFileName,
                    StringComparison.OrdinalIgnoreCase))
                .Take(2)
                .ToArray();

            var resultFilePath = ThreadingClass.SerializeObjectsParallelThreads<Car>(files[0], files[1], resultFileName);
            PrintFile(resultFilePath);

            return resultFilePath;
        }

        public static void RunTask3(string path)
        {
            Console.WriteLine("Running task 3");
            Console.WriteLine("\nOne thread read running: ");
            (string res1, TimeSpan elapsed1) = Performance.Measure<string>(() => ThreadingClass.ReadFileOneThread(path));
            Console.WriteLine(res1);
            Console.WriteLine(elapsed1.Seconds);
            Console.WriteLine(elapsed1.Ticks);

            Console.WriteLine("\nTwo thread read running: ");
            (string res2, TimeSpan elapsed2) = Performance.Measure<string>(() => ThreadingClass.ReadFileTwoThreads(path));
            Console.WriteLine(res2);
            Console.WriteLine(elapsed2.Seconds);
            Console.WriteLine(elapsed2.Ticks);

            Console.WriteLine("\nThree thread read running: ");
            (string res3, TimeSpan elapsed3) = Performance.Measure<string>(() => ThreadingClass.ReadFileTenThreads(path));
            Console.WriteLine(res3);
            Console.WriteLine(elapsed3.Seconds);
            Console.WriteLine(elapsed3.Ticks);
        }

        public static void PrintFile(string filePath)
        {
            FileAccessValidator.TryReadFile(filePath, out string contents);
            Console.WriteLine(contents);
        }
    }
}
