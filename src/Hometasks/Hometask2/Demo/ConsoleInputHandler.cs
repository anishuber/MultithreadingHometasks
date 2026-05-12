using Scenarios;

namespace Demo
{
    internal static class ConsoleInputHandler
    {
        public static bool HandleChoice(string? choice, CarXmlScenario scenario)
        {
            string attributeName;
            string newAttributeValue;
            int elementNumber;

            string normalizedChoice = choice?.Trim() ?? string.Empty;

            switch (normalizedChoice)
            {
                case "1":
                    scenario.PrintObjects();
                    return false;

                case "2":
                    scenario.CreateTemplateCarObjects();
                    return false;

                case "3":
                    scenario.SerializeCars();
                    return false;

                case "4":
                    scenario.PrintXml();
                    return false;

                case "5":
                    scenario.DeserializeCars();
                    return false;

                case "6":
                    scenario.FindXmlAttributeXDocument("Model");
                    return false;

                case "7":
                    scenario.FindXmlAttributeXmlDocument("Model");
                    return false;

                case "8":
                    ReadDataForChangingAttribute(out attributeName, out elementNumber, out newAttributeValue);
                    scenario.ChangeXmlAttributeXDocument(attributeName, elementNumber, newAttributeValue);
                    return false;

                case "9":
                    ReadDataForChangingAttribute(out attributeName, out elementNumber, out newAttributeValue);
                    scenario.ChangeXmlAttributeXmlDocument(attributeName, elementNumber, newAttributeValue);
                    return false;

                case "0":
                    return true;

                default:
                    Console.WriteLine("Invalid choice, please try again.");
                    return false;
            }
        }

        public static void ReadDataForChangingAttribute(
            out string attributeName,
            out int elementNumber,
            out string newAttributeValue)
        {
            Console.Write("Enter the name of the attribute to change: ");
            attributeName = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter the number of the element to change: ");
            string elementNumberInput = Console.ReadLine() ?? string.Empty;

            if (!int.TryParse(elementNumberInput, out elementNumber))
            {
                Console.WriteLine("Invalid input for element number. Defaulting to 0.");
                elementNumber = 0;
            }

            Console.Write("Enter the new value for the attribute: ");
            newAttributeValue = Console.ReadLine() ?? string.Empty;
        }
    }
}
