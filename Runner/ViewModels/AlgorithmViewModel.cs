using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AbstractionProvider.Configuration;
using AbstractionProvider.CostFunctions;
using ReactiveUI.Fody.Helpers;

namespace Runner.ViewModels;

public class AlgorithmViewModel : ViewModelBase
{
    [Reactive] public string[] CostMethods { get; set; }
    public IConfiguration Configuration { get; }

    public AlgorithmViewModel(IConfiguration configuration)
    {
        Configuration = configuration;
        var assembly = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name == "GeneticOptimization").ToArray()[0];
        var methods = assembly.GetTypes()
            .SelectMany(t => t.GetMethods())
            .Where(m => m.GetCustomAttributes(typeof(CostFunction), false).Length > 0)
            .ToArray();

        var files = new DirectoryInfo("Modules").GetFiles().Select(x => x.FullName).ToArray();

        foreach (var file in files)
        {
            var dynamicAssembly = Assembly.LoadFile(file);
            var dynamicallyLoadedMethods = dynamicAssembly.GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(CostFunction), false).Length > 0)
                .ToArray();

            methods = methods.Concat(dynamicallyLoadedMethods).ToArray();
        }
        

        CostMethods = methods.Select(x => x.DeclaringType.ToString()).ToArray();
    }
}
