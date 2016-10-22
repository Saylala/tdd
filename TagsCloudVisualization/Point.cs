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
