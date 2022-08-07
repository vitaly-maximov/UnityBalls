using NUnit.Framework;
using System.Collections.Generic;
using UnityBallsCore.Common;
using UnityBallsCore.Model;

namespace UnityBallsTests
{
    public class PhysicsTests
    {
        private const float Epsilon = 1e-6f;

        [Test]
        public void TestMove()
        {
            var obstacles = new List<Rect2f>
            {
                new Rect2f(5, 5, 10, 20)
            };

            // left
            var v = Physics.Move(new Vector2f(17, 10), new Vector2f(-1, 0), 1, obstacles);
            Assert.AreEqual(16, v.X, Epsilon);
            Assert.AreEqual(10, v.Y, Epsilon);

            v = Physics.Move(new Vector2f(17, 10), new Vector2f(-1, 0), 3, obstacles);
            Assert.AreEqual(15.501, v.X, Epsilon);
            Assert.AreEqual(10, v.Y, Epsilon);

            v = Physics.Move(new Vector2f(17, 10), new Vector2f(-3, 4), 3, obstacles);
            Assert.AreEqual(15.501, v.X, Epsilon);
            Assert.AreEqual(12.4, v.Y, Epsilon);

            v = Physics.Move(new Vector2f(17, 10), new Vector2f(-3, -4), 3, obstacles);
            Assert.AreEqual(15.501, v.X, Epsilon);
            Assert.AreEqual(7.6, v.Y, Epsilon);

            // right
            v = Physics.Move(new Vector2f(3, 10), new Vector2f(1, 0), 1, obstacles);
            Assert.AreEqual(4, v.X, Epsilon);
            Assert.AreEqual(10, v.Y, Epsilon);

            v = Physics.Move(new Vector2f(3, 10), new Vector2f(1, 0), 3, obstacles);
            Assert.AreEqual(4.499, v.X, Epsilon);
            Assert.AreEqual(10, v.Y, Epsilon);

            v = Physics.Move(new Vector2f(3, 10), new Vector2f(3, 4), 3, obstacles);
            Assert.AreEqual(4.499, v.X, Epsilon);
            Assert.AreEqual(12.4, v.Y, Epsilon);

            v = Physics.Move(new Vector2f(3, 10), new Vector2f(3, -4), 3, obstacles);
            Assert.AreEqual(4.499, v.X, Epsilon);
            Assert.AreEqual(7.6, v.Y, Epsilon);

            // up
            v = Physics.Move(new Vector2f(7, 3), new Vector2f(0, 1), 1, obstacles);
            Assert.AreEqual(7, v.X, Epsilon);
            Assert.AreEqual(4, v.Y, Epsilon);

            v = Physics.Move(new Vector2f(7, 3), new Vector2f(0, 1), 3, obstacles);
            Assert.AreEqual(7, v.X, Epsilon);
            Assert.AreEqual(4.499, v.Y, Epsilon);

            v = Physics.Move(new Vector2f(7, 3), new Vector2f(3, 4), 3, obstacles);
            Assert.AreEqual(8.8, v.X, Epsilon);
            Assert.AreEqual(4.499, v.Y, Epsilon);

            v = Physics.Move(new Vector2f(7, 3), new Vector2f(-3, 4), 3, obstacles);
            Assert.AreEqual(5.2, v.X, Epsilon);
            Assert.AreEqual(4.499, v.Y, Epsilon);

            // down
            v = Physics.Move(new Vector2f(7, 27), new Vector2f(0, -1), 1, obstacles);
            Assert.AreEqual(7, v.X, Epsilon);
            Assert.AreEqual(26, v.Y, Epsilon);

            v = Physics.Move(new Vector2f(7, 27), new Vector2f(0, -1), 3, obstacles);
            Assert.AreEqual(7, v.X, Epsilon);
            Assert.AreEqual(25.501, v.Y, Epsilon);

            v = Physics.Move(new Vector2f(7, 27), new Vector2f(3, -4), 3, obstacles);
            Assert.AreEqual(8.8, v.X, Epsilon);
            Assert.AreEqual(25.501, v.Y, Epsilon);

            v = Physics.Move(new Vector2f(7, 27), new Vector2f(-3, -4), 3, obstacles);
            Assert.AreEqual(5.2, v.X, Epsilon);
            Assert.AreEqual(25.501, v.Y, Epsilon);
        }
    }
}
