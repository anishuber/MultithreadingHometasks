namespace SerializationService
{
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public static class XmlSerializationUtils
    {
        public static T? DeserializeObjectFromXml<T>(XmlReader reader, XmlSerializer serializer)
        {
            while (reader.NodeType == XmlNodeType.Whitespace)
            {
                reader.Read();
            }

            if (reader.EOF)
            {
                return default;
            }

            if (reader.NodeType == XmlNodeType.EndElement)
            {
                return default;
            }

            if (reader.NodeType == XmlNodeType.Element && reader.Name == typeof(T).Name)
            {
                return (T?)serializer.Deserialize(reader);
            }

            return default;
        }

        public static void SerializeObjectsFromList<T>(string filePath, List<T> objectsToSerialize)
        {
            var serializer = new XmlSerializer(typeof(List<T>));

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
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
        }

        public static void PrintXml(string filePath)
        {
            ValidatePath(filePath);

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

        public static void ChangeXmlAttributeXmlDocument(
            string filePath,
            string attributeName,
            string elementWithAttributeName,
            int elementNumber,
            string newAttributeValue)
        {
            ValidatePath(filePath);
            var doc = new XmlDocument();
            doc.Load(filePath);

            XmlNodeList? nodes = doc.SelectNodes($"//{elementWithAttributeName}");

            if (nodes is null || nodes.Count == 0)
            {
                Console.WriteLine($"Элементы \"{elementWithAttributeName}\" не были найдены");
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
            doc.Save(filePath);
        }

        public static void ChangeXmlAttributeXDocument(
            string filePath,
            string attributeName,
            string elementWithAttributeName,
            int elementNumber,
            string newAttributeValue)
        {
            ValidatePath(filePath);
            XDocument? doc = XDocument.Load(filePath);
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
            doc.Save(filePath);
        }

        public static string[] FindXmlAttributeXDocument(string filePath, string attributeName, string elementWithAttributeName)
        {
            ValidatePath(filePath);
            XDocument doc = XDocument.Load(filePath);
            List<XAttribute> allAttributes = doc
                .Descendants(elementWithAttributeName)
                .Attributes()
                .Where(attr => attr.Name == attributeName)
                .ToList();

            string[] attributeNames = new string[allAttributes.Count];

            for (int i = 0; i < attributeNames.Length; i++)
            {
                attributeNames[i] = allAttributes[i].Value;
            }

            return attributeNames;
        }

        public static string[] FindXmlAttributeXmlDocument(string filePath, string attributeName, string elementWithAttributeName)
        {
            ValidatePath(filePath);
            var doc = new XmlDocument();
            doc.Load(filePath);

            XmlNodeList allElements = doc.GetElementsByTagName(elementWithAttributeName);
            string[] attributeNames = new string[allElements.Count];

            for (int i = 0; i < allElements.Count; i++)
            {
                XmlAttribute? attr = allElements[i]?.Attributes?[attributeName];

                if (attr is not null)
                {
                    attributeNames[i] = attr.Value;
                }
            }

            return attributeNames;
        }

        public static void ValidatePath(string filePath)
        {
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
        }
    }
}
