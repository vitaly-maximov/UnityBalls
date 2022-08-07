using UnityBallsCore.Model;

namespace UnityBallsCore.Command
{
    public class AddUserCommand : ICommand
    {
    }
    public class RemoveUserCommand : ICommand
    {
    }

    public class MoveUserCommand : ICommand
    {
        public MoveUserCommand(Vector2f direction)
        {
            Direction = direction;
        }

        public Vector2f Direction { get; }
    }
}
