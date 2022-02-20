using System.Reflection;

namespace CodeCompiler;

public class MethodProvider
{
    public static MethodInfo GetMethod(string name, Type attributeType)
    {
        var files = new DirectoryInfo("Modules").GetFiles().Where(x => x.FullName.EndsWith(".dll")).Select(x => x.FullName).ToArray();

        MethodInfo method = null;
        foreach (var file in files)
        {
            if (method is null)
            {
                var dynamicAssembly = Assembly.LoadFile(file);
                var dynamicallyLoadedMethods = dynamicAssembly.GetTypes()
                    .SelectMany(t => t.GetMethods())
                    .Where(m => m.GetCustomAttributes(attributeType, false).Length > 0)
                    .ToArray();

                method = dynamicallyLoadedMethods.FirstOrDefault(m => m.GetCustomAttributes(attributeType, false).Length > 0 &&
                                                                      m.DeclaringType.ToString().EndsWith(name));
            }
            else break;
        }

        return method;
    }

    public static string[] GetAllMethodNames(Type attributeType)
    {
        var included = GetIncludedMethodNames(attributeType);
        var dynamicallyAdded = GetDynamicMethodNames(attributeType);
        return included.Concat(dynamicallyAdded).ToArray();
    }

    public static string[] GetIncludedMethodNames(Type attributeType)
    {
        var assembly = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name == "GeneticOptimization").ToArray()[0];
        var methods = assembly.GetTypes()
            .SelectMany(t => t.GetMethods())
            .Where(m => m.GetCustomAttributes(attributeType, false).Length > 0)
            .ToArray();

        var methodNames = methods.Select(x => x.DeclaringType.ToString()).ToArray();
        return methodNames;
    }

    public static string[] GetDynamicMethodNames(Type attributeType)
    {
        var files = new DirectoryInfo("Modules").GetFiles().Where(x => x.FullName.EndsWith(".dll")).Select(x => x.FullName).ToArray();

        var methods = Array.Empty<MethodInfo>();
        foreach (var file in files)
        {
            var dynamicAssembly = Assembly.LoadFile(file);
            var dynamicallyLoadedMethods = dynamicAssembly.GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(attributeType, false).Length > 0)
                .ToArray();

            methods = methods.Concat(dynamicallyLoadedMethods).ToArray();
        }
        

        return methods.Select(x => x.DeclaringType.ToString()).ToArray();
    }
}