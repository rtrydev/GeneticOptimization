using System.Collections.ObjectModel;
using System.IO;
using AbstractionProvider.Operators;
using AbstractionProvider.PopulationModels;
using GeneticOptimization.Algorithm;
using GeneticOptimization.Configuration;
using Newtonsoft.Json;
using ReactiveUI.Fody.Helpers;

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
    public ResultViewModel(string data)
    {
        var jsonString = File.ReadAllText(data);
        Result = JsonConvert.DeserializeObject<GeneticAlgorithmResult<TspPopulationModel, TspConfiguration>>(jsonString);

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
    }

    public ResultViewModel()
    {
        
    }
}