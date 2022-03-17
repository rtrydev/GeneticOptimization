using System.Text;

namespace GeneticOptimization.Files;

public class FileWriter
{
    public static void WriteArray(string fileName, double[][] Matrix)
    {
        if(File.Exists(fileName)) return;
        using (var streamWriter = new StreamWriter(fileName))
        {
            for (int i = 0; i < Matrix.Length - 1; i++)
            {
                streamWriter.WriteLine(string.Join(" ", Matrix[i]));
            }
            streamWriter.Write(string.Join(" ", Matrix[^1]));
        }

    }
}