namespace Helpers
{
    public static class DisplayConsole
    {
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
