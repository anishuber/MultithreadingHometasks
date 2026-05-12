using Common.Diagnostics.Abstractions;

namespace Common.Diagnostics.Logging
{
    public sealed class ConsoleLogger : ILogger
    {
        public void Info(string message)
        {
            Console.WriteLine($"[INFO] {message}");
        }

        public void Warning(string message)
        {
            Console.WriteLine($"[WARNING] {message}");
        }

        public void Error(string message, Exception? exception = null)
        {
            Console.WriteLine($"[ERROR] {message}");
            if (exception is not null)
            {
                Console.WriteLine($"[ERROR] Exception: {exception}");
            }
        }
    }
}
