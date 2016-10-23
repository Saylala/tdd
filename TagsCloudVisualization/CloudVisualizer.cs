using System.Collections.Generic;
using System.Drawing;


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
        public void Draw(IEnumerable<System.Drawing.Rectangle> data, Bitmap image)
        {
            var graphics = Graphics.FromImage(image);
            graphics.Clear(backgroundColor);
            foreach (var sample in data)
                graphics.DrawRectangle(pen, sample);
            graphics.Save();
        }

        public void Visualise(IEnumerable<System.Drawing.Rectangle> data, string filename)
        {
            var image = new Bitmap(imageWidth, imageHeight);
            Draw(data, image);
            image.Save(filename);
        }
    }
}
