namespace Demo
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Task1.Run();
            var path = Task2.Run();
            Task3.Run(path);
        }
    }
}
