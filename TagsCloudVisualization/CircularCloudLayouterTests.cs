using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouterTests
    {
        [Test]
        public void putInCloudCenter_FirstRectangle()
        {
            var center = new Point(100, 100);
            var cloudLayouter = new CircularCloudLayouter(center, 0.5);

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
        public void doesNotIntersect_TwoRectangles()
        {
            var center = new Point(100, 100);
            var cloudLayouter = new CircularCloudLayouter(center, 0.5);

            var rectangleSize = new Size(10, 10);
            var firstRectangle = cloudLayouter.PutNextRectangle(rectangleSize);
            var secondRectangle = cloudLayouter.PutNextRectangle(rectangleSize);
            Assert.IsFalse(firstRectangle.DoesIntersect(secondRectangle));
        }

        [Test]
        public void areNotSame_TwoRectangles()
        {
            var center = new Point(100, 100);
            var cloudLayouter = new CircularCloudLayouter(center, 0.5);

            var rectangleSize = new Size(10, 10);
            var firstRectangle = cloudLayouter.PutNextRectangle(rectangleSize);
            var secondRectangle = cloudLayouter.PutNextRectangle(rectangleSize);
            Assert.AreNotEqual(firstRectangle.Center, secondRectangle.Center);
        }
    }
}
