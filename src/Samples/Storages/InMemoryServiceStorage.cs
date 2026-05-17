using Samples.CarLibrary;

namespace Samples.Storages;
/// <summary>
/// Provides an in-memory collection of sample <see cref="Car"/> instances used by demo tasks and tests.
/// </summary>
public static class InMemoryServiceStorage
{
    /// <summary>
    /// Pre-populated list of sample cars used for serialization/demonstration.
    /// </summary>
    public static IReadOnlyList<Service> Services { get; } = new List<Service>
    {
        new Service("CarFix", "Main Street 1", true),
        new Service("CarBox", "Second Street 2", true),
        new Service("WheeFix", "Third Street 3", false),
        new Service("WheelBox", "Fourth Street 4", true),
        new Service("FastRepair", "Fifth Street 5", true),
        new Service("AutoFix", "Sixth Street 6", false),
        new Service("AutoBox", "Seventh Street 7", true),
        new Service("MotoFix", "Eighth Street 8", true),
        new Service("MotoBox", "Nineth Street 9", true),
        new Service("EasyRepair", "Tenth Street 10", false),
        new Service("GearFix", "Eleventh Street 11", true),
        new Service("GearBox", "Twelveth Street 12", true),
        new Service("RoadFix", "Thirteenth Street 13", false),
        new Service("RoadBox", "Fourteenth Street 14", true),
        new Service("SwiftRepair", "Fifteenth Street 15", true),
        new Service("GarageFix", "Sixteenth Street 16", true),
        new Service("GarageBox", "Seventeenth Street 17", false),
        new Service("FixService", "Eighteenth Street 18", true),
        new Service("BoxService", "Nineteenth Street 19", true),
        new Service("SafeRepair", "Twentieth Street 20", true),
    };
}
