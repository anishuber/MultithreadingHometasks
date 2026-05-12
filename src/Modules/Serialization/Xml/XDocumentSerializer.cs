namespace Serialization.Xml;
using Common.IO;
using Serialization.Validation;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

public static class XDocumentSerializer
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

        XDocument? doc = XDocument.Load(filePath);

        var elements = XDocumentValidation.RequireElements(doc, elementName);
        var element = XDocumentValidation.RequireElementAt(elements, elementNumber);
        var attribute = XDocumentValidation.RequireAttribute(element, attributeName, elementName, elementNumber);

        attribute.Value = newValue;
        doc.Save(filePath);
        return filePath;
    }

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
