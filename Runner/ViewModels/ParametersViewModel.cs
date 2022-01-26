using System.Windows.Input;
using GeneticOptimization.Configuration;
using ReactiveUI.Fody.Helpers;
using Runner.Commands;

namespace Runner.ViewModels;

public class ParametersViewModel : ViewModelBase
{

    public ICommand SelectData { get; set; }
    public IConfiguration Configuration { get; set; }
    [Reactive] public string SelectedFilesString { get; set; }
    [Reactive] public string[] SelectedData { get; set; }
    
    public ParametersViewModel(IConfiguration model)
    {
        Configuration = model;
        SelectData = new SelectData(Configuration);
    }
    
    
}