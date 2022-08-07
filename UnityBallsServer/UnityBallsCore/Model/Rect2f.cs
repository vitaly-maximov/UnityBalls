namespace UnityBallsCore.Model
{
    public class Rect2f
    {
        public static readonly Rect2f Empty = new Rect2f(0, 0, 0, 0);

        public Rect2f(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public float X { get; set; }

        public float Y { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public float Left => X;

        public float Top => Y + Height;

        public float Right => X + Width;

        public float Bottom => Y;

        public float CenterX => X + Width / 2;

        public float CenterY => Y + Height / 2;

        public bool Contains(Vector2f point, float offset = 0) =>
            point.X >= Left - offset && point.X <= Right + offset &&
            point.Y >= Bottom - offset && point.Y <= Top + offset;

        public bool IntersectsWith(Rect2f rect) =>
            Left <= rect.Right &&
            rect.Left <= Right &&
            Bottom <= rect.Top &&
            rect.Bottom <= Top;
    }
}
