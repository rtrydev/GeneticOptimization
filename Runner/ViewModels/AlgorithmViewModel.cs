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
        CostMethods = MethodProvider.GetAllMethodNames(typeof(CostFunction));
    }
}
