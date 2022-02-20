using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using AbstractionProvider.Configuration;
using AbstractionProvider.CostFunctions;
using AbstractionProvider.Operators;
using CodeCompiler;
using ReactiveUI.Fody.Helpers;
using Runner.Commands;

namespace Runner.ViewModels;

public class ParametersViewModel : ViewModelBase
{

    public ICommand SelectData { get; set; }
    public IConfiguration Configuration { get; set; }
    [Reactive] public string SelectedFilesString { get; set; }
    [Reactive] public string[] SelectedData { get; set; }
    public string[] Resolvers { get; set; }
    
    public ParametersViewModel(IConfiguration model)
    {
        Configuration = model;
        SelectData = new SelectData(Configuration);
        var resolvers = ClassProvider.GetAllClassNames(typeof(ConflictResolver));

        Resolvers = resolvers;

    }
    
    
}