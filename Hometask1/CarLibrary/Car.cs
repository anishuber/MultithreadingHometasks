using System.Xml.Serialization;

namespace Task2.CarLibrary
{
    public class Car
    {
        private int VanId {  get; set; }

        [XmlAttribute]
        public string Model { get; set; } = string.Empty;

        [XmlAttribute]
        public string PlateNumber { get; set; } = string.Empty;

        [XmlAttribute]
        public CarType VehicleType { get; set; }

        public Car Create(int vanId, string model, string plateNumber, CarType vehicleType)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(vanId);
            ArgumentException.ThrowIfNullOrWhiteSpace(model);
            ArgumentException.ThrowIfNullOrWhiteSpace(plateNumber);

            this.VanId = vanId;
            this.Model = model;
            this.PlateNumber = plateNumber;
            this.VehicleType = vehicleType;

            return this;
        }

        public string PrintObject()
        {
            return $"Vehicle {this.VanId} ({this.PlateNumber}): {this.Model}, {this.VehicleType}";
        }
    }
}
