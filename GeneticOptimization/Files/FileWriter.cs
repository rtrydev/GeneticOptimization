using System.Text;

namespace GeneticOptimization.Files;

public class FileWriter
{
    public static void WriteArray(string fileName, double[][] Matrix)
    {

        StringBuilder stringBuilder = new StringBuilder();

        /*for (int i = 0; i < Matrix.Length - 1; i++)
            stringBuilder.AppendLine(string.Join(" ", Matrix[i]));
        stringBuilder.Append(string.Join(" ", Matrix[Matrix.Length - 1]));*/

        if(File.Exists(fileName)) return;
        using (var streamWriter = new StreamWriter(fileName))
        {
            for (int i = 0; i < Matrix.Length - 1; i++)
            {
                streamWriter.WriteLine(string.Join(" ", Matrix[i]));
            }
            streamWriter.Write(string.Join(" ", Matrix[^1]));
        }
        //File.WriteAllText(fileName, stringBuilder.ToString());

    }
}