using System.Reflection;

namespace Task2.ConsoleApp
{
    public static class Program
    {
        public static void Main()
        {
            string innerPath = @"CarLibrary\obj\Debug\net8.0\CarLibrary.dll";
            string goUpFourDirs = @"..\..\..\..";

            string path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, goUpFourDirs, innerPath));

            Assembly a = Assembly.LoadFrom(path);
            var types = a.GetTypes();

            foreach (var type in types)
            {
                Console.WriteLine(type.Name);

                var publicProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                foreach (var property in publicProperties)
                {
                    Console.WriteLine($"- [{property.PropertyType.Name}] {property.Name} (public)");
                }

                var privateProperties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var property in privateProperties)
                {
                    Console.WriteLine($"- [{property.PropertyType.Name}] {property.Name} (private)");
                }
            }
        }
    }
}
