using Hometask3.ThreadingClassLibrary;
using SerializationService;
using CarLibrary;
using Helpers;

namespace Demo
{
    public class Task2
    {
        public static string Run()
        {
            Console.WriteLine("\nRunning task 2\n");
            ThreadingClass tc = new ThreadingClass();

            string directoryPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                @"..\..\..\..",
                "SerializedObjects");

            Validators.ValidateDirectoryPath(directoryPath);

            var files = Directory.EnumerateFiles(directoryPath, "*.xml", SearchOption.TopDirectoryOnly).Take(2).ToArray();

            var resultFilePath = tc.ReadObjectsParallel<Car>(files[0], files[1]);
            DisplayConsole.PrintFileContents(resultFilePath);

            return resultFilePath;
        }
    }
}
