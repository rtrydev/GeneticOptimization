using System.Reflection;

namespace CodeCompiler;

public class ClassProvider
{
    public static string[] GetDynamicClassNames(Type baseType)
    {
        var files = new DirectoryInfo("Modules").GetFiles().Where(x => x.FullName.EndsWith(".dll")).Select(x => x.FullName).ToArray();

        var result = Array.Empty<string>();
        
        foreach (var file in files)
        {
            var dynamicAssembly = Assembly.LoadFile(file);
            var dynamicallyLoadedClasses = dynamicAssembly.GetTypes()
                .Where(m => m.IsSubclassOf(baseType)).ToArray().Select(x => x.ToString().Split(".").Last())
                .ToArray();

            result = result.Concat(dynamicallyLoadedClasses).ToArray();
        }

        return result;
    }

    public static Type GetClass(string name, Type baseType)
    {
        var files = new DirectoryInfo("Modules").GetFiles().Where(x => x.FullName.EndsWith(".dll")).Select(x => x.FullName).ToArray();

        Type tp = null;
        foreach (var file in files)
        {
            if (tp is null)
            {
                var dynamicAssembly = Assembly.LoadFile(file);
                var dynamicallyLoadedMethods = dynamicAssembly.GetTypes()
                    .Where(p => baseType.IsAssignableFrom(p) && p.IsClass)
                    .ToArray();

                tp = dynamicallyLoadedMethods.FirstOrDefault(x => x.Name.EndsWith(name));
            }
            else break;
        }

        return tp;
    }
}