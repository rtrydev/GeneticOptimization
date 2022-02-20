using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using AbstractionProvider.Configuration;
using AbstractionProvider.CostFunctions;
using CodeCompiler;
using ReactiveUI.Fody.Helpers;
using Runner.Commands;
using Runner.Models;

namespace Runner.ViewModels;

public class AlgorithmViewModel : ViewModelBase
{
    [Reactive] public string[] CostMethods { get; set; }
    public IConfiguration Configuration { get; }
    
    public ConsoleLogModel LogModel { get; set; }
    public ICommand Compile { get; set; }

    public AlgorithmViewModel(IConfiguration configuration, ConsoleLogModel logModel)
    {
        LogModel = logModel;
        Compile = new Compile(CostMethods);
        Configuration = configuration;
        var assembly = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name == "GeneticOptimization").ToArray()[0];
        var methods = assembly.GetTypes()
            .SelectMany(t => t.GetMethods())
            .Where(m => m.GetCustomAttributes(typeof(CostFunction), false).Length > 0)
            .ToArray();

        var methodNames = methods.Select(x => x.DeclaringType.ToString()).ToArray();

        CostMethods = methodNames.Concat(MethodProvider.GetMethodNames(typeof(CostFunction))).ToArray();
    }
}
