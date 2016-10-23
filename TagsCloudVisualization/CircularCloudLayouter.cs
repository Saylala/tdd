using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        public Point Center { get; }
        private readonly Spiral spiral;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private readonly Rectangle cloudBorder;
        private int currentNumber;
        public CircularCloudLayouter(Point center, double spiralIntensity, Size cloudSize)
        {
            Center = center;
            spiral = new Spiral(spiralIntensity, Center);
            cloudBorder = new Rectangle(center, cloudSize);
            currentNumber = 0;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var point = rectangles.Count > 0 ? rectangles[rectangles.Count - 1].Center : Center;
            var newPoint = GetNextPoint(point, rectangleSize, spiral);
            if (newPoint == null)
                return null;
            var rectangle = new Rectangle(newPoint, rectangleSize);
            rectangles.Add(rectangle);
            return rectangle;
        }

        private bool CanPutRectangle(Point point, Size rectangleSize)
        {
            var rectangle = new Rectangle(point, rectangleSize);
            return rectangles.All(x => !x.DoesIntersect(rectangle)) && rectangle.IsInside(cloudBorder);
        }

        private Point GetNextPoint(Point start, Size rectangleSize, ICurve curve)
        {
            var point = start;
            while (!CanPutRectangle(point, rectangleSize))
            {
                currentNumber++;
                point = curve.GetNextPoint(currentNumber);
                if (!point.IsInside(cloudBorder))
                    return null;
            }
            return point;
        }

        public Rectangle[] GetRectangles()
        {
            return rectangles.ToArray();
        }
    }
}
