using System;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Runner.Visualization;

public class TspImageGenerator
{
    public static void GenerateImageFromTspPath(int[] path, string tspFile)
    {
        var lines = File.ReadAllLines(tspFile);
        lines = lines.SkipWhile(x => x != "NODE_COORD_SECTION").Skip(1).SkipLast(1).ToArray();
        
        var xAxis = lines.Select(x => float.Parse(x.Split(" ").Skip(1).First())).ToArray();
        var yAxis = lines.Select(x => float.Parse(x.Split(" ").Skip(2).First())).ToArray();
        
        NormalizeValues(xAxis);
        NormalizeValues(yAxis);

        var xAxisLength = GetAxisLength(xAxis);
        var yAxisLength = GetAxisLength(yAxis);
        
        using(var img = new Image<Rgba32>(xAxisLength, yAxisLength))
        {
            img.Mutate(x => x.Fill(Color.White));

            var points = new EllipsePolygon[path.Length];
            var pen = new Pen(Color.Black, 3f);
            for (int i = 0; i < path.Length; i++)
            {
                var point1 = new PointF(xAxis[path[i]], yAxis[path[i]]);
                var point2 = i == (path.Length - 1) ? new PointF(xAxis[path[0]], yAxis[path[0]]) : new PointF(xAxis[path[i + 1]], yAxis[path[i + 1]]);
                
                img.Mutate(x => x.DrawLines(pen, new []{point1, point2}));
                points[i] = new EllipsePolygon(xAxis[i], yAxis[i], 5f);
            }
            foreach (var p in points)
            {
                img.Mutate(x => x.Fill(Color.Black, p));
            }
            img.SaveAsPng(".preview.png");
        }
        
    }

    private static int GetAxisLength(float[] values)
    {
        return Convert.ToInt32((values.Max() - values.Min()) * 1.1);
    }

    private static void NormalizeValues(float[] values)
    {
        var max = values.Max();
        var min = values.Min();

        var multiplier = 1000 / max;
        var margin = 0.05f * multiplier * (values.Max() - values.Min());

        for (int i = 0; i < values.Length; i++)
        {
            values[i] = (values[i] - min) * multiplier + margin;
        }
    }
}