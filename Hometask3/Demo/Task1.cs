using Hometask3.ThreadingClassLibrary;
using CarLibrary;
using Helpers;

namespace Demo
{
    /// <summary>
    /// Demo task 1: serializes the in-memory car list into chunked XML files and prints each chunk's contents.
    /// </summary>
    public static class Task1
    {
        /// <summary>
        /// Executes task 1: serialize sample cars in parallel and print the resulting files' contents.
        /// </summary>
        public static void Run()
        {
            Console.WriteLine("Running task 1\n");
            string directoryPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                @"..\..\..\..",
                "SerializedObjects");

            Validators.ValidateDirectoryPath(directoryPath);

            var carObjects = InMemoryCarStorage.Cars;

            var resultFiles = ThreadingClass.SerializeObjectsParallel<Car>(carObjects, directoryPath);

            foreach (var file in resultFiles)
            {
                Console.WriteLine($"\nContents of file {file}:");
                DisplayConsole.PrintFileContents(Path.Combine(directoryPath, file));
            }
        }
    }
}
