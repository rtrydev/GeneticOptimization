using System;
using System.Linq;
using System.Windows.Input;
using GeneticOptimization.Configuration;
using GeneticOptimization.Operators.ConflictResolvers;
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
        var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == "GeneticOptimization");
        Resolvers = assembly.GetTypes()
            .Where(p => typeof(IConflictResolver).IsAssignableFrom(p) && p.IsClass).ToArray().Select(x => x.ToString().Split(".").Last()).ToArray();
    }
    
    
}