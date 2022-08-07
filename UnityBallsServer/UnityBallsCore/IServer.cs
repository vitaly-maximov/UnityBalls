using UnityBallsCore.Command;

namespace UnityBallsCore
{
    public interface IServer
    {
        void MoveUser(MoveUserCommand command);
    }
}
