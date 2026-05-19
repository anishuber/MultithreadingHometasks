// <copyright file="XmlValidation.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Serialization.Validation;

using System.Xml;

/// <summary>
/// Validation helper methods for XML documents, ensuring the presence of required elements and attributes with informative error messages.
/// </summary>
internal static class XmlValidation
{
    /// <summary>
    /// Ensures that the specified XML document contains at least one element with the given name, throwing an exception if not found.
    /// </summary>
    /// <param name="doc">The XML document to validate.</param>
    /// <param name="elementName">The name of the element to look for.</param>
    /// <returns>A list of XML nodes matching the specified element name.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no elements with the specified name are found.</exception>
    public static XmlNodeList RequireElements(XmlDocument doc, string elementName)
    {
        XmlNodeList? nodes = doc.SelectNodes($"//{elementName}");
        if (nodes is null || nodes.Count == 0)
        {
            throw new InvalidOperationException($"No elements '{elementName}' were found in the XML document.");
        }

        return nodes;
    }

    /// <summary>
    /// Ensures that the specified XML node list contains an element at the given position.
    /// </summary>
    /// <param name="nodes">The list of XML nodes to validate.</param>
    /// <param name="elementNumber">The position of the element to retrieve (1-based).</param>
    /// <returns>The XML element at the specified position.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the element number is out of range.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the node at the specified position is not an element.</exception>
    public static XmlElement RequireElementAt(XmlNodeList nodes, int elementNumber)
    {
        if (elementNumber < 1 || elementNumber > nodes.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(elementNumber), $"Element number must be between 1 and {nodes.Count}.");
        }

        return nodes[elementNumber - 1] is not XmlElement element ?
            throw new InvalidOperationException($"The node at position {elementNumber} is not an element.") : element;
    }

    /// <summary>
    /// Ensiures that the specified XML element has an attribute with the given name, throwing an exception if not found.
    /// </summary>
    /// <param name="element">The XML element to validate.</param>
    /// <param name="attributeName">The name of the attribute to look for.</param>
    /// <param name="elementName">The name of the element for error messages.</param>
    /// <param name="elementNumber">The position of the element for error messages.</param>
    /// <exception cref="InvalidOperationException">Thrown when the attribute is not found.</exception>
    public static void RequireAttribute(
        XmlElement element,
        string attributeName,
        string elementName,
        int elementNumber)
    {
        if (!element.HasAttribute(attributeName))
        {
            throw new InvalidOperationException(
                $"No attributes '{attributeName}' were found in the element {elementName} at position {elementNumber}.");
        }
    }
}
