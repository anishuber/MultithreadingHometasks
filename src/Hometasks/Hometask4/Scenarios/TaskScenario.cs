using Common.IO;
using FileReading.Concurrent.Tasks;
using Samples.CarLibrary;
using Samples.Storages;

namespace Scenarios
{
    // TODO: extract common functionality to interface?
    public static class TaskScenario
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

            var resultFiles = TaskClass.SerializeObjectsParallel(carObjects, directoryPath);

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

            var resultFilePath = TaskClass.SerializeObjectsInTurns<Car>(files[0], files[1], resultFileName);
            PrintFile(resultFilePath);

            return resultFilePath;
        }

        public static void PrintFile(string filePath)
        {
            FileAccessValidator.TryReadFile(filePath, out string contents);
            Console.WriteLine(contents);
        }

        public static async Task RunTask3(string filePath)
        {
            try
            {
                string contents = await TaskFileReader.ReadAllTextAsync(filePath);
                Console.WriteLine(contents);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            }
        }
    }
}
