using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using AbstractionProvider.Configuration;
using AbstractionProvider.Operators;
using AbstractionProvider.PopulationModels;
using Avalonia.Media.Imaging;
using GeneticOptimization.Algorithm;
using GeneticOptimization.Configuration;
using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Avalonia;
using OxyPlot.Axes;
using ReactiveUI.Fody.Helpers;
using Runner.Commands;
using Runner.Models;
using Runner.Visualization;
using Legend = OxyPlot.Legends.Legend;
using LineSeries = OxyPlot.Series.LineSeries;

namespace Runner.ViewModels;

public class EpochValue
{
    public int Epoch { get; set; }
    public double Value { get; set; }
}

public class ResultViewModel : ViewModelBase
{
    public GeneticAlgorithmResult<TspPopulationModel, TspConfiguration> Result { get; set; }
    public OperatorInformation[] Operators { get; set; }

    public Collection<EpochValue> BestCosts { get; set; }
    [Reactive] public bool BestVisible { get; set; } = true;
    public Collection<EpochValue> AvgCosts { get; set; }
    [Reactive] public bool AvgVisible { get; set; } = true;
    public Collection<EpochValue> MedianCosts { get; set; }
    [Reactive] public bool MedianVisible { get; set; } = true;
    public Collection<EpochValue> WorstCosts { get; set; }
    [Reactive] public bool WorstVisible { get; set; } = true;
    public Bitmap Preview { get; set; }

    public ICommand LoadResultConfig { get; set; }
    public ICommand OpenLogFolder { get; set; }
    
    [Reactive] public bool LoadSuccessMessageVisible { get; set; }

    public ResultViewModel(ResultFileModel data)
    {
        var directory = "Results/" + data.FileName;
        var jsonString = File.ReadAllText( directory + "/result.json");
        Result = JsonConvert.DeserializeObject<GeneticAlgorithmResult<TspPopulationModel, TspConfiguration>>(jsonString);
        var dataset = directory + "/data.tsp";

        if (File.Exists(dataset) && dataset.EndsWith(".tsp"))
        {
            if (Result.Configuration.CostFunction == "TspCostFunction")
            {
                TspImageGenerator.GenerateImageFromTspPath(Result.BestIndividual.Body, dataset, directory);
                using (var fileStream = File.Open( $"{directory}/preview.png", FileMode.Open))
                {
                    Preview = Bitmap.DecodeToHeight(fileStream, 1000);
                }
            }

            if (Result.Configuration.CostFunction == "WarehouseCostFunction")
            {
                WarehouseImageGenerator.Generate(Result.BestIndividual.Body, dataset, directory);
                using (var fileStream = File.Open($"{directory}/preview.png", FileMode.Open))
                {
                    Preview = Bitmap.DecodeToHeight(fileStream, 1000);
                }
            }
        }

        LoadResultConfig = new LoadResultConfig(Result.Configuration, async () =>
        {
            LoadSuccessMessageVisible = true;
            Thread.Sleep(4000);
            LoadSuccessMessageVisible = false;
        });
        OpenLogFolder = new OpenLogFolder(directory);
        
        BestCosts = new Collection<EpochValue>();
        AvgCosts = new Collection<EpochValue>();
        WorstCosts = new Collection<EpochValue>();
        MedianCosts = new Collection<EpochValue>();
        for (int i = 0; i < Result.Configuration.MaxIterations; i++)
        {
            AvgCosts.Add(new EpochValue()
            {
                Value = Result.AvgCostHistory[i],
                Epoch = i
            });
            MedianCosts.Add(new EpochValue()
            {
                Value = Result.MedianCostHistory[i],
                Epoch = i
            });
            BestCosts.Add(new EpochValue()
            {
                Value = Result.BestCostHistory[i],
                Epoch = i
            });
            WorstCosts.Add(new EpochValue()
            {
                Value = Result.WorstCostHistory[i],
                Epoch = i
            });
        }
        Operators = Result.Configuration.OperatorInformation;
        if (!File.Exists($"{directory}/plot.bmp"))
        {
            var avgSeries = new LineSeries()
            {
                Title = "Avg Cost",
                StrokeThickness = 3,
                DataFieldX = "Epoch",
                DataFieldY = "Value",
                ItemsSource = AvgCosts,
                LineStyle = LineStyle.Solid,
                Color = OxyColor.Parse("#ff00ff")
            };
            var medianSeries = new LineSeries()
            {
                Title = "Median Cost",
                StrokeThickness = 3,
                DataFieldX = "Epoch",
                DataFieldY = "Value",
                ItemsSource = MedianCosts,
                LineStyle = LineStyle.Solid,
                Color = OxyColor.Parse("#6d6dfd")
            };
            var bestSeries = new LineSeries()
            {
                Title = "Best Cost",
                StrokeThickness = 3,
                DataFieldX = "Epoch",
                DataFieldY = "Value",
                ItemsSource = BestCosts,
                LineStyle = LineStyle.Solid,
                Color = OxyColor.Parse("#00dd00")
            };
            var worstSeries = new LineSeries()
            {
                Title = "Worst Cost",
                StrokeThickness = 3,
                DataFieldX = "Epoch",
                DataFieldY = "Value",
                ItemsSource = WorstCosts,
                LineStyle = LineStyle.Solid,
                Color = OxyColor.Parse("#fd6d00")
            };
            var plotModel = new PlotModel()
            {
                Series = { avgSeries, medianSeries, bestSeries, worstSeries },
                Legends = { new Legend()}
            };
            var pngExporter = new PngExporter { Width = 1000, Height = 600, Background = OxyColors.White };
            var bmp = pngExporter.ExportToBitmap(plotModel);
            bmp.Save($"{directory}/plot.bmp");
        }
        
    }

    public ResultViewModel()
    {
        
    }
}