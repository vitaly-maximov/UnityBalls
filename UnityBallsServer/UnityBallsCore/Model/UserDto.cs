namespace UnityBallsCore.Model
{
    public class UserDto
    {
        public int Id { get; set; }

        public Vector2f Position { get; set; } = Vector2f.Empty;

        public Vector2f Direction { get; set; } = Vector2f.Empty;

        public float Speed { get; set; }

        public uint ColorArgb { get; set; }
    }
}