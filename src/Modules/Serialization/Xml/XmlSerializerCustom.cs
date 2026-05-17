using Common.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Serialization.Xml
{
    public static class XmlSerializerCustom
    {
        // TODO: extract console output to a logger
        public static void SerializeObjectsFromList<T>(string filePath, List<T> objectsToSerialize)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
            ArgumentNullException.ThrowIfNull(objectsToSerialize);

           PathValidator.ValidateOrCreateFilePath(filePath);

            var serializer = new XmlSerializer(typeof(List<T>));

            using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            if (objectsToSerialize.Count > 0)
            {
                serializer.Serialize(stream, objectsToSerialize);
                Console.WriteLine("Objects were serialized");
            }
            else
            {
                Console.WriteLine("No objects to serialize");
            }
        }

        public static List<T> DeserializeObjectsFromXml<T>(XmlSerializer serializer, FileStream fileStream)
        {
            ArgumentNullException.ThrowIfNull(serializer);
            ArgumentNullException.ThrowIfNull(fileStream);

            var objectList = (List<T>?)serializer.Deserialize(fileStream);

            if (objectList is not null && objectList.Count > 0)
            {
                Console.WriteLine("Objects were Deserialized");
                return objectList;
            }

            Console.WriteLine("File is empty");
            return new List<T>();
        }

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
    }
}
