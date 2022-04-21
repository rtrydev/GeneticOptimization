using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.IO;
using AbstractionProvider.Configuration;
using AbstractionProvider.Operators;
using Runner.ViewModels;

namespace Runner;

public class ConfigReloader
{
    public static OperatorViewModel OperatorViewModel { get; set; }
    public static ParametersViewModel ParametersViewModel { get; set; }
    public static ControlViewModel ControlViewModel { get; set; }
    public static AlgorithmViewModel AlgorithmViewModel { get; set; }

    public static void Reload(IConfiguration config)
    {
        ParametersViewModel.Configuration = config;
        OperatorViewModel.Configuration = config;
        OperatorViewModel.OperatorInformation =
            new ObservableCollection<OperatorInformation>(config.OperatorInformation);
        ControlViewModel.ParametersModel = config;
        ControlViewModel.ReloadCommand();
        AlgorithmViewModel.Configuration = config;
    }
}