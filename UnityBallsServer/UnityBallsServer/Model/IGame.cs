using UnityBallsCore.Command;
using UnityBallsCore.Message;

namespace UnityBallsServer.Model
{
    internal record Message(IMessage ActualMessage, string? ConnectionId = null);

    internal record Command(ICommand ActualCommand, string ConnectionId);

    internal interface IGame
    {
        void EnqueueCommand(Command command);

        IReadOnlyList<Message> Process();
    }
}
