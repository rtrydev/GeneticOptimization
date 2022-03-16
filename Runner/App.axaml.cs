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
            var path = new[]
            {
                0, 21, 30, 17, 2, 16, 20, 41, 6, 1, 29, 22, 19, 49, 28, 15, 45, 43, 33, 34, 35, 38, 39, 36, 37, 47, 23,
                4, 14, 5, 3, 24, 11, 27, 26, 25, 46, 12, 13, 51, 10, 50, 32, 42, 9, 8, 7, 40, 18, 44, 31, 48
            };
            TspImageGenerator.GenerateImageFromTspPath(path, "/Users/rtry/Projects/GeneticOptimization/Datasets/TSP/be52.tsp");
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