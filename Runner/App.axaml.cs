using System;
using System.Globalization;
using System.IO;
using AbstractionProvider.Configuration;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GeneticOptimization.Configuration;
using Newtonsoft.Json;
using Runner.Models;
using Runner.ViewModels;
using Runner.Views;
using Runner.Visualization;

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

                IConfiguration config;
                if (File.Exists("default-config.json"))
                {
                    var jsonString = File.ReadAllText("default-config.json");
                    config = JsonConvert.DeserializeObject<TspConfiguration>(jsonString);
                }
                else
                {
                    config = new TspConfiguration();
                }
                var operatorVM = new OperatorViewModel(config);
                
                var parametersVM = new ParametersViewModel(config, operatorVM);
                var historyVM = new HistoryViewModel();

                var controlVM = new ControlViewModel(config, logModel, parametersVM, historyVM);
                parametersVM.ControlViewModel = controlVM;
                
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