namespace Demo
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nRunning task 1...\n");
            InteractiveMethodInvocation.Program.Main();
            Console.WriteLine("\nRunning task 2...\n");
            CarMetadataViewer.Program.Main();
            Console.WriteLine("\nRunning task 3...\n");
            CarCreation.Program.Main();
        }
    }
}
