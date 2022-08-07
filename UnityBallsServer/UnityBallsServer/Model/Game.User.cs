using UnityBallsCore.Model;

namespace UnityBallsServer.Model
{
    internal partial class Game
    {
        private class User
        {
            public User(int id, string connectionId)
            {
                Id = id;
                ConnectionId = connectionId;
            }

            public int Id { get; }

            public string ConnectionId { get; }

            public Vector2f Position { get; set; } = Vector2f.Empty;

            public Vector2f Direction { get; set; } = Vector2f.Up;

            public float Speed { get; set; }

            public uint ColorArgb { get; set; }
        }
    }
}
