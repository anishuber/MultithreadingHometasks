namespace Serialization.Xml;

using Common.IO;
using Serialization.Validation;
using System.Xml;

public static class XmlDocumentSerializer
{
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
