using UnityBallsCore.Model;

namespace UnityBallsCore.Message
{
    public class ConnectMessage : IMessage
    {
        public ConnectMessage(int userId, WorldDto world)
        {
            UserId = userId;
            World = world;
        }

        public int UserId { get; }

        public WorldDto World { get; }
    }

    public class UpdateSceneMessage : IMessage
    {
        public UpdateSceneMessage(StateDto state)
        {
            State = state;
        }

        public StateDto State { get; }
    }
}
