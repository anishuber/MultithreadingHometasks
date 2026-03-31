using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Task2.CarLibrary;

namespace SerializationService
{
    public class XmlSerializerHelper
    {
        private readonly List<Car> _cars = [];
        private readonly string filePath;

        public XmlSerializerHelper(string filePath) 
        { 
            this.CreateTemplateObjects();
            this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        private void CreateTemplateObjects()
        {
            for (int i = 1; i <= 10; i++)
            {
                var car = new Car();
                _cars.Add(car.Create(i, $"model-{i}", $"plateNumber-{i}", (CarType)(i%2)));
            }
        }

        public void SerializeContents()
        {
            var serializer = new XmlSerializer(typeof(List<Car>));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(stream, _cars);
                Console.WriteLine("Objects serialized");
            }

            PrintXml();
        }

        public List<Car> DeserializeContents()
        {
            var serializer = new XmlSerializer(typeof(List<Car>));

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                var carList = (List<Car>?)serializer.Deserialize(stream);

                if (carList is not null && carList.Count > 0)
                {
                    Console.WriteLine("Objects deserialized");
                    PrintObjects(carList);
                    return carList;
                }

                Console.WriteLine("Xml file empty");
                return [];
            }
        }

        public void PrintObjects()
        {
            PrintObjects(_cars);
        }

        public void PrintXml()
        {
            string text = File.ReadAllText(filePath);
            Console.WriteLine(text);
        }

        public void FindXmlAttributeXDocument(string attribute)
        {
            XDocument doc = XDocument.Load(filePath);
            IEnumerable<XAttribute> allAttributes = doc.Descendants("Car")
                .Attributes().Where(attr => attr.Name == attribute);

            foreach(var attr in allAttributes)
            {
                Console.WriteLine(attr.Value);
            }
        }

        public void FindXmlAttributeXmlDocument(string attribute)
        {
            XmlDocument doc = new();
            doc.Load(filePath);

            XmlNodeList allElements = doc.GetElementsByTagName("Car");

            for (int i = 0; i < allElements.Count; i++)
            {
                XmlAttribute? attr = allElements[i]?.Attributes?[attribute];

                if (attr is not null)
                {
                    Console.WriteLine(attr.Value);
                }                
            }
        }

        public void ChangeXmlAttributeXDocument(string attributeName, int elementNumber, string newAttributeValue)
        {
            XDocument? doc = XDocument.Load(filePath);
            var elements = doc.Descendants("Car");

            ArgumentOutOfRangeException.ThrowIfLessThan(elementNumber, elements.Count());

            XElement? element = elements.ElementAt(elementNumber - 1);

            if (element is not null)
            {
                XAttribute? attr = element.Attribute(attributeName);

                if (attr is not null)
                {
                    attr.Value = newAttributeValue;
                }
            }

            doc.Save(filePath);
            PrintXml();
        }

        public void ChangeXmlAttributeXmlDocument(string attributeName, int elementNumber, string newAttributeName)
        {
            XmlDocument doc = new();
            doc.Load(filePath);

            XmlNodeList? nodes = doc.SelectNodes("//ArrayOfCar/Car");

            if (nodes is not null && nodes.Count > 0)
            {
                ArgumentOutOfRangeException.ThrowIfLessThan(elementNumber, nodes.Count);

                XmlElement? elem = (XmlElement?)nodes[elementNumber - 1];
                elem?.SetAttribute(attributeName, newAttributeName);
            }

            doc.Save(filePath);
            PrintXml();
        }

        private static void PrintObjects(List<Car> objects)
        {
            foreach (var car in objects)
            {
                Console.WriteLine(car.PrintObject());
            }
        }
    }
}
