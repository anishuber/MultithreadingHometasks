using System.Xml.Linq;

namespace Serialization.Validation
{
    internal static class XDocumentValidation
    {
        public static List<XElement> RequireElements(XDocument doc, string elementName)
        {
            var elements = doc.Descendants(elementName).ToList();
            if (elements.Count == 0)
            {
                throw new InvalidOperationException($"No elements '{elementName}' were found in the XML document.");
            }
            return elements;
        }

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

        public static XAttribute RequireAttribute(XElement element, string attributeName, string elementName, int elementNumber)
        {
            var attribute = element.Attribute(attributeName);
            return attribute is null
                ? throw new InvalidOperationException(
                    $"No attributes '{attributeName}' were found in the element {elementName} at position {elementNumber}.")
                : attribute;
        }
    }
}
