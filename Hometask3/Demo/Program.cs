namespace Demo
{
    /// <summary>
    /// Program entry point for the demo application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main entry point. Executes demo tasks in order.
        /// </summary>
        public static void Main()
        {
            Task1.Run();
            var path = Task2.Run();
            Task3.Run(path);
        }
    }
}
