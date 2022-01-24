using GeneticOptimization;
using GeneticOptimization.Configuration;

var config = new TspConfiguration();
var geneticOptimization = new GeneticOptimizer(config);
geneticOptimization.Run();