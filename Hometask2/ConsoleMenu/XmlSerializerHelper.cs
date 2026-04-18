using SerializationService;
using System.Xml.Serialization;
using CarLibrary;
using Helpers;

namespace ConsoleMenu
{
    public class XmlSerializerHelper
    {
        private readonly List<Car> cars = new List<Car>();
        private readonly string filePath;

        public XmlSerializerHelper(string filePath)
        {
            Validators.ValidateFilePath(filePath);

            this.filePath = filePath;
        }

        public void CreateTemplateCarObjects()
        {
            this.cars.Clear();

            for (int i = 1; i <= 10; i++)
            {
                var car = new Car();
                this.cars.Add(car.Create(i, $"model-{i}", $"plateNumber-{i}", (CarType)(i % 2)));
            }
        }

        public void SerializeContents()
        {
            XmlSerializationUtils.SerializeObjectsFromList<Car>(this.filePath, this.cars);
        }

        public List<Car> DeserializeContents()
        {
            var serializer = new XmlSerializer(typeof(List<Car>));
            using var stream = new FileStream(this.filePath, FileMode.Open, FileAccess.Read);

            return XmlSerializationUtils.DeserializeObjectsFromXml<Car>(serializer, stream);
        }

        public void PrintXml()
        {
            DisplayConsole.PrintFileContents(this.filePath);
        }

        public void FindXmlAttributeXDocument(string attributeName)
        {
            string[] values = XmlSerializationUtils.FindXmlAttributeXDocument(this.filePath, attributeName, "Car");
            foreach (string value in values)
            {
                Console.WriteLine(value);
            }
        }

        public void FindXmlAttributeXmlDocument(string attributeName)
        {
            string[] values = XmlSerializationUtils.FindXmlAttributeXmlDocument(this.filePath, attributeName, "Car");
            foreach (string value in values)
            {
                Console.WriteLine(value);
            }
        }

        public void ChangeXmlAttributeXDocument(string attributeName, int elementNumber, string newAttributeValue)
        {
            XmlSerializationUtils.ChangeXmlAttributeXDocument(this.filePath, attributeName, "Car", elementNumber, newAttributeValue);
            this.PrintXml();
        }

        public void ChangeXmlAttributeXmlDocument(string attributeName, int elementNumber, string newAttributeValue)
        {
            XmlSerializationUtils.ChangeXmlAttributeXmlDocument(this.filePath, attributeName, "Car", elementNumber, newAttributeValue);
            this.PrintXml();
        }

        public void PrintObjects()
        {
            DisplayConsole.PrintObjectsFromList(this.cars);
        }
    }
}
