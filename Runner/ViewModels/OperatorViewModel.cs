using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AbstractionProvider.Configuration;
using AbstractionProvider.Operators;
using DynamicData.Binding;
using ReactiveUI.Fody.Helpers;
using Runner.Commands;

namespace Runner.ViewModels;

public class OperatorViewModel : ViewModelBase
{ 
    public IConfiguration Configuration { get; set; }
    [Reactive] public ObservableCollection<OperatorInformation> OperatorInformation { get; set; }
    public ICommand RemoveOperator { get; set; }
    public ICommand AddOperator { get; set; }
    public OperatorInformation OtherOperators { get; set; }
    public OperatorViewModel(IConfiguration configuration)
    {
        OperatorInformation = new ObservableCollection<OperatorInformation>(configuration.OperatorInformation);
        RemoveOperator = new RemoveOperator(OperatorInformation, configuration);
        AddOperator = new AddOperator(OperatorInformation, configuration);
        OtherOperators = new OperatorInformation(OperatorTypes.Other, "Generic");
        
        Configuration = configuration;
    }

    public void ReloadCommands()
    {
        RemoveOperator = new RemoveOperator(OperatorInformation, Configuration);
        AddOperator = new AddOperator(OperatorInformation, Configuration);
    }
}