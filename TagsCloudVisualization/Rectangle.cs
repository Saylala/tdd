using System.Linq;

namespace TagsCloudVisualization
{
    class Rectangle
    {
        public Point Center
        {
            get
            {
                var a = Vertices[0];
                var d = Vertices[3];
                return new Point((d.X + a.X) / 2, (d.Y + a.X) / 2);
            }
        }

        public Point[] Vertices { get; private set; }
        public Rectangle(Point a, Point b, Point c, Point d)
        {
            Vertices = new[] { a, b, c, d };
        }

        public Rectangle(Point center, Size rectangleSize)
        {
            var a = new Point(center.X - rectangleSize.Width / 2, center.Y - rectangleSize.Height / 2);
            var b = new Point(center.X - rectangleSize.Width / 2, center.Y + rectangleSize.Height / 2);
            var c = new Point(center.X + rectangleSize.Width / 2, center.Y - rectangleSize.Height / 2);
            var d = new Point(center.X + rectangleSize.Width / 2, center.Y + rectangleSize.Height / 2);
            Vertices = new[] { a, b, c, d };
        }

        public void Move(int dx, int dy)
        {
            Vertices = Vertices.Select(z => new Point(z.X + dx, z.Y + dy)).ToArray();
        }

        public bool DoesIntersect(Rectangle other)
        {
            var left = Vertices[0].X;
            var right = Vertices[3].X;
            var bottom = Vertices[0].Y;
            var top = Vertices[3].Y;

            var otherLeft = other.Vertices[0].X;
            var otherRight = other.Vertices[3].X;
            var otherBottom = other.Vertices[0].Y;
            var otherTop = other.Vertices[3].Y;

            return left <= otherRight && right >= otherLeft && top >= otherBottom && bottom <= otherTop;
        }
    }
}
