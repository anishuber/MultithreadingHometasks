// <copyright file="XmlSerializerCustom.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Serialization.Xml;

using System.Xml;
using System.Xml.Serialization;
using Common.IO;

/// <summary>
/// Additional methods for XML serialization and deserialization, including handling empty files and lists.
/// </summary>
public static class XmlSerializerCustom
{
    /// <summary>
    /// Serializes a list of objects to an XML file. If the list is empty, it will create an empty file.
    /// </summary>
    /// <typeparam name="T">The type of objects to serialize.</typeparam>
    /// <param name="filePath">The path of the XML file to create.</param>
    /// <param name="objectsToSerialize">The list of objects to serialize.</param>
    /// <exception cref="ArgumentException">Thrown when the file path is invalid.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the list of objects to serialize is null.</exception>
    public static void SerializeObjectsFromList<T>(string filePath, List<T> objectsToSerialize)
    {
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

    /// <summary>
    /// Deserializes a list of objects from an XML file. If the file is empty, it will return an empty list.
    /// </summary>
    /// <typeparam name="T">The type of objects to deserialize.</typeparam>
    /// <param name="serializer">The XmlSerializer instance to use for deserialization.</param>
    /// <param name="fileStream">The FileStream instance representing the XML file.</param>
    /// <exception cref="ArgumentNullException">Thrown when the serializer or file stream is null.</exception>
    /// <returns>A list of deserialized objects. If the file is empty, returns an empty list.</returns>
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

    /// <summary>
    /// Deserializes an object from an XML reader. If the reader is at the end of the file or at an end element, it will return null.
    /// </summary>
    /// <typeparam name="T">The type of object to deserialize.</typeparam>
    /// <param name="reader">The XmlReader instance to read from.</param>
    /// <param name="serializer">The XmlSerializer instance to use for deserialization.</param>
    /// <exception cref="ArgumentNullException">Thrown when the reader or serializer is null.</exception>
    /// <returns>The deserialized object, or null if the reader is at the end of the file or at an end element.</returns>
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
