namespace Demo
{
    internal static class MenuRenderer
    {
        public static void PrintMenu()
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Print objects to console");
            Console.WriteLine("2. Create 10 objects");
            Console.WriteLine("3. Serialize objects to XML file");
            Console.WriteLine("4. Read XML file and print contents");
            Console.WriteLine("5. Deserialize objects from file");
            Console.WriteLine("6. Find all values of the Model attribute (XDocument)");
            Console.WriteLine("7. Find all values of the Model attribute (XmlDocument)");
            Console.WriteLine("8. Change attribute value (XDocument)");
            Console.WriteLine("9. Change attribute value (XmlDocument)");
            Console.WriteLine("0. Exit");
            Console.Write("Select a menu item: ");
        }
    }
}
