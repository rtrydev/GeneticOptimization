using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using AbstractionProvider.Configuration;
using AbstractionProvider.CostFunctions;
using AbstractionProvider.Operators;
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
        var resolvers = assembly.GetTypes()
            .Where(p => typeof(IConflictResolver).IsAssignableFrom(p) && p.IsClass).ToArray().Select(x => x.ToString().Split(".").Last()).ToArray();
        var files = new DirectoryInfo("Modules").GetFiles().Select(x => x.FullName).ToArray();

        foreach (var file in files)
        {
            var dynamicAssembly = Assembly.LoadFile(file);
            var dynamicallyLoadedResolvers = dynamicAssembly.GetTypes()
                .Where(p => typeof(IConflictResolver).IsAssignableFrom(p) && p.IsClass).Select(x => x.FullName);

            resolvers = resolvers.Concat(dynamicallyLoadedResolvers).ToArray();
        }

        Resolvers = resolvers;

    }
    
    
}