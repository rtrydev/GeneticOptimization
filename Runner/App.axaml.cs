using System;
using System.Globalization;
using System.IO;
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
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (!File.Exists("Modules"))
            {
                Directory.CreateDirectory("Modules");
            }
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var logModel = new ConsoleLogModel();
                var config = new TspConfiguration();
                var parametersVM = new ParametersViewModel(config);
                var historyVM = new HistoryViewModel();

                var controlVM = new ControlViewModel(config, logModel, parametersVM, historyVM);
                var operatorVM = new OperatorViewModel(config);
                var algorithmVM = new AlgorithmViewModel(config, logModel);
                var logVM = new LogViewModel(logModel);
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(config, parametersVM, controlVM, logVM, operatorVM, historyVM, algorithmVM),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}