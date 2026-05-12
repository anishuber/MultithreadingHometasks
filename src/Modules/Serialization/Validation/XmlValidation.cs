using System.Xml;

namespace Serialization.Validation
{
    internal static class XmlValidation
    {
        public static XmlNodeList RequireElements(XmlDocument doc, string elementName)
        {
            XmlNodeList? nodes = doc.SelectNodes($"//{elementName}");
            if (nodes is null || nodes.Count == 0)
            {
                throw new InvalidOperationException($"No elements '{elementName}' were found in the XML document.");
            }
            return nodes;
        }

        public static XmlElement RequireElementAt(XmlNodeList nodes, int elementNumber)
        {
            if (elementNumber < 1 || elementNumber > nodes.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(elementNumber), $"Element number must be between 1 and {nodes.Count}.");
            }
            XmlElement? element = nodes[elementNumber - 1] as XmlElement;

            return element is null ? 
                throw new InvalidOperationException($"The node at position {elementNumber} is not an element.") : element;
        }

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
}
