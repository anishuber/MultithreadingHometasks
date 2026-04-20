using System.Collections.Generic;
using CarLibrary;

namespace Hometask3.ThreadingClassLibrary
{
    public static class InMemoryCarStorage
    {
        public static List<Car> Cars { get; } = new List<Car>
        {
            new Car { VanId = 1, Model = "Toyota Camry", PlateNumber = "AA1001BB", VehicleType = CarType.Sedan },
            new Car { VanId = 2, Model = "Honda Civic", PlateNumber = "AA1002BB", VehicleType = CarType.Sedan },
            new Car { VanId = 3, Model = "Ford Focus", PlateNumber = "AA1003BB", VehicleType = CarType.Hatchback },
            new Car { VanId = 4, Model = "Volkswagen Golf", PlateNumber = "AA1004BB", VehicleType = CarType.Hatchback },
            new Car { VanId = 5, Model = "BMW 4 Series", PlateNumber = "AA1005BB", VehicleType = CarType.Coupe },
            new Car { VanId = 6, Model = "Audi A5", PlateNumber = "AA1006BB", VehicleType = CarType.Coupe },
            new Car { VanId = 7, Model = "Mazda MX-5", PlateNumber = "AA1007BB", VehicleType = CarType.Convertible },
            new Car { VanId = 8, Model = "Ford Mustang", PlateNumber = "AA1008BB", VehicleType = CarType.Convertible },
            new Car { VanId = 9, Model = "Subaru Outback", PlateNumber = "AA1009BB", VehicleType = CarType.Wagon },
            new Car { VanId = 10, Model = "Volvo V60", PlateNumber = "AA1010BB", VehicleType = CarType.Wagon },
            new Car { VanId = 11, Model = "Toyota RAV4", PlateNumber = "AA1011BB", VehicleType = CarType.Suv },
            new Car { VanId = 12, Model = "Honda CR-V", PlateNumber = "AA1012BB", VehicleType = CarType.Suv },
            new Car { VanId = 13, Model = "Nissan Qashqai", PlateNumber = "AA1013BB", VehicleType = CarType.Crossover },
            new Car { VanId = 14, Model = "Hyundai Tucson", PlateNumber = "AA1014BB", VehicleType = CarType.Crossover },
            new Car { VanId = 15, Model = "Ford F-150", PlateNumber = "AA1015BB", VehicleType = CarType.Pickup },
            new Car { VanId = 16, Model = "Toyota Hilux", PlateNumber = "AA1016BB", VehicleType = CarType.Pickup },
            new Car { VanId = 17, Model = "Mercedes-Benz Vito", PlateNumber = "AA1017BB", VehicleType = CarType.Van },
            new Car { VanId = 18, Model = "Volkswagen Transporter", PlateNumber = "AA1018BB", VehicleType = CarType.Van },
            new Car { VanId = 19, Model = "Chrysler Pacifica", PlateNumber = "AA1019BB", VehicleType = CarType.Minivan },
            new Car { VanId = 20, Model = "Renault Espace", PlateNumber = "AA1020BB", VehicleType = CarType.Minivan },
        };
    }
}
