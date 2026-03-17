namespace Demo
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nRunning task 1...\n");            
            Task1.InteractiveMethodInvocation.Program.Main();
            Console.WriteLine("\nRunning task 2...\n");
            Task2.ConsoleApp.Program.Main();
            Console.WriteLine("\nRunning task 3...\n");
            Task3.CarCreationConsoleApp.Program.Main();
        }
    }
}
