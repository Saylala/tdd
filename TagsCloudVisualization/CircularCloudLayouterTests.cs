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

        [TestCase(10, 10, ExpectedResult = true)]
        [TestCase(100, 50, ExpectedResult = true)]
        [TestCase(20, 30, ExpectedResult = true)]
        [TestCase(1000, 2000, ExpectedResult = true)]
        [TestCase(500, 600, ExpectedResult = true)]
        public bool FirstRectangle_PlacedInsideCloud_IsPlacedInCenter(int width, int height)
        {
            var rectangleSize = new Size(width, height);

            var resultedRectangle = cloudLayouter.PutNextRectangle(rectangleSize);

            return resultedRectangle.GetCenter().Equals(center);
        }

        [TestCase(12, 11, ExpectedResult = false)]
        [TestCase(130, 18, ExpectedResult = false)]
        [TestCase(27, 38, ExpectedResult = false)]
        [TestCase(5000, 1, ExpectedResult = false)]
        [TestCase(4800, 600, ExpectedResult = false)]
        public bool TwoRectanlges_PlacedInsideCloud_DoNotIntersect(int width, int height)
        {
            var rectangleSize = new Size(width, height);

            var firstRectangle = cloudLayouter.PutNextRectangle(rectangleSize);
            var secondRectangle = cloudLayouter.PutNextRectangle(rectangleSize);

            return firstRectangle.IntersectsWith(secondRectangle);
        }

        [TestCase(5, 4, ExpectedResult = false)]
        [TestCase(125, 75, ExpectedResult = false)]
        [TestCase(20, 40, ExpectedResult = false)]
        [TestCase(1200, 1800, ExpectedResult = false)]
        [TestCase(400, 750, ExpectedResult = false)]
        public bool TwoRectangles_PlacedInsideCloud_DoNotHaveSameCenters(int width, int height)
        {
            var rectangleSize = new Size(width, height);

            var firstRectangle = cloudLayouter.PutNextRectangle(rectangleSize);
            var secondRectangle = cloudLayouter.PutNextRectangle(rectangleSize);

            return firstRectangle.GetCenter().Equals(secondRectangle.GetCenter());
        }

        [TestCase(100, 10, 10, ExpectedResult = false)]
        [TestCase(2, 50, 5, ExpectedResult = false)]
        [TestCase(70, 30, 12, ExpectedResult = false)]
        [TestCase(1000, 200, 25, ExpectedResult = false)]
        [TestCase(899, 100, 100, ExpectedResult = false)]
        public bool NewRectangle_PlacedInsideCloud_DoesNotIntersectsWithOthers(int width, int height, int count)
        {
            var rectangleSize = new Size(width, height);

            if (count == 0)
                return false;
            for (var i = 0; i < count; i++)
                cloudLayouter.PutNextRectangle(rectangleSize);
            var rectangles = cloudLayouter.GetRectangles();
            var rectangle = cloudLayouter.PutNextRectangle(rectangleSize);

            return rectangles.Any(x => x.IntersectsWith(rectangle));
        }

        [TestCase(1, 1, 8, ExpectedResult = false)]
        [TestCase(200, 51, 6, ExpectedResult = false)]
        [TestCase(278, 3, 17, ExpectedResult = false)]
        [TestCase(1210, 4012, 38, ExpectedResult = false)]
        [TestCase(555, 60, 87, ExpectedResult = false)]
        public bool OldRectangles_NewRectangleAddedToCloud_DoNotIntersectsWithEachOther(int width, int height, int count)
        {
            var rectangleSize = new Size(width, height);

            if (count == 0)
                return false;
            for (var i = 0; i < count - 1; i++)
                cloudLayouter.PutNextRectangle(rectangleSize);
            var rectangles = cloudLayouter.GetRectangles();
            var rectangle = cloudLayouter.PutNextRectangle(rectangleSize);
            cloudLayouter.PutNextRectangle(rectangleSize);

            return rectangles.Any(x => x.IntersectsWith(rectangle));
        }
    }
}
