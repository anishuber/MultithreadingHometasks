using System.Text;

namespace FileReading.Concurrent.Abstractions
{
    public sealed class FileReadOptions
    {
        public int WorkerCount { get; init; } = 1;

        public int? MaxConcurrency { get; init; }

        public Encoding Encoding { get; init; } = Encoding.UTF8;
    }
}
