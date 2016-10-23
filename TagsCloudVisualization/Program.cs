using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualization
{
    class Program
    {
        public static IEnumerable<System.Drawing.Rectangle> MakeData(CircularCloudLayouter layouter, int count, int width)
        {
            var bias = 5;
            var rnd = new Random();
            for (var i = bias; i < count + bias; i++)
                layouter.PutNextRectangle(new Size(rnd.Next(20, 30), rnd.Next(10, 15)));
            var result = layouter.GetRectangles();
            return result.Select(x =>
                new System.Drawing.Rectangle(
                    x.Vertices[0].X + width,
                    x.Vertices[0].Y + width,
                    x.Vertices[3].X - x.Vertices[0].X - width,
                    x.Vertices[3].Y - x.Vertices[0].Y - width));
        }
        static void Main(string[] args)
        {
            var width = 1;
            var intesity = double.Parse(args[0]);
            var visualiser = new CloudVisualizer();
            var layouter = new CircularCloudLayouter(new Point(500, 500), intesity, new Size(1000, 1000));
            var testData = MakeData(layouter, 150, width);
            visualiser.Visualise(testData, $"test{intesity}.png");
        }
    }
}
