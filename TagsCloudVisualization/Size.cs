namespace TagsCloudVisualization
{
    class Size
    {
        public int Width { get; }
        public int Height { get; }
        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"W: {Width}, H: {Height}";
        }
    }
}
