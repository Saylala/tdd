using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualization
{
    class Program
    {
        public static IEnumerable<System.Drawing.Rectangle> MakeData(CircularCloudLayouter layouter, int count)
        {
            for (var i = 0; i < count; i++)
                layouter.PutNextRectangle(new Size(i + 1, i + 1));
            var result = layouter.GetRectangles();
            return result.Select(x =>
                new System.Drawing.Rectangle(
                    x.Vertices[0].X,
                    x.Vertices[0].Y,
                    x.Vertices[3].X - x.Vertices[0].X,
                    x.Vertices[3].Y - x.Vertices[0].Y));
        }
        static void Main(string[] args)
        {
            var intesity = double.Parse(args[0]);
            var visualiser = new RectangleVisualizer();
            var layouter = new CircularCloudLayouter(new Point(500, 500), intesity, new Size(1000, 1000));
            var testData = MakeData(layouter, 100);
            visualiser.Visualise(testData, $"test{intesity}.png");
        }
    }
}
