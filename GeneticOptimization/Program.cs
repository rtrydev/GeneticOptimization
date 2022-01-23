using GeneticOptimization.Algorithm;
using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.Operators.ConflictResolvers;
using GeneticOptimization.Operators.Crossovers;
using GeneticOptimization.Operators.Eliminations;
using GeneticOptimization.Operators.Mutations;
using GeneticOptimization.Operators.Selections;

var config = new GeneticConfiguration();
var costMatrix = new TspCostMatrix(config);

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

