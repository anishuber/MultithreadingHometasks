using SerializationService;
using System.Xml.Serialization;
using CarLibrary;

namespace ConsoleMenu
{
    public class XmlSerializerHelper
    {
        private readonly List<Car> cars = new List<Car>();
        private readonly string filePath = string.Empty;

        public XmlSerializerHelper(string filePath)
        {
            this.CreateTemplateCarObjects();
            XmlSerializationUtils.ValidatePath(filePath);

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
            this.SerializeContents<Car>(this.cars);
        }

        public void SerializeContents<T>(List<T> objectsToSerialize)
        {
            var serializer = new XmlSerializer(typeof(List<T>));

            using (var stream = new FileStream(this.filePath, FileMode.Create, FileAccess.Write))
            {
                if (objectsToSerialize.Count > 0)
                {
                    serializer.Serialize(stream, objectsToSerialize);
                    Console.WriteLine("Объекты сериализованы");
                }
                else
                {
                    Console.WriteLine("Объекты для сериализации отсутствуют");
                }
            }

            this.PrintXml();
        }

        public List<Car> DeserializeContents()
        {
            return this.DeserializeContents<Car>();
        }

        public List<T> DeserializeContents<T>()
        {
            var serializer = new XmlSerializer(typeof(List<T>));

            using var stream = new FileStream(this.filePath, FileMode.Open, FileAccess.Read);
            var objectList = (List<T>?)serializer.Deserialize(stream);

            if (objectList is not null && objectList.Count > 0)
            {
                Console.WriteLine("Объекты десериализованы");
                return objectList;
            }

            Console.WriteLine("Файл пуст");
            return new List<T>();
        }

        public void PrintXml()
        {
            XmlSerializationUtils.PrintXml(this.filePath);
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
            PrintObjects(this.cars);
        }

        private static void PrintObjects(List<Car> objects)
        {
            if (objects.Count > 0)
            {
                foreach (var car in objects)
                {
                    Console.WriteLine(car.PrintObject());
                }
            }
            else
            {
                Console.WriteLine("Объекты отсутствуют");
            }
        }
    }
}
