namespace TagsCloudVisualization
{
    class Point
    {
        public int X { get; }
        public int Y { get; }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool IsInside(Rectangle border)
        {
            var left = border.Vertices[0].X;
            var right = border.Vertices[3].X;
            var bottom = border.Vertices[0].Y;
            var top = border.Vertices[3].Y;

            return left < X && X < right && Y < top && Y > bottom;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point))
                return false;
            var point = (Point)obj;
            return X == point.X && Y == point.Y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }
    }
}
