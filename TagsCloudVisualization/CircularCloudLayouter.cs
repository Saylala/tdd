using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        public Point Center { get; }
        private readonly Spiral spiral;
        public Rectangle CloudBorder { get; }
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        public CircularCloudLayouter(Point center)
        {
            Center = center;
            spiral = new Spiral(Center);

            var width = Math.Abs(Center.X) * 2;
            var height = Math.Abs(Center.Y) * 2;
            CloudBorder = new Rectangle(new Point(0, 0), new Size(width, height));
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var currentPoint = GetNextPoint(rectangleSize);
            if (currentPoint == Point.Empty)
                return Rectangle.Empty;
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
            return rectangles.All(x => !x.IntersectsWith(rectangle)) && rectangle.IsInside(CloudBorder);
        }

        private Point GetNextPoint(Size rectangleSize)
        {
            var point = Center;
            while (!CanPutRectangle(point, rectangleSize))
            {
                point = spiral.GetNextPoint();
                if (!point.IsInside(CloudBorder))
                    return Point.Empty;
            }
            return point;
        }

        public Rectangle[] GetRectangles()
        {
            return rectangles.ToArray();
        }
    }
}
