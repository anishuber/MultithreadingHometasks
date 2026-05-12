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
        [Obsolete("Use Measure<T> instead to get both the result and elapsed time.")]
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

        /// <summary>
        /// Measures execution time of the supplied method, and returns the method result along with the elapsed time.
        /// </summary>
        /// <typeparam name="T">Return type of the measured method.</typeparam>
        /// <param name="method">Delegate representing the method to measure. Must not be null.</param>
        /// <returns>A tuple containing the value returned by the executed method and the elapsed time.</returns>
        public static (T Result, TimeSpan Elapsed) Measure<T>(Func<T> method)
        {
            ArgumentNullException.ThrowIfNull(method);

            Stopwatch stopwatch = Stopwatch.StartNew();
            T result = method.Invoke();
            stopwatch.Stop();
            return (result, stopwatch.Elapsed);
        }
    }
}
