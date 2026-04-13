namespace ConsoleMenu
{
    using System.Runtime.Serialization;
    using System.Xml;

    public static class Program
    {
        public static void Main()
        {
            string filePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    @"..\..\..\..",
                    "SerializedObjects",
                    "SerializedCar.xml");

            XmlSerializerHelper? helper = TryCreateHelper(filePath);
            if (helper is null)
            {
                return;
            }

            while (true)
            {
                PrintMenu();
                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            helper.PrintObjects();
                            break;
                        case "2":
                            helper.CreateTemplateCarObjects();
                            break;
                        case "3":
                            helper.SerializeContents();
                            break;
                        case "4":
                            helper.PrintXml();
                            break;
                        case "5":
                            helper.DeserializeContents();
                            break;
                        case "6":
                            helper.FindXmlAttributeXDocument("Model");
                            break;
                        case "7":
                            helper.FindXmlAttributeXmlDocument("Model");
                            break;
                        case "8":
                        case "9":
                            ReadDataForChangingAttribute(
                                out string attributeName,
                                out int elementNumber,
                                out string newAttributeValue);

                            if (choice == "8")
                            {
                                helper.ChangeXmlAttributeXDocument(
                                    attributeName,
                                    elementNumber,
                                    newAttributeValue);
                            }
                            else
                            {
                                helper.ChangeXmlAttributeXmlDocument(
                                    attributeName,
                                    elementNumber,
                                    newAttributeValue);
                            }

                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Неверный выбор, попробуйте снова.");
                            break;
                    }
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine($"Неверный номер элемента: {ex.Message}");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Операция не выполнена: {ex.Message}");
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine($"Файл не найдет: {ex.Message}");
                }
                catch (DirectoryNotFoundException ex)
                {
                    Console.WriteLine($"Директория не найдена: {ex.Message}");
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine($"Доступ запрещен: {ex.Message}");
                }
                catch (XmlException ex)
                {
                    Console.WriteLine($"XML ошибка: {ex.Message}");
                }
                catch (SerializationException ex)
                {
                    Console.WriteLine($"Ошибка сериализации: {ex.Message}");
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Ошибка ввода: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                }

                Console.WriteLine();
            }
        }

        private static void PrintMenu()
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Вывывести экземпляры в консоль");
            Console.WriteLine("2. Создать 10 экземпляров");
            Console.WriteLine("3. Сериализовать объекты в XML-файл");
            Console.WriteLine("4. Прочитать XML-файл и вывести содержимое");
            Console.WriteLine("5. Десериализовать объекты из файла");
            Console.WriteLine("6. Найти все значения атрибута Model (XDocument)");
            Console.WriteLine("7. Найти все значения атрибута Model (XmlDocument)");
            Console.WriteLine("8. Изменить значение атрибута (XDocument)");
            Console.WriteLine("9. Изменить значение атрибута (XmlDocument)");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите пункт меню: ");
        }

        private static XmlSerializerHelper? TryCreateHelper(string filePath)
        {
            try
            {
                return new XmlSerializerHelper(filePath);
            }
            catch (Exception ex) when (ex is ArgumentException or DirectoryNotFoundException)
            {
                Console.WriteLine($"Ошибка инициализации: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Ошибка доступа: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }

            return null;
        }

        private static void ReadDataForChangingAttribute(out string attributeName, out int elementNumber, out string newAttributeValue)
        {
            ReadElement("Введите имя атрибута", out attributeName);

            string temp;
            do
            {
                ReadElement("Введите номер элемента с этим атрибутом (номер должен быть не меньше 1)", out temp);
            }
            while (!int.TryParse(temp, out elementNumber) || elementNumber < 1);

            ReadElement("Введите новое значение атрибута", out newAttributeValue);

            static void ReadElement(string inputMessage, out string result)
            {
                do
                {
                    Console.WriteLine(inputMessage);
                    result = Console.ReadLine() ?? string.Empty;
                }
                while (string.IsNullOrWhiteSpace(result));
            }
        }
    }
}
