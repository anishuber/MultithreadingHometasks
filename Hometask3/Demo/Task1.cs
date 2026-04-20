using Hometask3.ThreadingClassLibrary;
using CarLibrary;
using Helpers;

namespace Demo
{
    public class Task1
    {
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
