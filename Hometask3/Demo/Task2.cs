using Hometask3.ThreadingClassLibrary;
using SerializationService;
using CarLibrary;
using Helpers;

namespace Demo
{
    /// <summary>
    /// Demo task 2: merges two serialized XML files of <see cref="Car"/> into a single result file and prints it.
    /// </summary>
    public static class Task2
    {
        /// <summary>
        /// Executes task 2: finds two XML files, merges them into a single result XML and prints it.
        /// </summary>
        /// <returns>Path to the merged result XML file.</returns>
        public static string Run()
        {
            Console.WriteLine("\nRunning task 2\n");
            ThreadingClass tc = new ThreadingClass();

            string directoryPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                @"..\..\..\..",
                "SerializedObjects");

            Validators.ValidateDirectoryPath(directoryPath);

            string resultFileName = "resultFile.xml";
            var files = Directory.EnumerateFiles(directoryPath, "*.xml", SearchOption.TopDirectoryOnly)
                .Where(path => !string.Equals(
                    Path.GetFileName(path),
                    resultFileName,
                    StringComparison.OrdinalIgnoreCase))
                .Take(2)
                .ToArray();

            var resultFilePath = tc.ReadObjectsParallel<Car>(files[0], files[1], resultFileName);
            DisplayConsole.PrintFileContents(resultFilePath);

            return resultFilePath;
        }
    }
}
