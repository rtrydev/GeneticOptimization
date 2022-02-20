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

    public static string[] GetMethodNames(Type attributeType)
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