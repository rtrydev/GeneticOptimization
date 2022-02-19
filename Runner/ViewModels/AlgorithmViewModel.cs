using System;
using System.Linq;
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
        CostMethods = methods.Select(x => x.DeclaringType.ToString()).ToArray();
    }
}
