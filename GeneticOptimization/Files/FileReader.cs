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

        throw new ArgumentException();
    }
    
    private double[][] ReadTsp(string fileName)
    {
        string[] lines = File.ReadAllLines(fileName);
        int nonEmptyLines = 0;
        for (int i = 0; i < lines.Length; i++)
            if (lines[i].Trim().Length > 1 && lines[i]!="EOF")
                nonEmptyLines++;

        int linesBeforeValues = 0;
        for (int i = 0; i < nonEmptyLines; i++)
        {
            if(int.TryParse(lines[i].Split(" ")[0],out var n))
                break;
            linesBeforeValues++;
        }

        double[][] matrix = new double[nonEmptyLines-linesBeforeValues][];
        for (int i = 0; i < matrix.Length; i++)
        {
            matrix[i] = new double[matrix.Length];
        }
        double[] x = new double[nonEmptyLines-linesBeforeValues];
        double[] y = new double[nonEmptyLines-linesBeforeValues];

        for (int i = linesBeforeValues; i < nonEmptyLines; i++)
        {
            string[] s = lines[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            x[i-linesBeforeValues] = double.Parse(s[1]);
            y[i-linesBeforeValues] = double.Parse(s[2]);
        }

        for (int i = 0; i < x.Length; i++)
        {
            for (int j = 0; j < x.Length; j++)
            {
                matrix[i][j] = Math.Sqrt((x[i] - x[j]) * (x[i] - x[j]) + (y[i] - y[j]) * (y[i] - y[j]));
            }
        }

        return matrix;
    }
}