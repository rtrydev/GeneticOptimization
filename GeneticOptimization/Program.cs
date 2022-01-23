using GeneticOptimization.Algorithm;
using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.Files;
using GeneticOptimization.Operators.ConflictResolvers;
using GeneticOptimization.Operators.Crossovers;
using GeneticOptimization.Operators.Eliminations;
using GeneticOptimization.Operators.Mutations;
using GeneticOptimization.Operators.Selections;

var config = new GeneticConfiguration();
var fileReader = new FileReader();
var costMatrix = new TspCostMatrix(fileReader.ReadDistancesMatrix("/Users/rtry/be52.tsp"));

var conflictResolver = new NearestNeighborResolver(costMatrix);

var selection = new RouletteSelection(config);
var crossover = new AexCrossover(config, conflictResolver);
var elimination = new ElitismElimination(config);
var mutation = new RsmMutation(config);

var algorithm = new GeneticAlgorithm(config);

algorithm.AddOperator(selection);
algorithm.AddOperator(crossover);
algorithm.AddOperator(elimination);
algorithm.AddOperator(mutation);
algorithm.AddCostMatrix(costMatrix);
algorithm.Run();

