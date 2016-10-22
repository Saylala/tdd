using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        public Point Center { get; }
        private readonly Spiral spiral;
        public readonly List<Rectangle> Rectangles = new List<Rectangle>();
        private int currentNumber;
        public CircularCloudLayouter(Point center)
        {
            Center = center;
            spiral = new Spiral(0.5, Center);
            currentNumber = 0;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var center = Rectangles.Count > 0 ? Rectangles[Rectangles.Count - 1].Center : Center;
            var rectangle = new Rectangle(center, rectangleSize);
            while (Rectangles.Any(x => x.DoesIntersect(rectangle)))
            {
                currentNumber++;
                var nextPoint = spiral.GetNextPoint(currentNumber);
                rectangle.Move(nextPoint.X - rectangle.Center.X, nextPoint.Y - rectangle.Center.Y);
            }
            Rectangles.Add(rectangle);
            return rectangle;
        }
    }
}
