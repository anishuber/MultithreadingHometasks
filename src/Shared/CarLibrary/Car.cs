// <copyright file="Car.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CarLibrary
{
    using System.Xml.Serialization;

    /// <summary>
    /// Models a minimal car description used for serialization and display.
    /// </summary>
    [Serializable]
    public class Car
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Car"/> class.
        /// </summary>
        public Car()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Car"/> class with the specified values.
        /// </summary>
        /// <param name="vanId">Identifier of the vehicle.</param>
        /// <param name="model">Car model name.</param>
        /// <param name="plateNumber">License plate number.</param>
        /// <param name="vehicleType">Type of vehicle.</param>
        public Car(int vanId, string model, string plateNumber, CarType vehicleType)
        {
            this.Create(vanId, model, plateNumber, vehicleType);
        }

        /// <summary>
        /// Gets or sets the vehicle identifier.
        /// </summary>
        [XmlAttribute]
        public int VanId { get; set; }

        /// <summary>
        /// Gets or sets the model name of the vehicle.
        /// </summary>
        [XmlAttribute]
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the license plate number.
        /// </summary>
        [XmlAttribute]
        public string PlateNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the vehicle.
        /// </summary>
        [XmlAttribute]
        public CarType VehicleType { get; set; }

        /// <summary>
        /// Populates this instance with provided values and returns the instance.
        /// Validates input arguments.
        /// </summary>
        /// <param name="vanId">Identifier of the vehicle.</param>
        /// <param name="model">Car model name.</param>
        /// <param name="plateNumber">License plate number.</param>
        /// <param name="vehicleType">Type of vehicle.</param>
        /// <returns>The updated <see cref="Car"/> instance.</returns>
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

        /// <summary>
        /// Returns a compact string representation of this car for display.
        /// </summary>
        /// <returns>A string describing the car.</returns>
        public string PrintObject()
        {
            return $"Vehicle {this.VanId} ({this.PlateNumber}): {this.Model}, {this.VehicleType}";
        }

        /// <summary>
        /// Returns a string representation of the current object.
        /// </summary>
        /// <returns>A string describing the car.</returns>
        public override string ToString()
        {
            return this.PrintObject();
        }
    }
}