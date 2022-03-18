using System;
using System.IO;
using System.Linq;
using System.Net;
using Avalonia;
using Avalonia.Platform;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Point = SixLabors.ImageSharp.Point;
using Size = SixLabors.ImageSharp.Size;

namespace Runner.Visualization;

public class WarehouseImageGenerator
{
    public static void Generate(int[] order, string tspFile)
    {
        var lines = File.ReadAllLines(tspFile);
        lines = lines.SkipWhile(x => x != "NODE_COORD_SECTION").Skip(1).SkipLast(1).ToArray();
        
        var xAxis = lines.Select(x => float.Parse(x.Split(" ").Skip(1).First())).ToArray();
        var yAxis = lines.Select(x => float.Parse(x.Split(" ").Skip(2).First())).ToArray();
        
        NormalizeValues(xAxis, 1.6f);
        NormalizeValues(yAxis, 1f);

        var xAxisLength = GetAxisLength(xAxis);
        var yAxisLength = GetAxisLength(yAxis);

        if (!Directory.Exists("Fonts"))
        {
            Directory.CreateDirectory("Fonts");
        }

        if (!File.Exists("Fonts/Roboto-Regular.ttf"))
        {
            var webClient = new WebClient();
            webClient.DownloadFile("https://fonts.gstatic.com/s/roboto/v29/KFOmCnqEu92Fr1Me5Q.ttf", "Fonts/Roboto-Regular.ttf");
        }
        
        FontCollection collection = new();
        FontFamily family = collection.Add("Fonts/Roboto-Regular.ttf");
        Font font = family.CreateFont(26, FontStyle.Regular);
        
        using(var img = new Image<Rgba32>(xAxisLength, yAxisLength))
        {
            img.Mutate(x => x.Fill(Color.White));

            var points = new Rectangle[order.Length];
            var pen = new Pen(Color.Black, 3f);
            for (int i = 0; i < order.Length; i++)
            {
                points[i] = new Rectangle(new Point((int)xAxis[i] - 5, (int)yAxis[i]), new Size(50, 30));
            }
            foreach (var p in points)
            {
                img.Mutate(x => x.Fill(Color.Black, p));
            }

            for (int i = 0; i < order.Length; i++)
            {
                img.Mutate(x=> x.DrawText(order[i].ToString(), font, Color.White, new PointF(xAxis[i], yAxis[i])));
            }
            img.SaveAsPng(".preview.png");
        }
        
    }

    private static int GetAxisLength(float[] values)
    {
        return Convert.ToInt32((values.Max() - values.Min()) * 1.1);
    }

    private static void NormalizeValues(float[] values, float proportion)
    {
        var max = values.Max();
        var min = values.Min();

        var multiplier = (1000 / (max - min)) * proportion;
        var margin = 0.05f * multiplier * (values.Max() - values.Min());

        for (int i = 0; i < values.Length; i++)
        {
            values[i] = (values[i] - min) * multiplier + margin;
        }
    }
}