// <copyright file="XmlDocumentSerializer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Serialization.Xml;

using System.Xml;
using Common.IO;
using Serialization.Validation;

/// <summary>
/// Class responsible for serializing and deserializing XML documents using the XmlDocument class.
/// </summary>
public static class XmlDocumentSerializer
{
    /// <summary>
    /// Changes the value of the specified attribute for the specified element in the XML document at the given file path.
    /// </summary>
    /// <param name="filePath">The path to the XML file.</param>
    /// <param name="attributeName">The name of the attribute to change.</param>
    /// <param name="elementName">The name of the element containing the attribute.</param>
    /// <param name="elementNumber">The index of the element in the XML document.</param>
    /// <param name="newValue">The new value for the attribute.</param>
    /// <returns>The path to the updated XML file.</returns>
    public static string ChangeAttribute(
        string filePath,
        string attributeName,
        string elementName,
        int elementNumber,
        string newValue)
    {
        PathValidator.ValidateFilePath(filePath);
        ArgumentValidator.ValidateNonEmptyStrings(attributeName, elementName, newValue);

        var doc = new XmlDocument();
        doc.Load(filePath);

        XmlNodeList? nodes = XmlValidation.RequireElements(doc, elementName);

        XmlElement element = XmlValidation.RequireElementAt(nodes, elementNumber);

        XmlValidation.RequireAttribute(element, attributeName, elementName, elementNumber);

        element.SetAttribute(attributeName, newValue);
        doc.Save(filePath);

        return filePath;
    }

    /// <summary>
    /// Finds the value of the specified attribute for all elements with the specified name in the XML document at the given file path.
    /// </summary>
    /// <param name="filePath">The path to the XML file.</param>
    /// <param name="attributeName">The name of the attribute to find.</param>
    /// <param name="elementName">The name of the elements containing the attribute.</param>
    /// <returns>An array of attribute values.</returns>
    public static string[] FindAttribute(string filePath, string attributeName, string elementName)
    {
        PathValidator.ValidateFilePath(filePath);
        ArgumentValidator.ValidateNonEmptyStrings(attributeName, attributeName, elementName);

        var doc = new XmlDocument();
        doc.Load(filePath);

        XmlNodeList allElements = doc.GetElementsByTagName(elementName);
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
