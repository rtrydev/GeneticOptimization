using System.Reflection;

namespace CodeCompiler;

public class ClassProvider
{
    public static string[] GetAllClassNames(Type baseType)
    {
        var included = GetIncludedClassNames(baseType);
        var dynamicallyAdded = GetDynamicClassNames(baseType);
        return included.Concat(dynamicallyAdded).ToArray();
    }
    public static string[] GetIncludedClassNames(Type baseType)
    {
        var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == "GeneticOptimization");
        var classes = assembly.GetTypes()
            .Where(p => p.IsSubclassOf(baseType)).ToArray().Select(x => x.ToString().Split(".").Last()).ToArray();
        return classes;
    }
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
        var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == "GeneticOptimization");
        var tp = assembly.GetTypes().
            Where(p => p.IsSubclassOf(baseType)).FirstOrDefault(x => x.Name.EndsWith(name));
        
        var files = new DirectoryInfo("Modules").GetFiles().Where(x => x.FullName.EndsWith(".dll")).Select(x => x.FullName).ToArray();

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