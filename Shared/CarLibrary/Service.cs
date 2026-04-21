// <copyright file="Service.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CarLibrary
{
    /// <summary>
    /// Represents a service location with a name, address and working status.
    /// </summary>
    public class Service
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Service"/> class.
        /// </summary>
        public Service()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Service"/> class and sets the provided values.
        /// </summary>
        /// <param name="name">Service name.</param>
        /// <param name="address">Service address.</param>
        /// <param name="isServiceWorking">Indicates whether the service is currently working.</param>
        public Service(string name, string address, bool isServiceWorking)
        {
            this.Create(name, address, isServiceWorking);
        }

        /// <summary>
        /// Gets or sets the service name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the service address.
        /// </summary>
        public string Address { get; set; } = string.Empty;

        private bool IsServiceWorking { get; set; }

        /// <summary>
        /// Populates this service instance with provided values after validating inputs.
        /// </summary>
        /// <param name="name">Service name.</param>
        /// <param name="address">Service address.</param>
        /// <param name="isServiceWorking">Indicates whether the service is currently working.</param>
        /// <returns>The updated <see cref="Service"/> instance.</returns>
        public Service Create(string name, string address, bool isServiceWorking)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(address);

            this.Name = name;
            this.Address = address;
            this.IsServiceWorking = isServiceWorking;

            return this;
        }

        /// <summary>
        /// Returns a compact string representation of the service including its status.
        /// </summary>
        /// <returns>A string describing the service.</returns>
        public string PrintObject()
        {
            string status = this.IsServiceWorking ? "working" : "closed";
            return $"Service {this.Name}: {this.Address} ({status})";
        }

        /// <summary>
        /// Returns a string representation of the current object.
        /// </summary>
        /// <returns>A string describing the service.</returns>
        public override string ToString()
        {
            return this.PrintObject();
        }
    }
}
