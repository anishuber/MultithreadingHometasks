using System.Diagnostics;

namespace Helpers
{
    /// <summary>
    /// Helper methods for measuring execution performance of code blocks.
    /// </summary>
    public static class Performance
    {
        /// <summary>
        /// Measures execution time of the supplied method, writes timing information to the console,
        /// and returns the method result.
        /// </summary>
        /// <typeparam name="T">Return type of the measured method.</typeparam>
        /// <param name="method">Delegate representing the method to measure. Must not be null.</param>
        /// <returns>The value returned by the executed method.</returns>
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
