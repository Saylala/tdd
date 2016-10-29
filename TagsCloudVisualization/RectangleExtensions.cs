using System.Drawing;

namespace TagsCloudVisualization
{
    static class RectangleExtensions
    {
        public static Point GetCenter(this Rectangle rectangle)
        {
            var x = (rectangle.Right + rectangle.Left) / 2;
            var y = (rectangle.Top + rectangle.Bottom) / 2;
            return new Point(x, y);
        }

        public static bool IsInside(this Rectangle rectangle, Rectangle other)
        {
            return other.Left <= rectangle.Left && rectangle.Right <= other.Right &&
                   other.Top <= rectangle.Top && rectangle.Bottom <= other.Bottom;
        }
    }
}
