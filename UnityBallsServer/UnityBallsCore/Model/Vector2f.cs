using System;

namespace UnityBallsCore.Model
{
    public class Vector2f
    {
        private const float Epsilon = 1e-8f;

        public static readonly Vector2f Empty = new Vector2f(0, 0);

        public static readonly Vector2f Up = new Vector2f(0, 1);

        public Vector2f(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; }

        public float Y { get; }

        public float GetLength() => (float)Math.Sqrt(X * X + Y * Y);

        public Vector2f Normalize()
        {
            float length = GetLength();
            if (Math.Abs(length - 1) < Epsilon)
            {
                return this;
            }

            return new Vector2f(X / length, Y / length);
        }

        public override bool Equals(object? obj) => obj is Vector2f that && this == that;

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public static bool operator ==(Vector2f a, Vector2f b) =>
            a.X == b.X && a.Y == b.Y;

        public static bool operator !=(Vector2f a, Vector2f b) => !(a == b);

        public static Vector2f operator -(Vector2f a) =>
            new Vector2f(-a.X, -a.Y);

        public static Vector2f operator +(Vector2f a, Vector2f b) =>
            new Vector2f(a.X + b.X, a.Y + b.Y);

        public static Vector2f operator -(Vector2f a, Vector2f b) =>
            new Vector2f(a.X - b.X, a.Y - b.Y);

        public static Vector2f operator *(float a, Vector2f b) =>
            new Vector2f(a * b.X, a * b.Y);
    }
}
