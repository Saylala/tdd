using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        public Point Center { get; }
        private readonly Spiral spiral;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        public CircularCloudLayouter(Point center)
        {
            Center = center;
            spiral = new Spiral(Center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var currentPoint = GetNextPoint(rectangleSize);
            var upperLeftPoint = new Point(currentPoint.X - rectangleSize.Width / 2,
                currentPoint.Y - rectangleSize.Height / 2);
            var rectangle = new Rectangle(upperLeftPoint, rectangleSize);
            rectangles.Add(rectangle);
            return rectangle;
        }

        private bool CanPutRectangle(Point center, Size rectangleSize)
        {
            var upperLeftPoint = new Point(center.X - rectangleSize.Width / 2, center.Y - rectangleSize.Height / 2);
            var rectangle = new Rectangle(upperLeftPoint, rectangleSize);
            return rectangles.All(x => !x.IntersectsWith(rectangle));
        }

        private Point GetNextPoint(Size rectangleSize)
        {
            var point = Center;
            while (!CanPutRectangle(point, rectangleSize))
                point = spiral.GetNextPoint();
            return point;
        }

        public Rectangle[] GetRectangles()
        {
            return rectangles.ToArray();
        }
    }
}
