using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GeneticOptimization.Configuration;
using Runner.Models;
using Runner.ViewModels;
using Runner.Views;

namespace Runner
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var logModel = new ConsoleLogModel();
                var config = new TspConfiguration();
                var parametersVM = new ParametersViewModel(config);
                var controlVM = new ControlViewModel(config, logModel, parametersVM);
                var operatorVM = new OperatorViewModel(config);
                var historyVM = new HistoryViewModel();
                var logVM = new LogViewModel(logModel);
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(config, parametersVM, controlVM, logVM, operatorVM, historyVM),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}