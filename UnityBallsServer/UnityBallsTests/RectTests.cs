using NUnit.Framework;
using UnityBallsCore.Model;

namespace UnityBallsTests
{
    public class RectTests
    {
        [Test]
        public void TestRect()
        {
            var rect = new Rect2f(-5, -18.5f, 30, 45);

            Assert.AreEqual(-5, rect.X);
            Assert.AreEqual(-18.5f, rect.Y);
            Assert.AreEqual(30, rect.Width);
            Assert.AreEqual(45, rect.Height);
            Assert.AreEqual(-5, rect.Left);
            Assert.AreEqual(26.5f, rect.Top);
            Assert.AreEqual(25, rect.Right);
            Assert.AreEqual(-18.5f, rect.Bottom);
            Assert.AreEqual(10, rect.CenterX);
            Assert.AreEqual(4, rect.CenterY);
        }

        [Test]
        public void TestContains()
        {
            var rect = new Rect2f(10, -5, 25, 40);

            var tests = new (double X, double Y, bool Inside)[]
            {
                (9, 6, false),
                (12, -7, false),
                (36, 11, false),
                (17, 36, false),
                (9, -5, false),
                (10, -7, false),
                (10, -5, true),
                (12, -5, true),
                (13, 35, true),
                (35, 12, true),
                (35, 35, true),
                (13, 22, true)
            };

            foreach (var test in tests)
            {
                var v = new Vector2f((float)test.X, (float)test.Y);
                Assert.That(rect.Contains(v), Is.EqualTo(test.Inside));
            }
        }

        [Test]
        public void TestIntersectsWith()
        {
            var rect = new Rect2f(0, 0, 5, 5);

            var tests = new (Rect2f R, bool Intersects)[]
            {
                (new Rect2f(1, -3, 2, 2), false),
                (new Rect2f(-3, 2, 2, 2), false),
                (new Rect2f(-3, -3, 2, 10), false),
                (new Rect2f(6, -3, 2, 10), false),
                (new Rect2f(-3, -3, 10, 2), false),
                (new Rect2f(-3, 6, 10, 2), false),
                (new Rect2f(-1, -1, 2, 2), true),
                (new Rect2f(-1, 1, 2, 2), true),
                (new Rect2f(-1, 4, 2, 2), true),
                (new Rect2f(-1, 1, 2, 2), true),
                (new Rect2f(1, 1, 2, 2), true),
                (new Rect2f(-1, -1, 7, 7), true),
                (new Rect2f(1, -1, 2, 7), true),
                (new Rect2f(-1, 1, 7, 2), true),
            };

            foreach (var test in tests)
            {
                Assert.That(rect.IntersectsWith(test.R), Is.EqualTo(test.Intersects));
            }
        }
    }
}
