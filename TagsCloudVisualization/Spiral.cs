using System;


namespace TagsCloudVisualization
{
    class Spiral
    {
        private readonly double step;
        private const double Bias = 50;
        private readonly Point center;

        public Spiral(double step, Point center)
        {
            this.step = step;
            this.center = center;
        }

        public Point GetNextPoint(int currentNumber)
        {
            var phi = Bias * currentNumber;
            var r = step * currentNumber;
            var x = r * Math.Cos(phi);
            var y = r * Math.Sin(phi);
            return new Point(center.X + (int)x, center.Y + (int)y);
        }
    }
}
