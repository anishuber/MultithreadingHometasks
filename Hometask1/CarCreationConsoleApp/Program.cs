using System.Reflection;
using ReflectionService;

namespace Task3.CarCreationConsoleApp
{
    public static class Program
    {
        public static void Main()
        {
            string innerPath = @"CarLibrary\obj\Debug\net8.0\CarLibrary.dll";
            string goUpFourDirs = @"..\..\..\..";
            string createMethodName = "Create";
            string printMethodName = "PrintObject";


            string path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, goUpFourDirs, innerPath));

            Assembly a = Assembly.LoadFrom(path);
            var types = a.GetTypes().Where(t => !t.IsValueType);

            foreach (var type in types)
            {
                object[] parameters;

                var instance = Activator.CreateInstance(type);
                Console.WriteLine($"{type.Name} instantiated");

                var createMethod = ReflectionServiceHelper.GetUserMethod(createMethodName, type);

                parameters = PopulateParameters(createMethod);

                var curTypeParameters = ReflectionServiceHelper.ValidateParameters(parameters.ToArray(), createMethod);
                createMethod?.Invoke(instance, curTypeParameters);
                Console.WriteLine($"{createMethodName} method invoked");

                var printMethod = ReflectionServiceHelper.GetUserMethod(printMethodName, type);
                
                parameters = PopulateParameters(printMethod);

                curTypeParameters = ReflectionServiceHelper.ValidateParameters(parameters.ToArray(), printMethod);
                printMethod?.Invoke(instance, curTypeParameters);
                Console.WriteLine($"{printMethodName} method invoked");
            }
        }

        private static object[] PopulateParameters(MethodInfo methodInfo)
        {
            List<object> parameters = [];
            var parameterInfos = methodInfo.GetParameters();

            if (parameterInfos.Length == 0)
            {
                return [];
            }

            foreach (var param in parameterInfos)
            {
                if (param.ParameterType == typeof(string))
                {
                    parameters.Add("Default string");
                }
                else if (param.ParameterType == typeof(int))
                {
                    parameters.Add(1);
                }
                else
                {
                    parameters.Add(param.ParameterType.IsValueType
                        ? Activator.CreateInstance(param.ParameterType)
                        : null);
                }
            }

            return parameters.ToArray();
        }
    }
}
