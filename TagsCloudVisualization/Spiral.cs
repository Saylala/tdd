using System;
using System.Drawing;


namespace TagsCloudVisualization
{
    class Spiral : ICurve
    {
        private const double Step = 0.01;
        private const double Frequency = 50;
        private readonly Point bias;
        private int currentNumber;
        public Spiral(Point bias)
        {
            this.bias = bias;
            currentNumber = 1;
        }

        public Point GetNextPoint()
        {
            var phi = Frequency * currentNumber;
            var r = Step * currentNumber;
            var x = r * Math.Cos(phi);
            var y = r * Math.Sin(phi);
            currentNumber++;
            return new Point(bias.X + (int)x, bias.Y + (int)y);
        }
    }
}
