using GeneticOptimization.Data;

namespace GeneticOptimization.Files;

public class FileReader
{
    public double[][] ReadDistancesMatrix(string fileName)
    {
        if (fileName.EndsWith(".tsp"))
        {
            var matrix = ReadTsp(fileName);
            return matrix;
        }

        if (fileName.EndsWith(".mtrx"))
        {
            var matrix = ReadMatrix(fileName);
            return matrix;
        }

        if (fileName.EndsWith(".txt"))
        {
            var data = ReadMatrix(fileName);
            var matrix = Dijkstra.GenerateDistanceArray(data);
            return matrix;
        }

        throw new ArgumentException();
    }

    private double[][] ReadMatrix(string fileName)
    {
        string[] lines = File.ReadAllLines(fileName);
        int nonEmptyLines = 0;
        for (int i = 0; i < lines.Length; i++)
            if (lines[i].Trim().Length > 1)
                nonEmptyLines++;

        double[][] Matrix = new double[nonEmptyLines][];
        for (int i = 0; i < nonEmptyLines; i++)
        {
            string[] s = lines[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            Matrix[i] = Array.ConvertAll(s, double.Parse);
        }

        return Matrix;
    }
    
    private double[][] ReadTsp(string fileName)
    {
        var lines = File.ReadAllLines(fileName);
        var type = "";
        var lineWithType = lines.Where(x => x.Contains("TYPE : ")).FirstOrDefault();
        if (lineWithType is null)
        {
            lineWithType = lines.Where(x => x.Contains("TYPE: ")).FirstOrDefault();
            type = lineWithType.Replace("TYPE: ", "");
        }
        else
        {
            type = lineWithType.Replace("TYPE : ", "");

        }

        lines = lines.SkipWhile(x => x != "NODE_COORD_SECTION").Skip(1).SkipLast(1).ToArray();

        var result = lines.Select(x => new TspPoint()
        {
            Id = int.Parse(x.Split(" ")[0]), 
            X = double.Parse(x.Split(" ")[1]), 
            Y = double.Parse(x.Split(" ")[2])
        }).ToArray();
        
        var matrix = new double[result.Length][];
        for (int i = 0; i < matrix.Length; i++)
        {
            matrix[i] = new double[result.Length];
        }

        
        if (type == "TSP")
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix.Length; j++)
                {
                    var xDiff = result[i].X - result[j].X;
                    var yDiff = result[i].Y - result[j].Y;
                    matrix[i][j] = Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
                }
            }
        }

        if (type == "WAREHOUSE")
        {
            

            var lowestY = result.Where(x => x.Id != 1).Min(x => x.Y);
            var highestY = result.Where(x => x.Id != 1).Max(x => x.Y);

            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix.Length; j++)
                {
                    if (i == j)
                    {
                        matrix[i][j] = 0d;
                        continue;
                    }
                    var fromX = result[i].X < result[j].X ? result[i].X : result[j].X;
                    var toX = result[i].X > result[j].X ? result[i].X : result[j].X;
                    var distance = 0d;

                    if (result.Any(x => x.X > fromX && x.X < toX))
                    {
                        var yDiff1 = Math.Abs(lowestY - result[i].Y) + Math.Abs(lowestY - result[j].Y);
                        var yDiff2 = Math.Abs(highestY - result[i].Y) + Math.Abs(highestY - result[j].Y);
                        
                        var xDiff = Math.Abs(result[j].X - result[i].X);
                        var yDiff = yDiff1 < yDiff2 ? yDiff1 : yDiff2;
                        matrix[i][j] = yDiff + xDiff;
                    }
                    else
                    {
                        var xDiff = Math.Abs(result[j].X - result[i].X);
                        var yDiff = Math.Abs(result[j].Y - result[i].Y);
                        matrix[j][i] = xDiff + yDiff;
                    }
                    

                    
                }
            }
        }

        

        return matrix;
    }
}