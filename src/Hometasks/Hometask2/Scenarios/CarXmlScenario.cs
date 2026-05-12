namespace Scenarios;

using System.Xml.Serialization;
using Common.IO;
using Samples.CarLibrary;
using Samples.Storages;
using Serialization.Xml;

public class CarXmlScenario
{
    private readonly string filePath;
    private readonly List<Car> cars = new();

    public CarXmlScenario(string filePath)
    {
        try
        {
            PathValidator.ValidateFilePath(filePath);
        }
        catch (FileNotFoundException)
        {
            using FileStream _ = File.Create(filePath);
        }

        this.filePath = filePath;
    }

    public IReadOnlyList<Car> GetCurrentCars() => this.cars;

    public void CreateTemplateCarObjects()
    {
        this.cars.Clear();
        this.cars.AddRange(InMemoryCarStorage.Cars.Take(10));
    }

    public void SerializeCars()
    {
        XmlSerializerCustom.SerializeObjectsFromList(this.filePath, this.cars);
    }

    public List<Car> DeserializeCars()
    {
        var serializer = new XmlSerializer(typeof(List<Car>));
        using var stream = new FileStream(this.filePath, FileMode.Open, FileAccess.Read);
    
        return XmlSerializerCustom.DeserializeObjectsFromXml<Car>(serializer, stream);
    }

    public void PrintXml()
    {
        FileAccessValidator.TryReadFile(this.filePath, out string contents);
        Console.WriteLine(contents);
    }

    public void FindXmlAttributeXDocument(string attributeName)
    {
        string[] values = XDocumentSerializer.FindAttribute(this.filePath, attributeName, "Car");
        foreach (string value in values)
        {
            Console.WriteLine(value);
        }
    }

    public void FindXmlAttributeXmlDocument(string attributeName)
    {
        string[] values = XmlDocumentSerializer.FindAttribute(this.filePath, attributeName, "Car");
        foreach (string value in values)
        {
            Console.WriteLine(value);
        }
    }

    public void ChangeXmlAttributeXDocument(string attributeName, int elementNumber, string newAttributeValue)
    {
        XDocumentSerializer.ChangeAttribute(this.filePath, attributeName, "Car", elementNumber, newAttributeValue);
        this.PrintXml();
    }

    public void ChangeXmlAttributeXmlDocument(string attributeName, int elementNumber, string newAttributeValue)
    {
        XmlDocumentSerializer.ChangeAttribute(this.filePath, attributeName, "Car", elementNumber, newAttributeValue);
        this.PrintXml();
    }

    public void PrintObjects()
    {
        if (this.cars.Count > 0)
        {
            foreach (var obj in this.cars)
            {
                Console.WriteLine(obj?.ToString());
            }
        }
        else
        {
            Console.WriteLine("Objects are absent");
        }
    }
}
