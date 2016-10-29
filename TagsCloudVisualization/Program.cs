using System;
using System.Diagnostics;
using System.Drawing;

namespace TagsCloudVisualization
{
    class Program
    {
        public static Rectangle[] CreateLayout(CircularCloudLayouter layouter, int count)
        {
            const int bias = 5;
            var rnd = new Random();
            for (var i = bias; i < count + bias; i++)
                layouter.PutNextRectangle(new Size(rnd.Next(10, 50), rnd.Next(10, 50)));
            return layouter.GetRectangles();
        }

        static void Main(string[] args)
        {
            var testNumber = double.Parse(args[0]);
            var layouter = new CircularCloudLayouter(new Point(500, 500));
            var testData = CreateLayout(layouter, 500);
            var visualiser = new CloudVisualizer(layouter.CloudBorder.Size);
            visualiser.Visualise(testData, $"test{testNumber}.png");
            Process.Start($"test{testNumber}.png");
        }
    }
}
