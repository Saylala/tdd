using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;


namespace TagsCloudVisualization
{
    class CloudVisualizer
    {
        private readonly Pen pen;
        private readonly Color backgroundColor;
        private readonly int imageWidth;
        private readonly int imageHeight;

        public CloudVisualizer(Pen pen, Color backgroundColor, int imageWidth, int imageHeight)
        {
            this.pen = pen;
            this.backgroundColor = backgroundColor;
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
        }

        public CloudVisualizer()
        {
            pen = new Pen(Color.Yellow, 1);
            backgroundColor = Color.Black;
            imageWidth = 1000;
            imageHeight = 1000;
        }

        private void DrawRectangles(Rectangle[] data, Graphics graphics)
        {
            graphics.Clear(backgroundColor);
            foreach (var sample in data)
                graphics.DrawRectangle(pen, sample);
            graphics.Save();
        }

        public void Visualise(Rectangle[] data, string filename)
        {
            var image = new Bitmap(imageWidth, imageHeight);
            var graphics = Graphics.FromImage(image);
            DrawRectangles(data, graphics);
            image.Save(filename, ImageFormat.Png);
        }

        public void VisualiseWithCircle(Rectangle[] data, string filename)
        {
            var image = new Bitmap(imageWidth, imageHeight);
            var graphics = Graphics.FromImage(image);
            DrawRectangles(data, graphics);
            DrawCircle(data, graphics);
            image.Save(filename, ImageFormat.Png);
        }

        private void DrawCircle(Rectangle[] data, Graphics graphics)
        {
            var rectanglesArea = data.Sum(x => x.Width * x.Height);
            const double tolerance = 1.5;
            var circleArea = rectanglesArea * tolerance;

            var radius = (int)Math.Ceiling(Math.Sqrt(circleArea / Math.PI));
            var center = data[0].GetCenter();
            var location = new Point(center.X - radius, center.Y - radius);
            var circleRectangle = new Rectangle(location, new Size(radius * 2, radius * 2));
            var circlePen = new Pen(Color.Chartreuse, 1);
            graphics.DrawEllipse(circlePen, circleRectangle);
        }
    }
}
