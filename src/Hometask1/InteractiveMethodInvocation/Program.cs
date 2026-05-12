using System.Reflection;
using ReflectionService;

namespace InteractiveMethodInvocation
{
    public static class Program
    {
        public static void Main()
        {
            try
            {
                ProcessInput(out Type type, out MethodInfo method, out object?[] argsDictionary);
                var instance = Activator.CreateInstance(type);
                var result = method.Invoke(instance, argsDictionary);
                Console.WriteLine($"Invoked method successfully, result: {result?.ToString()}\n");
            }
            catch (Exception ex) when
            (ex is ArgumentException or
            ArgumentNullException or
            ArgumentOutOfRangeException or
            FormatException)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void ProcessInput(out Type type, out MethodInfo method, out object?[] argsDictionary)
        {
            string className, methodName;
            string[] methodArguments;

            className = HandleStringInputVariables(nameof(className), "(for example, CarExample)");
            type = ReflectionServiceHelper.GetUserType(className);

            methodName = HandleStringInputVariables(nameof(methodName), "(for example, Create)");
            method = ReflectionServiceHelper.GetUserMethod(methodName, type);

            if (method.GetParameters().Length != 0)
            {
                methodArguments = HandleStringInputVariables(nameof(methodArguments), "split with one comma, for example: 1, MyAwesomeCar, T777OP)").Split(",", StringSplitOptions.TrimEntries);
                argsDictionary = ReflectionServiceHelper.ValidateParameters(methodArguments, method);
            }
            else
            {
                argsDictionary = [];
            }

            static string HandleStringInputVariables(string paramName, string instructions)
            {
                Console.WriteLine($"Enter the {paramName} {instructions}: ");
                var input = Console.ReadLine();

                while (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine($"{paramName} cannot be empty");
                    input = Console.ReadLine();
                }

                return input;
            }
        }
    }
}
