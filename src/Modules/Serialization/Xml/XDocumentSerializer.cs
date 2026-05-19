// <copyright file="XDocumentSerializer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Serialization.Xml;

using System.Xml.Linq;
using Common.IO;
using Serialization.Validation;

/// <summary>
/// Serializer class for XML documents using LINQ to XML (XDocument).
/// It provides methods to change attributes and find attributes in XML files, with validation for file paths and XML structure.
/// </summary>
public static class XDocumentSerializer
{
    /// <summary>
    /// Method to change the value of a specific attribute in an XML file.
    /// It validates the file path, checks for the existence of the specified element and attribute,
    /// and updates the attribute's value before saving the document.
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

        XDocument? doc = XDocument.Load(filePath);

        var elements = XDocumentValidation.RequireElements(doc, elementName);
        var element = XDocumentValidation.RequireElementAt(elements, elementNumber);
        var attribute = XDocumentValidation.RequireAttribute(element, attributeName, elementName, elementNumber);

        attribute.Value = newValue;
        doc.Save(filePath);
        return filePath;
    }

    /// <summary>
    /// Finds attributes with a specific name within elements of a given name in an XML file.
    /// </summary>
    /// <param name="filePath">The path to the XML file.</param>
    /// <param name="attributeName">The name of the attribute to find.</param>
    /// <param name="elementName">The name of the elements containing the attribute.</param>
    /// <returns>An array of attribute values.</returns>
    public static string[] FindAttribute(string filePath, string attributeName, string elementName)
    {
        PathValidator.ValidateFilePath(filePath);

        ArgumentValidator.ValidateNonEmptyStrings(attributeName, attributeName, elementName);

        XDocument doc = XDocument.Load(filePath);

        List<XAttribute> allAttributes = doc
            .Descendants(elementName)
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
}
