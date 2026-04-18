using System.Diagnostics;

namespace Helpers
{
    public static class Performance
    {
        public static T MeasurePerformance<T>(Func<T> method)
        {
            ArgumentNullException.ThrowIfNull(method);

            Stopwatch stopwatch = Stopwatch.StartNew();
            T result = method.Invoke();
            stopwatch.Stop();

            Console.WriteLine($"Ticks elapsed: {stopwatch.ElapsedTicks}");
            Console.WriteLine($"Milliseconds elapsed: {stopwatch.ElapsedMilliseconds}");

            return result;
        }
    }
}
