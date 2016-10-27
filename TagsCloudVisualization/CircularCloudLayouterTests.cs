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
            var visualiser = new CloudVisualizer(cloudLayouter.CloudBorder.Size);
            var data = cloudLayouter.GetRectangles();
            var name = TestContext.CurrentContext.Test.Name + ".png";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name);
            visualiser.Visualise(data, path);
            Console.WriteLine($"Tag cloud visualization saved to file {name}");
        }

        [TestCase(10, 10)]
        [TestCase(100, 50)]
        [TestCase(20, 30)]
        [TestCase(500, 600)]
        public void FirstRectangle_PlacedInsideCloud_IsPlacedInCenter(int width, int height)
        {
            var rectangleSize = new Size(width, height);

            var resultedRectangle = cloudLayouter.PutNextRectangle(rectangleSize);

            Assert.IsTrue(resultedRectangle.GetCenter().Equals(center));
        }

        [TestCase(1001, 1)]
        [TestCase(100, 1050)]
        [TestCase(20, 3000)]
        [TestCase(5000, 600)]
        [TestCase(10000, 10000)]
        public void Layouter_TryPlaceRectangleBiggerThanBorder_ReturnsEmptyRectangle(int width, int height)
        {
            var rectangleSize = new Size(width, height);

            var resultedRectangle = cloudLayouter.PutNextRectangle(rectangleSize);

            Assert.AreEqual(Rectangle.Empty, resultedRectangle);
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

            var rectangles = CreateLayout(count, width, height);
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

            var oldRectangles = CreateLayout(count, width, height);
            cloudLayouter.PutNextRectangle(rectangleSize);
            var intersections = 0;
            foreach (var rectangle in oldRectangles)
            {
                var rectangles = oldRectangles.Where(x => !x.Equals(rectangle));
                intersections += rectangles.Count(x => x.IntersectsWith(rectangle));
            }

            Assert.AreEqual(0, intersections);
        }

        private Rectangle[] CreateLayout(int count, int width, int height)
        {
            for (var i = 0; i < count; i++)
                cloudLayouter.PutNextRectangle(new Size(width, height));
            return cloudLayouter.GetRectangles();
        }
        private Rectangle[] CreateLayoutWithRandomSizes(int count, int maxWidth, int maxHeight)
        {
            const int minValue = 1;
            var rnd = new Random();
            for (var i = 0; i < count; i++)
                cloudLayouter.PutNextRectangle(new Size(rnd.Next(minValue, maxWidth), rnd.Next(minValue, maxHeight)));
            return cloudLayouter.GetRectangles();
        }

        private static bool IsInsideCircle(Point point, Point circleCenter, int radius)
        {
            var distance = Math.Sqrt(Math.Pow(point.X - circleCenter.X, 2) + Math.Pow(point.Y - circleCenter.Y, 2));
            return distance < radius;
        }

        private static double GetRectanglesOutsideCircle(Rectangle[] rectangles, double spacingCoefficent)
        {
            var rectanglesArea = rectangles.Sum(x => x.Width * x.Height);
            var circleArea = rectanglesArea * spacingCoefficent;
            var radius = (int)Math.Ceiling(Math.Sqrt(circleArea / Math.PI));
            var circleCenter = rectangles[0].GetCenter();
            double outerRectanglesCount = rectangles.Count(x => !IsInsideCircle(x.GetCenter(), circleCenter, radius));
            return outerRectanglesCount;
        }

        [TestCase(1, 1, 1)]
        [TestCase(5, 227, 155)]
        [TestCase(10, 100, 70)]
        [TestCase(50, 50, 20)]
        [TestCase(100, 20, 25)]
        [TestCase(250, 12, 9)]
        [TestCase(500, 3, 4)]
        [TestCase(1000, 1, 2)]
        public void Cloud_FormedWithSameRectangles_IsCircleWithTolerance(int rectanglesCount, int width, int height)
        {
            var rectangles = CreateLayout(rectanglesCount, width, height);
            const double spacingCoefficent = 1.5;
            const double tolerance = 0.15;

            var outerRectanglesCount = GetRectanglesOutsideCircle(rectangles, spacingCoefficent);
            var outerRectanglesCoefficent = outerRectanglesCount / rectangles.Length;

            Assert.Less(outerRectanglesCoefficent, tolerance);
        }

        [TestCase(1, 1, 1)]
        [TestCase(15, 127, 155)]
        [TestCase(10, 100, 70)]
        [TestCase(50, 50, 20)]
        [TestCase(100, 20, 25)]
        [TestCase(250, 12, 9)]
        [TestCase(500, 3, 4)]
        [TestCase(1000, 1, 2)]
        public void Cloud_FormedWithDifferentRectangles_IsCircleWithTolerance(int rectanglesCount, int maxWidth, int maxHeight)
        {
            var rectangles = CreateLayoutWithRandomSizes(rectanglesCount, maxWidth, maxHeight);
            const double spacingCoefficent = 1.5;
            const double tolerance = 0.15;

            var outerRectanglesCount = GetRectanglesOutsideCircle(rectangles, spacingCoefficent);
            var outerRectanglesCoefficent = outerRectanglesCount / rectangles.Length;

            Assert.Less(outerRectanglesCoefficent, tolerance);
        }

        [Test]
        [Repeat(100)]
        public void Cloud_FormedWithDifferentRectangles_IsCircleWithTolerance_Repeating()
        {
            const int rectanglesCount = 100;
            const int maxWidth = 50;
            const int maxHeight = 50;
            Cloud_FormedWithDifferentRectangles_IsCircleWithTolerance(rectanglesCount, maxWidth, maxHeight);
        }
    }
}
