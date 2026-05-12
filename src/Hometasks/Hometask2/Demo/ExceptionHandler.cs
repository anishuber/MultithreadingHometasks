using System.Runtime.Serialization;
using System.Xml;

namespace Demo
{
    internal class ExceptionHandler
    {
        public static void HandleException(Exception ex)
        {
            switch (ex)
            {
                case ArgumentOutOfRangeException:
                    Console.WriteLine($"Неверный номер элемента: {ex.Message}");
                    break;
                case InvalidOperationException:
                    Console.WriteLine($"Операция не выполнена: {ex.Message}");
                    break;
                case FileNotFoundException:
                    Console.WriteLine($"Файл не найден: {ex.Message}");
                    break;
                case DirectoryNotFoundException:
                    Console.WriteLine($"Директория не найдена: {ex.Message}");
                    break;
                case UnauthorizedAccessException:
                    Console.WriteLine($"Доступ запрещен: {ex.Message}");
                    break;
                case XmlException:
                    Console.WriteLine($"XML ошибка: {ex.Message}");
                    break;
                case SerializationException:
                    Console.WriteLine($"Ошибка сериализации: {ex.Message}");
                    break;
                case IOException:
                    Console.WriteLine($"Ошибка ввода/вывода: {ex.Message}");
                    break;
                default:
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                    break;
            }
        }
    }
}
