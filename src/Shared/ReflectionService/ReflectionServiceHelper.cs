using System.Globalization;
using System.Reflection;

namespace ReflectionService
{
    public static class ReflectionServiceHelper
    {
        public static Assembly GetUserAssembly(string filePath)
        {
            return Assembly.Load(filePath);
        }

        public static Type GetUserType(string className)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var type = assemblies
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t =>
                    t.Name.Equals(className, StringComparison.Ordinal) ||
                    t.FullName?.EndsWith($"{className}.cs", StringComparison.Ordinal) == true);

            if (type is null)
            {
                throw new ArgumentNullException(nameof(className), $"Type {className} was not found");
            }

            return type;
        }

        public static MethodInfo GetUserMethod(string methodName, Type targetType)
        {
            MethodInfo[] methodsWithOverloads = targetType
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                .Where(m =>
                    m.Name.Equals(methodName, StringComparison.Ordinal))
                .ToArray();

            if (methodsWithOverloads is null || methodsWithOverloads.Length == 0)
            {
                throw new ArgumentNullException(nameof(methodName), $"Method {methodName} was not found");
            }

            var method = methodsWithOverloads[0];

            if (methodsWithOverloads.Length > 1)
            {
                method = ChooseOverload(methodsWithOverloads);
            }

            return method;
        }

        public static MethodInfo ChooseOverload(MethodInfo[] methods)
        {
            Console.WriteLine($"{methods[0].Name} is overloaded.");
            Console.WriteLine($"{methods.Length} methods were found: ");
            for (int i = 1; i <= methods.Length; i++)
            {
                var parameters = methods[i - 1].GetParameters();
                Console.WriteLine($"[{i}] {methods[i - 1].Name} taking {parameters.Length} arguments:");
                foreach (var parameter in parameters)
                {
                    Console.WriteLine($"{parameter.ParameterType.Name}: {parameter.Name}");
                }
            }

            Console.WriteLine("Choose the required method by providing its number: ");
            var method = Console.ReadLine();

            return methods[HandleInput()];

            int HandleInput()
            {
                bool flag = false;
                int result = 0;

                while (!flag)
                {
                    if (string.IsNullOrWhiteSpace(method))
                    {
                        Console.WriteLine("Method name cannot be empty");
                        method = Console.ReadLine();
                        continue;
                    }

                    if (!int.TryParse(method, out result))
                    {
                        Console.WriteLine("Method choice should be a number");
                        method = Console.ReadLine();
                        continue;
                    }

                    if (result > methods.Length || result < 1)
                    {
                        Console.WriteLine($"Method choice should be between 1 and {methods.Length}");
                        method = Console.ReadLine();
                        continue;
                    }

                    flag = true;
                }

                return result - 1;
            }           
        }

        public static object?[] ValidateParameters(object[] methodArgsSplit, MethodInfo targetMehtod)
        {
            var methodParams = targetMehtod.GetParameters();

            if (methodParams.Length != methodArgsSplit.Length)
            {
                Console.WriteLine("Invalid number of parameters provided.");
            }

            var length = methodParams.Length;
            object?[] parsedVariables = new object?[length];

            for (int i = 0; i < length; i++)
            {
                Type currentType = methodParams[i].ParameterType;
                currentType = Nullable.GetUnderlyingType(currentType) ?? currentType;

                try
                {
                    var value = Convert.ChangeType(methodArgsSplit[i], currentType, CultureInfo.InvariantCulture);
                    parsedVariables[i] = value;
                }
                catch
                {
                    throw new FormatException(
                        $"Type of {methodArgsSplit[i]} is either mismatching {currentType} or cannot be parsed");
                }
            }
            return parsedVariables;
        }
    }
}
