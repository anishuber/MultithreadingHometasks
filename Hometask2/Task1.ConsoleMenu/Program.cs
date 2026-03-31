using SerializationService;
using System.Reflection;

namespace Task1.ConsoleMenu
{
    public class Program
    {
        static void Main(string[] args)
        {
            var helper = new XmlSerializerHelper(
                Path.Combine(
                    Directory.GetCurrentDirectory(), @"..\..\..\..", "SerializedObjects", "SerializedCar.xml"
                    )
                );

            while (true)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Создать 10 экземпляров и вывести в консоль");
                Console.WriteLine("2. Сериализовать объекты в XML-файл");
                Console.WriteLine("3. Прочитать XML-файл и вывести содержимое");
                Console.WriteLine("4. Десериализовать объекты из файла");
                Console.WriteLine("5. Найти все значения атрибута Model (XDocument)");
                Console.WriteLine("6. Найти все значения атрибута Model (XmlDocument)");
                Console.WriteLine("7. Изменить значение атрибута (XDocument)");
                Console.WriteLine("8. Изменить значение атрибута (XmlDocument)");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите пункт меню: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        helper.PrintObjects();
                        break;
                    case "2":
                        helper.SerializeContents();
                        break;
                    case "3":
                        helper.PrintXml();
                        break;
                    case "4":
                        helper.DeserializeContents();
                        break;
                    case "5":
                        helper.FindXmlAttributeXDocument("Model");
                        break;
                    case "6":
                        helper.FindXmlAttributeXmlDocument("Model");
                        break;
                    case "7":
                    case "8":
                        ReadDataForChangingAttribute(out string attributeName, out int elementNumber, out string newAttributeValue);

                        if (choice == "7")
                        {
                            try
                            {
                                helper.ChangeXmlAttributeXDocument(attributeName, elementNumber, newAttributeValue);
                            }
                            catch(ArgumentOutOfRangeException e)
                            {
                                Console.WriteLine($"Changing attribute failed: {e.Message}");
                            }
                        }
                        else
                        {
                            helper.ChangeXmlAttributeXmlDocument(attributeName, elementNumber, newAttributeValue);
                        }

                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор, попробуйте снова.");
                        break;
                }
            }
        }

        private static void ReadDataForChangingAttribute(out string attributeName, out int elementNumber, out string newAttributeValue)
        {
            Console.WriteLine("Введите имя атрибута");
            attributeName = Console.ReadLine();
            Console.WriteLine("Введите номер элемента с этим атрибутом");

            string temp = Console.ReadLine();

            if (!int.TryParse(temp, out elementNumber) || elementNumber < 1)
            {
                Console.WriteLine("Неверно введен номер. Введите номер элемента с этим атрибутом");
                temp = Console.ReadLine();
            }

            Console.WriteLine("Введите новое значение атрибута");
            newAttributeValue = Console.ReadLine();
        }
    }
}
