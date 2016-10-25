using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class Program
    {
        public static IEnumerable<Rectangle> MakeData(CircularCloudLayouter layouter, int count, int width)
        {
            const int bias = 5;
            var rnd = new Random();
            for (var i = bias; i < count + bias; i++)
                layouter.PutNextRectangle(new Size(rnd.Next(25, 50), rnd.Next(10, 30)));
            return layouter.GetRectangles();
        }

        static void Main(string[] args)
        {
            const int width = 1;
            var testNumber = double.Parse(args[0]);
            var visualiser = new CloudVisualizer();
            var layouter = new CircularCloudLayouter(new Point(500, 500));
            var testData = MakeData(layouter, 500, width);
            visualiser.Visualise(testData, $"test{testNumber}.png");
        }
    }
}
