using System.Diagnostics;

namespace Common.Diagnostics
{
    /// <summary>
    /// Helper methods for measuring execution performance of code blocks.
    /// </summary>
    public static class Performance
    {
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

        /// <summary>
        /// Measures execution time of the supplied method, and returns the elapsed time.
        /// </summary>
        /// <param name="action">Delegate representing the method to measure. Must not be null.</param>
        /// <returns>The elapsed time of the executed method.</returns>
        public static TimeSpan Measure(Action action)
        {
            ArgumentNullException.ThrowIfNull(action);

            Stopwatch stopwatch = Stopwatch.StartNew();
            action.Invoke();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}
