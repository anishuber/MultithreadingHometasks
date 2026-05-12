using Scenarios;

namespace Demo
{
    internal static class Program
    {
        static void Main()
        {
            // TODO: add file path for artifacts to config file

            string filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                @"..\..\..\..\..\..\..",
                "artifacts",
                "SerializedCars.xml");

            CarXmlScenario? scenario = TryCreateScenario(filePath);

            if (scenario is null)
            {
                return;
            }

            RunMenu(scenario);
        }

        private static void RunMenu(CarXmlScenario scenario)
        {
            while (true)
            {
                MenuRenderer.PrintMenu();
                string? choice = Console.ReadLine();

                try
                {
                    if (ConsoleInputHandler.HandleChoice(choice, scenario))
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }

                Console.WriteLine();
            }
        }

        private static CarXmlScenario? TryCreateScenario(string filePath)
        {
            try
            {
                return new CarXmlScenario(filePath);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка инициализации: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Ошибка доступа: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }

            return null;
        }
    }
}
