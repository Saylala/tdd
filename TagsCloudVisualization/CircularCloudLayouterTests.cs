using System;
using System.Linq;
using System.Drawing;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouterTests
    {
        private readonly Point center = new Point(500, 500);
        private CircularCloudLayouter cloudLayouter;

        [SetUp]
        public void SetUp()
        {
            cloudLayouter = new CircularCloudLayouter(center);
        }

        [TearDown]
        public void TearDown()
        {
            if (!Equals(TestContext.CurrentContext.Result.Outcome, ResultState.Failure))
                return;
            var visualiser = new CloudVisualizer();
            var data = cloudLayouter.GetRectangles();
            var name = TestContext.CurrentContext.Test.Name + ".png";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name);
            visualiser.Visualise(data, path);
            Console.WriteLine($"Tag cloud visualization saved to file {name}");
        }

        [TestCase(10, 10)]
        [TestCase(100, 50)]
        [TestCase(20, 30)]
        [TestCase(1000, 2000)]
        [TestCase(500, 600)]
        public void FirstRectangle_PlacedInsideCloud_IsPlacedInCenter(int width, int height)
        {
            var rectangleSize = new Size(width, height);

            var resultedRectangle = cloudLayouter.PutNextRectangle(rectangleSize);

            Assert.IsTrue(resultedRectangle.GetCenter().Equals(center));
        }

        [TestCase(12, 11)]
        [TestCase(130, 18)]
        [TestCase(27, 38)]
        [TestCase(5000, 1)]
        [TestCase(4800, 600)]
        public void TwoRectanlges_PlacedInsideCloud_DoNotIntersect(int width, int height)
        {
            var rectangleSize = new Size(width, height);

            var firstRectangle = cloudLayouter.PutNextRectangle(rectangleSize);
            var secondRectangle = cloudLayouter.PutNextRectangle(rectangleSize);

            Assert.False(firstRectangle.IntersectsWith(secondRectangle));
        }

        [TestCase(5, 4)]
        [TestCase(125, 75)]
        [TestCase(20, 40)]
        [TestCase(1200, 1800)]
        [TestCase(400, 750)]
        public void TwoRectangles_PlacedInsideCloud_DoNotHaveSameCenters(int width, int height)
        {
            var rectangleSize = new Size(width, height);

            var firstRectangle = cloudLayouter.PutNextRectangle(rectangleSize);
            var secondRectangle = cloudLayouter.PutNextRectangle(rectangleSize);

            Assert.False(firstRectangle.GetCenter().Equals(secondRectangle.GetCenter()));
        }

        [TestCase(100, 10, 10)]
        [TestCase(2, 50, 5)]
        [TestCase(70, 30, 12)]
        [TestCase(1000, 200, 25)]
        [TestCase(899, 100, 100)]
        public void NewRectangle_PlacedInsideCloud_DoesNotIntersectWithOthers(int width, int height, int count)
        {
            var rectangleSize = new Size(width, height);

            for (var i = 0; i < count; i++)
                cloudLayouter.PutNextRectangle(rectangleSize);
            var rectangles = cloudLayouter.GetRectangles();
            var rectangle = cloudLayouter.PutNextRectangle(rectangleSize);

            Assert.False(rectangles.Any(x => x.IntersectsWith(rectangle)));
        }

        [TestCase(1, 1, 8)]
        [TestCase(200, 51, 6)]
        [TestCase(278, 3, 17)]
        [TestCase(1210, 4012, 38)]
        [TestCase(555, 60, 87)]
        public void OldRectangles_NewRectangleAddedToCloud_DoNotIntersectWithEachOther(int width, int height, int count)
        {
            var rectangleSize = new Size(width, height);

            for (var i = 0; i < count; i++)
                cloudLayouter.PutNextRectangle(rectangleSize);
            var oldRectangles = cloudLayouter.GetRectangles();
            cloudLayouter.PutNextRectangle(rectangleSize);


            foreach (var rectangle in oldRectangles)
            {
                var rectangles = oldRectangles.Where(x => !x.Equals(rectangle));
                Assert.False(rectangles.Any(x => x.IntersectsWith(rectangle)));
            }
        }

        private Rectangle[] CreateLayout(int count, int maxWidth, int maxHeight)
        {
            const int bias = 5;
            var rnd = new Random();
            for (var i = bias; i < count + bias; i++)
                cloudLayouter.PutNextRectangle(new Size(rnd.Next(10, maxWidth), rnd.Next(10, maxHeight)));
            return cloudLayouter.GetRectangles();
        }

        private static bool IsInsideCircle(Point point, Point circleCenter, int radius)
        {
            var distance = Math.Sqrt(Math.Pow(point.X - circleCenter.X, 2) + Math.Pow(point.Y - circleCenter.Y, 2));
            return distance < radius;
        }

        [Test]
        public void Cloud_Formed_IsCircleWithTolerance()
        {
            var rectangles = CreateLayout(100, 50, 50);
            const double spacingCoefficent = 1.5;
            const double tolerance = 0.2;

            var rectanglesArea = rectangles.Sum(x => x.Width * x.Height);
            var circleArea = rectanglesArea * spacingCoefficent;
            var radius = (int)Math.Ceiling(Math.Sqrt(circleArea / Math.PI));
            var circleCenter = rectangles[0].GetCenter();
            double outerRectanglesCount = rectangles.Count(x => !IsInsideCircle(x.GetCenter(), circleCenter, radius));
            var outerRectanglesCoefficent = outerRectanglesCount / rectangles.Length;

            Assert.Less(outerRectanglesCoefficent, tolerance);
        }
    }
}
