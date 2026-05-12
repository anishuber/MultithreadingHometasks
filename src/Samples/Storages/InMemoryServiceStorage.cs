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
    public static IReadOnlyList<Service> Services { get; } = new List<Service> { };
}
