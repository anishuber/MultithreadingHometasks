namespace SerializationService
{
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using Task2.CarLibrary;

    public class XmlSerializerHelper
    {
        private readonly List<Car> cars = new List<Car>();
        private readonly string filePath = string.Empty;

        public XmlSerializerHelper(string filePath)
        {
            this.CreateTemplateCarObjects();

            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

            if (!Path.IsPathFullyQualified(filePath))
            {
                throw new ArgumentException("Путь должен быть абсолютным", nameof(filePath));
            }

            if (Directory.Exists(filePath))
            {
                throw new ArgumentException("Данный путь указывает на директорию, а не на файл", nameof(filePath));
            }

            string? directory = Path.GetDirectoryName(filePath);
            if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException("Директория файла отсутсвует");
            }

            this.filePath = filePath;
        }

        public void CreateTemplateCarObjects()
        {
            this.cars.Clear();

            for (int i = 1; i <= 10; i++)
            {
                var car = new Car();
                this.cars.Add(car.Create(i, $"model-{i}", $"plateNumber-{i}", (CarType)(i % 2)));
            }
        }

        public void SerializeContents()
        {
            this.SerializeContents<Car>(this.cars);
        }

        public void SerializeContents<T>(List<T> objectsToSerialize)
        {
            var serializer = new XmlSerializer(typeof(List<T>));

            using (var stream = new FileStream(this.filePath, FileMode.Create, FileAccess.Write))
            {
                if (objectsToSerialize.Count > 0)
                {
                    serializer.Serialize(stream, objectsToSerialize);
                    Console.WriteLine("Объекты сериализованы");
                }
                else
                {
                    Console.WriteLine("Объекты для сериализации отсутствуют");
                }
            }

            this.PrintXml();
        }

        public List<Car> DeserializeContents()
        {
            return this.DeserializeContents<Car>();
        }

        public List<T> DeserializeContents<T>()
        {
            var serializer = new XmlSerializer(typeof(List<T>));

            using var stream = new FileStream(this.filePath, FileMode.Open, FileAccess.Read);
            var objectList = (List<T>?)serializer.Deserialize(stream);

            if (objectList is not null && objectList.Count > 0)
            {
                Console.WriteLine("Объекты десериализованы");
                return objectList;
            }

            Console.WriteLine("Файл пуст");
            return new List<T>();
        }

        public void PrintXml()
        {
            try
            {
                string text = File.ReadAllText(this.filePath);

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

        public void FindXmlAttributeXDocument(string attributeName)
        {
            this.FindXmlAttributeXDocument(attributeName, "Car");
        }

        public void FindXmlAttributeXDocument(string attributeName, string elementWithAttributeName)
        {
            XDocument doc = XDocument.Load(this.filePath);
            IEnumerable<XAttribute> allAttributes = doc
                .Descendants(elementWithAttributeName)
                .Attributes()
                .Where(attr => attr.Name == attributeName);

            foreach (var attr in allAttributes)
            {
                Console.WriteLine(attr.Value);
            }
        }

        public void FindXmlAttributeXmlDocument(string attributeName)
        {
            this.FindXmlAttributeXmlDocument(attributeName, "Car");
        }

        public void FindXmlAttributeXmlDocument(string attributeName, string elementWithAttributeName)
        {
            var doc = new XmlDocument();
            doc.Load(this.filePath);

            XmlNodeList allElements = doc.GetElementsByTagName(elementWithAttributeName);

            for (int i = 0; i < allElements.Count; i++)
            {
                XmlAttribute? attr = allElements[i]?.Attributes?[attributeName];

                if (attr is not null)
                {
                    Console.WriteLine(attr.Value);
                }
            }
        }

        public void ChangeXmlAttributeXDocument(string attributeName, int elementNumber, string newAttributeValue)
        {
            this.ChangeXmlAttributeXDocument(attributeName, "Car", elementNumber, newAttributeValue);
        }

        public void ChangeXmlAttributeXDocument(
            string attributeName,
            string elementWithAttributeName,
            int elementNumber,
            string newAttributeValue)
        {
            XDocument? doc = XDocument.Load(this.filePath);
            var elements = doc.Descendants(elementWithAttributeName).ToList();

            if (elementNumber < 1 || elementNumber > elements.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(elementNumber));
            }

            XElement element = elements[elementNumber - 1];
            XAttribute? attr = element.Attribute(attributeName);

            if (attr is null)
            {
                Console.WriteLine($"Атрибут \"{attributeName}\" не был найден");
                return;
            }

            attr.Value = newAttributeValue;
            doc.Save(this.filePath);
            this.PrintXml();
        }

        public void ChangeXmlAttributeXmlDocument(string attributeName, int elementNumber, string newAttributeValue)
        {
            this.ChangeXmlAttributeXmlDocument(attributeName, "Car", elementNumber, newAttributeValue);
        }

        public void ChangeXmlAttributeXmlDocument(
            string attributeName,
            string elementWithAttributeName,
            int elementNumber,
            string newAttributeValue)
        {
            var doc = new XmlDocument();
            doc.Load(this.filePath);

            XmlNodeList? nodes = doc.SelectNodes($"//{elementWithAttributeName}");

            if (nodes is null || nodes.Count == 0)
            {
                Console.WriteLine($"Элементы {elementWithAttributeName} не были найдены");
                return;
            }

            if (elementNumber < 1 || elementNumber > nodes.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(elementNumber), "Номер элемента был < 1 или превышал число элементов");
            }

            XmlElement? element = nodes[elementNumber - 1] as XmlElement;

            if (element?.HasAttribute(attributeName) is not true)
            {
                Console.WriteLine($"Атрибут \"{attributeName}\" не был найден");
                return;
            }

            element.SetAttribute(attributeName, newAttributeValue);
            doc.Save(this.filePath);
            this.PrintXml();
        }

        public void PrintObjects()
        {
            PrintObjects(this.cars);
        }

        private static void PrintObjects(List<Car> objects)
        {
            if (objects.Count > 0)
            {
                foreach (var car in objects)
                {
                    Console.WriteLine(car.PrintObject());
                }
            }
            else
            {
                Console.WriteLine("Объекты отсутствуют");
            }
        }
    }
}
