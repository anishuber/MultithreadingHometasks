namespace Helpers
{
    /// <summary>
    /// Console helper methods for printing files and collections to standard output.
    /// </summary>
    public static class DisplayConsole
    {
        /// <summary>
        /// Prints the contents of the specified file to the console.
        /// Catches common IO errors and prints localized error messages.
        /// </summary>
        /// <param name="filePath">Path to the file to print.</param>
        public static void PrintFileContents(string filePath)
        {
            Validators.ValidateFilePath(filePath);

            try
            {
                string text = File.ReadAllText(filePath);

                if (string.IsNullOrWhiteSpace(text))
                {
                    Console.WriteLine("Файл пуст");
                    return;
                }

                Console.WriteLine(text);
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Доступ к файлу запрещен");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка ввода: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        /// <summary>
        /// Prints each object from a list to the console using its <see cref="object.ToString"/> representation.
        /// </summary>
        /// <typeparam name="T">Type of objects in the list.</typeparam>
        /// <param name="objects">List of objects to print.</param>
        public static void PrintObjectsFromList<T>(List<T> objects)
        {
            ArgumentNullException.ThrowIfNull(objects);

            if (objects.Count > 0)
            {
                foreach (var obj in objects)
                {
                    Console.WriteLine(obj?.ToString());
                }
            }
            else
            {
                Console.WriteLine("Объекты отсутствуют");
            }
        }
    }
}
