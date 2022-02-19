using AbstractionProvider.Configuration;

namespace Runner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public IConfiguration Configuration { get; }
        public ParametersViewModel ParametersViewModel { get; }
        public ControlViewModel ControlViewModel { get; }
        public LogViewModel LogViewModel { get; }
        public HistoryViewModel HistoryViewModel { get; }
        
        public AlgorithmViewModel AlgorithmViewModel { get; }
        
        public OperatorViewModel OperatorViewModel { get; }
        public MainWindowViewModel(IConfiguration configuration, ParametersViewModel parametersViewModel, ControlViewModel controlViewModel, LogViewModel logViewModel, OperatorViewModel operatorViewModel, HistoryViewModel historyViewModel, AlgorithmViewModel algorithmViewModel)
        {
            Configuration = configuration;
            LogViewModel = logViewModel;
            ParametersViewModel = parametersViewModel;
            ControlViewModel = controlViewModel;
            OperatorViewModel = operatorViewModel;
            HistoryViewModel = historyViewModel;
            AlgorithmViewModel = algorithmViewModel;
            ControlViewModel.ParametersModel = Configuration;
        }

        public MainWindowViewModel()
        {
            
        }
    }
}