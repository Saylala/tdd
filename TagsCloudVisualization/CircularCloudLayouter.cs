using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        public Point Center { get; }
        private readonly Spiral spiral;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private int currentNumber;
        private Size cloudSize;
        public CircularCloudLayouter(Point center, double spiralIntensity, Size cloudSize)
        {
            Center = center;
            spiral = new Spiral(spiralIntensity, Center);
            currentNumber = 0;
            this.cloudSize = cloudSize;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var center = rectangles.Count > 0 ? rectangles[rectangles.Count - 1].Center : Center;
            var rectangle = new Rectangle(center, rectangleSize);
            while (rectangles.Any(x => x.DoesIntersect(rectangle)))
            {
                currentNumber++;
                var nextPoint = spiral.GetNextPoint(currentNumber);
                rectangle.Move(nextPoint.X - rectangle.Center.X, nextPoint.Y - rectangle.Center.Y);
            }
            rectangles.Add(rectangle);
            return rectangle;
        }

        public Rectangle[] GetRectangles()
        {
            return rectangles.ToArray();
        }
    }
}
