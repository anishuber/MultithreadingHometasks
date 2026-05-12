// <copyright file="XmlSerializationUtils.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SerializationService
{
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using Helpers;

    /// <summary>
    /// Utility methods for XML (de)serialization and simple XML attribute operations.
    /// </summary>
    public static class XmlSerializationUtils
    {
        /// <summary>
        /// Reads the next element of type <typeparamref name="T"/> from the provided <see cref="XmlReader"/>.
        /// Skips whitespace and handles end-of-file/end-element conditions.
        /// </summary>
        /// <typeparam name="T">The expected element type.</typeparam>
        /// <param name="reader">XML reader positioned at or before the target element.</param>
        /// <param name="serializer">Serializer for type <typeparamref name="T"/>.</param>
        /// <returns>Deserialized instance of <typeparamref name="T"/> or default if no element is available.</returns>
        public static T? DeserializeObjectFromXml<T>(XmlReader reader, XmlSerializer serializer)
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(serializer);

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

        /// <summary>
        /// Deserializes a list of objects from a file stream using the provided serializer.
        /// </summary>
        /// <typeparam name="T">Type of objects in the list.</typeparam>
        /// <param name="serializer">Serializer for <see cref="List{T}"/>.</param>
        /// <param name="fileStream">File stream containing the serialized list.</param>
        /// <returns>Deserialized list of objects; an empty list if file contained no objects.</returns>
        public static List<T> DeserializeObjectsFromXml<T>(XmlSerializer serializer, FileStream fileStream)
        {
            ArgumentNullException.ThrowIfNull(serializer);
            ArgumentNullException.ThrowIfNull(fileStream);

            var objectList = (List<T>?)serializer.Deserialize(fileStream);

            if (objectList is not null && objectList.Count > 0)
            {
                Console.WriteLine("Объекты десериализованы");
                return objectList;
            }

            Console.WriteLine("Файл пуст");
            return new List<T>();
        }

        /// <summary>
        /// Serializes a list of objects to the specified file path.
        /// </summary>
        /// <typeparam name="T">Type of objects to serialize.</typeparam>
        /// <param name="filePath">Destination file path.</param>
        /// <param name="objectsToSerialize">List of objects to write.</param>
        public static void SerializeObjectsFromList<T>(string filePath, List<T> objectsToSerialize)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
            ArgumentNullException.ThrowIfNull(objectsToSerialize);

            Validators.ValidateFilePath(filePath);

            var serializer = new XmlSerializer(typeof(List<T>));

            using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
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

        /// <summary>
        /// Changes an attribute value of the nth element with the given name using <see cref="XmlDocument"/>.
        /// </summary>
        /// <param name="filePath">Path to the XML file.</param>
        /// <param name="attributeName">Name of the attribute to change.</param>
        /// <param name="elementWithAttributeName">Name of the element that contains the attribute.</param>
        /// <param name="elementNumber">1-based index of the element to modify.</param>
        /// <param name="newAttributeValue">New value for the attribute.</param>
        public static void ChangeXmlAttributeXmlDocument(
            string filePath,
            string attributeName,
            string elementWithAttributeName,
            int elementNumber,
            string newAttributeValue)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
            ArgumentException.ThrowIfNullOrWhiteSpace(attributeName);
            ArgumentException.ThrowIfNullOrWhiteSpace(elementWithAttributeName);
            ArgumentException.ThrowIfNullOrWhiteSpace(newAttributeValue);

            Validators.ValidateFilePath(filePath);
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

        /// <summary>
        /// Changes an attribute value of the nth element with the given name using LINQ to XML (<see cref="XDocument"/>).
        /// </summary>
        /// <param name="filePath">Path to the XML file.</param>
        /// <param name="attributeName">Name of the attribute to change.</param>
        /// <param name="elementWithAttributeName">Name of the element that contains the attribute.</param>
        /// <param name="elementNumber">1-based index of the element to modify.</param>
        /// <param name="newAttributeValue">New value for the attribute.</param>
        public static void ChangeXmlAttributeXDocument(
            string filePath,
            string attributeName,
            string elementWithAttributeName,
            int elementNumber,
            string newAttributeValue)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
            ArgumentException.ThrowIfNullOrWhiteSpace(attributeName);
            ArgumentException.ThrowIfNullOrWhiteSpace(elementWithAttributeName);
            ArgumentException.ThrowIfNullOrWhiteSpace(newAttributeValue);

            Validators.ValidateFilePath(filePath);
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

        /// <summary>
        /// Finds and returns values of a named attribute from all elements with the specified name using LINQ to XML.
        /// </summary>
        /// <param name="filePath">Path to the XML file.</param>
        /// <param name="attributeName">Attribute name to search for.</param>
        /// <param name="elementWithAttributeName">Element name that should contain the attribute.</param>
        /// <returns>Array of attribute values (one per found element).</returns>
        public static string[] FindXmlAttributeXDocument(string filePath, string attributeName, string elementWithAttributeName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
            ArgumentException.ThrowIfNullOrWhiteSpace(attributeName);
            ArgumentException.ThrowIfNullOrWhiteSpace(elementWithAttributeName);

            Validators.ValidateFilePath(filePath);
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

        /// <summary>
        /// Finds and returns values of a named attribute from all elements with the specified name using <see cref="XmlDocument"/>.
        /// </summary>
        /// <param name="filePath">Path to the XML file.</param>
        /// <param name="attributeName">Attribute name to search for.</param>
        /// <param name="elementWithAttributeName">Element name that should contain the attribute.</param>
        /// <returns>Array of attribute values (one per element index in the XML).</returns>
        public static string[] FindXmlAttributeXmlDocument(string filePath, string attributeName, string elementWithAttributeName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
            ArgumentException.ThrowIfNullOrWhiteSpace(attributeName);
            ArgumentException.ThrowIfNullOrWhiteSpace(elementWithAttributeName);

            Validators.ValidateFilePath(filePath);
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
    }
}
