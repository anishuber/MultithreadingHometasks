// <copyright file="XDocumentValidation.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Serialization.Validation;

using System.Xml.Linq;

/// <summary>
/// Validation helper methods for XML documents using LINQ to XML (XDocument), ensuring the presence of required elements and attributes with informative error messages.
/// </summary>
internal static class XDocumentValidation
{
    /// <summary>
    /// Ensures that the specified XML document contains at least one element with the given name, throwing an exception if not found.
    /// </summary>
    /// <param name="doc">The XML document to validate.</param>
    /// <param name="elementName">The name of the element to look for.</param>
    /// <returns>A list of XML elements matching the specified element name.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no elements with the specified name are found.</exception>
    public static List<XElement> RequireElements(XDocument doc, string elementName)
    {
        var elements = doc.Descendants(elementName).ToList();
        if (elements.Count == 0)
        {
            throw new InvalidOperationException($"No elements '{elementName}' were found in the XML document.");
        }

        return elements;
    }

    /// <summary>
    /// Ensures that the specified list of XML elements contains an element at the given position.
    /// </summary>
    /// <param name="elements">The list of XML elements to validate.</param>
    /// <param name="elementNumber">The position of the element to retrieve (1-based).</param>
    /// <returns>The XML element at the specified position.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the element number is out of range.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the element at the specified position is null.</exception>
    public static XElement RequireElementAt(List<XElement> elements, int elementNumber)
    {
        if (elementNumber < 1 || elementNumber > elements.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(elementNumber), $"Element number must be between 1 and {elements.Count}.");
        }

        var element = elements[elementNumber - 1];

        return element is null ?
            throw new InvalidOperationException($"The element at position {elementNumber} is null.") : element;
    }

    /// <summary>
    /// Ensures that the specified XML element has an attribute with the given name, throwing an exception if not found.
    /// </summary>
    /// <param name="element">The XML element to validate.</param>
    /// <param name="attributeName">The name of the attribute to look for.</param>
    /// <param name="elementName">The name of the element for error messages.</param>
    /// <param name="elementNumber">The position of the element for error messages.</param>
    /// <returns>The XML attribute with the specified name.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the attribute is not found.</exception>
    public static XAttribute RequireAttribute(XElement element, string attributeName, string elementName, int elementNumber)
    {
        var attribute = element.Attribute(attributeName);
        return attribute is null
            ? throw new InvalidOperationException(
                $"No attributes '{attributeName}' were found in the element {elementName} at position {elementNumber}.")
            : attribute;
    }
}
