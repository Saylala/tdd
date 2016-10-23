using System;


namespace TagsCloudVisualization
{
    class Spiral : ICurve
    {
        private readonly double step;
        private const double Frequency = 50;
        private readonly Point bias;

        public Spiral(double step, Point bias)
        {
            this.step = step;
            this.bias = bias;
        }

        public Point GetNextPoint(int currentNumber)
        {
            var phi = Frequency * currentNumber;
            var r = step * currentNumber;
            var x = r * Math.Cos(phi);
            var y = r * Math.Sin(phi);
            return new Point(bias.X + (int)x, bias.Y + (int)y);
        }
    }
}
