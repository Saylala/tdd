using System.Linq;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouterTests
    {
        private Point center;
        private CircularCloudLayouter cloudLayouter;
        [SetUp]
        public void SetUp()
        {
            center = new Point(500, 500);
            cloudLayouter = new CircularCloudLayouter(center, 0.01, new Size(1000, 1000));
        }
        [Test]
        public void placedInCloudCenter_FirstRectangle()
        {
            var rectangleSize = new Size(10, 10);
            var resultedRectangle = cloudLayouter.PutNextRectangle(rectangleSize);
            Assert.AreEqual(center, resultedRectangle.Center);
        }

        [Test]
        public void doesIntersect_TwoRectangles()
        {
            var rectangleSize = new Size(200, 200);
            var center1 = new Point(100, 100);
            var center2 = new Point(110, 110);
            var firstRectangle = new Rectangle(center1, rectangleSize);
            var secondRectangle = new Rectangle(center2, rectangleSize);
            Assert.IsTrue(firstRectangle.DoesIntersect(secondRectangle));
        }

        [Test]
        public void doesIntersectOnBorder_TwoRectangles()
        {
            var rectangleSize = new Size(200, 200);
            var center1 = new Point(100, 100);
            var center2 = new Point(300, 100);
            var firstRectangle = new Rectangle(center1, rectangleSize);
            var secondRectangle = new Rectangle(center2, rectangleSize);
            Assert.IsTrue(firstRectangle.DoesIntersect(secondRectangle));
        }

        [Test]
        public void point_isInsideRectangle()
        {
            var point = new Point(110, 110);
            var rectangleSize = new Size(200, 200);
            var rectangleCenter = new Point(100, 100);
            var rectangle = new Rectangle(rectangleCenter, rectangleSize);
            Assert.IsTrue(point.IsInside(rectangle));
        }

        [Test]
        public void point_IsNotInsideRectangle_IfOnBorder()
        {
            var point = new Point(200, 110);
            var rectangleSize = new Size(200, 200);
            var rectangleCenter = new Point(100, 100);
            var rectangle = new Rectangle(rectangleCenter, rectangleSize);
            Assert.IsFalse(point.IsInside(rectangle));
        }

        [Test]
        public void rectangle_isInsideOther()
        {
            var rectangleSize1 = new Size(10, 10);
            var rectangleSize2 = new Size(200, 200);
            var center1 = new Point(110, 110);
            var center2 = new Point(100, 100);
            var firstRectangle = new Rectangle(center1, rectangleSize1);
            var secondRectangle = new Rectangle(center2, rectangleSize2);
            Assert.IsTrue(firstRectangle.IsInside(secondRectangle));
        }

        [Test]
        public void rectangle_isNotInsideOther_IfHasSameBorder()
        {
            var rectangleSize1 = new Size(20, 10);
            var rectangleSize2 = new Size(200, 200);
            var center1 = new Point(290, 110);
            var center2 = new Point(100, 100);
            var firstRectangle = new Rectangle(center1, rectangleSize1);
            var secondRectangle = new Rectangle(center2, rectangleSize2);
            Assert.IsFalse(firstRectangle.IsInside(secondRectangle));
        }

        [Test]
        public void doesNotIntersect_TwoPlacedRectangles()
        {
            var rectangleSize = new Size(10, 10);
            var firstRectangle = cloudLayouter.PutNextRectangle(rectangleSize);
            var secondRectangle = cloudLayouter.PutNextRectangle(rectangleSize);
            Assert.IsFalse(firstRectangle.DoesIntersect(secondRectangle));
        }

        [Test]
        public void areNotSame_TwoPlacedRectangleCenters()
        {
            var rectangleSize = new Size(10, 10);
            var firstRectangle = cloudLayouter.PutNextRectangle(rectangleSize);
            var secondRectangle = cloudLayouter.PutNextRectangle(rectangleSize);
            Assert.AreNotEqual(firstRectangle.Center, secondRectangle.Center);
        }

        [Test]
        public void returnsNull_rectangleCannotBePlaced()
        {
            var rectangleSize = new Size(10000, 10);
            var firstRectangle = cloudLayouter.PutNextRectangle(rectangleSize);
            Assert.AreEqual(null, firstRectangle);
        }

        [Test]
        public void cannotPlace_rectangleBiggerThanCloudBorder()
        {
            var rectangleSize = new Size(10000, 10);
            var firstRectangle = cloudLayouter.PutNextRectangle(rectangleSize);
            Assert.AreEqual(null, firstRectangle);
        }

        [Test]
        public void newRectangle_doesNotIntersectWithOthers()
        {
            var rectangleSize = new Size(10, 10);
            cloudLayouter.PutNextRectangle(rectangleSize);
            cloudLayouter.PutNextRectangle(rectangleSize);
            var rectangles = cloudLayouter.GetRectangles();
            var rectanlge = cloudLayouter.PutNextRectangle(rectangleSize);
            Assert.IsFalse(rectangles.Any(x => x.DoesIntersect(rectanlge)));
        }
    }
}
