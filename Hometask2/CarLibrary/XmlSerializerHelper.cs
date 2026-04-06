namespace SerializationService
{
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using Task2.CarLibrary;

    public class XmlSerializerHelper
    {
        private readonly List<Car> _cars = new List<Car>();
        private readonly string _filePath = string.Empty;

        public XmlSerializerHelper(string filePath)
        {
            this.CreateTemplateObjects();

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

            this._filePath = filePath;
        }

        public void CreateTemplateObjects()
        {
            this._cars.Clear();

            for (int i = 1; i <= 10; i++)
            {
                var car = new Car();
                this._cars.Add(car.Create(i, $"model-{i}", $"plateNumber-{i}", (CarType)(i % 2)));
            }
        }

        public void SerializeContents()
        {
            var serializer = new XmlSerializer(typeof(List<Car>));

            using (var stream = new FileStream(this._filePath, FileMode.Create, FileAccess.Write))
            {
                if (this._cars.Count > 0)
                {
                    serializer.Serialize(stream, this._cars);
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
            var serializer = new XmlSerializer(typeof(List<Car>));

            using var stream = new FileStream(this._filePath, FileMode.Open, FileAccess.Read);
            var carList = (List<Car>?)serializer.Deserialize(stream);

            if (carList is not null && carList.Count > 0)
            {
                Console.WriteLine("Объекты десериализованы");
                PrintObjects(carList);
                return carList;
            }

            Console.WriteLine("Файл пуст");
            return new List<Car>();
        }

        public void PrintXml()
        {
            try
            {
                string text = File.ReadAllText(this._filePath);

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

        public void FindXmlAttributeXDocument(string attribute)
        {
            XDocument doc = XDocument.Load(this._filePath);
            IEnumerable<XAttribute> allAttributes = doc
                .Descendants("Car")
                .Attributes()
                .Where(attr => attr.Name == attribute);

            foreach (var attr in allAttributes)
            {
                Console.WriteLine(attr.Value);
            }
        }

        public void FindXmlAttributeXmlDocument(string attribute)
        {
            var doc = new XmlDocument();
            doc.Load(this._filePath);

            XmlNodeList allElements = doc.GetElementsByTagName("Car");

            for (int i = 0; i < allElements.Count; i++)
            {
                XmlAttribute? attr = allElements[i]?.Attributes?[attribute];

                if (attr is not null)
                {
                    Console.WriteLine(attr.Value);
                }
            }
        }

        public void ChangeXmlAttributeXDocument(string attributeName, int elementNumber, string newAttributeValue)
        {
            XDocument? doc = XDocument.Load(this._filePath);
            var elements = doc.Descendants("Car").ToList();

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
            doc.Save(this._filePath);
            this.PrintXml();
        }

        public void ChangeXmlAttributeXmlDocument(string attributeName, int elementNumber, string newAttributeValue)
        {
            var doc = new XmlDocument();
            doc.Load(this._filePath);

            XmlNodeList? nodes = doc.SelectNodes("//ArrayOfCar/Car");

            if (nodes is null || nodes.Count == 0)
            {
                Console.WriteLine("Элементы Car не были найдены");
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
            doc.Save(this._filePath);
            this.PrintXml();
        }

        public void PrintObjects()
        {
            PrintObjects(this._cars);
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
