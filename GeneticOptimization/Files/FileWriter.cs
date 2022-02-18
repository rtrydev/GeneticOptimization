using System.Text;

namespace GeneticOptimization.Files;

public class FileWriter
{
    public static void WriteArray(string fileName, double[][] Matrix)
    {

        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < Matrix.Length - 1; i++)
            stringBuilder.AppendLine(string.Join(" ", Matrix[i]));
        stringBuilder.Append(string.Join(" ", Matrix[Matrix.Length - 1]));

        File.WriteAllText(fileName, stringBuilder.ToString());

    }
}