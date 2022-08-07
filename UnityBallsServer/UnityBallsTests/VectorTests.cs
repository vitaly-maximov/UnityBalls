using NUnit.Framework;
using System;
using UnityBallsCore.Model;

namespace UnityBallsTests
{
    public class VectorTests
    {
        private const float Epsilon = 1e-6f;

        [Test]
        public void TestGetLength()
        {
            var tests = new (double X, double Y, double Length)[]
            {
                (0, 0, 0),
                (1, 0, 1),
                (0, 1, 1),
                (1, 1, Math.Sqrt(2)),
                (3, 4, 5),
                (-1, 2, Math.Sqrt(5)),
                (2, -Math.Sqrt(5), 3)
            };

            foreach (var test in tests)
            {
                var v = new Vector2f((float)test.X, (float)test.Y);
                Assert.AreEqual(test.Length, v.GetLength(), Epsilon);
            }
        }

        [Test]
        public void TestNormalize()
        {
            var tests = new ((double X, double Y), (double X, double Y))[]
            {
                ((1, 0), (1, 0)),
                ((0, 1), (0, 1)),
                ((-1, 1), (-Math.Sqrt(0.5), Math.Sqrt(0.5))),
                ((3, -4), (0.6, -0.8))
            };

            foreach (var (a, b) in tests)
            {
                var v = new Vector2f((float)a.X, (float)a.Y).Normalize();
                var w = new Vector2f((float)b.X, (float)b.Y);

                Assert.AreEqual(w.X, v.X, Epsilon);
                Assert.AreEqual(w.Y, v.Y, Epsilon);
            }            
        }

        [Test]
        public void TestEquals()
        {
            var tests = new ((double X, double Y), (double X, double Y), bool)[]
            {
                ((0, 0), (0, 0), true),
                ((0, 1.21), (0, 1.22), false),
                ((-2.345, Math.PI), (-2.345, 3.14), false),
                ((Math.Sqrt(5), -4), (Math.Sqrt(5), -4), true)
            };

            foreach (var (a, b, same) in tests)
            {
                var v = new Vector2f((float)a.X, (float)a.Y);
                var w = new Vector2f((float)b.X, (float)b.Y);

                Assert.That(v == w, Is.EqualTo(same));
                Assert.That(v != w, Is.EqualTo(!same));
                Assert.That(v.Equals(w), Is.EqualTo(same));
                Assert.That(v.Equals(null), Is.EqualTo(false));
            }
        }

        [Test]
        public void TestNegation()
        {
            var tests = new ((double X, double Y), (double X, double Y))[]
            {
                ((0, 0), (0, 0)),
                ((0, 3), (0, -3)),
                ((-2.345, Math.E), (2.345, -Math.E)),
                ((-Math.Sqrt(7), -4), (Math.Sqrt(7), 4))
            };

            foreach (var (a, b) in tests)
            {
                var v = -new Vector2f((float)a.X, (float)a.Y);
                var w = new Vector2f((float)b.X, (float)b.Y);

                Assert.AreEqual(w.X, v.X, Epsilon);
                Assert.AreEqual(w.Y, v.Y, Epsilon);
            }
        }

        [Test]
        public void TestFactor()
        {
            var tests = new (double, (double X, double Y), (double X, double Y))[]
            {
                (7, (0, 0), (0, 0)),
                (0, (2, 3), (0, 0)),
                (1, (2.1, 3.2), (2.1, 3.2)),
                (3, (2, 4), (6, 12)),
                (-1.5, (2, 4), (-3, -6))
            };

            foreach (var (k, a, b) in tests)
            {
                var v = (float)k * new Vector2f((float)a.X, (float)a.Y);
                var w = new Vector2f((float)b.X, (float)b.Y);

                Assert.AreEqual(w.X, v.X, Epsilon);
                Assert.AreEqual(w.Y, v.Y, Epsilon);
            }
        }

        [Test]
        public void TestSum()
        {
            var tests = new ((double X, double Y), (double X, double Y), (double X, double Y))[]
            {
                ((0, 0), (0, 0), (0, 0)),
                ((2, 3), (0, 0), (2, 3)),
                ((1, 3), (-1, -3), (0, 0)),
                ((2.2, -1.5), (0.8, -2.5), (3, -4))
            };

            foreach (var (a, b, c) in tests)
            {
                var v = new Vector2f((float)a.X, (float)a.Y) + new Vector2f((float)b.X, (float)b.Y);
                var w = new Vector2f((float)c.X, (float)c.Y);

                Assert.AreEqual(w.X, v.X, Epsilon);
                Assert.AreEqual(w.Y, v.Y, Epsilon);
            }
        }

        [Test]
        public void TestDiff()
        {
            var tests = new ((double X, double Y), (double X, double Y), (double X, double Y))[]
            {
                ((0, 0), (0, 0), (0, 0)),
                ((2, 3), (0, 0), (2, 3)),
                ((1, 3), (-1, -3), (2, 6)),
                ((2.2, -1.5), (0.8, -2.5), (1.4, 1.0))
            };

            foreach (var (a, b, c) in tests)
            {
                var v = new Vector2f((float)a.X, (float)a.Y) - new Vector2f((float)b.X, (float)b.Y);
                var w = new Vector2f((float)c.X, (float)c.Y);

                Assert.AreEqual(w.X, v.X, Epsilon);
                Assert.AreEqual(w.Y, v.Y, Epsilon);
            }
        }
    }
}